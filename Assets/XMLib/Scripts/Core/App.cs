/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 2:49:00 PM
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 应用程序
    /// </summary>
    public abstract class App
    {
        #region 基础

        /// <summary>
        /// 当新建Application时
        /// </summary>
        public static event Action<IApplication> OnNewApplication;

        /// <summary>
        /// 实例
        /// </summary>
        private static IApplication _instance;

        /// <summary>
        ///实例
        /// </summary>
        public static IApplication Handler
        {
            get
            {
                if (_instance == null)
                {
                    return New();
                }
                return _instance;
            }
            set
            {
                _instance = value;
                if (OnNewApplication != null)
                {
                    OnNewApplication.Invoke(_instance);
                }
            }
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns>实例</returns>
        private static IApplication New()
        {
            return Application.New();
        }

        #endregion 基础

        #region Application API

        /// <summary>
        /// 终止程序
        /// </summary>
        public static void Terminate()
        {
            Handler.Terminate();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="serviceProvider">服务实例</param>
        public static void Register(IServiceProvider serviceProvider)
        {
            Handler.Register(serviceProvider);
        }

        /// <summary>
        /// 服务提供者是否已经注册过
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>服务提供者是否已经注册过</returns>
        public static bool IsRegisted(IServiceProvider serviceProvider)
        {
            return Handler.IsRegisted(serviceProvider);
        }

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public static bool IsMainThread
        {
            get
            {
                return Handler.IsMainThread;
            }
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="method">函数</param>
        /// <returns>优先级</returns>
        public static int GetPriority(Type type, string method = null)
        {
            return Handler.GetPriority(type, method);
        }

        #endregion Application API

        #region Dispatcher API

        /// <summary>
        /// 调用函数,函数无参时将清空输入参数以完成调用
        /// </summary>
        /// <param name="target">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="userParams">用户传入的参数</param>
        /// <returns>方法返回值</returns>
        public static object Call(object target, MethodInfo methodInfo, params object[] args)
        {
            return Handler.Call(target, methodInfo, args);
        }

        /// <summary>
        /// 是否存在事件的监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在</returns>
        public static bool HasListener(string eventName)
        {
            return Handler.HasListener(eventName);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果集合</returns>
        public static List<object> Trigger(string eventName, params object[] args)
        {
            return Handler.Trigger(eventName, args);
        }

        /// <summary>
        /// 触发一个事件，遇到第一个事件存在处理结果后终止，并获取事件监听的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public static object TriggerHalt(string eventName, params object[] args)
        {
            return Handler.TriggerHalt(eventName, args);
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
        public static void Off(object target)
        {
            Handler.Off(target);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="target">调用目标</param>
        /// <param name="method">处理函数名</param>
        /// <returns>对象</returns>
        public static IEvent On(string eventName, object target, string method = null)
        {
            return Handler.On(eventName, target, method);
        }

        #region Action

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On(string eventName, Action method, object group = null)
        {
            return Handler.On(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1>(string eventName, Action<T1> method, object group = null)
        {
            return Handler.On(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2>(string eventName, Action<T1, T2> method, object group = null)
        {
            return Handler.On(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2, T3>(string eventName, Action<T1, T2, T3> method, object group = null)
        {
            return Handler.On(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> method, object group = null)
        {
            return Handler.On(eventName, method, group);
        }

        #endregion Action

        #region Func

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<TResult>(string eventName, Func<TResult> method, object group = null)
        {
            return Handler.Listen(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, TResult>(string eventName, Func<T1, TResult> method, object group = null)
        {
            return Handler.Listen(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, TResult>(string eventName, Func<T1, T2, TResult> method, object group = null)
        {
            return Handler.Listen(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, T3, TResult>(string eventName, Func<T1, T2, T3, TResult> method, object group = null)
        {
            return Handler.Listen(eventName, method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, T3, T4, TResult>(string eventName, Func<T1, T2, T3, T4, TResult> method, object group = null)
        {
            return Handler.Listen(eventName, method, group);
        }

        #endregion Func

        #endregion Dispatcher API

        #region Debug

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void Log(string format, params object[] args)
        {
            Debug.Log(Format(Color.black, format, args));
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void LogWaring(string format, params object[] args)
        {
            Debug.LogWarning(Format(Color.yellow, format, args));
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void LogError(string format, params object[] args)
        {
            Debug.LogError(Format(Color.red, format, args));
        }

        private static string Format(Color color, string format, params object[] args)
        {
            string msg = string.Format(format, args);
            string output = string.Format("[<color=#{0}><b>XBLib</b></color>]{1}", ColorUtility.ToHtmlStringRGBA(color), msg);
            return output;
        }

        #endregion Debug
    }
}