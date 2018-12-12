/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 2:38:20 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib
{
    /// <summary>
    /// 数组工具
    /// </summary>
    public static class ArrayUtil
    {
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="sources">数组</param>
        /// <returns>合并后的数组</returns>
        public static T[] Merge<T>(params T[][] sources)
        {
            Checker.Requires<ArgumentNullException>(sources != null);

            int length = 0;
            foreach (var source in sources)
            {
                if (null == source || source.Length <= 0)
                {
                    continue;
                }

                length += source.Length;
            }

            if (0 >= length)
            {
                return new T[0];
            }

            T[] merge = new T[length];
            int current = 0;
            foreach (var source in sources)
            {
                if (null == source || source.Length <= 0)
                {
                    continue;
                }

                Array.Copy(source, 0, merge, current, source.Length);
                current += source.Length;
            }

            return merge;
        }
    }
}