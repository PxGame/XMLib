using System;
using UnityEngine;

namespace XM.Services.UI
{
    /// <summary>
    /// 面板接口
    /// </summary>
    public abstract class IUIPanel : MonoBehaviour
    {
        #region 属性

        [SerializeField]
        protected string _panelName;

        [SerializeField]
        protected UIPanelStatus _status = UIPanelStatus.Leave;

        private UIService _service;

        /// <summary>
        /// 面板名字
        /// </summary>
        public string PanelName { get { return _panelName; } }

        /// <summary>
        /// UI状态
        /// </summary>
        public UIPanelStatus Status { get { return _status; } }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get { return string.Format("[UI]{0}", PanelName); } }

        /// <summary>
        /// UI服务
        /// </summary>
        public UIService Service { get { return _service; } }

        #endregion 属性

        #region 函数

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="service">UI服务</param>
        public void Create(UIService service)
        {
            _service = service;
            OnCreate();
        }

        /// <summary>
        /// Debug 输出
        /// </summary>
        /// <param name="debugType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Debug(DebugType debugType, string format, params object[] args)
        {
            Checker.NotNull(_service, "UI服务未初始化");

            _service.Debug(debugType, "[" + _panelName + "]" + format, args);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            OnDispose();
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// 强制完成操作
        /// </summary>
        public void ForceCompleteOperation()
        {
            OnForceCompleteOperation();
        }

        /// <summary>
        /// 设置根节点
        /// </summary>
        /// <param name="root"></param>
        public void SetRoot(Transform root)
        {
            transform.SetParent(root, false);
        }

        #endregion 函数

        #region 重写

        public override string ToString()
        {
            return string.Format("UIPanel:{0}", _panelName);
        }

        #endregion 重写

        #region 回调

        /// <summary>
        /// 创建
        /// </summary>
        protected virtual void OnCreate()
        {
            Debug(DebugType.Debug, "OnCreate");
        }

        /// <summary>
        /// 销毁
        /// </summary>
        protected virtual void OnDispose()
        {
            Debug(DebugType.Debug, "OnDispose");
        }

        /// <summary>
        /// 强制完成
        /// </summary>
        protected virtual void OnForceCompleteOperation()
        {
            Debug(DebugType.Debug, "OnForceCompleteOperation");
        }

        #region 进入

        /// <summary>
        /// 进入前
        /// </summary>
        internal virtual void OnPreEnter()
        {
            Debug(DebugType.Debug, "OnPreEnter");
            _status = UIPanelStatus.Enter;
        }

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnEnter(Action complete)
        {
            Debug(DebugType.Debug, "OnEnter");
            if (null != complete)
            {
                complete();
            }
        }

        /// <summary>
        /// 进入后
        /// </summary>
        internal virtual void OnLateEnter()
        {
            Debug(DebugType.Debug, "OnLateEnter");
        }

        #endregion 进入

        #region 离开

        /// <summary>
        /// 退出前
        /// </summary>
        internal virtual void OnPreLeave()
        {
            Debug(DebugType.Debug, "OnPreLeave");
            _status = UIPanelStatus.Leave;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnLeave(Action complete)
        {
            Debug(DebugType.Debug, "OnLeave");
            if (null != complete)
            {
                complete();
            }
        }

        /// <summary>
        /// 退出后
        /// </summary>
        internal virtual void OnLateLeave()
        {
            Debug(DebugType.Debug, "OnLateLeave");
        }

        #endregion 离开

        #region 暂停

        /// <summary>
        /// 暂停前
        /// </summary>
        internal virtual void OnPrePause()
        {
            Debug(DebugType.Debug, "OnPrePause");
            _status = UIPanelStatus.Pause;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnPause(Action complete)
        {
            Debug(DebugType.Debug, "OnPause");
            if (null != complete)
            {
                complete();
            }
        }

        /// <summary>
        /// 暂停后
        /// </summary>
        internal virtual void OnLatePause()
        {
            Debug(DebugType.Debug, "OnLatePause");
        }

        #endregion 暂停

        #region 恢复

        /// <summary>
        /// 唤醒前
        /// </summary>
        internal virtual void OnPreResume()
        {
            Debug(DebugType.Debug, "OnPreResume");
            _status = UIPanelStatus.Resume;
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnResume(Action complete)
        {
            Debug(DebugType.Debug, "OnResume");
            if (null != complete)
            {
                complete();
            }
        }

        /// <summary>
        /// 唤醒后
        /// </summary>
        internal virtual void OnLateResume()
        {
            Debug(DebugType.Debug, "OnLateResume");
        }

        #endregion 恢复

        #endregion 回调

        #region 编辑器

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_panelName))
            {
                _panelName = gameObject.name;

                if (System.Text.RegularExpressions.Regex.IsMatch(_panelName, "^.+(P|p)anel$"))
                {
                    _panelName.Remove(_panelName.Length - 6, 5);
                }
            }
        }

#endif

        #endregion 编辑器
    }
}