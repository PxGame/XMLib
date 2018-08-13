namespace XM.Services
{
    /// <summary>
    /// Unity Update调用
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        void Update();
    }
}