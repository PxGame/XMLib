/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:39:06 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 移动输入
    /// </summary>
    public class MobileInput : VirtualInput
    {
        /// <summary>
        /// 死区
        /// </summary>
        private float _deadZoom;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deadZoom">轴死区</param>
        public MobileInput(float deadZoom = 0)
        {
            _deadZoom = deadZoom;
        }

        #region 控制

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxis(string name)
        {
            if (!AxisExist(name))
            {
                return 0;
            }

            float value = _axisDict[name].Value;
            return Mathf.Abs(value) >= Mathf.Abs(_deadZoom) ? value : 0;
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxisRaw(string name)
        {
            if (!AxisExist(name))
            {
                return 0;
            }

            float value = _axisDict[name].Value;
            return Mathf.Abs(value) >= Mathf.Abs(_deadZoom) ? Mathf.Sign(value) : 0;
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public override void SetAxis(string name, float value)
        {
            if (!AxisExist(name))
            {
                VirtualAxis axis = new VirtualAxis(name);
                RegistAxis(axis);
            }

            _axisDict[name].SetAxis(value);
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public override bool GetButton(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].Holding;
        }

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public override bool GetButtonDown(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].Down;
        }

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public override bool GetButtonUp(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].Up;
        }

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonDown(string name)
        {
            if (!ButtonExist(name))
            {
                VirtualButton button = new VirtualButton(name);
                RegistButton(button);
            }

            _buttonDict[name].Pressed();
        }

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonUp(string name)
        {
            if (!ButtonExist(name))
            {
                VirtualButton button = new VirtualButton(name);
                RegistButton(button);
            }

            _buttonDict[name].Released();
        }

        /// <summary>
        /// 按钮将要按住
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按住</returns>
        public bool WillButton(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].WillHolding;
        }

        /// <summary>
        /// 按钮将要按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public bool WillButtonDown(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].WillDown;
        }

        /// <summary>
        /// 按钮将要抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public bool WillButtonUp(string name)
        {
            if (!ButtonExist(name))
            {
                return false;
            }

            return _buttonDict[name].WillUp;
        }

        #endregion 控制
    }
}