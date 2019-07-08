/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/14/2018 5:43:46 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 未能解决异常
    /// </summary>
    public class UnresolvableException : RuntimeException
    {
        /// <summary>
        /// 未能解决异常
        /// </summary>
        public UnresolvableException()
            : base()
        {
        }

        /// <summary>
        /// 未能解决异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public UnresolvableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 未能解决异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public UnresolvableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}