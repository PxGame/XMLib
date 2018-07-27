namespace XM.Services
{
    /// <summary>
    /// UI服务接口
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Debug 输出
        /// </summary>
        /// <param name="debugType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        ///
        void DebugPanel(DebugType debugType, string format, params string[] args);
    }
}