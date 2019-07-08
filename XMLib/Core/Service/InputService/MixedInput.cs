/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/3/2019 12:03:43 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputService
{
    /// <summary>
    /// 混合输入
    /// </summary>
    public class MixedInput : VirtualInput
    {
        /// <summary>
        /// 标准输入
        /// </summary>
        private StandAloneInput _standAlone;

        /// <summary>
        /// 移动输入
        /// </summary>
        private MobileInput _mobile;

        /// <summary>
        /// 死区
        /// </summary>
        private readonly float _deadZoom;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deadZoom">轴死区</param>
        public MixedInput(float deadZoom = 0)
        {
            _deadZoom = deadZoom;

            _standAlone = new StandAloneInput();
            _mobile = new MobileInput(_deadZoom);
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxis(string name)
        {
            float value = _mobile.GetAxis(name);
            if (0 == value)
            {
                value = _standAlone.GetAxis(name);
            }
            return value;
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxisRaw(string name)
        {
            float value = _mobile.GetAxisRaw(name);
            if (0 == value)
            {
                value = _standAlone.GetAxisRaw(name);
            }
            return value;
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public override bool GetButton(string name)
        {
            bool value = _mobile.GetButton(name);
            if (!value && !_mobile.WillButton(name))
            {
                value = _standAlone.GetButton(name);
            }
            return value;
        }

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public override bool GetButtonDown(string name)
        {
            bool value = _mobile.GetButtonDown(name);
            if (!value && !_mobile.WillButtonDown(name))
            {
                value = _standAlone.GetButtonDown(name);
            }
            return value;
        }

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public override bool GetButtonUp(string name)
        {
            bool value = _mobile.GetButtonUp(name);
            if (!value && !_mobile.WillButtonUp(name))
            {
                value = _standAlone.GetButtonUp(name);
            }
            return value;
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public override void SetAxis(string name, float value)
        {
            _standAlone.SetAxis(name, value);
            _mobile.SetAxis(name, value);
        }

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonDown(string name)
        {
            _standAlone.SetButtonDown(name);
            _mobile.SetButtonDown(name);
        }

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonUp(string name)
        {
            _standAlone.SetButtonUp(name);
            _mobile.SetButtonUp(name);
        }
    }
}