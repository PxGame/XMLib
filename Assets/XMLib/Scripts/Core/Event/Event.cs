/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 5:10:22 PM
 */

using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 事件对象
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// 事件名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 事件分组
        /// </summary>
        public object Group { get; private set; }

        /// <summary>
        /// 目标对象
        /// </summary>
        private readonly object _target;

        /// <summary>
        /// 调用函数
        /// </summary>
        private readonly MethodInfo _methodInfo;

        /// <summary>
        /// 调用应用
        /// </summary>
        private readonly IApplication _app;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app">调用应用</param>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">目标函数</param>
        /// <param name="group">分组</param>
        public Event(IApplication app, string eventName, object target, MethodInfo methodInfo, object group)
        {
            Name = eventName;
            Group = group;

            _app = app;
            _target = target;
            _methodInfo = methodInfo;
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        public object Call(params object[] args)
        {
            return _app.Call(_target, _methodInfo, args);
        }

        /// <summary>
        /// 事件是否可用
        /// </summary>
        /// <returns>是否可用</returns>
        public virtual bool IsActiveAndEnabled()
        {
            return true;
        }

        /// <summary>
        /// 是否有效
        /// <para>无效时,会被移除监听列表</para>
        /// </summary>
        /// <returns>是否有效</returns>
        public virtual bool IsValid()
        {
            return true;
        }
    }
}