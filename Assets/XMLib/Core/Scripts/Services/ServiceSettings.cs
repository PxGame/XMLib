using System.Collections.Generic;
using UnityEngine;
using XM.Attributes;
using XM.Tools;

namespace XM.Services
{
    /// <summary>
    /// 服务设置集合
    /// 可根据使用的服务来进行设置及实现
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Service Settings")]
    public class ServiceSettings : ScriptableObject
    {
        #region 公开

        /// <summary>
        /// debug 等级
        /// </summary>
        public DebugType DebugType { get { return _debugType; } }

        #endregion 公开

        #region 设置

        [SerializeField]
        [EnumFlags]
        protected DebugType _debugType;

        [SerializeField]
        protected List<SimpleSetting> _settings;

        #endregion 设置

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSetting<T>() where T : SimpleSetting
        {
            T setting = (T)_settings.Find(t => t.GetType() == typeof(T));
            return setting;
        }
    }
}