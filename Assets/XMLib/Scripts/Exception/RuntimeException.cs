/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/11/2018 4:41:38 PM
 */

using System;

namespace XMLib
{
    /// <summary>
    /// 运行时异常
    /// </summary>
    public class RuntimeException : Exception
    {
        public RuntimeException()
            : base()
        {
        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public RuntimeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public RuntimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}