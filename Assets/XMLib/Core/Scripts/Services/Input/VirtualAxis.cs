using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 轴
    /// </summary>
    public class VirtualAxis
    {
        protected string _name;
        protected float _value;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// 值
        /// </summary>
        public float Value { get { return _value; } }

        public float RawValue { get { return _value == 0 ? 0 : _value > 0 ? 1 : -1; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">轴名</param>
        public VirtualAxis(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 更新值
        /// </summary>
        /// <param name="value"></param>
        public void Update(float value)
        {
            _value = value;
        }
    }
}