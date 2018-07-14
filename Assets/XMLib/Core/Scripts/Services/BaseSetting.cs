using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM.Tools;

namespace XM.Services
{
    /// <summary>
    /// 基础设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Base Setting")]
    public class BaseSetting : ScriptableObject
    {
        #region 设置

        /// <summary>
        /// 输出控制
        /// </summary>
        [SerializeField]
        [EnumFlags]
        protected DebugType _debugType;

        #endregion 设置

        #region 公开

        /// <summary>
        /// debug 等级
        /// </summary>
        public DebugType DebugType { get { return _debugType; } }

        #endregion 公开
    }
}