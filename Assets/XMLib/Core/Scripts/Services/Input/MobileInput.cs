using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Input
{
    /// <summary>
    /// 移动输入
    /// </summary>
    public class MobileInput : VirtualInput
    {
        #region 重写

        public override float GetAxis(string axisName)
        {
            VirtualAxis axis;
            if (!_axisDict.TryGetValue(axisName, out axis))
            {
                return 0;
            }

            return axis.Value;
        }

        public override float GetAxisRaw(string axisName)
        {
            VirtualAxis axis;
            if (!_axisDict.TryGetValue(axisName, out axis))
            {
                return 0;
            }

            return axis.RawValue;
        }

        public override bool GetButton(string buttonName)
        {
            VirtualButton button;
            if (!_buttonDict.TryGetValue(buttonName, out button))
            {
                return false;
            }

            return button.IsPressed;
        }

        public override bool GetButtonDown(string buttonName)
        {
            VirtualButton button;
            if (!_buttonDict.TryGetValue(buttonName, out button))
            {
                return false;
            }

            return button.IsDown;
        }

        public override bool GetButtonUp(string buttonName)
        {
            VirtualButton button;
            if (!_buttonDict.TryGetValue(buttonName, out button))
            {
                return false;
            }

            return button.IsUp;
        }

        public override void SetAxis(string axisName, float value)
        {
            VirtualAxis axis;
            if (!_axisDict.TryGetValue(axisName, out axis))
            {
                axis = new VirtualAxis(axisName);
                RegistAxis(axis);
            }

            axis.Update(value);
        }

        public override void SetButton(string buttonName, bool status)
        {
            VirtualButton button;
            if (!_buttonDict.TryGetValue(buttonName, out button))
            {
                button = new VirtualButton(buttonName);
                RegistButton(button);
            }

            button.Update(status);
        }

        #endregion 重写
    }
}