/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/13/2019 3:01:36 PM
 */

using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 刚体控制器设置
    /// </summary>
    [Serializable]
    public class RigidbodySetting
    {
        /// <summary>
        /// 重力缩放
        /// </summary>
        [Tooltip("重力缩放")]
        public float gravityScale = 1f;

        /// <summary>
        /// 移动平台Tag
        /// </summary>
        [Tooltip("移动平台Tag")]
        public string movePlatformTag = "MovePlatform";
    }
}