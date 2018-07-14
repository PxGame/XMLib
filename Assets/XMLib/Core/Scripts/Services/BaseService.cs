using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IService where T : BaseSetting
    {
        private IAppEntry _entry;
        private T _setting;

        /// <summary>
        /// 应用入口
        /// </summary>
        public IAppEntry Entry { get { return _entry; } }

        /// <summary>
        /// 设置
        /// </summary>
        public T Setting { get { return _setting; } }

        /// <summary>
        /// 服务名
        /// </summary>
        public virtual string ServiceName { get { return GetType().FullName; } }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        public override void AddService(IAppEntry appEntry)
        {
            _entry = appEntry;

            _setting = _entry.Settings.GetSetting<T>();
            if (null == _setting)
            {
                throw new Exception(string.Format("未找到 {0} 的设置文件 {1}", ServiceName, typeof(T).FullName));
            }

            OnAddService();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public override void InitService()
        {
            OnInitService();
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        public override void RemoveService()
        {
            OnRemoveService();
            _entry = null;
        }

        /// <summary>
        /// 清理服务
        /// </summary>
        public override void ClearService()
        {
            OnClearService();
        }

        /// <summary>
        /// debug 输出
        /// </summary>
        /// <param name="debugType">debug 类型</param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public void Debug(DebugType debugType, string format, params object[] args)
        {
            if (0 == (debugType & _setting.DebugType))
            {//不符合输出要求
                return;
            }

            string msg = string.Format(format, args);
            string outLog = string.Format("[{0}]{1}", ServiceName, msg);
            Entry.Debug(debugType, outLog);
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected virtual void OnAddService()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInitService()
        {
        }

        /// <summary>
        /// 移除
        /// </summary>
        protected virtual void OnRemoveService()
        {
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void OnClearService()
        {
        }
    }
}