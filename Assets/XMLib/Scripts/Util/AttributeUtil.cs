/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 4:42:46 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 属性工具
    /// </summary>
    public static class AttributeUtil
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">   类型 </param>
        /// <param name="method"> 函数 </param>
        /// <returns> 优先级 </returns>
        public static int GetPriority(Type type, string method = null)
        {
            Checker.Requires<ArgumentNullException>(type != null);
            Type priority = typeof(PriorityAttribute);
            int currentPriority = int.MaxValue;

            MethodInfo methodInfo;
            if (method != null &&
                (methodInfo = type.GetMethod(method)) != null &&
                methodInfo.IsDefined(priority, false))
            {//获取函数
                currentPriority = ((PriorityAttribute)methodInfo.GetCustomAttributes(priority, false)[0]).Priorities;
            }
            else if (type.IsDefined(priority, false))
            {//获取类
                currentPriority = ((PriorityAttribute)type.GetCustomAttributes(priority, false)[0]).Priorities;
            }

            return currentPriority;
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <typeparam name="T"> 类型 </typeparam>
        /// <param name="method"> 函数 </param>
        /// <returns> 优先级 </returns>
        public static int GetPriority<T>(string method = null)
        {
            return GetPriority(typeof(T), method);
        }
    }
}