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
        /// <summary>
        /// 设置
        /// </summary>
        public UIServiceSetting setting { get { return _setting; } }

        /// <summary>
        /// 设置
        /// </summary>
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
        /// 面板实例字典
        /// </summary>
        private readonly Dictionary<Guid, UIPanel> _panelDict;

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
            _uiRoot = uiRoot;

            //初始化对象
            _panelDict = new Dictionary<Guid, UIPanel>();
            _panelBindDict = new Dictionary<Guid, UIPanelBindData>();
            _panelStack = new Stack<Guid>();
            _panelList = new List<Guid>();
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
            _panelBindDict.Add(bindData.id, bindData);

            //创建面板
            UIPanel uiPanel = CreatePanel(bindData, args);
            _panelDict.Add(bindData.id, uiPanel);

            //
            if (inStack)
            {
                ShowInStack(bindData, uiPanel);
            }
            else
            {
                ShowNotStack(bindData, uiPanel);
            }

            return bindData;
        }

        /// <summary>
        /// 非堆栈显示
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="uiPanel">面板实例</param>
        /// <returns>是否成功</returns>
        private bool ShowNotStack(UIPanelBindData bindData, UIPanel uiPanel)
        {
            return true;
        }

        /// <summary>
        /// 堆栈显示
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="uiPanel">面板实例</param>
        /// <returns>是否成功</returns>
        private bool ShowInStack(UIPanelBindData bindData, UIPanel uiPanel)
        {
            return true;
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
            UIPanelBindData data = new UIPanelBindData(this, layerName, panelName, inStack);

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
            GameObject obj = LoadPanel(bindData);
            obj.transform.parent = _uiRoot.Get(bindData.layerName);
            obj.transform.SetAsLastSibling();//移到末尾

            UIPanel uiPanel = obj.GetComponent<UIPanel>();

            //调用初始化函数
            ReflectionUtil.InvokeMethod(uiPanel, "OnInitialize", args);

            return uiPanel;
        }

        /// <summary>
        /// 加载面板实例
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <returns>面板实例</returns>
        private GameObject LoadPanel(UIPanelBindData bindData)
        {
            GameObject preObj = Resources.Load<GameObject>(bindData.path);
            GameObject obj = GameObject.Instantiate(preObj);
            return obj;
        }

        #endregion Panel
    }
}