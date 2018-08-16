using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 输入接口
    /// </summary>
    public interface IVirtualInput
    {
        #region 注册与反注册

        #region 摇杆

        /// <summary>
        /// 注册轴
        /// </summary>
        /// <param name="axis">轴对象</param>
        void RegistAxis(VirtualAxis axis);

        /// <summary>
        /// 反注册轴
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>是否成功</returns>
        bool UnRegistAxis(string axisName);

        /// <summary>
        /// 是否存在轴
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>是否存在</returns>
        bool ExistAxis(string axisName);

        #endregion 摇杆

        #region 按钮

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="button">按钮对象</param>
        void RegistButton(VirtualButton button);

        /// <summary>
        /// 反注册按钮
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>是否成功</returns>
        bool UnRegistButton(string buttonName);

        /// <summary>
        /// 是否存在按钮
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>是否存在</returns>
        bool ExistButton(string buttonName);

        #endregion 按钮

        #endregion 注册与反注册

        #region 输入

        #region 读取输入

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>轴值</returns>
        float GetAxis(string axisName);

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>轴值</returns>
        float GetAxisRaw(string axisName);

        /// <summary>
        /// 按钮是否按下
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>是否按下</returns>
        bool GetButton(string buttonName);

        /// <summary>
        /// 按钮是否按下
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>是否按下</returns>
        bool GetButtonDown(string buttonName);

        /// <summary>
        /// 按钮是否抬起
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>是否抬起</returns>
        bool GetButtonUp(string buttonName);

        #endregion 读取输入

        #region 写入输入

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <param name="value">轴值</param>
        void SetAxis(string axisName, float value);

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <param name="status">按钮状态</param>
        void SetButton(string buttonName, bool status);

        #endregion 写入输入

        #region 重置输入

        /// <summary>
        /// 重置轴值
        /// </summary>
        void ResetAxis();

        /// <summary>
        /// 重置按钮值
        /// </summary>
        void ResetButton();

        #endregion 重置输入

        #endregion 输入
    }
}