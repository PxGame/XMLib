/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 2:36:59 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 服务设置
    /// </summary>
    [Serializable]
    public abstract class ServiceSetting : IServiceSetting
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool enable { get { return _enable; } }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SerializeField]
        protected bool _enable;

        /// <summary>
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public abstract IServiceProvider NewServiceProvider();
    }
}