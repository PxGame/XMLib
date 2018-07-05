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

        private Dictionary<string, GameObject> _objDict;
        private Dictionary<string, Stack<PoolItem>> _pools = new Dictionary<string, Stack<PoolItem>>();

        private Transform _poolRoot;

        private string _settingPath = "PoolSetting";

        #endregion private members

        #region Base

        protected override void OnAddService()
        {
            GameObject poolObj = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(poolObj);
            _poolRoot = poolObj.transform;

            //获取资源
            PoolSetting setting = Resources.Load<PoolSetting>(_settingPath);
            if (null == setting)
            {
                throw new System.Exception("未找到对象池配置资源:" + _settingPath);
            }
            _objDict = setting.Get();
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

        #endregion Base

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

            string poolName = item.PoolName;
            Stack<PoolItem> items = null;
            if (!_pools.TryGetValue(item.PoolName, out items))
            {
                items = new Stack<PoolItem>(10);
                _pools.Add(item.PoolName, items);
            }

            Debug(DebugType.Normal, "压入对象:{0}", poolName);
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
                Debug(DebugType.Normal, "创建对象:{0}", poolName);
                GameObject preObj = null;
                if (!_objDict.TryGetValue(poolName, out preObj))
                {
                    throw new System.Exception("对象池预制中没有该类型对象:" + poolName);
                }

                obj = GameObject.Instantiate(preObj);
                obj.name = preObj.name;
                item = obj.GetComponent<PoolItem>();
            }
            else
            {//有则取出
                Debug(DebugType.Normal, "取出对象:{0}", poolName);
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