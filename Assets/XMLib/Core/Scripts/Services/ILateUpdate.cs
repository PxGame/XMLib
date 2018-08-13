namespace XM.Services
{
    /// <summary>
    /// Unity LateUpdate调用
    /// </summary>
    public interface ILateUpdate
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        void LateUpdate();
    }
}