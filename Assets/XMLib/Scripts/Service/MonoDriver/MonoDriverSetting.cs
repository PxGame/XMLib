/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 2:34:17 PM
 */

using System;
using UnityEngine;

namespace XMLib.MonoDriver
{
    /// <summary>
    /// Mono驱动设置
    /// </summary>
    [Serializable]
    public class MonoDriverSetting : ServiceSetting
    {
        /// <summary>
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public override IServiceProvider NewServiceProvider()
        {
            return new MonoDriverProvider(this);
        }
    }
}