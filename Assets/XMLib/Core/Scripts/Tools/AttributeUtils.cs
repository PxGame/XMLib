using System;
using System.Reflection;

namespace XM
{
    /// <summary>
    /// 特性工具
    /// </summary>
    public static class AttributeUtils
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="method">方法</param>
        /// <returns>优先级</returns>
        public static int GetPriority(Type type, string method = null)
        {
            Type priority = typeof(PriorityAttribute);
            int currentPriority = GlobalSetting.Dafault.Priority;

            MethodInfo methodInfo;
            if (method != null &&
                (methodInfo = type.GetMethod(method)) != null &&
                methodInfo.IsDefined(priority, false))
            {
                currentPriority = ((PriorityAttribute)methodInfo.GetCustomAttributes(priority, false)[0]).Priorities;
            }
            else if (type.IsDefined(priority, false))
            {
                currentPriority = ((PriorityAttribute)type.GetCustomAttributes(priority, false)[0]).Priorities;
            }

            return currentPriority;
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>优先级</returns>
        public static int GetPriority<T>(string method = null)
        {
            return GetPriority(typeof(T), method);
        }
    }
}