using System.Collections;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry">入口实例</param>
        void CreateService(AppEntry appEntry);

        /// <summary>
        /// 初始化服务
        /// </summary>
        void InitService();

        /// <summary>
        /// 开始移除服务
        /// </summary>
        void DisposeBeforeService();

        /// <summary>
        /// 结束移除服务
        /// </summary>
        void DisposedService();

        /// <summary>
        /// 清理服务
        /// </summary>
        void ClearService();

        /// <summary>
        /// 开启协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <returns>协程</returns>
        Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// 关闭协程
        /// </summary>
        /// <param name="routine">协程</param>
        void StopCoroutine(Coroutine routine);
    }
}