/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 2:49:51 PM
 */

using System;
using System.Collections.Generic;

namespace XMLib
{
    /// <summary>
    /// 检查者
    /// </summary>
    public static class Checker
    {
        /// <summary>
        /// 验证一个条件,并在该协定的条件失败时引发异常。
        /// </summary>
        /// <typeparam name="TException">异常</typeparam>
        /// <param name="condition">条件</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void Requires<TException>(bool condition) where TException : Exception, new()
        {
            if (condition)
            {
                return;
            }
            throw new TException();
        }

        /// <summary>
        /// 不为空或者null
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void NotEmptyOrNull(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 长度大于0
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void CountGreaterZero<T>(IList<T> argumentValue, string argumentName)
        {
            if (argumentValue.Count <= 0)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 元素部位空或者null
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void ElementNotEmptyOrNull(IList<string> argumentValue, string argumentName)
        {
            foreach (var val in argumentValue)
            {
                if (string.IsNullOrEmpty(val))
                {
                    throw new ArgumentNullException(argumentName, "Argument element can not be Empty or Null.");
                }
            }
        }

        /// <summary>
        /// 内容不为空
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void NotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}