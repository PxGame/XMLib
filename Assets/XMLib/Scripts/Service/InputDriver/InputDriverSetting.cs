/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 2:34:43 PM
 */

using System;
using UnityEngine;

namespace XMLib.InputDriver
{
    /// <summary>
    /// Input驱动设置
    /// </summary>
    [Serializable]
    public class InputDriverSetting : ServiceSetting
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
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public override IServiceProvider NewServiceProvider()
        {
            return new InputDriverProvider(_method, _deadZoom);
        }
    }
}