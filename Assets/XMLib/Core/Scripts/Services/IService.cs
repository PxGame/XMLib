namespace XM.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public interface IService<AE> where AE : IAppEntry<AE>
    {
        string ServiceName { get; }

        /// <summary>
        /// 应用入口
        /// </summary>
        AE Entry { get; }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        void CreateService(AE appEntry);

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