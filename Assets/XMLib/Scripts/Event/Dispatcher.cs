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
    public class Dispatcher : IDispatcher
    {
        /// <summary>
        /// 分组映射
        /// </summary>
        private readonly Dictionary<object, List<IEvent>> _groupMapping;

        /// <summary>
        /// 事件监听
        /// </summary>
        private readonly Dictionary<string, SortList<IEvent, int>> _listeners;

        /// <summary>
        /// 同步锁
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 绑定的应用实例
        /// </summary>
        private IApplication _app;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app">应用实例</param>
        public Dispatcher(IApplication app)
        {
            _app = app;
            _groupMapping = new Dictionary<object, List<IEvent>>();
            _listeners = new Dictionary<string, SortList<IEvent, int>>();
            _syncRoot = new object();
        }

        /// <summary>
        /// 是否存在事件的监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在</returns>
        public bool HasListener(string eventName)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");
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
            Checker.NotEmptyOrNull(eventName, "eventName");
            return Dispatch(false, eventName, args) as List<object>;
        }

        /// <summary>
        /// 触发一个事件，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public object TriggerHalt(string eventName, params object[] args)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");
            return Dispatch(true, eventName, args);
        }

        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">调用方法</param>
        /// <param name="group">分组</param>
        /// <returns>监听</returns>
        public IEvent On(string eventName, object target, MethodInfo methodInfo, object group = null)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");
            Checker.Requires<ArgumentException>(methodInfo != null);

            if (!methodInfo.IsStatic)
            {//非静态函数,必须有实例
                Checker.Requires<ArgumentNullException>(target != null);
            }

            lock (_syncRoot)
            {
                IEvent evt = SetupListener(eventName, target, methodInfo, group);

                if (null == group)
                {//无分组则直接返回
                    return evt;
                }

                //添加到分组
                List<IEvent> events;
                if (!_groupMapping.TryGetValue(group, out events))
                {
                    events = new List<IEvent>();
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
        /// <para>如果传入的是事件对象(<code>IEvent</code>)那么解除对应事件</para>
        /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
        /// </param>
        public void Off(object target)
        {
            Checker.Requires<ArgumentException>(target != null);

            lock (_syncRoot)
            {
                IEvent evt = target as IEvent;
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
        private void ForgetListen(IEvent evt)
        {
            //移除分组中的监听
            List<IEvent> events = null;
            if (null != evt.Group)
            {
                if (_groupMapping.TryGetValue(evt.Group, out events))
                {
                    events.Remove(evt);
                    if (events.Count <= 0)
                    {
                        _groupMapping.Remove(evt.Group);
                    }
                }
            }

            //移除监听
            SortList<IEvent, int> sortEvents = null;
            if (_listeners.TryGetValue(evt.Name, out sortEvents))
            {
                sortEvents.Remove(evt);
                if (sortEvents.Count <= 0)
                {
                    _listeners.Remove(evt.Name);
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        private void ForgetEvent(string eventName)
        {
            SortList<IEvent, int> events;
            if (!_listeners.TryGetValue(eventName, out events))
            {
                return;
            }

            IEvent[] eventArray = events.ToArray();
            foreach (IEvent evt in eventArray)
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
            List<IEvent> events;
            if (!_groupMapping.TryGetValue(group, out events))
            {
                return;
            }

            IEvent[] eventArray = events.ToArray();
            foreach (IEvent evt in eventArray)
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
        private IEvent SetupListener(string eventName, object target, MethodInfo methodInfo, object group)
        {
            SortList<IEvent, int> events;
            if (!_listeners.TryGetValue(eventName, out events))
            {
                events = new SortList<IEvent, int>();
                _listeners[eventName] = events;
            }

            //创建事件对象
            IEvent evt = MakeEvent(eventName, target, methodInfo, group);

            //获取优先级
            int priority = AttributeUtil.GetPriority(methodInfo);

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
        protected virtual IEvent MakeEvent(string eventName, object target, MethodInfo methodInfo, object group)
        {
            return new Event(eventName, target, methodInfo, group);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="half">遇到第一个事件且有结果</param>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        private object Dispatch(bool half, string eventName, object[] args)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");

            lock (_syncRoot)
            {
                List<object> outputs = new List<object>();

                IEnumerable<IEvent> events = GetListener(eventName);

                if (null != events)
                {
                    List<IEvent> invalidEvents = null;

                    IEvent target = null;
                    try
                    {
                        foreach (IEvent evt in events)
                        {//遍历监听
                            target = evt;
                            if (!target.IsValid())
                            {//非有效事件,移除
                                if (null == invalidEvents)
                                {
                                    invalidEvents = new List<IEvent>();
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

                            object result = _app.Call(target.Target, target.MethodInfo, args);

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
                        string msg = "事件调用异常";

                        if (null != target)
                        {
                            msg = string.Format(
                                                 "<color=red>事件调用异常:目标对象 ({0}) , 调用函数 ({1}) , 声明类型({2})</color>",
                                                 target.Target,
                                                 target.MethodInfo,
                                                 target.MethodInfo.DeclaringType);
                        }

                        throw new RuntimeException(msg, ex);
                    }

                    if (null != invalidEvents)
                    {//移除无效事件
                        foreach (IEvent evt in invalidEvents)
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
        private IEnumerable<IEvent> GetListener(string eventName)
        {
            //目前不需要将获取到的监听器对象另存列表
            //这样可以减少性能开销
            //但需要在函数调用出判断结果是否为null
            //List<IEvent> events = new List<IEvent>();

            SortList<IEvent, int> result = null;
            if (_listeners.TryGetValue(eventName, out result))
            {//添加到调用监听列表
            }

            return result;
        }
    }
}