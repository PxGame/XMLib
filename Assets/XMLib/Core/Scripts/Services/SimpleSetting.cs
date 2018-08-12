using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 基础设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Base Setting")]
    public class SimpleSetting : ScriptableObject
    {
        /// <summary>
        /// 输出控制
        /// </summary>
        [SerializeField]
        [EnumFlags]
        protected DebugType _debugType;

        /// <summary>
        /// debug 等级
        /// </summary>
        public DebugType DebugType { get { return _debugType; } }
    }
}