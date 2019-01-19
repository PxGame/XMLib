/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/20/2018 11:50:22 AM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.MonoDriver
{
    /// <summary>
    /// Mono 驱动提供者
    /// </summary>
    public sealed class MonoDriverProvider : IServiceProvider
    {
        /// <summary>
        /// 服务设置
        /// </summary>
        private readonly MonoDriverSetting _setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MonoDriverProvider()
        {
            _setting = App.Make<MonoDriverSetting>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority(-5000)]
        public void Init()
        {
            App.Make<IMonoDriver>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<MonoDriver>()
                .Alias<IMonoDriver>();
        }
    }
}