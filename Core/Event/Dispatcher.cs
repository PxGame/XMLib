/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 5:18:17 PM
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 事件调度
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// 分组映射
        /// </summary>
        private readonly Dictionary<object, List<DispatchEvent>> _groupMapping;

        /// <summary>
        /// 事件监听
        /// </summary>
        private readonly Dictionary<string, SortList<DispatchEvent, int>> _listeners;

        /// <summary>
        /// 容器
        /// </summary>
        private Container _container;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object _syncRoot;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app">应用实例</param>
        public Dispatcher(Container container)
        {
            _groupMapping = new Dictionary<object, List<DispatchEvent>>();
            _listeners = new Dictionary<string, SortList<DispatchEvent, int>>();
            _container = container;
            _syncRoot = new object();
        }

        /// <summary>
        /// 是否存在事件的监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在</returns>
        public bool HasListener(string eventName)
        {
            lock (_syncRoot)
            {
                return _listeners.ContainsKey(eventName);
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public List<object> Trigger(string eventName, params object[] args)
        {
            return Dispatch(false, eventName, false, args) as List<object>;
        }

        /// <summary>
        /// 触发一个事件，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public object TriggerHalt(string eventName, params object[] args)
        {
            return Dispatch(true, eventName, false, args);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public List<object> TriggerFast(string eventName, params object[] args)
        {
            return Dispatch(false, eventName, true, args) as List<object>;
        }

        /// <summary>
        /// 触发一个事件，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public object TriggerHaltFast(string eventName, params object[] args)
        {
            return Dispatch(true, eventName, true, args);
        }

        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">调用方法</param>
        /// <param name="group">分组</param>
        /// <returns>监听</returns>
        public DispatchEvent On(string eventName, object target, MethodInfo methodInfo, object group = null)
        {
            if (!methodInfo.IsStatic && null == target)
            {//非静态函数,必须有实例
                throw new RuntimeException("事件调用失败,非静态函数需要指定调用实例 > ", eventName);
            }

            lock (_syncRoot)
            {
                DispatchEvent evt = SetupListener(eventName, target, methodInfo, group);

                if (null == group)
                {//无分组则直接返回
                    return evt;
                }

                //添加到分组
                List<DispatchEvent> events;
                if (!_groupMapping.TryGetValue(group, out events))
                {
                    events = new List<DispatchEvent>();
                    _groupMapping[group] = events;
                }
                events.Add(evt);

                return evt;
            }
        }

        /// <summary>
        /// 解除注册的事件监听器
        /// </summary>
        /// <param name="target">
        /// 事件解除目标
        /// <para>如果传入的是字符串(<code>string</code>)将会解除对应事件名的所有事件</para>
        /// <para>如果传入的是事件对象(<code>DispatchEvent</code>)那么解除对应事件</para>
        /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
        /// </param>
        public void Off(object target)
        {
            lock (_syncRoot)
            {
                DispatchEvent evt = target as DispatchEvent;
                if (null != evt)
                {//移除监听
                    ForgetListen(evt);
                    return;
                }

                if (target is string)
                {//移除事件
                    ForgetEvent((string)target);
                }

                //移除分组
                ForgetGroup(target);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="evt">事件对象</param>
        private void ForgetListen(DispatchEvent evt)
        {
            //移除分组中的监听
            List<DispatchEvent> events = null;
            if (null != evt.group)
            {
                if (_groupMapping.TryGetValue(evt.group, out events))
                {
                    events.Remove(evt);
                    if (events.Count <= 0)
                    {
                        _groupMapping.Remove(evt.group);
                    }
                }
            }

            //移除监听
            SortList<DispatchEvent, int> sortEvents = null;
            if (_listeners.TryGetValue(evt.name, out sortEvents))
            {
                sortEvents.Remove(evt);
                if (sortEvents.count <= 0)
                {
                    _listeners.Remove(evt.name);
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        private void ForgetEvent(string eventName)
        {
            SortList<DispatchEvent, int> events;
            if (!_listeners.TryGetValue(eventName, out events))
            {
                return;
            }

            DispatchEvent[] eventArray = events.ToArray();
            foreach (DispatchEvent evt in eventArray)
            {
                ForgetListen(evt);
            }
        }

        /// <summary>
        /// 移除分组
        /// </summary>
        /// <param name="group">分组</param>
        private void ForgetGroup(object group)
        {
            List<DispatchEvent> events;
            if (!_groupMapping.TryGetValue(group, out events))
            {
                return;
            }

            DispatchEvent[] eventArray = events.ToArray();
            foreach (DispatchEvent evt in eventArray)
            {
                ForgetListen(evt);
            }
        }

        /// <summary>
        /// 设置监听对象
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="func">响应函数</param>
        /// <param name="group">分组</param>
        /// <returns>监听</returns>
        private DispatchEvent SetupListener(string eventName, object target, MethodInfo methodInfo, object group)
        {
            SortList<DispatchEvent, int> events;
            if (!_listeners.TryGetValue(eventName, out events))
            {
                events = new SortList<DispatchEvent, int>();
                _listeners[eventName] = events;
            }

            //创建事件对象
            DispatchEvent evt = MakeEvent(eventName, target, methodInfo, group);

            //获取优先级
            int priority = ReflectionUtil.GetPriority(methodInfo);

            //添加到事件列表
            events.Add(evt, priority);

            return evt;
        }

        /// <summary>
        /// 创建监听对象
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标</param>
        /// <param name="methodInfo">响应函数</param>
        /// <param name="group">分组</param>
        /// <returns>监听</returns>
        private DispatchEvent MakeEvent(string eventName, object target, MethodInfo methodInfo, object group)
        {
            return new DispatchEvent(eventName, target, methodInfo, group);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="half">遇到第一个事件且有结果</param>
        /// <param name="eventName">事件名</param>
        /// <param name="fastMode">快速模式</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        private object Dispatch(bool half, string eventName, bool fastMode, object[] args)
        {
            //
            //UnityEngine.Debug.Log("事件调用 :" + eventName);
            //

            lock (_syncRoot)
            {
                List<object> outputs = new List<object>();

                IEnumerable<DispatchEvent> events = GetListener(eventName);

                if (null != events)
                {
                    List<DispatchEvent> invalidEvents = null;

                    DispatchEvent target = null;
                    try
                    {
                        foreach (DispatchEvent evt in events)
                        {//遍历监听
                            target = evt;
                            if (!target.IsValid())
                            {//非有效事件,移除
                                if (null == invalidEvents)
                                {
                                    invalidEvents = new List<DispatchEvent>();
                                }

                                //添加到无效列表
                                invalidEvents.Add(target);
                                continue;
                            }

                            if (!target.IsActiveAndEnabled())
                            {//非激活状态事件
                                continue;
                            }

                            //调用事件

                            object result = null;

                            if (fastMode)
                            {
                                result = target.methodInfo.Invoke(target.target, args);
                            }
                            else
                            {
                                result = _container.Call(target.target, target.methodInfo, args);
                            }

                            //只取一个结果时
                            if (half && result != null)
                            {
                                return result;
                            }

                            outputs.Add(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = null == target ? "事件调用异常" : string.Format(
#if UNITY_EDITOR
                        "<color=red>事件调用异常:{0})</color>"
#else
                        "事件调用异常:{0} "
#endif
                        , target);
                        throw new RuntimeException(msg, ex);
                    }

                    if (null != invalidEvents)
                    {//移除无效事件
                        foreach (DispatchEvent evt in invalidEvents)
                        {
                            ForgetListen(evt);
                        }
                    }
                }

                return half ? null : outputs;
            }
        }

        /// <summary>
        /// 获取事件对象
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>事件对象列表</returns>
        private IEnumerable<DispatchEvent> GetListener(string eventName)
        {
            //目前不需要将获取到的监听器对象另存列表
            //这样可以减少性能开销
            //但需要在函数调用出判断结果是否为null
            //List<DispatchEvent> events = new List<DispatchEvent>();

            SortList<DispatchEvent, int> result = null;
            if (_listeners.TryGetValue(eventName, out result))
            {//添加到调用监听列表
            }

            return result;
        }

        #region 扩展

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="target">调用目标</param>
        /// <param name="method">处理函数名</param>
        /// <returns>对象</returns>
        public DispatchEvent On(string eventName, object target, string method = null)
        {
            if (null == method)
            {//事件名为函数名
                method = eventName;
            }

            MethodInfo methodInfo = target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return On(eventName, target, methodInfo, target);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent On(string eventName, Action method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent On<T1>(string eventName, Action<T1> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent On<T1, T2>(string eventName, Action<T1, T2> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent On<T1, T2, T3>(string eventName, Action<T1, T2, T3> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent Listen<TResult>(string eventName, Func<TResult> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent Listen<T1, TResult>(string eventName, Func<T1, TResult> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent Listen<T1, T2, TResult>(string eventName, Func<T1, T2, TResult> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent Listen<T1, T2, T3, TResult>(string eventName, Func<T1, T2, T3, TResult> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public DispatchEvent Listen<T1, T2, T3, T4, TResult>(string eventName, Func<T1, T2, T3, T4, TResult> method, object group = null)
        {
            return On(eventName, method.Target, method.Method, group);
        }

        #endregion 扩展
    }
}