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
        void CreateService(IAppEntry appEntry);

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
    }
}