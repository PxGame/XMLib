using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM
{
    /// <summary>
    /// 带序号的值
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public class IndexValue<T> : IComparable<IndexValue<T>>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }

        public IndexValue()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="value">值</param>
        public IndexValue(int index, T value)
        {
            Index = index;
            Value = value;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其他对象</param>
        /// <returns></returns>
        public int CompareTo(IndexValue<T> other)
        {
            return Index.CompareTo(other.Index);
        }
    }
}