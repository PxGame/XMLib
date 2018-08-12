using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 服务设置集合 可根据使用的服务来进行设置及实现
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Service Settings")]
    public class ServiceSettings : ScriptableObject
    {
        /// <summary>
        /// debug 等级
        /// </summary>
        public DebugType DebugType { get { return _debugType; } }

        [SerializeField]
        [EnumFlags]
        protected DebugType _debugType;

        [SerializeField]
        protected List<SimpleSetting> _settings;

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <typeparam name="T">设置类型</typeparam>
        /// <returns>设置实例</returns>
        public T GetSetting<T>() where T : SimpleSetting
        {
            T setting = (T)_settings.Find(t => t.GetType() == typeof(T));
            return setting;
        }
    }
}