using UnityEngine;

namespace XM.Tool
{
    /// <summary>
    /// Unity单例模板
    /// </summary>
    /// <typeparam name="T">MonoBehaviour 子类</typeparam>
    public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
    {
        protected static T _instance;

        /// <summary>
        /// 单实例
        /// </summary>
        public static T Instance { get { return _instance; } }

        /// <summary>
        /// 是否初始化
        /// </summary>
        public static bool IsInitialized { get { return _instance != null; } }

        protected virtual void Awake()
        {
            if (_instance != null)
            {//重复实例化
                string msg = string.Format("重复实例化单实例 {0}", GetType().Name);
                throw new System.Exception(msg);
            }

            _instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (this == _instance)
            {
                _instance = null;
            }
        }
    }
}