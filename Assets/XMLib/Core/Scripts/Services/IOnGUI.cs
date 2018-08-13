namespace XM.Services
{
    /// <summary>
    /// Unity OnGUI调用
    /// </summary>
    public interface IOnGUI
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        void OnGUI();
    }
}