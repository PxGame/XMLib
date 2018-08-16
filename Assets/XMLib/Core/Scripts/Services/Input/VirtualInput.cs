using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 输入
    /// </summary>
    public abstract class VirtualInput : IVirtualInput
    {
        /// <summary>
        /// 轴列表
        /// </summary>
        protected Dictionary<string, VirtualAxis> _axisDict = new Dictionary<string, VirtualAxis>();

        /// <summary>
        /// 按键列表
        /// </summary>
        protected Dictionary<string, VirtualButton> _buttonDict = new Dictionary<string, VirtualButton>();

        #region 实现

        public bool ExistAxis(string axisName)
        {
            return _axisDict.ContainsKey(axisName);
        }

        public void RegistAxis(VirtualAxis axis)
        {
            if (!_axisDict.ContainsKey(axis.Name))
            {//添加
                _axisDict.Add(axis.Name, axis);
            }
        }

        public bool UnRegistAxis(string axisName)
        {
            //移除
            return _axisDict.Remove(axisName);
        }

        public bool ExistButton(string buttonName)
        {
            return _buttonDict.ContainsKey(buttonName);
        }

        public void RegistButton(VirtualButton button)
        {
            if (!_buttonDict.ContainsKey(button.Name))
            {//添加
                _buttonDict.Add(button.Name, button);
            }
        }

        public bool UnRegistButton(string buttonName)
        {
            //移除
            return _buttonDict.Remove(buttonName);
        }

        public void ResetAxis()
        {
            foreach (var axisPair in _axisDict)
            {
                axisPair.Value.Update(0);
            }
        }

        public void ResetButton()
        {
            foreach (var buttonPair in _buttonDict)
            {
                buttonPair.Value.Update(false);
            }
        }

        public abstract float GetAxisRaw(string axisName);

        public abstract float GetAxis(string axisName);

        public abstract bool GetButton(string buttonName);

        public abstract bool GetButtonDown(string buttonName);

        public abstract bool GetButtonUp(string buttonName);

        public abstract void SetAxis(string axisName, float value);

        public abstract void SetButton(string buttonName, bool status);

        #endregion 实现
    }
}