using System;

namespace XM.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public abstract class IService
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        public abstract void AddService(IAppEntry appEntry);

        /// <summary>
        /// 初始化服务
        /// </summary>
        public abstract void InitService();

        /// <summary>
        /// 移除服务
        /// </summary>
        public abstract void RemoveService();

        /// <summary>
        /// 清理服务
        /// </summary>
        public abstract void ClearService();
    }
}