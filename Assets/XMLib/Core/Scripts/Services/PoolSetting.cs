using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 对象池实例设置
    /// </summary>
    [CreateAssetMenu]
    public class PoolSetting : ScriptableObject
    {
        [SerializeField]
        protected List<GameObject> Items;

        /// <summary>
        /// 获取字典
        /// </summary>
        public Dictionary<string, GameObject> Get()
        {
            //转换成字典
            int length = Items.Count;
            Dictionary<string, GameObject> itemDict = new Dictionary<string, GameObject>(length);

            GameObject obj;
            PoolItem item;
            for (int i = 0; i < length; i++)
            {
                obj = Items[i];
                item = obj.GetComponent<PoolItem>();
                itemDict.Add(item.PoolName, obj);
            }

            return itemDict;
        }
    }
}