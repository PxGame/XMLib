namespace XM.Services
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public abstract class BaseService<AE> : IService<AE> where AE : IAppEntry<AE>
    {
        private AE _entry;

        /// <summary>
        /// 应用入口
        /// </summary>
        public AE Entry { get { return _entry; } }

        /// <summary>
        /// 服务名
        /// </summary>
        public virtual string ServiceName { get { return GetType().FullName; } }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        public void AddService(AE appEntry)
        {
            InitData(appEntry);

            OnAddService();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="appEntry"></param>
        protected virtual void InitData(AE appEntry)
        {
            _entry = appEntry;
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void InitService()
        {
            OnInitService();
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        public void RemoveService()
        {
            OnRemoveService();
            _entry = null;
        }

        /// <summary>
        /// 清理服务
        /// </summary>
        public void ClearService()
        {
            OnClearService();
        }

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="debugType">debug 类型</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public virtual void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & Entry.Settings.DebugType))
            {//不符合输出要求
                return;
            }

            Entry.Debug(debugType, "[" + ServiceName + "]" + format, args);
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected abstract void OnAddService();

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void OnInitService();

        /// <summary>
        /// 移除
        /// </summary>
        protected abstract void OnRemoveService();

        /// <summary>
        /// 清理
        /// </summary>
        protected abstract void OnClearService();
    }
}