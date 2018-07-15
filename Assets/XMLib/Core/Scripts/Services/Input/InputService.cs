namespace XM.Services
{
    /// <summary>
    /// 输入服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class InputService<AE> : SimpleService<AE, InputSetting> where AE : IAppEntry<AE>
    {
        #region Base

        protected override void OnAddService()
        {
        }

        protected override void OnClearService()
        {
        }

        protected override void OnInitService()
        {
        }

        protected override void OnRemoveService()
        {
        }

        #endregion Base
    }
}