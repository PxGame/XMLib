using System;
using UnityEngine;

namespace XM.Exceptions
{
    /// <summary>
    /// 异常
    /// </summary>
    public class StringException : Exception
    {
        public override string Message
        {
            get
            {
                return _message;
            }
        }

        protected string _message = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public StringException(string format, params object[] args)
        {
            _message = string.Format("[XM]" + format, args);
            Debug.LogError(_message);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="innerExcepation">子异常</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public StringException(Exception innerExcepation, string format, params object[] args) : base(null, innerExcepation)
        {
            _message = string.Format("[XM]" + format, args);
            Debug.LogError(_message);
        }
    }
}