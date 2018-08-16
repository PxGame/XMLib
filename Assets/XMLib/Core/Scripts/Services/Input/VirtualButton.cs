using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class VirtualButton
    {
        protected string _name;
        protected bool _isPressed = false;
        protected int _downFrame = 0;
        protected int _upFrame = 0;

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// 是否按住
        /// </summary>
        public bool IsPressed { get { return _isPressed; } }

        /// <summary>
        /// 当前帧是否按下
        /// </summary>
        public bool IsDown { get { return _downFrame + 1 == Time.frameCount; } }

        /// <summary>
        /// 当前帧是否抬起
        /// </summary>
        public bool IsUp { get { return _upFrame + 1 == Time.frameCount; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">按钮名</param>
        public VirtualButton(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">按下/抬起</param>
        public void Update(bool status)
        {
            if (status)
            {//按下
                if (_isPressed)
                {
                    return;
                }

                _isPressed = true;
                _downFrame = Time.frameCount;
            }
            else
            {//抬起
                _isPressed = false;
                _upFrame = Time.frameCount;
            }
        }
    }
}