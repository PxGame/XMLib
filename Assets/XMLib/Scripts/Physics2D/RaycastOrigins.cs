/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/11/2019 4:53:31 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 射线检测起点
    /// </summary>
    [Serializable]
    public class RaycastOrigins
    {
        /// <summary>
        /// 上左
        /// </summary>
        public Vector2 topLeft;

        /// <summary>
        /// 上右
        /// </summary>
        public Vector2 topRight;

        /// <summary>
        /// 下左
        /// </summary>
        public Vector2 bottomLeft;

        /// <summary>
        /// 下右
        /// </summary>
        public Vector2 bottomRight;
    }
}