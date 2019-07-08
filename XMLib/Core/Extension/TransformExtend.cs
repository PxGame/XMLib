/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 3:33:48 PM
 */

using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Transform 扩展
    /// </summary>
    public static class TransformExtend
    {
        /// <summary>
        /// 遍历每个子节点
        /// </summary>
        /// <param name="transform">实例</param>
        /// <param name="callback">返回子节点</param>
        public static void ForeachChild(this Transform transform, Action<Transform> callback)
        {
            foreach (Transform child in transform)
            {
                callback(child);

                child.ForeachChild(callback);
            }
        }

        /// <summary>
        /// 遍历每个子节点及自身
        /// </summary>
        /// <param name="transform">实例</param>
        /// <param name="callback">返回子节点和自身</param>
        public static void ForeachChildWithSelf(this Transform transform, Action<Transform> callback)
        {
            callback(transform);

            foreach (Transform child in transform)
            {
                child.ForeachChild(callback);
            }
        }
    }
}