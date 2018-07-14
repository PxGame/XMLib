using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI节点
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        #region public members

        /// <summary>
        /// 一般节点
        /// </summary>
        public Transform Normal { get { return _normal; } }

        /// <summary>
        /// 缓存
        /// </summary>
        public Transform Cache { get { return _cache; } }

        #endregion public members

        #region private members

        [SerializeField]
        private Transform _normal;

        [SerializeField]
        private Transform _cache;

        #endregion private members
    }
}