using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 服务设置集合
    /// </summary>
    [System.Serializable]
    public class ServiceSettings : IUISettingValue, IPoolSettingValue
    {
        public UISetting UISetting { get { return _uiSetting; } }
        public PoolSetting PoolSetting { get { return _poolSetting; } }

        [SerializeField]
        protected UISetting _uiSetting;

        [SerializeField]
        protected PoolSetting _poolSetting;
    }
}