using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 对象池服务接口
    /// </summary>
    public interface IPoolService
    {
        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="obj"></param>
        void Push(GameObject obj);

        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="item"></param>
        void Push(PoolItem item);

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <param name="poolName">对象名</param>
        /// <returns></returns>
        GameObject Pop(string poolName);

        /// <summary>
        /// 清空
        /// </summary>
        void Clear();

        /// <summary>
        /// 清空指定对象
        /// </summary>
        /// <param name="poolName">对象名</param>
        void Clear(string poolName);
    }
}