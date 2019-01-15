/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 2:23:28 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 空输入
    /// </summary>
    internal class NoneInput : VirtualInput
    {
        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public override void SetAxis(string name, float value)
        {
        }

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonDown(string name)
        {
        }

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonUp(string name)
        {
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxis(string name)
        {
            return 0;
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxisRaw(string name)
        {
            return 0;
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public override bool GetButton(string name)
        {
            return false;
        }

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public override bool GetButtonDown(string name)
        {
            return false;
        }

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public override bool GetButtonUp(string name)
        {
            return false;
        }
    }
}