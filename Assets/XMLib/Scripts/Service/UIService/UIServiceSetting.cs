/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 2:27:33 PM
 */

using System;
using System.IO;
using UnityEngine;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 驱动设置
    /// </summary>
    [Serializable]
    public class UIServiceSetting : ServiceSetting
    {
        /// <summary>
        /// UIRoot 资源路径
        /// </summary>
        [SerializeField]
        private string _uiRootPath;

        /// <summary>
        /// 面板资源文件夹
        /// </summary>
        [SerializeField]
        private string _panelDir;

        /// <summary>
        /// UIRoot 资源路径
        /// </summary>
        public string uiRootPath { get { return _uiRootPath; } }

        /// <summary>
        /// 面板资源路径
        /// </summary>
        public string panelDir { get { return _panelDir; } }

        /// <summary>
        /// 获取服务提供者实例
        /// </summary>
        /// <returns>服务提供者实例</returns>
        public override IServiceProvider NewServiceProvider()
        {
            return new UIServiceProvider();
        }

        /// <summary>
        /// 获取面板资源路径
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <returns>路径</returns>
        public string GetPanelPath(string panelName)
        {
            return Path.Combine(_panelDir, panelName);
        }
    }
}