using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 空输入
    /// </summary>
    public class NoneInput : VirtualInput
    {
        public override float GetAxis(string axisName)
        {
            return 0;
        }

        public override float GetAxisRaw(string axisName)
        {
            return 0;
        }

        public override bool GetButton(string buttonName)
        {
            return false;
        }

        public override bool GetButtonDown(string buttonName)
        {
            return false;
        }

        public override bool GetButtonUp(string buttonName)
        {
            return false;
        }

        public override void SetAxis(string axisName, float value)
        {
            throw new StringException("UnityInput输入方式不支持 SetAxis 方法：{0} => {1}", axisName, value);
        }

        public override void SetButton(string buttonName, bool status)
        {
            throw new StringException("UnityInput输入方式不支持 SetButton 方法：{0} => {1}", buttonName, status);
        }
    }
}