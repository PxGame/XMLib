namespace XM.Services
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public abstract class BaseService<AE> : IService where AE : IAppEntry
    {
        private AE _entry;

        /// <summary>
        /// 应用入口
        /// </summary>
        public AE Entry { get { return _entry; } }

        /// <summary>
        /// 服务名
        /// </summary>
        public virtual string ServiceName { get { return GetType().Name; } }

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="appEntry">入口实例</param>
        public virtual void CreateService(IAppEntry appEntry)
        {
            _entry = (AE)appEntry;

            OnServiceCreate();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public virtual void InitService()
        {
            OnServiceInit();
        }

        /// <summary>
        /// 开始移除
        /// </summary>
        public virtual void DisposeBeforeService()
        {
            OnServiceDisposeBefore();
        }

        /// <summary>
        /// 完成移除
        /// </summary>
        public virtual void DisposedService()
        {
            OnServiceDisposed();
            _entry = null;
        }

        /// <summary>
        /// 清理服务
        /// </summary>
        public virtual void ClearService()
        {
            OnServiceClear();
        }

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="debugType">debug 类型</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public virtual void Debug(DebugType debugType, string format, params object[] args)
        {
            Entry.Debug(debugType, "[" + ServiceName + "]" + format, args);
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected virtual void OnServiceCreate() { }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnServiceInit() { }

        /// <summary>
        /// 开始移除
        /// </summary>
        protected virtual void OnServiceDisposeBefore() { }

        /// <summary>
        /// 完成移除
        /// </summary>
        protected virtual void OnServiceDisposed() { }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void OnServiceClear() { }
    }
}