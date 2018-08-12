using UnityEngine;

namespace XM.Services.UI
{
    /// <summary>
    /// UI节点
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        /// <summary>
        /// 一般节点
        /// </summary>
        public Transform Normal { get { return _normal; } }

        /// <summary>
        /// 缓存
        /// </summary>
        public Transform Cache { get { return _cache; } }

        [SerializeField]
        private Transform _normal;

        [SerializeField]
        private Transform _cache;
    }
}