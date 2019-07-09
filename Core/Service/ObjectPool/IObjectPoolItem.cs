/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/17/2019 12:18:57 PM
 */

using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 对象池元素
    /// </summary>
    public interface IObjectPoolItem
    {
        /// <summary>
        /// 类型名
        /// </summary>
        string typeName { get; }

        /// <summary>
        /// 对象名
        /// </summary>
        string resName { get; }

        /// <summary>
        /// 对象
        /// </summary>
        GameObject gameObject { get; }
    }
}