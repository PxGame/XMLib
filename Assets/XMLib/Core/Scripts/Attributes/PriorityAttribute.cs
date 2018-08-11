using System;

namespace XM.Attributes
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
        /// 优先级
        /// </summary>
        /// <param name="priority">优先级</param>
        public PriorityAttribute(int priority = 0)
        {
            Priorities = priority;
        }
    }
}