using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM
{
    /// <summary>
    /// 值带编号的列表
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public class IndexList<T> : List<IndexValue<T>> where T : class
    {
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="index">编号</param>
        /// <param name="value">值</param>
        public void Add(int index, T value)
        {
            IndexValue<T> item = new IndexValue<T>(index, value);
            Add(item);
        }

        /// <summary>
        /// 移除所有该值引用
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool RemoveAll(T value)
        {
            return RemoveAll(t => t.Value == value) > 0;
        }
    }
}