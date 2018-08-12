using UnityEngine;

namespace XM.Services.Pool
{
    /// <summary>
    /// 对象池服务接口
    /// </summary>
    public interface IPoolService
    {
        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="obj">对象实例</param>
        void Push(GameObject obj);

        /// <summary>
        /// 压入对象池
        /// </summary>
        /// <param name="item">对象实例</param>
        void Push(PoolItem item);

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <param name="poolName">对象名</param>
        /// <returns>对象实例</returns>
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