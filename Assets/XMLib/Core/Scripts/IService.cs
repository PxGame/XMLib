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

        /// <summary>
        /// 应用入口
        /// </summary>
        protected IAppEntry Entry { get { return _entry; } }

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
            OnAddService();
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        public void RemoveService()
        {
            OnRemoveService();
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