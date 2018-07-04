using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XM.Services
{
    /// <summary>
    /// 对象池服务
    /// </summary>
    public class PoolService : IService
    {
        #region private members

        private Dictionary<string, Stack<PoolItem>> _pools = new Dictionary<string, Stack<PoolItem>>();

        private Transform _poolRoot;

        #endregion private members

        protected override void OnAddService()
        {
            GameObject poolObj = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(poolObj);
            _poolRoot = poolObj.transform;
        }

        protected override void OnInitService()
        {
        }

        protected override void OnRemoveService()
        {
            if (null != _poolRoot)
            {
                GameObject.Destroy(_poolRoot.gameObject);
                _poolRoot = null;
            }

            _pools.Clear();
        }

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
            if (null == item)
            {
                throw new System.Exception(string.Format("对象池压入对象为null。"));
            }

            Stack<PoolItem> items = null;
            if (!_pools.TryGetValue(item.PoolName, out items))
            {
                items = new Stack<PoolItem>(10);
                _pools.Add(item.PoolName, items);
            }

            items.Push(item);

            //先调用EnterPool再改变父节点
            item.EnterPool();
            item.transform.parent = _poolRoot;
        }

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <param name="objName">对象名</param>
        /// <returns></returns>
        public GameObject Pop(string objName)
        {
            PoolItem item = null;
            Stack<PoolItem> items = null;
            if (!_pools.TryGetValue(objName, out items))
            {
                return null;
            }

            item = items.Pop();

            if (0 == items.Count)
            {
                _pools.Remove(objName);
            }

            //先改变父节点再调用LeavePool
            MoveToCurrent(item.gameObject);
            item.LeavePool();

            return item.gameObject;
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
                    item.Destory();
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
                item.Destory();
            }

            _pools.Remove(poolName);
        }
    }
}