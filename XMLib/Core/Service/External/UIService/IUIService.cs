/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:13:26 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using XMLib.UIService;

namespace XMLib
{
    /// <summary>
    /// UI 服务接口
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="layerName">显示层</param>
        /// <param name="panelName">面板名</param>
        /// <param name="inStack">是否在堆栈</param>
        /// <param name="callback">回调</param>
        /// <param name="args">参数</param>
        /// <returns>绑定数据</returns>
        IUIPanelBindData Show(string layerName, string panelName, bool inStack, Action<UIOperationStatus, IUIPanelBindData> callback, params object[] args);

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="callback">回调</param>
        /// <returns>是否成功</returns>
        bool Hide(IUIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback);

        /// <summary>
        /// 获取绑定数据
        /// </summary>
        /// <param name="id">面板id</param>
        /// <returns>绑定数据</returns>
        IUIPanelBindData GetBind(Guid id);

        /// <summary>
        /// 获取面板实例
        /// </summary>
        /// <param name="id">面板id</param>
        /// <returns>面板实例</returns>
        UIPanel GetPanel(Guid id);

        /// <summary>
        /// 获取面板id
        /// </summary>
        /// <param name="uiPanel">面板实例</param>
        /// <returns>面板id</returns>
        Guid GetID(UIPanel uiPanel);
    }
}