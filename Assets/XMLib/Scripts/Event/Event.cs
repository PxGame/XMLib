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
        public string name { get; private set; }

        /// <summary>
        /// 事件分组
        /// </summary>
        public object group { get; private set; }

        /// <summary>
        /// 目标对象
        /// </summary>
        public object target { get; private set; }

        /// <summary>
        /// 调用函数
        /// </summary>
        public MethodInfo methodInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">目标函数</param>
        /// <param name="group">分组</param>
        public Event(string eventName, object target, MethodInfo methodInfo, object group)
        {
            name = eventName;
            this.group = group;
            this.target = target;
            this.methodInfo = methodInfo;
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

        /// <summary>
        /// 转换到字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string extra = "";
            if (null != methodInfo)
            {
                extra = string.Format("(Info:{0},DeclaringType:{1}", methodInfo, methodInfo.DeclaringType);
            }

            string str = string.Format("[{0}](Name:{1},Group:{2},Target:{3},MethodInfo:{4})", GetType().Name, name, group, target, extra);
            return str;
        }
    }
}