using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XM.Services.UI
{
    /// <summary>
    ///  UI服务
    /// </summary>
    /// <typeparam name="AE">程序入口类型</typeparam>
    public class UIService : SimpleService<AppEntry, UISetting>, IUIService
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

        protected override void OnDisposeService()
        {
        }

        #endregion SimpleService

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

        /// <summary>
        /// 获取面板
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        private IUIPanel Get(string panelName)
        {
            IUIPanel uiPanel = null;

            if (_dict.TryGetValue(panelName, out uiPanel))
            {
            }

            return uiPanel;
        }

        /// <summary>
        /// 获取顶层面板
        /// </summary>
        /// <returns></returns>
        private IUIPanel GetTop()
        {
            IUIPanel uiPanel = null;
            if (0 < _stack.Count)
            {
                string topName = _stack.Peek();
                uiPanel = Get(topName);
                if (null == uiPanel)
                {
                    Debug(DebugType.Error, "堆栈顶层面板未找到对应实例 {0}", topName);
                }
            }

            return uiPanel;
        }

        /// <summary>
        /// 调用面板初始化方法
        /// </summary>
        /// <param name="uiPanel"></param>
        /// <param name="args"></param>
        private void InvokeInitialize(IUIPanel uiPanel, params object[] args)
        {
            //初始化当前面板
            Type[] argTypes = new Type[args.Length];
            int argLength = argTypes.Length;
            for (int i = 0; i < argLength; i++)
            {
                argTypes[i] = args[i].GetType();
            }

            //Initialize 函数
            MethodInfo method;

            //先查找公开方法
            method = uiPanel.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public, null, argTypes, null);
            if (null == method)
            {//未找到时，再查找非公开的方法
                method = uiPanel.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic, null, argTypes, null);
            }

            if (null != method)
            {
                //调用初始化
                method.Invoke(uiPanel, args);
            }
            else
            {
                Debug(DebugType.Warning, "未找在面板 {0} 匹配的Initialize函数，参数数 {1}", uiPanel, argLength);
            }
        }

        /// <summary>
        ///  打开面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <param name="args">完成事件</param>
        /// <returns>是否成功</returns>
        public bool Open(string panelName, params object[] args)
        {
            return Open(panelName, null, args);
        }

        /// <summary>
        ///  打开面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        public bool Open(string panelName, Action onComplete, params object[] args)
        {
            IUIPanel topPanel = GetTop();
            IUIPanel nextPanel = Get(panelName);

            if (null == nextPanel)
            {//创建面板
                nextPanel = InstToCache(panelName);
            }
            else
            {
                if (UIPanelStatus.Leave != nextPanel.Status)
                {
                    Debug(DebugType.Error, "{0} 正在显示", panelName);
                    return false;
                }
            }

            //预处理
            if (null != topPanel)
            {
                topPanel.ForceCompleteOperation();
                topPanel.OnPrePause();
            }

            nextPanel.ForceCompleteOperation();
            nextPanel.OnPreEnter();

            //修改堆栈
            _caches.Remove(panelName);
            _stack.Push(panelName);

            //移动节点，并初始化
            nextPanel.SetRoot(_uiRoot.Normal);
            InvokeInitialize(nextPanel, args);

            //下一步
            Action nextStep = () =>
            {
                if (null != topPanel)
                {
                    //暂停后
                    topPanel.OnLatePause();
                }
                //进入后
                nextPanel.OnLateEnter();

                //完成事件
                if (null != onComplete)
                {
                    onComplete();
                }
            };

            if (null != topPanel)
            {//先暂停顶层
                topPanel.OnPause(() =>
                {
                    //进入
                    nextPanel.OnEnter(nextStep);
                });
            }
            else
            {//进入
                nextPanel.OnEnter(nextStep);
            }

            return true;
        }

        /// <summary>
        /// 关闭顶层窗口
        /// </summary>
        /// <param name="isDispose">是否删除</param>
        /// <param name="onComplate">完成</param>
        public bool CloseTop(bool isDispose = true, Action onComplate = null)
        {
            IUIPanel topPanel = GetTop();
            if (null == topPanel)
            {
                Debug(DebugType.Warning, "没有顶层窗口可关闭");
                return false;
            }

            //强制完成当前操作
            topPanel.ForceCompleteOperation();

            //预处理
            topPanel.OnPreLeave();

            //移除顶层
            string topName = _stack.Pop();

            //获取新的顶层
            IUIPanel nextPanel = GetTop();
            if (null != nextPanel)
            {
                //强制完成当前操作
                nextPanel.ForceCompleteOperation();
                //预处理
                nextPanel.OnPreResume();
            }

            //下一步
            Action nextStep = () =>
            {
                //离开后
                topPanel.OnLateLeave();

                if (null != nextPanel)
                {//唤醒后
                    nextPanel.OnLateResume();
                }

                if (isDispose)
                {//移除
                    _dict.Remove(topName);
                    topPanel.Dispose();
                }
                else
                {//添加到缓存
                    _caches.Add(topName);
                    topPanel.SetRoot(_uiRoot.Cache);
                }

                //完成
                if (null != onComplate)
                {
                    onComplate();
                }
            };

            //关闭
            if (null != nextPanel)
            {
                topPanel.OnLeave(() =>
                {
                    nextPanel.OnResume(nextStep);
                });
            }
            else
            {
                topPanel.OnLeave(nextStep);
            }

            return true;
        }

        /// <summary>
        /// 切换顶层面板
        /// </summary>
        /// <param name="panelName">面板名</param>
        /// <param name="isDispose">是否删除</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public bool SwitchTop(string panelName, bool isDispose = true, Action onComplete = null, params object[] args)
        {
            IUIPanel topPanel = GetTop();
            if (null == topPanel)
            {
                Debug(DebugType.Warning, "没有顶层窗口可关闭");
                return false;
            }

            //获取面包那
            IUIPanel nextPanel = Get(panelName);
            if (null == nextPanel)
            {//创建面板
                nextPanel = InstToCache(panelName);
            }
            else
            {
                if (UIPanelStatus.Leave != nextPanel.Status)
                {
                    Debug(DebugType.Error, "{0} 正在显示", panelName);
                    return false;
                }
            }

            //强制完成当前操作
            topPanel.ForceCompleteOperation();
            //预处理
            topPanel.OnPreLeave();
            string topName = _stack.Pop();

            nextPanel.ForceCompleteOperation();
            nextPanel.OnPreEnter();

            //修改堆栈
            _caches.Remove(panelName);
            _stack.Push(panelName);

            //移动节点，并初始化
            nextPanel.SetRoot(_uiRoot.Normal);
            InvokeInitialize(nextPanel, args);

            //下一步
            Action nextStep = () =>
            {
                //关闭后
                topPanel.OnLateLeave();
                //进入后
                nextPanel.OnLateEnter();

                if (isDispose)
                {//移除
                    _dict.Remove(topName);
                    topPanel.Dispose();
                }
                else
                {//添加到缓存
                    _caches.Add(topName);
                    topPanel.SetRoot(_uiRoot.Cache);
                }

                //完成事件
                if (null != onComplete)
                {
                    onComplete();
                }
            };

            //关闭顶层
            topPanel.OnLeave(() =>
            {
                //进入
                nextPanel.OnEnter(nextStep);
            });

            return true;
        }

        /// <summary>
        /// 关闭所有
        /// </summary>
        /// <param name="isDispose"></param>
        /// <returns></returns>
        public bool CloseAll(bool isDispose = true)
        {
            string topName = "";
            IUIPanel topPanel;
            while (0 < _stack.Count)
            {
                topName = _stack.Pop();
                topPanel = Get(topName);
                if (null == topPanel)
                {
                    Debug(DebugType.Error, "堆栈顶层面板未找到对应实例 {0}", topName);
                    return false;
                }

                //强制完成当前操作
                topPanel.ForceCompleteOperation();

                //预关闭
                topPanel.OnPreLeave();

                //关闭
                topPanel.OnLeave(() =>
                {
                    //关闭后
                    topPanel.OnLateLeave();

                    if (isDispose)
                    {//移除
                        _dict.Remove(topName);
                        topPanel.Dispose();
                    }
                    else
                    {//添加到缓存
                        _caches.Add(topName);
                        topPanel.SetRoot(_uiRoot.Cache);
                    }
                });

                //强制完成关闭操作
                topPanel.ForceCompleteOperation();
            }

            return true;
        }

        #endregion Normal Panel
    }
}