﻿using System;
using System.Reflection;
using XM.Exceptions;

namespace XM.Services.Task
{
    /// <summary>
    /// 任务数据
    /// </summary>
    public class TaskData : ITaskData, IMethodData
    {
        /// <summary>
        /// 函数参数
        /// </summary>
        public object[] MethodArgs { get { return _methodArgs; } }

        /// <summary>
        /// 函数目标对象
        /// </summary>
        public object MethodTarget { get { return _methodTarget; } }

        /// <summary>
        /// 函数信息
        /// </summary>
        public MethodInfo MethodInfo { get { return _methodInfo; } }

        /// <summary>
        /// 结果回调
        /// </summary>
        public Action<object> ResultCallback { get { return _resultCallback; } }

        #region private member

        protected object[] _methodArgs;

        protected object _methodTarget;

        protected MethodInfo _methodInfo;

        protected Action<object> _resultCallback;

        #endregion private member

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="methodTarget">函数目标对象</param>
        /// <param name="methodInfo">函数信息</param>
        /// <param name="resultCallback">结果回调函数</param>
        /// <param name="methodArgs">函数参数</param>
        public TaskData(object methodTarget, MethodInfo methodInfo, Action<object> resultCallback, params object[] methodArgs)
        {
            Checker.IsFalse(null == methodTarget && !methodInfo.IsStatic, "非静态函数，Target不能为null");

            _methodArgs = methodArgs;
            _methodTarget = methodTarget;
            _methodInfo = methodInfo;
            _resultCallback = resultCallback;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public void Call()
        {
            object result;
            try
            {
                result = _methodInfo.Invoke(_methodTarget, _methodArgs);
                if (null != _resultCallback)
                {
                    _resultCallback(result);
                }
            }
            catch (Exception ex)
            {
                throw new StringException(ex, "事件调用异常 类：{0} 函数：{1}", _methodTarget, _methodInfo);
            }
        }
    }
}