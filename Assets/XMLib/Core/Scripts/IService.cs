using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public class IService
    {
        private IAppEntry _entry;
        private bool _enableDebug = true;

        /// <summary>
        /// 应用入口
        /// </summary>
        protected IAppEntry Entry { get { return _entry; } }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get { return GetType().FullName; } }

        /// <summary>
        /// 启用Debug
        /// </summary>
        public bool EnableDebug { get { return _enableDebug; } set { _enableDebug = value; } }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="appEntry"></param>
        public void AddService(IAppEntry appEntry)
        {
            _entry = appEntry;
            OnAddService();
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
        /// debug 输出
        /// </summary>
        /// <param name="level">debug 等级= 0-一般 1-警告 2-错误 3-GG </param>
        /// <param name="format">格式化</param>
        /// <param name="args">参数</param>
        public void Debug(int level, string format, params object[] args)
        {
            if (!_enableDebug)
            {
                return;
            }

            string msg = string.Format(format, args);
            string outLog = string.Format("[{0}]{1}", ServiceName, msg);
            Entry.Debug(level, outLog);
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
    }
}