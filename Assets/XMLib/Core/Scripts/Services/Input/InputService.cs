namespace XM.Services.Input
{
    /// <summary>
    /// 输入服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class InputService : SimpleService<AppEntry, InputSetting>
    {
        private IVirtualInput _input;

        protected override void OnServiceCreate()
        {
            base.OnServiceCreate();

            //初始化输入方式
            SwitchActiveInputMethod(Setting.InputMethod);
        }

        /// <summary>
        /// 切换输入模式
        /// </summary>
        /// <param name="activeInputMethod">输入方式</param>
        public void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
        {
            if (null != _input)
            {
                ResetAll();
            }

            switch (activeInputMethod)
            {
                case ActiveInputMethod.None:
                    _input = new NoneInput();
                    break;

                case ActiveInputMethod.UInput:
                    _input = new UnityInput();
                    break;

                case ActiveInputMethod.Mobile:
                    _input = new MobileInput();
                    break;

                default:
                    throw new StringException("输入方式不存在:{0}", Setting.InputMethod);
            }
        }

        #region 获取输入

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>轴值</returns>
        public float GetAxis(string axisName)
        {
            return _input.GetAxis(axisName);
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>轴值</returns>
        public float GetAxisRaw(string axisName)
        {
            return _input.GetAxisRaw(axisName);
        }

        /// <summary>
        /// 按钮是否按住
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>按住</returns>
        public bool GetButton(string buttonName)
        {
            return _input.GetButton(buttonName);
        }

        /// <summary>
        /// 按钮是否按下
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <returns>按下</returns>
        public bool GetButtonDown(string buttonName)
        {
            return _input.GetButtonDown(buttonName);
        }

        /// <summary>
        /// 按钮时否抬起
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns>抬起</returns>
        public bool GetButtonUp(string buttonName)
        {
            return _input.GetButtonUp(buttonName);
        }

        #endregion 获取输入

        #region 设置输入

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <param name="value">轴值</param>
        public void SetAxis(string axisName, float value)
        {
            _input.SetAxis(axisName, value);
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="buttonName">按钮名</param>
        /// <param name="status">按下/抬起</param>
        public void SetButton(string buttonName, bool status)
        {
            _input.SetButton(buttonName, status);
        }

        #endregion 设置输入

        #region 重置输入

        /// <summary>
        /// 重置轴
        /// </summary>
        public void ResetAxis()
        {
            _input.ResetAxis();
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        public void ResetButton()
        {
            _input.ResetButton();
        }

        /// <summary>
        /// 重置所有
        /// </summary>
        public void ResetAll()
        {
            ResetAxis();
            ResetButton();
        }

        #endregion 重置输入
    }
}