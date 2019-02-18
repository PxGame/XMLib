/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2/18/2019 8:03:46 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.P2D
{
    /// <summary>
    /// 玩家设置
    /// </summary>

    [Serializable]
    public class PlayerSetting
    {
        /// <summary>
        /// 速度
        /// </summary>
        [Tooltip("速度")]
        public float speed = 3f;

        /// <summary>
        /// 跳跃高度
        /// </summary>
        [Tooltip("跳跃高度")]
        public Vector2 jumpHeightRange = new Vector2(1f, 3f);
    }
}