/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 9:26:10 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// Dispatcher 扩展类
    /// </summary>
    public static class DispatcherExt
    {
        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="target">调用目标</param>
        /// <param name="methodInfo">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On(this IDispatcher dispatcher, string eventName, object target, MethodInfo methodInfo, object group = null)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");
            Checker.Requires<ArgumentNullException>(methodInfo != null);

            if (!methodInfo.IsStatic)
            {//非静态函数,必须有实例
                Checker.Requires<ArgumentNullException>(target != null);
            }

            return dispatcher.On(eventName, (_, args) => methodInfo.Invoke(target, args), group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="target">调用目标</param>
        /// <param name="method">处理函数名</param>
        /// <returns>对象</returns>
        public static IEvent On(this IDispatcher dispatcher, string eventName, object target, string method = null)
        {
            Checker.NotEmptyOrNull(eventName, "eventName");
            Checker.Requires<ArgumentException>(method != string.Empty);
            Checker.Requires<ArgumentNullException>(target != null);

            if (null == method)
            {//事件名为函数名
                method = eventName;
            }

            MethodInfo methodInfo = target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Checker.Requires<ArgumentNullException>(methodInfo != null);

            return dispatcher.On(eventName, target, methodInfo, target);
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
        public static IEvent On(this IDispatcher dispatcher, string eventName, Action method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1>(this IDispatcher dispatcher, string eventName, Action<T1> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2>(this IDispatcher dispatcher, string eventName, Action<T1, T2> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2, T3>(this IDispatcher dispatcher, string eventName, Action<T1, T2, T3> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent On<T1, T2, T3, T4>(this IDispatcher dispatcher, string eventName, Action<T1, T2, T3, T4> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
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
        public static IEvent Listen<TResult>(this IDispatcher dispatcher, string eventName, Func<TResult> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, TResult>(this IDispatcher dispatcher, string eventName, Func<T1, TResult> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, TResult>(this IDispatcher dispatcher, string eventName, Func<T1, T2, TResult> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, T3, TResult>(this IDispatcher dispatcher, string eventName, Func<T1, T2, T3, TResult> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">名</param>
        /// <param name="method">处理函数</param>
        /// <param name="group">分组</param>
        /// <returns>对象</returns>
        public static IEvent Listen<T1, T2, T3, T4, TResult>(this IDispatcher dispatcher, string eventName, Func<T1, T2, T3, T4, TResult> method, object group = null)
        {
            Checker.Requires<ArgumentNullException>(method != null);

            return dispatcher.On(eventName, method.Target, method.Method, group);
        }

        #endregion Func
    }
}