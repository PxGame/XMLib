/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/14/2019 10:12:16 PM
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// UI 服务
    /// </summary>
    public class UIService
    {
        /// <summary>
        /// 根节点
        /// </summary>
        private UIRoot _uiRoot;

        /// <summary>
        /// 绑定数据字典
        /// </summary>
        private readonly Dictionary<Guid, UIPanelBindData> _panelBindDict;

        /// <summary>
        /// 反向查找面板实例字典
        /// </summary>
        private readonly Dictionary<UIPanel, Guid> _panelRevertDict;

        /// <summary>
        /// 面板实例字典
        /// </summary>
        private readonly Dictionary<Guid, UIPanel> _panelDict;

        /// <summary>
        /// 堆栈面板
        /// </summary>
        private readonly List<Guid> _panelStack;

        /// <summary>
        /// 非堆栈面板
        /// </summary>
        private readonly List<Guid> _panelList;

        /// <summary>
        /// 设置
        /// </summary>
        private readonly AppSetting _setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting">设置</param>
        public UIService(AppSetting setting)
        {
            _setting = setting;

            //初始化对象
            _panelDict = new Dictionary<Guid, UIPanel>();
            _panelRevertDict = new Dictionary<UIPanel, Guid>();
            _panelBindDict = new Dictionary<Guid, UIPanelBindData>();
            _panelStack = new List<Guid>();
            _panelList = new List<Guid>();
        }

        public string GetPanelPath(string panelName)
        {
            return null;
        }

        #region IUIService

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="layerName">显示层</param>
        /// <param name="panelName">面板名</param>
        /// <param name="inStack">是否在堆栈</param>
        /// <param name="callback">回调</param>
        /// <param name="args">参数</param>
        /// <returns>绑定数据</returns>
        public IUIPanelBindData Show(string layerName, string panelName, bool inStack, Action<UIOperationStatus, IUIPanelBindData> callback, params object[] args)
        {
            //创建绑定数据
            UIPanelBindData bindData = CreateBindData(layerName, panelName, inStack);

            //
            if (inStack)
            {
                ShowInStack(bindData, callback);
            }
            else
            {
                ShowNotStack(bindData, callback);
            }

            return bindData;
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="callback">回调</param>
        /// <returns>是否成功</returns>
        public bool Hide(IUIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback)
        {
            if (!_panelBindDict.ContainsKey(bindData.id))
            {
                Debug.LogWarningFormat("面板已不存在 : {0}", bindData);
                return false;
            }

            bool bRet = false;
            if (bindData.inStack)
            {
                bRet = HideInStack(bindData, callback);
            }
            else
            {
                bRet = HideNotStack(bindData, callback);
            }

            return bRet;
        }

        /// <summary>
        /// 获取绑定数据
        /// </summary>
        /// <param name="id">面板id</param>
        /// <returns>绑定数据</returns>
        public IUIPanelBindData GetBind(Guid id)
        {
            UIPanelBindData bindData = null;
            if (!_panelBindDict.TryGetValue(id, out bindData))
            {
                return null;
            }

            return bindData;
        }

        /// <summary>
        /// 获取面板实例
        /// </summary>
        /// <param name="id">面板id</param>
        /// <returns>面板实例</returns>
        public UIPanel GetPanel(Guid id)
        {
            UIPanel uiPanel = null;

            if (!_panelDict.TryGetValue(id, out uiPanel))
            {
                return null;
            }
            return uiPanel;
        }

        /// <summary>
        /// 获取面板id
        /// <para>未找到返回  <see cref="Guid.Empty"/> </para>
        /// </summary>
        /// <param name="uiPanel">面板实例</param>
        /// <returns>面板id，未找到返回 Guid.Empty </returns>
        public Guid GetID(UIPanel uiPanel)
        {
            Guid guid;
            if (!_panelRevertDict.TryGetValue(uiPanel, out guid))
            {
                return Guid.Empty;
            }

            return guid;
        }

        #endregion IUIService

        #region Panel

        /// <summary>
        /// 隐藏非堆栈面板
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="callback">回调</param>
        /// <returns>是否成功</returns>
        private bool HideNotStack(IUIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback)
        {
            //面板实例
            UIPanel uiPanel = _panelDict[bindData.id];

            //移除
            _panelDict.Remove(bindData.id);
            _panelRevertDict.Remove(uiPanel);
            _panelList.Remove(bindData.id);
            _panelBindDict.Remove(bindData.id);

            //预退出
            uiPanel.OnPreLeave();
            if (null != callback) { callback(UIOperationStatus.PreLeave, bindData); }

            //退出
            uiPanel.OnLeave(() =>
            {
                if (null != callback) { callback(UIOperationStatus.Leave, bindData); }

                //退出后
                uiPanel.OnLateLeave();
                if (null != callback) { callback(UIOperationStatus.LateLeave, bindData); }

                //销毁面板
                DestroyPanel(bindData, uiPanel);
            });

            return true;
        }

        /// <summary>
        /// 隐藏堆栈面板
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="callback">回调</param>
        /// <returns>是否成功</returns>
        private bool HideInStack(IUIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback)
        {
            //面板实例
            UIPanel uiPanel = _panelDict[bindData.id];

            //获取在堆栈中的序号
            int index = _panelStack.FindIndex((t) => t == bindData.id);
            //是否为顶层窗口
            bool isTop = _panelStack.Count == (index + 1);

            //处理id
            _panelDict.Remove(bindData.id);
            _panelRevertDict.Remove(uiPanel);
            _panelStack.Remove(bindData.id);
            _panelBindDict.Remove(bindData.id);

            if (isTop && 0 < _panelStack.Count)
            { //为顶层窗口,且堆栈中还有窗口
                //顶层面板GUID
                Guid topGuid = _panelStack[_panelStack.Count - 1];
                //顶层面板实例
                UIPanel topPanel = _panelDict[topGuid];
                //顶层面板绑定数据
                UIPanelBindData topBindData = _panelBindDict[topGuid];

                //面板预离开
                uiPanel.OnPreLeave();
                if (null != callback) { callback(UIOperationStatus.PreLeave, bindData); }

                //顶层面板预唤醒
                topPanel.OnPreResume();
                if (null != callback) { callback(UIOperationStatus.PreResume, topBindData); }

                //面板离开
                uiPanel.OnLeave(() =>
                {
                    if (null != callback) { callback(UIOperationStatus.Leave, bindData); }

                    ///顶层面板唤醒
                    topPanel.OnResume(() =>
                    {
                        if (null != callback) { callback(UIOperationStatus.Resume, topBindData); }

                        //面板离开后
                        uiPanel.OnLateLeave();
                        if (null != callback) { callback(UIOperationStatus.LateLeave, bindData); }

                        //顶层面板唤醒后
                        topPanel.OnLateResume();
                        if (null != callback) { callback(UIOperationStatus.LateResume, topBindData); }

                        //销毁面板
                        DestroyPanel(bindData, uiPanel);
                    });
                });
            }
            else
            { //非顶层窗口,或堆栈中仅此一个面板
                //面板预离开
                uiPanel.OnPreLeave();

                //面板离开
                uiPanel.OnLeave(() =>
                {
                    //面板离开后
                    uiPanel.OnLateLeave();

                    //销毁面板
                    DestroyPanel(bindData, uiPanel);
                });
            }

            return true;
        }

        /// <summary>
        /// 非堆栈显示
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="callback">回调</param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        private void ShowNotStack(UIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback, params object[] args)
        {
            //创建面板
            UIPanel uiPanel = CreatePanel(bindData, args);

            //处理id
            _panelBindDict.Add(bindData.id, bindData);
            _panelRevertDict.Add(uiPanel, bindData.id);
            _panelDict.Add(bindData.id, uiPanel);
            _panelList.Add(bindData.id);

            //预进入
            uiPanel.OnPreEnter();
            if (null != callback) { callback(UIOperationStatus.PreEnter, bindData); }

            //进入
            uiPanel.OnEnter(() =>
            {
                if (null != callback) { callback(UIOperationStatus.Enter, bindData); }

                //进入之后
                uiPanel.OnLateEnter();
                if (null != callback) { callback(UIOperationStatus.LateEnter, bindData); }
            });
        }

        /// <summary>
        /// 堆栈显示
        /// </summary>
        /// <param name="bindData">面板绑定数据</param>
        /// <param name="callback">回调</param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        private void ShowInStack(UIPanelBindData bindData, Action<UIOperationStatus, IUIPanelBindData> callback, params object[] args)
        {
            //创建面板
            UIPanel uiPanel = CreatePanel(bindData, args);

            if (0 < _panelStack.Count)
            { //存在顶层窗口
                //获取顶层窗口数据
                Guid guid = _panelStack[_panelStack.Count - 1];
                UIPanelBindData topBindData = _panelBindDict[guid];
                UIPanel topPanel = _panelDict[guid];

                //处理id
                _panelBindDict.Add(bindData.id, bindData);
                _panelDict.Add(bindData.id, uiPanel);
                _panelRevertDict.Add(uiPanel, bindData.id);
                _panelStack.Add(bindData.id);
                //

                //顶层面板预暂停
                topPanel.OnPrePause();
                if (null != callback) { callback(UIOperationStatus.PrePause, topBindData); }

                // 新面板预进入
                uiPanel.OnPreEnter();
                if (null != callback) { callback(UIOperationStatus.PreEnter, bindData); }

                //顶层面板暂停
                topPanel.OnPause(() =>
                {
                    if (null != callback) { callback(UIOperationStatus.Pause, topBindData); }

                    //新面板进入
                    uiPanel.OnEnter(() =>
                    {
                        if (null != callback) { callback(UIOperationStatus.Enter, bindData); }

                        //暂停后顶层面板
                        topPanel.OnLatePause();
                        if (null != callback) { callback(UIOperationStatus.LatePause, topBindData); }

                        //新面板启用后
                        uiPanel.OnLateEnter();
                        if (null != callback) { callback(UIOperationStatus.LateEnter, bindData); }
                    });
                });
            }
            else
            { //没有顶层窗口
                //处理id
                _panelBindDict.Add(bindData.id, bindData);
                _panelDict.Add(bindData.id, uiPanel);
                _panelRevertDict.Add(uiPanel, bindData.id);
                _panelStack.Add(bindData.id);

                //预进入
                uiPanel.OnPreEnter();
                if (null != callback) { callback(UIOperationStatus.PreEnter, bindData); }

                //进入
                uiPanel.OnEnter(() =>
                {
                    if (null != callback) { callback(UIOperationStatus.Enter, bindData); }

                    //进入之后
                    uiPanel.OnLateEnter();
                    if (null != callback) { callback(UIOperationStatus.LateEnter, bindData); }
                });
            }
        }

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

            obj.name = bindData.DisplayName;

            RectTransform rectTransform = (RectTransform)obj.transform;
            rectTransform.SetParent(_uiRoot.Get(bindData.layerName), false);
            rectTransform.SetAsLastSibling(); //移到末尾

            UIPanel uiPanel = obj.GetComponent<UIPanel>();

            //调用初始化函数
            ReflectionUtil.InvokeMethod(uiPanel, "Initialize", args);

            return uiPanel;
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="bindData">绑定数据</param>
        /// <param name="uiPanel">面板实例</param>
        private void DestroyPanel(IUIPanelBindData bindData, UIPanel uiPanel)
        {
            //销毁实例
            GameObject.Destroy(uiPanel.gameObject);
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