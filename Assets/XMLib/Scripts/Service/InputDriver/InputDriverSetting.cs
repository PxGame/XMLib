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
        /// 输入模式
        /// </summary>
        private ActiveInputMethod method { get { return _method; } }

        /// <summary>
        /// 输入模式
        /// </summary>
        [SerializeField]
        protected ActiveInputMethod _method;

        /// <summary>
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public override IServiceProvider NewServiceProvider()
        {
            return new InputDriverProvider();
        }
    }
}