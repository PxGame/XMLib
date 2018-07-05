using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI设置
    /// </summary>
    public interface IUISettingValue
    {
        /// <summary>
        /// UI设置
        /// </summary>
        UISetting UISetting { get; }
    }
}