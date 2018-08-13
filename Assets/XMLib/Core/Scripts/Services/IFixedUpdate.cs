namespace XM.Services
{
    /// <summary>
    /// Unity FixedUpdate调用
    /// </summary>
    public interface IFixedUpdate
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        void FixedUpdate();
    }
}