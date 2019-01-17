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
        /// 输入方式
        /// </summary>
        private ActiveInputMethod _method;

        /// <summary>
        /// 死区
        /// <para>None及StandAlone模式下,该参数被忽略</para>
        /// </summary>
        private float _deadZoom;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="method">输入方式</param>
        /// <param name="deadZoom">死区,None及StandAlone模式下,该参数被忽略</param>
        public InputServiceProvider(ActiveInputMethod method, float deadZoom)
        {
            _method = method;
            _deadZoom = deadZoom;
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
                .Alias<IInputService>().OnAfterResolving((instance) =>
                {
                    InputService InputService = (InputService)instance;

                    //设置输入方式
                    InputService.SwitchInputMethod(_method, _deadZoom);
                });
        }
    }
}