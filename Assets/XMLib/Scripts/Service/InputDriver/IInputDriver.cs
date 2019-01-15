/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:30:14 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 输入服务
    /// </summary>
    internal interface IInputDriver : IInput
    {
        /// <summary>
        /// 当前输入方式
        /// </summary>
        ActiveInputMethod Method { get; }

        /// <summary>
        /// 切换输入方式
        /// </summary>
        /// <param name="method">输入方式</param>
        /// <param name="deadZoom">死区</param>
        void SwitchInputMethod(ActiveInputMethod method, float deadZoom = 0);
    }
}