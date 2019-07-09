/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 12:06:21 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    public class MonoDriver
    {
        #region Mono

        private readonly SortList<IUpdate, int> _updates;
        private readonly SortList<ILateUpdate, int> _lateUpdates;
        private readonly SortList<IFixedUpdate, int> _fixedUpdates;
        private readonly SortList<IOnDestroy, int> _onDestroys;
        private readonly SortList<IOnGUI, int> _onGUIs;

        #endregion Mono

        private readonly HashSet<object> _loadSet;
        private readonly object _syncRoot;
        private readonly Queue<Action> _mainThreadDispatcherQueue;
        private readonly MonoBehaviour _mono;
        private readonly Func<bool> _isMainThread;

        public MonoDriver(MonoBehaviour mono, Func<bool> isMainThread)
        {
            _mono = mono;
            _isMainThread = isMainThread;

            _updates = new SortList<IUpdate, int>();
            _lateUpdates = new SortList<ILateUpdate, int>();
            _fixedUpdates = new SortList<IFixedUpdate, int>();
            _onDestroys = new SortList<IOnDestroy, int>();
            _onGUIs = new SortList<IOnGUI, int>();

            _loadSet = new HashSet<object>();
            _syncRoot = new object();
            _mainThreadDispatcherQueue = new Queue<Action>();
        }

        /// <summary>
        /// 全局服务释放事件
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="instance">对象</param>
        public void OnGlobalResolving(BindData bindData, object instance)
        {
            if (null == instance)
            {
                return;
            }

            if (bindData.isStatic)
            {//解决静态服务
                Attach(instance);
            }
        }

        /// <summary>
        /// 全局服务解决事件
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="instance">对象</param>
        public void OnGlobalRelease(BindData bindData, object instance)
        {
            if (null == instance)
            {
                return;
            }

            if (bindData.isStatic)
            {//解决静态服务
                Detach(instance);
            }
        }

        /// <summary>
        /// 转换并从移除列表
        /// </summary>
        /// <param name="sortList">目标列表</param>
        /// <param name="obj">对象</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndRemove<T>(ISortList<T, int> sortList, object obj) where T : class
        {
            T target = obj as T;
            return null != target && sortList.Remove(target);
        }

        /// <summary>
        /// 转换并添加到列表
        /// </summary>
        /// <param name="sortList">目标列表</param>
        /// <param name="obj">对象</param>
        /// <param name="function">函数名</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndAdd<T>(ISortList<T, int> sortList, object obj, string function) where T : class
        {
            T target = obj as T;

            if (null == target)
            {
                return false;
            }

            int weight = ReflectionUtil.GetPriority(target.GetType(), function);
            sortList.Add(target, weight);

            return true;
        }

        /// <summary>
        /// 包装器
        /// </summary>
        /// <param name="action">回调函数</param>
        /// <returns>迭代器</returns>
        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield break;
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">协程，执行会处于主线程</param>
        public void MainThread(IEnumerator action)
        {
            if (_isMainThread())
            {
                _mono.StartCoroutine(action);
                return;
            }

            lock (_syncRoot)
            {
                _mainThreadDispatcherQueue.Enqueue(() =>
                {
                    _mono.StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">回调，回调的内容会处于主线程</param>
        public void MainThread(Action action)
        {
            if (_isMainThread())
            {
                action.Invoke();
                return;
            }

            MainThread(ActionWrapper(action));
        }

        /// <summary>
        /// 卸载对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Detach(object obj)
        {
            if (!_loadSet.Contains(obj))
            {
                return;
            }

            ConvertAndRemove(_updates, obj);
            ConvertAndRemove(_lateUpdates, obj);
            ConvertAndRemove(_fixedUpdates, obj);
            ConvertAndRemove(_onGUIs, obj);
            if (ConvertAndRemove(_onDestroys, obj))
            {//调用销毁事件
                ((IOnDestroy)obj).OnDestroy();
            }

            _loadSet.Remove(obj);
        }

        /// <summary>
        /// 装载对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Attach(object obj)
        {
            if (_loadSet.Contains(obj))
            {
                throw new RuntimeException("对象[" + obj + "]已经被加载");
            }

            bool isLoad = ConvertAndAdd(_updates, obj, "Update");
            isLoad = ConvertAndAdd(_lateUpdates, obj, "LateUpdate") || isLoad;
            isLoad = ConvertAndAdd(_fixedUpdates, obj, "FixedUpdate") || isLoad;
            isLoad = ConvertAndAdd(_onGUIs, obj, "OnGUI") || isLoad;
            isLoad = ConvertAndAdd(_onDestroys, obj, "OnDestroy") || isLoad;

            if (isLoad)
            {
                _loadSet.Add(obj);
            }
        }

        #region Unity Event

        /// <summary>
        /// unity update
        /// </summary>
        public void Update()
        {
            int count = _updates.count;
            for (int i = 0; i < count; i++)
            {
                _updates[i].Update();
            }

            lock (_syncRoot)
            {
                while (_mainThreadDispatcherQueue.Count > 0)
                {
                    _mainThreadDispatcherQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// unity lateupdate
        /// </summary>
        public void LateUpdate()
        {
            int count = _lateUpdates.count;
            for (int i = 0; i < count; i++)
            {
                _lateUpdates[i].LateUpdate();
            }
        }

        /// <summary>
        /// unity fixed update
        /// </summary>
        public void FixedUpdate()
        {
            int count = _fixedUpdates.count;
            for (int i = 0; i < count; i++)
            {
                _fixedUpdates[i].FixedUpdate();
            }
        }

        /// <summary>
        /// unity on gui
        /// </summary>
        public void OnGUI()
        {
            int count = _onGUIs.count;
            for (int i = 0; i < count; i++)
            {
                _onGUIs[i].OnGUI();
            }
        }

        /// <summary>
        /// unity on destroy
        /// </summary>
        public void OnDestroy()
        {
            int count = _onDestroys.count;
            for (int i = 0; i < count; i++)
            {
                _onDestroys[i].OnDestroy();
            }

            _updates.Clear();
            _lateUpdates.Clear();
            _fixedUpdates.Clear();
            _onGUIs.Clear();
            _onDestroys.Clear();
            _loadSet.Clear();
        }

        #endregion Unity Event
    }
}