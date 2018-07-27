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
        #region private members

        private GameObject _uiRootObj;
        private UIRoot _uiRoot;

        #endregion private members

        #region SimpleService

        protected override void OnCreateService()
        {
            //实例化根节点
            GameObject preUIRoot = Setting.GetRoot();
            _uiRootObj = GameObject.Instantiate(preUIRoot);
            _uiRoot = _uiRootObj.GetComponent<UIRoot>();
            //
        }

        protected override void OnClearService()
        {
        }

        protected override void OnInitService()
        {
        }

        protected override void OnRemoveService()
        {
        }

        #endregion SimpleService

        #region IUIService

        /// <summary>
        /// Debug 输出
        /// </summary>
        /// <param name="debugType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        ///
        public void DebugPanel(DebugType debugType, string format, params string[] args)
        {
            Debug(debugType, format, args);
        }

        #endregion IUIService

        #region Normal Panel

        private Dictionary<string, IUIPanel> _dict = new Dictionary<string, IUIPanel>();
        private Stack<string> _stack = new Stack<string>();
        private List<string> _caches = new List<string>();

        /// <summary>
        /// 实例化面板到缓存中
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        private IUIPanel InstToCache(string panelName)
        {
            if (_dict.ContainsKey(panelName))
            {
                Debug(DebugType.Error, "{0} 已经实例化", panelName);
                return null;
            }

            GameObject prePanel = Setting.GetPanel(panelName);
            if (null == prePanel)
            {
                Debug(DebugType.Error, "未找到 {0} 预制", panelName);
                return null;
            }

            GameObject panel = GameObject.Instantiate(prePanel);
            IUIPanel uiPanel = panel.GetComponent<IUIPanel>();

            uiPanel.SetRoot(_uiRoot.Cache);
            uiPanel.Create(this);

            _dict.Add(panelName, uiPanel);
            _caches.Add(panelName);

            return uiPanel;
        }

        #endregion Normal Panel
    }
}