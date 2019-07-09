/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:38:18 AM
 */

using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 虚拟轴
    /// </summary>
    public class VirtualAxis
    {
        /// <summary>
        /// 轴名
        /// </summary>
        public string name { get { return _name; } }

        /// <summary>
        /// 值
        /// </summary>
        public float value { get { return Time.frameCount > _lastUpdateFrame ? _value : _lastValue; } }

        /// <summary>
        /// 轴名
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// 值
        /// </summary>
        private float _value;

        private float _lastValue;
        private int _lastUpdateFrame;

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
            _lastValue = _value;
            _lastUpdateFrame = Time.frameCount;

            _value = value;
        }
    }
}