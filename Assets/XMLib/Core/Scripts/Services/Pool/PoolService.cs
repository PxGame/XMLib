using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XM.Services.Pool
{
    using Scene = UnityEngine.SceneManagement.Scene;

    /// <summary>
    /// 对象池服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class PoolService : SimpleService<IAppEntry, PoolSetting>, IPoolService
    {
        #region 属性

        /// <summary>
        /// 对象池字典
        /// </summary>
        private Dictionary<string, Stack<PoolItem>> _pools = new Dictionary<string, Stack<PoolItem>>(10);

        /// <summary>
        /// 根节点对象
        /// </summary>
        private GameObject _poolObj;

        /// <summary>
        /// 根节点变换
        /// </summary>
        private Transform _poolRoot;

        #endregion 属性

        #region 重写

        protected override void OnServiceCreate()
        {
            _poolObj = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(_poolObj);
            _poolRoot = _poolObj.transform;
        }

        protected override void OnServiceDisposed()
        {
            if (null != _poolObj)
            {
                GameObject.Destroy(_poolObj);
                _poolObj = null;
                _poolRoot = null;
            }

            _pools.Clear();
        }

        protected override void OnServiceClear()
        {
            Clear();
        }

        #endregion 重写

        #region 函数

        /// <summary>
        /// 移动对象到当前场景
        /// </summary>
        /// <param name="obj">目标对象</param>
        private void MoveToCurrent(GameObject obj)
        {
            obj.transform.parent = null;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.MoveGameObjectToScene(obj, currentScene);
        }

        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="obj"></param>
        public void Push(GameObject obj)
        {
            PoolItem item = obj.GetComponent<PoolItem>();
            Push(item);
        }

        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="item"></param>
        public void Push(PoolItem item)
        {
            Checker.NotNull(item, "对象池压入对象为null");

            string poolName = item.PoolName;
            Stack<PoolItem> items = null;
            if (!_pools.TryGetValue(item.PoolName, out items))
            {
                items = new Stack<PoolItem>(10);
                _pools.Add(item.PoolName, items);
            }

            Debug(DebugType.Debug, "压入对象:{0}", poolName);
            //压入池
            items.Push(item);

            //
            item.EnterPool();
            item.gameObject.SetActive(false);
            item.transform.parent = _poolRoot;
        }

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <param name="poolName">对象名</param>
        /// <returns></returns>
        public GameObject Pop(string poolName)
        {
            GameObject obj = null;
            PoolItem item = null;
            Stack<PoolItem> items = null;
            if (!_pools.TryGetValue(poolName, out items))
            {//没有则创建
                Debug(DebugType.Debug, "创建对象:{0}", poolName);
                GameObject preObj = Setting.GetItem(poolName);

                Checker.NotNull(preObj, "对象池预制中没有该类型对象:{0}", poolName);

                obj = GameObject.Instantiate(preObj);
                obj.name = preObj.name;
                item = obj.GetComponent<PoolItem>();
                item.Create((IPoolService)this);//实例化后初始化
            }
            else
            {//有则取出
                Debug(DebugType.Debug, "取出对象:{0}", poolName);
                item = items.Pop();
                obj = item.gameObject;

                if (0 == items.Count)
                {
                    _pools.Remove(poolName);
                }

                //移到当前场景
                MoveToCurrent(obj);
                obj.SetActive(true);
            }

            item.LeavePool();

            return obj;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            PoolItem item = null;
            Stack<PoolItem> items = null;
            foreach (var itemsPair in _pools)
            {
                items = itemsPair.Value;

                while (items.Count > 0)
                {
                    item = items.Pop();
                    item.Delete();
                }
            }
            _pools.Clear();
        }

        /// <summary>
        /// 清空指定对象
        /// </summary>
        /// <param name="poolName">对象名</param>
        public void Clear(string poolName)
        {
            PoolItem item = null;
            Stack<PoolItem> items = null;

            if (!_pools.TryGetValue(poolName, out items))
            {
                return;
            }

            while (items.Count > 0)
            {
                item = items.Pop();
                item.Delete();
            }

            _pools.Remove(poolName);
        }

        #endregion 函数
    }
}