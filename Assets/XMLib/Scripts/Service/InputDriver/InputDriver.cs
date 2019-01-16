/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:30:00 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 输入服务
    /// </summary>
    public class InputDriver : IInputDriver
    {
        /// <summary>
        /// 当前输入方式
        /// </summary>
        public ActiveInputMethod Method { get { return _method; } }

        /// <summary>
        /// 当前输入方式
        /// </summary>
        private ActiveInputMethod _method;

        /// <summary>
        /// 输入对象
        /// </summary>
        private IVirtualInput _input;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InputDriver()
        {
            //设置默认
            _method = ActiveInputMethod.None;
            _input = new NoneInput();
        }

        /// <summary>
        /// 切换输入方式
        /// </summary>
        /// <param name="method">输入方式</param>
        /// <param name="deadZoom">死区,可选参数,在None及StandAlone模式下被忽略</param>
        public void SwitchInputMethod(ActiveInputMethod method, float deadZoom = 0)
        {
            _input = null;

            switch (method)
            {
                case ActiveInputMethod.None:
                    _input = new NoneInput();
                    break;

                case ActiveInputMethod.StandAlone:
                    _input = new StandAloneInput();
                    break;

                case ActiveInputMethod.Mobile:
                    _input = new MobileInput(deadZoom);
                    break;

                case ActiveInputMethod.Mixed:
                    _input = new MixedInput(deadZoom);
                    break;
            }

            Checker.NotNull(_input);

            Debug.LogFormat("InputDriver输入模式切换: {0} => {1}", _method, method);
            _method = method;
        }

        #region 控制

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public float GetAxis(string name)
        {
            return _input.GetAxis(name);
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public float GetAxisRaw(string name)
        {
            return _input.GetAxisRaw(name);
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public void SetAxis(string name, float value)
        {
            _input.SetAxis(name, value);
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public bool GetButton(string name)
        {
            return _input.GetButton(name);
        }

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public bool GetButtonDown(string name)
        {
            return _input.GetButtonDown(name);
        }

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public bool GetButtonUp(string name)
        {
            return _input.GetButtonUp(name);
        }

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public void SetButtonDown(string name)
        {
            _input.SetButtonDown(name);
        }

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public void SetButtonUp(string name)
        {
            _input.SetButtonUp(name);
        }

        #endregion 控制
    }
}