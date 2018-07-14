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

        protected override void InitData(AE appEntry)
        {
            base.InitData(appEntry);

            _setting = Entry.Settings.GetSetting<ST>();
            if (null == _setting)
            {
                throw new Exception(string.Format("未找到 {0} 的设置文件 {1}", ServiceName, typeof(ST).FullName));
            }
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