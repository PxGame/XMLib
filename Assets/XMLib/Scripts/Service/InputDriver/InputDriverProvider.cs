/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/28/2018 10:30:48 AM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.InputDriver
{
    /// <summary>
    /// 输入服务提供者
    /// </summary>
    public sealed class InputDriverProvider : IServiceProvider
    {
        private ActiveInputMethod _method;

        [Tooltip("None及StandAlone模式下,该参数被忽略")]
        [SerializeField]
        private float _deadZoom;

        /// <summary>
        /// 服务提供者初始化
        /// <para>可设置优先级</para>
        /// </summary>
        [Priority()]
        public void Init()
        {
            App.Make<IInput>();
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<InputDriver>()
                .Alias<IInput>().OnAfterResolving((instance) =>
                {
                    InputDriver inputDriver = (InputDriver)instance;

                    //设置输入方式
                    inputDriver.SwitchInputMethod(_method, _deadZoom);
                });
        }
    }
}