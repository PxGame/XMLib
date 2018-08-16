using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UInput = UnityEngine.Input;

namespace XM.Services.Input
{
    /// <summary>
    /// unity输入
    /// </summary>
    public class UnityInput : IVirtualInput
    {
        public float GetAxis(string axisName)
        {
            return UInput.GetAxis(axisName);
        }

        public float GetAxisRaw(string axisName)
        {
            return UInput.GetAxisRaw(axisName);
        }

        public bool GetButton(string buttonName)
        {
            return UInput.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return UInput.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return UInput.GetButtonUp(buttonName);
        }

        public void ResetAxis()
        {
            UInput.ResetInputAxes();
        }

        public void ResetButton()
        {
            UInput.ResetInputAxes();
        }

        #region 当前输入方式不支持

        public bool ExistAxis(string axisName)
        {
            throw new StringException("UnityInput输入方式不支持 ExistAxis 方法：{0}", axisName);
        }

        public bool ExistButton(string buttonName)
        {
            throw new StringException("UnityInput输入方式不支持 ExistButton 方法：{0}", buttonName);
        }

        public void RegistAxis(VirtualAxis axis)
        {
            throw new StringException("UnityInput输入方式不支持 RegistAxis 方法：{0}", axis.Name);
        }

        public void RegistButton(VirtualButton button)
        {
            throw new StringException("UnityInput输入方式不支持 RegistButton 方法：{0}", button.Name);
        }

        public void SetAxis(string axisName, float value)
        {
            throw new StringException("UnityInput输入方式不支持 SetAxis 方法：{0} => {1}", axisName, value);
        }

        public void SetButton(string buttonName, bool status)
        {
            throw new StringException("UnityInput输入方式不支持 SetButton 方法：{0} => {1}", buttonName, status);
        }

        public bool UnRegistAxis(string axisName)
        {
            throw new StringException("UnityInput输入方式不支持 UnRegistAxis 方法：{0}", axisName);
        }

        public bool UnRegistButton(string buttonName)
        {
            throw new StringException("UnityInput输入方式不支持 UnRegistButton 方法：{0}", buttonName);
        }

        #endregion 当前输入方式不支持
    }
}