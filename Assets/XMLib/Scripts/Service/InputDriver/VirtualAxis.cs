/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:38:18 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 虚拟轴
    /// </summary>
    internal class VirtualAxis
    {
        /// <summary>
        /// 轴名
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// 值
        /// </summary>
        public float Value { get { return _value; } }

        /// <summary>
        /// 轴名
        /// </summary>
        private string _name;

        /// <summary>
        /// 值
        /// </summary>
        private float _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名字</param>
        public VirtualAxis(string name)
        {
            _name = name;
            _value = 0;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value">值</param>
        public void SetAxis(float value)
        {
            _value = value;
        }
    }
}