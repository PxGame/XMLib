/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:37:09 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 虚拟输入
    /// </summary>
    public interface IVirtualInput
    {
        #region 注册

        /// <summary>
        /// 轴是否存在
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>轴是否存在</returns>
        bool AxisExist(string name);

        /// <summary>
        /// 注册轴
        /// </summary>
        /// <param name="axis">轴引用</param>
        void RegistAxis(VirtualAxis axis);

        /// <summary>
        /// 卸载轴
        /// </summary>
        /// <param name="name">名字</param>
        void UnregistAxis(string name);

        /// <summary>
        /// 按钮是否存在
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>按钮是否存在</returns>
        bool ButtonExist(string name);

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="button">按钮引用</param>
        void RegistButton(VirtualButton button);

        /// <summary>
        /// 卸载按钮
        /// </summary>
        /// <param name="name">按钮名</param>
        void UnRegistButton(string name);

        #endregion 注册

        #region 控制

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        void SetAxis(string name, float value);

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        void SetButtonDown(string name);

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        void SetButtonUp(string name);

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        float GetAxis(string name);

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        float GetAxisRaw(string name);

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        bool GetButton(string name);

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        bool GetButtonDown(string name);

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        bool GetButtonUp(string name);

        #endregion 控制
    }
}