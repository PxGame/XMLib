/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:12:16 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 服务
    /// </summary>
    public class UIService : IUIService
    {
        /// <summary>
        /// 根节点
        /// </summary>
        private UIRoot _uiRoot;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uiRoot">根节点</param>
        public void Init(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="layerName">层名</param>
        /// <param name="panelName">面板名</param>
        /// <param name="inStack">是否放入堆栈</param>
        /// <param name="args">参数</param>
        /// <returns>绑定数据</returns>
        public IUIPanelBindData Show(string layerName, string panelName, bool inStack, params object[] args)
        {
            return null;
        }

        /// <summary>
        /// 隐藏指定面板
        /// </summary>
        /// <param name="paneId"></param>
        /// <returns></returns>
        public bool Hide(Guid paneId)
        {
            return true;
        }
    }
}