/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 4:41:13 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 优先级
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class PriorityAttribute : Attribute
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priorities { get; private set; }

        /// <summary>
        /// 优先级(0最高)
        /// </summary>
        /// <param name="priority">优先级(0为默认级别)，值越小，优先级越高</param>
        public PriorityAttribute(int priority = 0)
        {
            Priorities = priority;
        }
    }
}