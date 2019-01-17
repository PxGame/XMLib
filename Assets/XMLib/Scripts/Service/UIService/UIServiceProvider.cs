/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:10:04 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 驱动提供者
    /// </summary>
    public sealed class UIServiceProvider : IServiceProvider
    {
        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority()]
        public void Init()
        {
            App.Make<IUIService>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<UIService>().Alias<IUIService>();
        }
    }
}