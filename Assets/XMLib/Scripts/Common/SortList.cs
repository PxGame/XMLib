/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 5:09:54 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 有序List
    /// </summary>
    /// <typeparam name="TValue"> 值类型 </typeparam>
    /// <typeparam name="TWeight"> 权重 </typeparam>
    public sealed class SortList<TValue, TWeight> : IEnumerable<TValue>
        where TWeight : IComparable<TWeight>
    {
        #region 定义

        /// <summary>
        /// 成员
        /// </summary>
        private class ValueWeightPair
        {
            /// <summary>
            /// 值
            /// </summary>
            public TValue Value;

            /// <summary>
            /// 权重
            /// </summary>
            public TWeight Weight;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="value">  值 </param>
            /// <param name="weight"> 权重 </param>
            public ValueWeightPair(TValue value, TWeight weight)
            {
                this.Value = value;
                this.Weight = weight;
            }

            public ValueWeightPair()
            {
            }
        }

        /// <summary>
        /// 迭代
        /// </summary>
        public class Enumerator : IEnumerator<TValue>
        {
            private readonly SortList<TValue, TWeight> _sortlist;
            private readonly List<ValueWeightPair> _list;
            private readonly bool _forward;
            private int _index;

            public Enumerator(SortList<TValue, TWeight> sortlist, bool forward)
            {
                _sortlist = sortlist;
                _list = _sortlist._list;
                _forward = forward;
                _index = _forward ? -1 : _list.Count;
            }

            public TValue Current
            {
                get
                {
                    try
                    {
                        ValueWeightPair pair = _list[_index];
                        return pair.Value;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_forward)
                {
                    _index++;
                    return _index < _list.Count;
                }
                else
                {
                    _index--;
                    return _index > 0;
                }
            }

            public void Reset()
            {
                _index = _forward ? -1 : _list.Count;
            }
        }

        #endregion 定义

        /// <summary>
        /// 迭代器迭代方向
        /// </summary>
        public bool Forward { get { return _forward; } set { _forward = value; } }

        /// <summary>
        /// 比较方法
        /// </summary>
        public Comparison<TWeight> ComparerWeight
        {
            get { return _comparerWeight; }
            set { _comparerWeight = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return _list.Count; } }

        /// <summary>
        /// 同步锁
        /// </summary>
        public object SyncRoot { get { return _syncRoot; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity"> 默认容器大小 </param>
        public SortList(int capacity = 10)
        {
            _list = new List<ValueWeightPair>(capacity);
            _dict = new Dictionary<TValue, ValueWeightPair>(capacity);
            _forward = true;
            _sorted = false;
            _syncRoot = new object();
            _comparerWeight = OnComparerWeight;
        }

        /// <summary>
        /// 获取权重
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 权重 </returns>
        public TWeight this[TValue value]
        {
            get
            {
                return GetWeight(value);
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"> 序号 </param>
        /// <returns> 值 </returns>
        public TValue this[int index]
        {
            get
            {
                return GetValue(index);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="value">  值 </param>
        /// <param name="weight"> 权重 </param>
        public void Add(TValue value, TWeight weight)
        {
            Checker.Requires<ArgumentNullException>(value != null);
            Checker.Requires<ArgumentNullException>(weight != null);

            //移除已存在的
            RemoveElement(value);
            //添加新的
            AddElement(value, weight);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 是否成功 </returns>
        public bool Remove(TValue value)
        {
            Checker.Requires<ArgumentNullException>(value != null);

            //移除已存在的
            return RemoveElement(value);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="index"> 序号 </param>
        public void RemoveIndex(int index)
        {
            Checker.Requires<ArgumentOutOfRangeException>(index >= 0);
            Checker.Requires<ArgumentOutOfRangeException>(index < _list.Count);

            //检查排序
            Sort();

            //
            ValueWeightPair pair = _list[index];
            _dict.Remove(pair.Value);
            _list.RemoveAt(index);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _dict.Clear();
            _list.Clear();
            _sorted = false;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
        {
            Checker.Requires<InvalidOperationException>(_comparerWeight != null);

            if (_sorted)
            {
                return;
            }

            _list.Sort(OnComparerPair);
            _sorted = true;
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 是否包含 </returns>
        public bool Contains(TValue value)
        {
            Checker.Requires<ArgumentNullException>(value != null);
            return _dict.ContainsKey(value);
        }

        /// <summary>
        /// 获取权重
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 权重 </returns>
        public TWeight GetWeight(TValue value)
        {
            Checker.Requires<ArgumentNullException>(value != null);
            ValueWeightPair pair;
            if (!_dict.TryGetValue(value, out pair))
            {
                throw new KeyNotFoundException();
            }
            return pair.Weight;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"> 序号 </param>
        /// <returns> 值 </returns>
        public TValue GetValue(int index)
        {
            Checker.Requires<ArgumentOutOfRangeException>(index >= 0);
            Checker.Requires<ArgumentOutOfRangeException>(index < _list.Count);

            //检查排序
            Sort();

            //
            ValueWeightPair pair = _list[index];
            return pair.Value;
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 从 0 开始，未找到为-1 </returns>
        public int GetIndex(TValue value)
        {
            Checker.Requires<ArgumentNullException>(value != null);

            //检查排序
            Sort();

            //
            ValueWeightPair pair;
            if (!_dict.TryGetValue(value, out pair))
            {
                throw new KeyNotFoundException();
            }

            return _list.IndexOf(pair);
        }

        #region 私有

        private readonly List<ValueWeightPair> _list;
        private readonly Dictionary<TValue, ValueWeightPair> _dict;

        private Comparison<TWeight> _comparerWeight;
        private bool _forward;
        private bool _sorted;
        private object _syncRoot;

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="x">  </param>
        /// <param name="y">  </param>
        /// <returns>  </returns>
        private int OnComparerPair(ValueWeightPair x, ValueWeightPair y)
        {
            return _comparerWeight(x.Weight, y.Weight);
        }

        /// <summary>
        /// 默认比较权重
        /// </summary>
        /// <param name="x">  </param>
        /// <param name="y">  </param>
        /// <returns>  </returns>
        private int OnComparerWeight(TWeight x, TWeight y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="value">  值 </param>
        /// <param name="weight"> 权重 </param>
        private void AddElement(TValue value, TWeight weight)
        {
            //添加新的
            ValueWeightPair item = new ValueWeightPair(value, weight);
            _dict.Add(value, item);
            _list.Add(item);

            _sorted = false;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="value"> 值 </param>
        /// <returns> 是否成功 </returns>
        private bool RemoveElement(TValue value)
        {
            bool isRemoved = false;
            ValueWeightPair item;
            if (_dict.TryGetValue(value, out item))
            {
                _dict.Remove(value);
                _list.Remove(item);
                _sorted = false;
                isRemoved = true;
            }
            return isRemoved;
        }

        #endregion 私有

        #region 迭代器

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns> 迭代器 </returns>
        public Enumerator GetEnumerator()
        {
            //检查排序
            Sort();

            return new Enumerator(this, _forward);
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion 迭代器
    }
}