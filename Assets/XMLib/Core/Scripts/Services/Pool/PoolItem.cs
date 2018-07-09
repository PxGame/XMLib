using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 对象池物体
    /// </summary>
    public class PoolItem : MonoBehaviour
    {
        /// <summary>
        /// 池名字
        /// </summary>
        public string PoolName { get { return _poolName; } set { _poolName = value; } }

        #region protected members

        [SerializeField]
        protected string _poolName = "";

        /// <summary>
        /// 对象池服务
        /// </summary>
        protected PoolService Service { get { return _service; } }

        #endregion protected members

        #region private members

        private PoolService _service;

        #endregion private members

        /// <summary>
        /// 创建时初始化
        /// </summary>
        /// <param name="service">所在服务</param>
        public void Create(PoolService service)
        {
            _service = service;
        }

        /// <summary>
        /// 进入对象池
        /// </summary>
        public void EnterPool()
        {
            OnEnterPool();
        }

        /// <summary>
        /// 离开对象池
        /// </summary>
        public void LeavePool()
        {
            OnLeavePool();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Delete()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 进入对象池
        /// </summary>
        protected virtual void OnEnterPool()
        {
        }

        /// <summary>
        /// 离开对象池
        /// </summary>
        protected virtual void OnLeavePool()
        {
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_poolName))
            {
                _poolName = gameObject.name;
            }
        }

#endif
    }
}