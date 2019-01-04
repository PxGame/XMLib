/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/4/2019 1:57:18 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 射线检查项
    /// </summary>
    public class CheckItem
    {
        /// <summary>
        /// 起点
        /// </summary>
        public Vector2 origin;

        /// <summary>
        /// 长度
        /// </summary>
        public float length;

        /// <summary>
        /// 方向
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckItem()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="length">长度</param>
        /// <param name="direction">方向</param>
        public CheckItem(Vector2 origin, float length, Vector2 direction)
        {
            this.origin = origin;
            this.length = length;
            this.direction = direction;
        }
    }
}