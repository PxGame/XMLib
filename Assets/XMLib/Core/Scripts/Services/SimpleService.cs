namespace XM.Services
{
    /// <summary>
    /// 简单服务基类，带配置文件设置
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    /// <typeparam name="ST">服务配置类型</typeparam>
    public abstract class SimpleService<AE, ST> : BaseService<AE> where ST : SimpleSetting where AE : IAppEntry<AE>
    {
        private ST _setting;

        /// <summary>
        /// 设置
        /// </summary>
        public ST Setting { get { return _setting; } }

        #region 重写

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="appEntry">入口实例</param>
        public override void CreateService(AE appEntry)
        {
            //初始化设置
            _setting = appEntry.Settings.GetSetting<ST>();
            Checker.NotNull(_setting, "未找到 {0} 的设置文件 {1}", ServiceName, typeof(ST).FullName);

            //必须最后调用
            base.CreateService(appEntry);
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="debugType">日志类型</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public override void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & _setting.DebugType))
            {//不符合输出要求
                return;
            }

            base.Debug(debugType, format, args);
        }

        #endregion 重写
    }
}