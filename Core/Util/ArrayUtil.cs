/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 2:38:20 PM
 */

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

        /// <summary>
        /// 从原数组中分割指定长度的元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="source">数组</param>
        /// <param name="start">起点下表</param>
        /// <param name="length">长度,向前为正，向后为负</param>
        /// <returns>分割出来的数组</returns>
        public static T[] Splice<T>(ref T[] source, int start, int length)
        {
            Checker.Requires<ArgumentNullException>(null != source);

            int maxLength = source.Length;

            if (length < 0)
            {//矫正下标与长度
                length *= -1;
                start = start - length + 1;
            }

            Checker.Requires<IndexOutOfRangeException>(0 <= start && (start + length) <= maxLength);

            int newMaxLength = maxLength - length;
            T[] newSource = new T[newMaxLength];
            T[] spliceSource = new T[length];

            //
            int index = 0;
            int newSourceIndex = 0;

            //拷贝前半部分
            if (0 < start)
            {
                Array.Copy(source, 0, newSource, 0, start);
                index += start;
                newSourceIndex += start;
            }

            //拷贝中间部分
            Array.Copy(source, index, spliceSource, 0, length);
            index += length;

            //拷贝后半部分
            int surplus = maxLength - index;
            if (surplus > 0)
            {
                Array.Copy(source, index, newSource, newSourceIndex, surplus);
            }

            //传值
            source = newSource;
            return spliceSource;
        }

        /// <summary>
        /// 移除指定下标的元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="source">数组</param>
        /// <param name="index">下标</param>
        /// <returns>移除的元素</returns>
        public static T RemoveAt<T>(ref T[] source, int index)
        {
            Checker.Requires<ArgumentNullException>(null != source);
            Checker.Requires<IndexOutOfRangeException>(0 <= index || index < source.Length);

            T[] result = Splice(ref source, index, 1);
            return 0 < result.Length ? result[0] : default(T);
        }
    }
}