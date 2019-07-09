using UnityEngine;

namespace XMLib
{
    [CreateAssetMenu(fileName = "AppSetting", menuName = "XMLib/应用程序配置")]
    [System.Serializable]
    public class AppSetting : ScriptableObject
    {
        #region Pool

        [SerializeField]
        [Tooltip("对象池最大大小，超过时直接删除")]
        private int _poolMaxSize = 10;

        /// <summary>
        /// 对象池最大大小，超过时直接删除
        /// </summary>
        public int poolMaxSize { get { return _poolMaxSize; } }

        #endregion Pool

        #region Input

        [SerializeField]
        [Tooltip("输入模式")]
        private ActiveInputMethod _inputMethod = ActiveInputMethod.Mobile;

        [SerializeField]
        [Tooltip("输入死区")]
        [Range(0, 1f)]
        private float _inputDeadZoom = 0;

        /// <summary>
        /// 输入死区
        /// </summary>
        public ActiveInputMethod inputMethod { get { return _inputMethod; } }

        /// <summary>
        /// 输入死区
        /// </summary>
        public float inputDeadZoom { get { return _inputDeadZoom; } }

        #endregion Input
    }
}