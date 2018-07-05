using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 对象池设置
    /// </summary>
    public interface IPoolSettingValue
    {
        /// <summary>
        /// 对象池设置
        /// </summary>
        PoolSetting PoolSetting { get; }
    }
}