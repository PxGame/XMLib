using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM
{
    /// <summary>
    /// 检查者
    /// </summary>
    public static class Checker
    {
        /// <summary>
        /// 不为空
        /// </summary>
        /// <param name="obj">检查对象</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void NotNull(object obj, string format, params object[] args)
        {
            if (null != obj)
            {
                return;
            }

            string msg = string.Format(format, args);
            Debug.LogError(msg);
        }
    }
}