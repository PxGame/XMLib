/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/4/2019 11:21:41 AM
 */

using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 物理检测设置
    /// </summary>
    [Serializable]
    public class RaycastSetting
    {
        /// <summary>
        /// 最小射线检测距离
        /// </summary>
        [Tooltip("最小射线检测距离")]
        public float rayMinDistance = 0.2f;

        /// <summary>
        /// 物理检测缓冲区最大数量
        /// </summary>
        [Tooltip("物理检测缓冲区最大数量")]
        public int rayBufferCount = 10;

        /// <summary>
        /// 射线间距
        /// </summary>
        [Tooltip("射线间距")]
        public float raySpacing = 0.25f;

        /// <summary>
        /// 皮肤宽度
        /// </summary>
        [Tooltip("皮肤宽度")]
        public float skinWidth = 0.03f;

        /// <summary>
        /// 射线设置
        /// </summary>
        [Tooltip("射线设置")]
        public ContactFilter2D contactFilter2D;
    }
}