/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:12:16 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 服务
    /// </summary>
    public class UIService : IUIService
    {
        private readonly UIServiceSetting _setting;

        /// <summary>
        /// 根节点
        /// </summary>
        private UIRoot _uiRoot;

        /// <summary>
        /// 绑定数据字典
        /// </summary>
        private readonly Dictionary<Guid, UIPanelBindData> _panelBindDict;

        /// <summary>
        /// 面板字典
        /// </summary>
        private readonly Dictionary<string, List<Guid>> _panelNameDict;

        /// <summary>
        /// 堆栈面板
        /// </summary>
        private readonly Stack<Guid> _panelStack;

        /// <summary>
        /// 非堆栈面板
        /// </summary>
        private readonly List<Guid> _panelList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting">设置</param>
        public UIService(UIServiceSetting setting, UIRoot uiRoot)
        {
            _setting = setting;

            //初始化对象
            _panelBindDict = new Dictionary<Guid, UIPanelBindData>();
            _panelNameDict = new Dictionary<string, List<Guid>>();
            _panelStack = new Stack<Guid>();
            _panelList = new List<Guid>();
        }

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
            //创建绑定数据
            UIPanelBindData bindData = CreateBindData(layerName, panelName, inStack);
            //床创建面板
            UIPanel uiPanel = CreatePanel(bindData, args);

            return bindData;
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

        #region Panel

        /// <summary>
        /// 创建绑定数据
        /// </summary>
        /// <param name="layerName">层名</param>
        /// <param name="panelName">面板名</param>
        /// <param name="inStack">是否在堆栈</param>
        /// <returns>绑定数据</returns>
        private UIPanelBindData CreateBindData(string layerName, string panelName, bool inStack)
        {
            UIPanelBindData data = new UIPanelBindData(layerName, panelName, inStack);

            return data;
        }

        /// <summary>
        /// 创建面板
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="args">参数</param>
        /// <returns>面板实例</returns>
        private UIPanel CreatePanel(UIPanelBindData bindData, params object[] args)
        {
            UIPanel uiPanel = null;

            return uiPanel;
        }

        private GameObject LoadPanel(UIPanelBindData bindData)
        {
            return null;
        }

        #endregion Panel
    }
}