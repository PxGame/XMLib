using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI服务
    /// </summary>
    public class UIService : IService
    {
        #region Private members

        /// <summary>
        /// 根节点预制
        /// </summary>
        private GameObject _preRoot;

        /// <summary>
        /// 面板预制字典
        /// </summary>
        private Dictionary<string, GameObject> _prePanelDict;

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

        #region Status

        private bool _isOperating = false;

        #endregion Status

        #region Base

        protected override void OnAddService()
        {
            //获取设置
            IUISettingValue setting = Entry.Settings as IUISettingValue;
            if (null == setting)
            {
                throw new System.Exception("启用对象池服务后,服务设置机核必须实现IPoolSettingValue接口.");
            }

            _prePanelDict = setting.UISetting.GetPanelDict();
            _preRoot = setting.UISetting.GetRoot();
        }

        protected override void OnInitService()
        {
            //创建根节点
            _root = GameObject.Instantiate(_preRoot);
            _root.name = _preRoot.name;
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
            GameObject panelObj = null;

            if (!_prePanelDict.TryGetValue(panelName, out panelObj))
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
            uiPanel.Create(this);
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
        /// <param name="useAnima">是否使用动画</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public bool ShowPanel(string panelName, bool useAnima, Action onComplete, params object[] args)
        {
            if (_isOperating)
            {//是否有面板正在显示
                Debug(DebugType.Warning, "有面板正在操作中，当前显示操作失败：{0}", panelName);
                return false;
            }

            IUIPanel uiPanel = CreatePanel(panelName);
            if (uiPanel.IsDisplay)
            {//该面板未显示
                Debug(DebugType.Warning, "该面板已显示，当前显示操作失败：{0}", panelName);
                return false;
            }

            //开始操作
            _isOperating = true;

            //执行协程
            Entry.StartCoroutine(_ShowPanel(uiPanel, useAnima, onComplete, args));

            return true;
        }

        /// <summary>
        /// 隐藏顶层面板
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        /// <param name="onComplete">完成事件</param>
        /// <returns></returns>
        public bool HidePanel(bool useAnima, Action onComplete)
        {
            if (_isOperating)
            {//是否有面板正在显示
                Debug(DebugType.Warning, "有面板正在操作中，当前隐藏操作失败");
                return false;
            }

            IUIPanel topPanel = GetTopPanel();
            if (null == topPanel)
            {//没有顶层面板
                return false;
            }

            //开始操作
            _isOperating = true;

            //执行协程
            Entry.StartCoroutine(_HidePanel(topPanel, useAnima, onComplete));

            return true;
        }

        #endregion Operation

        #region Operation coroutines

        /// <summary>
        /// 显示携程
        /// </summary>
        /// <param name="uiPanel">面板实例</param>
        /// <param name="useAnima">是否使用动画</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        private IEnumerator _ShowPanel(IUIPanel uiPanel, bool useAnima, Action onComplete, params object[] args)
        {
            IUIPanel topPanel = GetTopPanel();
            if (null != topPanel)
            {//暂停顶层面板
                yield return topPanel.Pause(useAnima);
            }

            string panelName = uiPanel.PanelName;

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

            //Initialize 函数不能是public
            MethodInfo method = uiPanel.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic, null, argTypes, null);
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
            yield return uiPanel.Enter(useAnima);

            //操作完成
            _isOperating = false;

            if (null != onComplete)
            {
                onComplete();
            }
        }

        /// <summary>
        /// 关闭协程
        /// </summary>
        /// <param name="topPanel">顶层面板</param>
        /// <param name="useAnim">是否使用动画</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        private IEnumerator _HidePanel(IUIPanel topPanel, bool useAnim, Action onComplete)
        {
            //关闭顶层面板
            yield return topPanel.Leave(useAnim);

            string topPanelName = topPanel.PanelName;

            //设置根节点
            topPanel.SetRoot(_uiRoot.Cache);

            //堆中中移除
            _panelCache.Add(topPanelName);

            //添加到缓存
            _panelStack.Pop();

            //

            //获取新的顶层面板
            IUIPanel newTopPanel = GetTopPanel();
            if (null != newTopPanel)
            {//唤醒新的顶层面板
                yield return newTopPanel.Resume(useAnim);
            }

            //操作完成
            _isOperating = false;

            if (null != onComplete)
            {
                onComplete();
            }
        }

        #endregion Operation coroutines
    }
}