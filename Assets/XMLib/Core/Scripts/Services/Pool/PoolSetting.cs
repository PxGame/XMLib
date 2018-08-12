using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Pool
{
    /// <summary>
    /// 对象池实例设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Pool Setting")]
    public class PoolSetting : SimpleSetting
    {
        #region 设置

        [SerializeField]
        protected List<GameObject> _items;

        #endregion 设置

        #region 属性

        protected Dictionary<string, GameObject> _itemDict;

        #endregion 属性

        #region 函数

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
            int length = _items.Count;
            _itemDict = new Dictionary<string, GameObject>(length);

            GameObject obj;
            PoolItem item;
            for (int i = 0; i < length; i++)
            {
                obj = _items[i];
                item = obj.GetComponent<PoolItem>();
                _itemDict.Add(item.PoolName, obj);
            }
        }

        #endregion 函数
    }
}