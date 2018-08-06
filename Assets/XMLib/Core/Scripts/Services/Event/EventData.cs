using System;
using System.Reflection;

namespace XM.Services.Event
{
    /// <summary>
    /// 事件监听
    /// </summary>
    public class EventData : IEventData
    {
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get { return _eventName; } }

        /// <summary>
        /// 函数目标对象
        /// </summary>
        public object MethodTarget { get { return _methodTarget; } }

        /// <summary>
        /// 函数信息
        /// </summary>
        public MethodInfo MethodInfo { get { return _methodInfo; } }

        /// <summary>
        /// 最大调用次数
        /// </summary>
        public int InvokeMaxCount { get { return _invokeMaxCount; } }

        /// <summary>
        /// 当前调用次数
        /// </summary>
        public int CurrentInvokeCount { get { return _currentInvokeCount; } }

        protected string _eventName;
        protected object _methodTarget;
        protected MethodInfo _methodInfo;
        protected int _invokeMaxCount;
        protected int _currentInvokeCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="methodTarget">静态函数可以设置为null</param>
        /// <param name="methodInfo">函数信息</param>
        /// <param name="invokeMaxCount">达到最大调用次数后移除</param>
        public EventData(string eventName, object methodTarget, MethodInfo methodInfo, int invokeMaxCount)
        {
            if (null == methodTarget && !methodInfo.IsStatic)
            {//非静态函数，Target不能为null
                throw new Exception("非静态函数，Target不能为null");
            }

            _eventName = eventName;
            _methodTarget = methodTarget;
            _methodInfo = methodInfo;
            _currentInvokeCount = 0;
            _invokeMaxCount = invokeMaxCount;
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Call(params object[] args)
        {
            ++_currentInvokeCount;

            object bRet = null;

            try
            {
                bRet = _methodInfo.Invoke(_methodTarget, args);
            }
            catch (Exception ex)
            {
                string msg = string.Format("事件调用异常 事件名：{0} 类：{1} 函数：{2}", _eventName, _methodTarget, _methodInfo);
                throw new Exception(msg, ex);
            }

            return bRet;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsVaild()
        {
            if (null == _methodInfo)
            {
                return false;
            }

            if (!_methodInfo.IsStatic && null == _methodTarget)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 需要移除
        /// </summary>
        /// <returns></returns>
        public bool CheckRemove()
        {
            if ((-1 != _invokeMaxCount)
                && (_currentInvokeCount >= _invokeMaxCount))
            {
                return true;
            }

            return false;
        }
    }
}