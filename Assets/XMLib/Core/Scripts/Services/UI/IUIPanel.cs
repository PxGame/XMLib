using System;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 面板接口
    /// </summary>
    public abstract class IUIPanel : MonoBehaviour
    {
        #region protected members

        [SerializeField]
        protected string _panelName;

        [SerializeField]
        protected UIPanelStatus _status = UIPanelStatus.Leave;

        #endregion protected members

        #region private members

        private IUIService _service;

        #endregion private members

        #region public members

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
        public IUIService Service { get { return _service; } }

        #endregion public members

        #region public Function

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="service">UI服务</param>
        public void Create(IUIService service)
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
        public void Debug(DebugType debugType, string format, params string[] args)
        {
            if (null == _service)
            {
                throw new Exception("UI服务未初始化");
            }

            _service.DebugPanel(PanelName, debugType, format, args);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            GameObject.Destroy(gameObject);
            OnDispose();
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

        #endregion public Function

        #region Event

        #region Base

        /// <summary>
        /// 创建
        /// </summary>
        protected virtual void OnCreate()
        {
            Debug(DebugType.Normal, "OnCreate");
        }

        /// <summary>
        /// 销毁
        /// </summary>
        protected virtual void OnDispose()
        {
            Debug(DebugType.Normal, "OnDispose");
        }

        /// <summary>
        /// 强制完成
        /// </summary>
        protected virtual void OnForceCompleteOperation()
        {
            Debug(DebugType.Normal, "OnForceCompleteOperation");
        }

        #endregion Base

        #region Enter status

        /// <summary>
        /// 进入前
        /// </summary>
        internal virtual void OnPreEnter()
        {
            Debug(DebugType.Normal, "OnPreEnter");
            _status = UIPanelStatus.Enter;
        }

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnEnter(Action complete)
        {
            Debug(DebugType.Normal, "OnEnter");
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
            Debug(DebugType.Normal, "OnLateEnter");
        }

        #endregion Enter status

        #region Leave status

        /// <summary>
        /// 退出前
        /// </summary>
        internal virtual void OnPreLeave()
        {
            Debug(DebugType.Normal, "OnPreLeave");
            _status = UIPanelStatus.Leave;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnLeave(Action complete)
        {
            Debug(DebugType.Normal, "OnLeave");
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
            Debug(DebugType.Normal, "OnLateLeave");
        }

        #endregion Leave status

        #region Pause status

        /// <summary>
        /// 暂停前
        /// </summary>
        internal virtual void OnPrePause()
        {
            Debug(DebugType.Normal, "OnPrePause");
            _status = UIPanelStatus.Pause;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnPause(Action complete)
        {
            Debug(DebugType.Normal, "OnPause");
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
            Debug(DebugType.Normal, "OnLatePause");
        }

        #endregion Pause status

        #region Resume status

        /// <summary>
        /// 唤醒前
        /// </summary>
        internal virtual void OnPreResume()
        {
            Debug(DebugType.Normal, "OnPreResume");
            _status = UIPanelStatus.Resume;
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="complete">操作完成</param>
        internal virtual void OnResume(Action complete)
        {
            Debug(DebugType.Normal, "OnResume");
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
            Debug(DebugType.Normal, "OnLateResume");
        }

        #endregion Resume status

        #endregion Event

        #region Editor

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

        #endregion Editor
    }
}