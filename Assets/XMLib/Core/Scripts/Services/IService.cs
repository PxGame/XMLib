using System;

namespace XM.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IService<AE> where AE : IAppEntry<AE>
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        void AddService(AE appEntry);

        /// <summary>
        /// 初始化服务
        /// </summary>
        void InitService();

        /// <summary>
        /// 移除服务
        /// </summary>
        void RemoveService();

        /// <summary>
        /// 清理服务
        /// </summary>
        void ClearService();
    }
}