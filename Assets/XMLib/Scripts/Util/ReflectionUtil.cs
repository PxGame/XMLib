/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 4:42:46 PM
 */

using System;
using System.Reflection;

namespace XMLib
{
    /// <summary>
    /// 反射工具
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="method">函数</param>
        /// <returns>优先级</returns>
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
        /// <typeparam name="T">类型</typeparam>
        /// <param name="method">函数</param>
        /// <returns>优先级</returns>
        public static int GetPriority<T>(string method = null)
        {
            return GetPriority(typeof(T), method);
        }

        /// <summary>
        /// 获取函数优先级
        /// </summary>
        /// <param name="method">函数</param>
        /// <returns>优先级</returns>
        public static int GetPriority(MethodInfo method)
        {
            Type priority = typeof(PriorityAttribute);
            int currentPriority = int.MaxValue;

            if (method.IsDefined(priority, false))
            {
                currentPriority = ((PriorityAttribute)method.GetCustomAttributes(priority, false)[0]).Priorities;
            }

            return currentPriority;
        }

        /// <summary>
        /// 调用函数,未找到函数时不会调用
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="methodName">函数名</param>
        /// <param name="result">返回值</param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        public static bool InvokeMethod(object target, string methodName, out object result, params object[] args)
        {
            Type[] argTypes = new Type[args.Length];
            int length = argTypes.Length;
            for (int i = 0; i < length; i++)
            {
                argTypes[i] = args[i].GetType();
            }

            MethodInfo method = target.GetType().GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                argTypes,
                null);

            if (null == method)
            {//未找到函数
                result = null;
                return false;
            }

            //调用
            result = method.Invoke(target, args);

            return true;
        }

        /// <summary>
        /// 调用函数,未找到函数时不会调用
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="methodName">函数名</param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        public static bool InvokeMethod(object target, string methodName, params object[] args)
        {
            object result;
            return InvokeMethod(target, methodName, out result, args);
        }
    }
}