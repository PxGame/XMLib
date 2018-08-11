using System;

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

        public override void CreateService(AE appEntry)
        {
            //初始化设置
            _setting = appEntry.Settings.GetSetting<ST>();
            Checker.NotNull(_setting, "未找到 {0} 的设置文件 {1}", ServiceName, typeof(ST).FullName);

            //必须最后调用
            base.CreateService(appEntry);
        }

        public override void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & _setting.DebugType))
            {//不符合输出要求
                return;
            }

            base.Debug(debugType, format, args);
        }
    }
}