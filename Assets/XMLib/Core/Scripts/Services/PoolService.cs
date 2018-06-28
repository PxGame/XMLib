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

        private Dictionary<string, Stack<GameObject>> _pools = new Dictionary<string, Stack<GameObject>>();

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
        /// <param name="objName">对象名</param>
        /// <param name="obj">对象</param>
        public void Push(string objName, GameObject obj)
        {
            if (null == obj)
            {
                throw new System.Exception(string.Format("对象池压入对象 {0} 为null。", objName));
            }

            obj.transform.parent = _poolRoot;
            obj.SetActive(false);

            Stack<GameObject> objs = null;
            if (!_pools.TryGetValue(objName, out objs))
            {
                objs = new Stack<GameObject>(10);
                _pools.Add(objName, objs);
            }

            objs.Push(obj);
        }

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <param name="objName">对象名</param>
        /// <returns></returns>
        public GameObject Pop(string objName)
        {
            GameObject obj = null;
            Stack<GameObject> objs = null;
            if (!_pools.TryGetValue(objName, out objs))
            {
                return obj;
            }

            obj = objs.Pop();

            MoveToCurrent(obj);
            obj.SetActive(true);

            if (0 == objs.Count)
            {
                _pools.Remove(objName);
            }

            return obj;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            GameObject obj = null;
            Stack<GameObject> objs = null;
            foreach (var objsPair in _pools)
            {
                objs = objsPair.Value;

                while (objs.Count > 0)
                {
                    obj = objs.Pop();
                    GameObject.DestroyImmediate(obj);
                }
            }
            _pools.Clear();
        }

        /// <summary>
        /// 清空指定对象
        /// </summary>
        /// <param name="objName">对象名</param>
        public void Clear(string objName)
        {
            GameObject obj = null;
            Stack<GameObject> objs = null;

            if (!_pools.TryGetValue(objName, out objs))
            {
                return;
            }

            while (objs.Count > 0)
            {
                obj = objs.Pop();
                GameObject.DestroyImmediate(obj);
            }

            _pools.Remove(objName);
        }
    }
}