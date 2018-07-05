using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI服务
    /// </summary>
    public class UIService : IService
    {
        #region Private members

        private Dictionary<string, GameObject> _panelDict;

        #endregion Private members

        protected override void OnAddService()
        {
            //获取设置
            IUISettingValue setting = Entry.ServiceSettings as IUISettingValue;
            if (null == setting)
            {
                throw new System.Exception("启用对象池服务后,服务设置机核必须实现IPoolSettingValue接口.");
            }
            _panelDict = setting.UISetting.GetPanelDict();
        }

        protected override void OnInitService()
        {
        }

        protected override void OnRemoveService()
        {
        }
    }
}