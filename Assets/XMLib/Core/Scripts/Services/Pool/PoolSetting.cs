using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM.Tools;

namespace XM.Services
{
    /// <summary>
    /// 对象池实例设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Pool Setting")]
    public class PoolSetting : BaseSetting
    {
        #region 设置

        [SerializeField]
        protected List<GameObject> Items;

        #endregion 设置

        #region 公开

        /// <summary>
        /// 获取元素
        /// </summary>
        /// <param name="poolName">对象池名</param>
        /// <returns></returns>
        public GameObject GetItem(string poolName)
        {
            UpdateItemDict();

            GameObject itemObj;
            if (_itemDict.TryGetValue(poolName, out itemObj))
            {
            }

            return itemObj;
        }

        #endregion 公开

        #region 不公开

        protected Dictionary<string, GameObject> _itemDict;

        /// <summary>
        /// 更新元素字典
        /// </summary>
        /// <param name="isForce">强制更新</param>
        protected void UpdateItemDict(bool isForce = false)
        {
            if (!isForce && null != _itemDict)
            {
                return;
            }

            //转换成字典
            int length = Items.Count;
            _itemDict = new Dictionary<string, GameObject>(length);

            GameObject obj;
            PoolItem item;
            for (int i = 0; i < length; i++)
            {
                obj = Items[i];
                item = obj.GetComponent<PoolItem>();
                _itemDict.Add(item.PoolName, obj);
            }
        }

        #endregion 不公开
    }
}