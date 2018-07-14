namespace XM.Services
{
    /// <summary>
    /// UI服务接口
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// 清理缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 删除所有面板
        /// </summary>
        void Clear();

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        void ShowPanel(string panelName, params object[] args);

        /// <summary>
        /// 隐藏顶层面板
        /// </summary>
        /// <returns></returns>
        void HidePanel();
    }
}