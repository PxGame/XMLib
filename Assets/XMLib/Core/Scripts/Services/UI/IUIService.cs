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
        /// <param name="panelName">面板名字</param>
        /// <param name="debugType">类型</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        void DebugPanel(string panelName, DebugType debugType, string format, params string[] args);
    }
}