/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:39:18 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.InputService
{
    /// <summary>
    /// 标准输入
    /// </summary>
    public class StandAloneInput : VirtualInput
    {
        #region 控制

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxis(string name)
        {
            return UnityEngine.Input.GetAxis(name);
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public override float GetAxisRaw(string name)
        {
            return UnityEngine.Input.GetAxisRaw(name);
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public override void SetAxis(string name, float value)
        {
            //Debug.LogWarningFormat("标准输入方式不支持设置轴值: {0} = {1}", name, value);
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public override bool GetButton(string name)
        {
            return UnityEngine.Input.GetButton(name);
        }

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public override bool GetButtonDown(string name)
        {
            return UnityEngine.Input.GetButtonDown(name);
        }

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public override bool GetButtonUp(string name)
        {
            return UnityEngine.Input.GetButtonUp(name);
        }

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonDown(string name)
        {
            //Debug.LogWarningFormat("标准输入方式不支持设置按钮状态: {0} = 按下", name);
        }

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public override void SetButtonUp(string name)
        {
            //Debug.LogWarningFormat("标准输入方式不支持设置按钮状态: {0} = 抬起", name);
        }

        #endregion 控制
    }
}