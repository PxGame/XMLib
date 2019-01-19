/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 12:20:45 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.ObjectPool
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : IObjectPool
    {
        /// <summary>
        /// 设置
        /// </summary>
        private readonly ObjectPoolSetting _setting;

        /// <summary>
        /// 根节点
        /// </summary>
        private Transform _root;

        private Dictionary<string, Dictionary<string, Stack<GameObject>>> _poolDict;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool _isInit;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting">设置</param>
        public ObjectPool(ObjectPoolSetting setting)
        {
            _setting = setting;
            _poolDict = new Dictionary<string, Dictionary<string, Stack<GameObject>>>();
            _isInit = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int maxSize)
        {
            if (_isInit)
            {
                return;
            }

            GameObject obj = new GameObject("ObjectPoolRoot");
            GameObject.DontDestroyOnLoad(obj);
            _root = obj.transform;

            _isInit = true;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="obj"></param>
        private void OnDestroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }

        /// <summary>
        /// 对象弹出后
        /// </summary>
        /// <param name="obj">对象</param>
        private void OnPop(GameObject obj)
        {
            //移动到当前场景并启用
            SceneUtil.MoveObjectToCurrent(obj);
            obj.SetActive(true);
        }

        /// <summary>
        /// 对象压入后
        /// </summary>
        /// <param name="obj">对象</param>
        private void OnPush(GameObject obj)
        {
            //移动到对象池根节点下并禁用
            obj.SetActive(false);
            obj.transform.parent = _root;
        }

        /// <summary>
        /// 弹出
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns>对象实例</returns>
        public GameObject Pop(string typeName, string objectName)
        {
            Dictionary<string, Stack<GameObject>> pool = null;
            if (!_poolDict.TryGetValue(typeName, out pool))
            {//没有该类型
                return null;
            }

            Stack<GameObject> objs = null;
            if (!pool.TryGetValue(objectName, out objs))
            {//没有该对象
                return null;
            }

            GameObject obj = null;
            if (0 < objs.Count)
            {//有对象则取出一个
                obj = objs.Pop();
            }

            //移除空的池
            if (0 == objs.Count)
            {
                if (pool.Remove(objectName))
                {
                    if (0 == pool.Count)
                    {
                        _poolDict.Remove(typeName);
                    }
                }
            }

            if (null != obj)
            {
                //弹出后的处理
                OnPop(obj);
            }

            return obj;
        }

        /// <summary>
        /// 压入
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <param name="obj">对象实例</param>
        /// <returns>是否压入成功,失败将直接删除或者传入的对象为null</returns>
        public bool Push(string typeName, string objectName, GameObject obj)
        {
            if (null == obj)
            {
                return false;
            }

            Dictionary<string, Stack<GameObject>> pool = null;
            if (!_poolDict.TryGetValue(typeName, out pool))
            {//创建类型池
                pool = new Dictionary<string, Stack<GameObject>>();
                _poolDict.Add(typeName, pool);
            }

            Stack<GameObject> objs = null;
            if (!pool.TryGetValue(objectName, out objs))
            {//创建对象池
                objs = new Stack<GameObject>(_setting.maxSize / 2);
                pool.Add(objectName, objs);
            }

            if (objs.Count > _setting.maxSize)
            {//超出最大限制，直接删除
                //删除处理
                OnDestroy(obj);
                return false;
            }

            //添加到池中
            objs.Push(obj);

            //已添加后的处理
            OnPush(obj);

            return true;
        }

        /// <summary>
        /// 压入
        /// </summary>
        /// <param name="item">对象池元素</param>
        /// <returns>是否压入成功,失败将直接删除或者传入的对象为null</returns>
        public bool Push(IObjectPoolItem item)
        {
            if (null == item)
            {
                return false;
            }

            return Push(item.typeName, item.objectName, item.gameObject);
        }

        /// <summary>
        /// 压入，前提是对象上有实现IObjectPoolItem的组件
        /// </summary>
        /// <param name="obj">对象池元素</param>
        /// <returns>是否压入成功,失败将直接删除或者传入的对象为null</returns>
        public bool Push(GameObject obj)
        {
            if (null == obj)
            {
                return false;
            }

            IObjectPoolItem item = obj.GetComponent<IObjectPoolItem>();
            if (null == item)
            {//未实现IObjectPoolItem接口，直接删除
                OnDestroy(obj);
                return false;
            }

            return Push(item);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            List<GameObject> rms = new List<GameObject>();

            foreach (var poolPair in _poolDict)
            {
                Dictionary<string, Stack<GameObject>> pool = poolPair.Value;
                foreach (var objsPair in pool)
                {
                    Stack<GameObject> objs = objsPair.Value;

                    while (0 < objs.Count)
                    {//弹出并添加到删除列表
                        GameObject obj = objs.Pop();
                        rms.Add(obj);
                    }
                }
            }

            //清理
            _poolDict.Clear();

            //删除
            foreach (var obj in rms)
            {
                if (null != obj)
                {//删除处理
                    OnDestroy(obj);
                }
            }
        }

        /// <summary>
        /// 清理指定类型
        /// </summary>
        /// <param name="typeName">类型名</param>
        public void Clear(string typeName)
        {
            Dictionary<string, Stack<GameObject>> pool = null;
            if (!_poolDict.TryGetValue(typeName, out pool))
            {//创建类型池
                return;
            }

            List<GameObject> rms = new List<GameObject>();
            foreach (var objsPair in pool)
            {
                Stack<GameObject> objs = objsPair.Value;

                while (0 < objs.Count)
                {//弹出并添加到删除列表
                    GameObject obj = objs.Pop();
                    rms.Add(obj);
                }
            }

            //移除指定类型
            _poolDict.Remove(typeName);

            //删除
            foreach (var obj in rms)
            {
                if (null != obj)
                {//删除处理
                    OnDestroy(obj);
                }
            }
        }

        /// <summary>
        /// 清理指定类型且指定名字
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        public void Clear(string typeName, string objectName)
        {
            Dictionary<string, Stack<GameObject>> pool = null;
            if (!_poolDict.TryGetValue(typeName, out pool))
            {//创建类型池
                return;
            }

            Stack<GameObject> objs = null;
            if (!pool.TryGetValue(objectName, out objs))
            {//创建对象池
                return;
            }

            List<GameObject> rms = new List<GameObject>();
            while (0 < objs.Count)
            {
                GameObject obj = objs.Pop();
                rms.Add(obj);
            }

            //移除空的池
            if (pool.Remove(objectName))
            {
                if (0 == pool.Count)
                {
                    _poolDict.Remove(typeName);
                }
            }

            //删除
            foreach (var obj in rms)
            {
                if (null != obj)
                {//删除处理
                    OnDestroy(obj);
                }
            }
        }

        /// <summary>
        /// 获取指定类型且指定名字的对象数量
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns>数量</returns>
        public int GetCount(string typeName, string objectName)
        {
            Dictionary<string, Stack<GameObject>> pool = null;
            if (!_poolDict.TryGetValue(typeName, out pool))
            {//创建类型池
                return 0;
            }

            Stack<GameObject> objs = null;
            if (!pool.TryGetValue(objectName, out objs))
            {//创建对象池
                return 0;
            }

            return objs.Count;
        }

        /// <summary>
        /// 存在对象
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="objectName">对象名</param>
        /// <returns>是否存在</returns>
        public bool HasObj(string typeName, string objectName)
        {
            return GetCount(typeName, objectName) > 0;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            //删除所有对象
            Clear();

            _isInit = false;
            if (null != _root)
            {//删除根节点
                GameObject.Destroy(_root);
                _root = null;
            }
        }
    }
}