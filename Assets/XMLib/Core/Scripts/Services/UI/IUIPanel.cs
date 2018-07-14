using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 面板接口
    /// </summary>
    public abstract class IUIPanel : MonoBehaviour
    {
        #region Public members

        /// <summary>
        /// 面板名字
        /// </summary>
        public string PanelName { get { return _panelName; } }

        #endregion Public members

        #region Protected memebers

        /// <summary>
        /// UI服务
        /// </summary>
        protected UIService Service { get { return _service; } }

        #endregion Protected memebers

        #region Private memebers

        [SerializeField]
        private string _panelName;

        private UIService _service;

        #endregion Private memebers

        #region Base

        /// <summary>
        /// 进入
        /// </summary>
        internal abstract void Enter();

        /// <summary>
        /// 退出
        /// </summary>
        internal abstract void Leave();

        /// <summary>
        /// 暂停
        /// </summary>
        internal abstract void Pause();

        /// <summary>
        /// 唤醒
        /// </summary>
        internal abstract void Resume();

        /// <summary>
        /// 窗口创建
        /// </summary>
        /// <param name="service"></param>
        internal void Create(UIService service)
        {
            _service = service;
            OnCreate();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        internal void Delete()
        {
            OnDelete();
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        internal void ResetStatus()
        {
            OnResetStatus();
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="root">父节点</param>
        internal void SetRoot(Transform root)
        {
            transform.SetParent(root, false);
        }

        #endregion Base

        #region Other

        /// <summary>
        /// 默认初始化
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// 窗口创建
        /// </summary>
        protected virtual void OnCreate()
        {
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected virtual void OnDelete()
        {
            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual void OnResetStatus()
        {
        }

        #endregion Other

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_panelName))
            {
                _panelName = gameObject.name;
            }
        }

#endif
    }
}