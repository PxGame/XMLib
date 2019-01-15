/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:38:43 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 虚拟输入
    /// </summary>
    internal abstract class VirtualInput : IVirtualInput
    {
        protected Dictionary<string, VirtualAxis> _axisDict;
        protected Dictionary<string, VirtualButton> _buttonDict;

        /// <summary>
        /// 构造函数
        /// </summary>
        public VirtualInput()
        {
            _axisDict = new Dictionary<string, VirtualAxis>();
            _buttonDict = new Dictionary<string, VirtualButton>();
        }

        #region 注册

        /// <summary>
        /// 轴是否存在
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>轴是否存在</returns>
        public bool AxisExist(string name)
        {
            return _axisDict.ContainsKey(name);
        }

        /// <summary>
        /// 注册轴
        /// </summary>
        /// <param name="axis">轴引用</param>
        public void RegistAxis(VirtualAxis axis)
        {
            if (AxisExist(axis.Name))
            {
                return;
            }

            _axisDict.Add(axis.Name, axis);
        }

        /// <summary>
        /// 卸载轴
        /// </summary>
        /// <param name="name">名字</param>
        public void UnregistAxis(string name)
        {
            if (AxisExist(name))
            {
                _axisDict.Remove(name);
            }
        }

        /// <summary>
        /// 按钮是否存在
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>按钮是否存在</returns>
        public bool ButtonExist(string name)
        {
            return _buttonDict.ContainsKey(name);
        }

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="button">按钮引用</param>
        public void RegistButton(VirtualButton button)
        {
            if (ButtonExist(button.Name))
            {
                return;
            }

            _buttonDict.Add(button.Name, button);
        }

        /// <summary>
        /// 卸载按钮
        /// </summary>
        /// <param name="name">按钮名</param>
        public void UnRegistButton(string name)
        {
            if (ButtonExist(name))
            {
                _buttonDict.Remove(name);
            }
        }

        #endregion 注册

        #region 控制

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public abstract float GetAxis(string name);

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取轴值</returns>
        public abstract float GetAxisRaw(string name);

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="value">值</param>
        public abstract void SetAxis(string name, float value);

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮状态</returns>
        public abstract bool GetButton(string name);

        /// <summary>
        /// 获取按钮是否按下
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否按下</returns>
        public abstract bool GetButtonDown(string name);

        /// <summary>
        /// 获取按钮是否抬起
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>获取按钮是否抬起</returns>
        public abstract bool GetButtonUp(string name);

        /// <summary>
        /// 设置按钮按下
        /// </summary>
        /// <param name="name">名字</param>
        public abstract void SetButtonDown(string name);

        /// <summary>
        /// 设置按钮抬起
        /// </summary>
        /// <param name="name">名字</param>
        public abstract void SetButtonUp(string name);

        #endregion 控制
    }
}