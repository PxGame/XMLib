/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2/20/2019 12:33:09 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 移动平台接口
    /// </summary>
    public interface IMovePlatform
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        /// <value></value>
        Vector2 velocity { get; }
    }
}