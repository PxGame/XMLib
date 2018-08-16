using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 输入方式
    /// </summary>
    public enum ActiveInputMethod
    {
        /// <summary>
        /// 无输入
        /// </summary>
        None,

        /// <summary>
        /// Unity输入
        /// </summary>
        UInput,

        /// <summary>
        /// 移动输入
        /// </summary>
        Mobile
    }
}