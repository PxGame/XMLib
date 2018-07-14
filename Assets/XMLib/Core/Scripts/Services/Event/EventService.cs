using System;
using System.Collections.Generic;

namespace XM.Services
{
    /// <summary>
    /// 事件服务
    /// </summary>
    public class EventService : BaseService<EventSetting>
    {
        #region protected members

        /// <summary>
        /// 同步
        /// </summary>
        protected readonly object SyncRoot = new object();

        protected Dictionary<string, List<IEventData>> _eventDict = new Dictionary<string, List<IEventData>>();

        #endregion protected members

        #region Base

        protected override void OnClearService()
        {
            RemoveEvent();
        }

        #endregion Base

        #region ext opt

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public List<object> Call(string eventName, params object[] args)
        {
            return Call<object>(eventName, args);
        }

        internal IEventData Add<T>(object onTest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public IEventData Add(string eventName, Action callback, int invokeMaxCount = -1)
        {
            EventData eventData = new EventData(eventName, callback.Target, callback.Method, invokeMaxCount);
            Add(eventData);
            return eventData;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public IEventData Add<T>(string eventName, Action<T> callback, int invokeMaxCount = -1)
        {
            EventData eventData = new EventData(eventName, callback.Target, callback.Method, invokeMaxCount);
            Add(eventData);
            return eventData;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public IEventData Add<T1, T2>(string eventName, Action<T1, T2> callback, int invokeMaxCount = -1)
        {
            EventData eventData = new EventData(eventName, callback.Target, callback.Method, invokeMaxCount);
            Add(eventData);
            return eventData;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public IEventData Add<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback, int invokeMaxCount = -1)
        {
            EventData eventData = new EventData(eventName, callback.Target, callback.Method, invokeMaxCount);
            Add(eventData);
            return eventData;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public IEventData Add<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback, int invokeMaxCount = -1)
        {
            EventData eventData = new EventData(eventName, callback.Target, callback.Method, invokeMaxCount);
            Add(eventData);
            return eventData;
        }

        #endregion ext opt

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventData"></param>
        public void Add(IEventData eventData)
        {
            lock (SyncRoot)
            {
                List<IEventData> events;
                if (!_eventDict.TryGetValue(eventData.EventName, out events))
                {//不存在则添加
                    events = new List<IEventData>();
                    _eventDict.Add(eventData.EventName, events);
                }

                events.Add(eventData);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventData"></param>
        public void Remove(IEventData eventData)
        {
            List<IEventData> events;

            lock (SyncRoot)
            {
                if (_eventDict.TryGetValue(eventData.EventName, out events))
                {
                    //移除监听
                    events.Remove(eventData);

                    if (0 == events.Count)
                    {//监听数为0则移除事件
                        _eventDict.Remove(eventData.EventName);
                    }
                }
            }
        }

        /// <summary>
        /// 移除对象上的所有监听
        /// </summary>
        /// <param name="target"></param>
        public void Remove(object target)
        {
            List<string> removeEvents = new List<string>();//需要移除的事件
            int count;
            List<IEventData> events;
            IEventData eventDataTmp = null;

            lock (SyncRoot)
            {
                //遍历所有事件
                foreach (var eventsPair in _eventDict)
                {
                    events = eventsPair.Value;

                    count = events.Count;
                    for (int i = 0; i < count; i++)
                    {
                        eventDataTmp = events[i];

                        if (eventDataTmp.MethodTarget == target)
                        {//移除监听
                            events.RemoveAt(i);
                            --count;
                            --i;
                        }
                    }

                    if (0 == events.Count)
                    {//监听数为0则移除事件
                        removeEvents.Add(eventsPair.Key);
                    }
                }

                //清理事件
                count = removeEvents.Count;
                for (int i = 0; i < count; i++)
                {
                    _eventDict.Remove(removeEvents[i]);
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventName"></param>
        public void RemoveEvent(string eventName)
        {
            lock (SyncRoot)
            {
                _eventDict.Remove(eventName);
            }
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        public void RemoveEvent()
        {
            lock (SyncRoot)
            {
                _eventDict.Clear();
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="eventName">事件名</param>
        /// <param name="args">事件参数</param>
        /// <returns>返回结果</returns>
        public List<T> Call<T>(string eventName, params object[] args)
        {
            List<T> results = new List<T>();
            T result;
            List<IEventData> eventDatas = null;

            lock (SyncRoot)
            {
                if (!_eventDict.TryGetValue(eventName, out eventDatas))
                {//没有该事件
                    return results;
                }

                IEventData eventData = null;
                int count = eventDatas.Count;

                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        eventData = eventDatas[i];

                        if (!eventData.IsVaild())
                        {//无效则移除事件
                            Debug(DebugType.Warning, "发现无效事件，执行移除操作");
                            eventDatas.Remove(eventData);
                            --count;
                            --i;
                            continue;
                        }

                        //调用
                        result = (T)eventData.Call(args);
                        results.Add(result);

                        if (eventData.CheckRemove())
                        {//检查是否需要移除该事件
                            eventDatas.Remove(eventData);
                            --count;
                            --i;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug(DebugType.Exception, "事件调用异常:", ex);
                    }
                }

                //没有监听时，移除事件
                if (0 == count)
                {
                    _eventDict.Remove(eventName);
                }
            }

            return results;
        }

        /// <summary>
        /// 事件是否存在
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns></returns>
        public bool IsExist(string eventName)
        {
            lock (SyncRoot)
            {
                return _eventDict.ContainsKey(eventName);
            }
        }
    }
}