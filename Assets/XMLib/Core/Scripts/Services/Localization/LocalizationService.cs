using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Localization
{
    /// <summary>
    /// 本地化服务
    /// </summary>
    /// <typeparam name="AE"></typeparam>
    public class LocalizationService : SimpleService<AppEntry, LocalizationSetting>
    {
        #region Base

        protected override void OnClearService()
        {
        }

        protected override void OnCreateService()
        {
            Inst = this;
        }

        protected override void OnInitService()
        {
        }

        protected override void OnDisposeService()
        {
            Inst = null;
        }

        #endregion Base

        #region public function

        public static LocalizationService Inst { get; protected set; }

        #endregion public function
    }
}