using UnityEngine;

namespace XM.Services.Pool
{
    /// <summary>
    /// 对象池物体
    /// </summary>
    public class PoolItem : MonoBehaviour
    {
        #region 属性

        /// <summary>
        /// 池名字
        /// </summary>
        public string PoolName { get { return _poolName; } set { _poolName = value; } }

        [SerializeField]
        protected string _poolName = "";

        /// <summary>
        /// 对象池服务
        /// </summary>
        protected IPoolService Service { get { return _service; } }

        private IPoolService _service;

        #endregion 属性

        #region 函数

        /// <summary>
        /// 创建时初始化
        /// </summary>
        /// <param name="service">所在服务</param>
        public void Create(IPoolService service)
        {
            _service = service;
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

        #endregion 函数

        #region 回调

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

        #endregion 回调

        #region 编辑器

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_poolName))
            {
                _poolName = gameObject.name;
            }
        }

#endif

        #endregion 编辑器
    }
}