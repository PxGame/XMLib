/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/4/2019 11:21:41 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 物理设置
    /// </summary>
    [Serializable]
    public class Physics2DSetting
    {
        /// <summary>
        /// 最小射线检测距离
        /// </summary>
        public float raycastLineMinLength = 0.2f;

        /// <summary>
        /// 物理检测缓冲区最大数量
        /// </summary>
        [Tooltip("物理检测缓冲区最大数量")]
        public int raycastBufferCount = 10;

        /// <summary>
        /// 横向射线数量
        /// </summary>
        [Tooltip("横向射线数量")]
        public int horizontalRayCount = 5;

        /// <summary>
        /// 纵向射线数量
        /// </summary>
        [Tooltip("纵向射线数量")]
        public int verticalRayCount = 5;

        /// <summary>
        /// 射线设置
        /// </summary>
        [Tooltip("射线设置")]
        public ContactFilter2D contactFilter2D;
    }
}