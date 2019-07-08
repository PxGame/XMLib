/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:30:48 AM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.InputService
{
    /// <summary>
    /// 输入服务提供者
    /// </summary>
    public class InputServiceProvider : IServiceProvider
    {
        /// <summary>
        /// 服务设置
        /// </summary>
        //private readonly InputServiceSetting _setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InputServiceProvider()
        {
            //_setting = App.Make<InputServiceSetting>();
        }

        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority()]
        public void Init()
        {
            App.Make<IInputService>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<InputService>()
                .Alias<IInputService>();
        }
    }
}