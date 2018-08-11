using System;
using System.Collections;
using System.Collections.Generic;
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

        public StringException(string format, params object[] args)
        {
            _message = string.Format("[XM]" + format, args);
            Debug.LogError(_message);
        }

        public StringException(Exception innerExcepation, string format, params object[] args) : base(null, innerExcepation)
        {
            _message = string.Format("[XM]" + format, args);
            Debug.LogError(_message);
        }
    }
}