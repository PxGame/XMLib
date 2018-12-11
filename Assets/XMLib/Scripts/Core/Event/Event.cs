/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 5:10:22 PM
 */

using System;

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
        /// 调用方法
        /// </summary>
        private readonly Func<string, object[], object> _func;

        public Event(string eventName, object group, Func<string, object[], object> func)
        {
            Name = eventName;
            Group = group;
            _func = func;
        }

        /// <summary>
        /// 事件调用
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Call(string eventName, params object[] args)
        {
            try
            {
                return _func(eventName, args);
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("调用 {0} 事件异常", eventName);
                throw new RuntimeException(errMsg, ex);
            }
        }
    }
}