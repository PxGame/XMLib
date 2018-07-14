using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    ///  UI服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class UIService<AE> : SimpleService<AE, UISetting>, IUIService where AE : IAppEntry<AE>
    {
        #region Private members

        /// <summary>
        /// 根节点实例
        /// </summary>
        private GameObject _root;

        /// <summary>
        /// 根节点控制脚本
        /// </summary>
        private UIRoot _uiRoot;

        /// <summary>
        /// 已实例化的面板
        /// </summary>
        private Dictionary<string, IUIPanel> _panelPool = new Dictionary<string, IUIPanel>();

        /// <summary>
        /// 已显示的堆栈中的面板
        /// </summary>
        private Stack<string> _panelStack = new Stack<string>();

        /// <summary>
        /// 已实例化未使用的面板
        /// </summary>
        private List<string> _panelCache = new List<string>();

        #endregion Private members

        #region Base

        protected override void OnAddService()
        {
        }

        protected override void OnInitService()
        {
            //创建根节点
            GameObject rootObj = Setting.GetRoot();
            _root = GameObject.Instantiate(rootObj);
            _root.name = rootObj.name;
            GameObject.DontDestroyOnLoad(_root);

            //获取UIRoot
            _uiRoot = _root.GetComponent<UIRoot>();
        }

        protected override void OnRemoveService()
        {
            if (null != _root)
            {
                GameObject.Destroy(_root);
                _root = null;
                _uiRoot = null;
            }
        }

        protected override void OnClearService()
        {
        }

        #endregion Base

        #region Base Operation

        /// <summary>
        /// 获取已实例化面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <returns></returns>
        private IUIPanel GetPanel(string panelName)
        {
            IUIPanel panel = null;
            if (_panelPool.TryGetValue(panelName, out panel))
            {
            }

            return panel;
        }

        /// <summary>
        /// 获取面板预制
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <returns></returns>
        private GameObject GetPrePanel(string panelName)
        {
            GameObject panelObj = Setting.GetPanel(panelName);

            if (null == panelObj)
            {
                throw new Exception("该面板未找到预制:" + panelName);
            }

            return panelObj;
        }

        /// <summary>
        /// 该面板是否已经实例化
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <returns></returns>
        private bool ExistPanel(string panelName)
        {
            return _panelPool.ContainsKey(panelName);
        }

        /// <summary>
        /// 如果面板不存在则创建面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <returns></returns>
        private IUIPanel CreatePanel(string panelName)
        {
            IUIPanel panel = null;
            if (!ExistPanel(panelName))
            {//不存在则实例化
                InstPanelToCache(panelName);
            }

            //
            panel = GetPanel(panelName);

            return panel;
        }

        /// <summary>
        /// 实例化面板到缓存
        /// </summary>
        /// <param name="panelName">面板名</param>
        private void InstPanelToCache(string panelName)
        {
            GameObject panelPreObj = GetPrePanel(panelName);
            GameObject panelObj = GameObject.Instantiate(panelPreObj);
            panelObj.name = panelPreObj.name;
            IUIPanel uiPanel = panelObj.GetComponent<IUIPanel>();

            //添加到面板池中
            _panelPool.Add(panelName, uiPanel);

            //新实例化的面板首先放到缓存中
            _panelCache.Add(panelName);
            uiPanel.SetRoot(_uiRoot.Cache);
            uiPanel.Create((IUIService)this);
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void ClearCache()
        {
            IUIPanel uiPanel = null;
            string panelName;
            int length = _panelCache.Count;

            for (int i = 0; i < length; i++)
            {
                panelName = _panelCache[i];

                uiPanel = GetPanel(panelName);
                if (null == uiPanel)
                {
                    throw new Exception("发现面板缓存上有面板丢失引用:" + panelName);
                }

                _panelPool.Remove(panelName);
                uiPanel.Delete();
            }

            _panelCache.Clear();
        }

        /// <summary>
        /// 删除所有面板
        /// </summary>
        public void Clear()
        {
            IUIPanel panel;
            foreach (var panelPair in _panelPool)
            {
                panel = panelPair.Value;

                if (null == panel)
                {
                }

                panel.Delete();
            }
        }

        #endregion Base Operation

        #region Stack Operation

        /// <summary>
        /// 获取堆栈顶层面板
        /// </summary>
        /// <returns></returns>
        private IUIPanel GetTopPanel()
        {
            IUIPanel panel = null;

            if (0 < _panelStack.Count)
            {
                string topPanelName = _panelStack.Peek();
                panel = GetPanel(topPanelName);

                if (null == panel)
                {
                    throw new Exception("发现面板堆栈上有面板丢失引用:" + topPanelName);
                }
            }

            return panel;
        }

        #endregion Stack Operation

        #region Operation

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public void ShowPanel(string panelName, params object[] args)
        {
            if (_panelStack.Contains(panelName))
            {//已经在堆栈中
                Debug(DebugType.Warning, "{0} 已在显示面板堆栈中", panelName);
                return;
            }

            IUIPanel uiPanel = CreatePanel(panelName);

            IUIPanel topPanel = GetTopPanel();
            if (null != topPanel)
            {//暂停顶层面板
                topPanel.Pause();
            }

            //设置根节点
            uiPanel.SetRoot(_uiRoot.Normal);

            //缓存中移除
            _panelCache.Remove(panelName);

            //添加到堆中
            _panelStack.Push(panelName);

            //

            //初始化当前面板
            Type[] argTypes = new Type[args.Length];
            int argLength = argTypes.Length;
            for (int i = 0; i < argLength; i++)
            {
                argTypes[i] = args[i].GetType();
            }

            //Initialize 函数
            MethodInfo method;

            //先查找非public方法
            method = uiPanel.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic, null, argTypes, null);
            if (null == method)
            {//未找到时，再查找public的方法
                method = uiPanel.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public, null, argTypes, null);
            }

            if (null != method)
            {
                //调用初始化
                method.Invoke(uiPanel, args);
            }
            else
            {
                Debug(DebugType.Warning, "未找在面板 {0} 匹配的Initialize函数，参数数 {1}", panelName, argLength);
            }
            //

            //打开当前面板
            uiPanel.Enter();
        }

        /// <summary>
        /// 隐藏顶层面板
        /// </summary>
        /// <returns></returns>
        public void HidePanel()
        {
            IUIPanel topPanel = GetTopPanel();
            if (null == topPanel)
            {//没有顶层面板
                Debug(DebugType.Warning, "没有顶层窗口需要隐藏");
                return;
            }

            //关闭顶层面板
            topPanel.Leave();

            string topPanelName = topPanel.PanelName;

            //设置根节点
            topPanel.SetRoot(_uiRoot.Cache);

            //堆中中移除
            _panelStack.Pop();

            //添加到缓存
            _panelCache.Add(topPanelName);

            //

            //获取新的顶层面板
            IUIPanel newTopPanel = GetTopPanel();
            if (null != newTopPanel)
            {//唤醒新的顶层面板
                newTopPanel.Resume();
            }
        }

        #endregion Operation
    }
}