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
        public RuntimeException(string format, params object[] args)
            : base(FixedMessage(format, args))
        {
        }

        public RuntimeException(Exception innerException, string format, params object[] args)
            : base(FixedMessage(format, args), innerException)
        {
        }

        protected static string FixedMessage(string format, params object[] args)
        {
            return string.Format(
#if UNITY_EDITOR
            "<color=red>[XMLib]</color>"
#else
            "[XMLib]"
#endif
             + format, args);
        }
    }
}