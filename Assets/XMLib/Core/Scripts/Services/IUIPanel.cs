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
        /// 面板状态
        /// </summary>
        public UIPanelStatus Status { get { return _status; } }

        #endregion Public members

        #region Protected memebers

        protected UIPanelStatus _status;

        /// <summary>
        /// UI服务
        /// </summary>
        protected UIService Service { get { return _service; } }

        #endregion Protected memebers

        #region Private memebers

        private UIService _service;

        #endregion Private memebers

        #region Base

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        internal IEnumerator Enter(bool useAnima)
        {
            //设置状态
            _status = UIPanelStatus.Entering;

            //完成所有动画
            KillAllAnimations();

            yield return OnEnterStart();

            yield return OnEnterAnimation(useAnima);

            //设置状态
            _status = UIPanelStatus.Enter;

            yield return OnEnterEnd();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        internal IEnumerator Leave(bool useAnima)
        {
            //设置状态
            _status = UIPanelStatus.Leaving;

            //完成所有动画
            KillAllAnimations();

            yield return OnLeaveStart();

            yield return OnLeaveAnimation(useAnima);

            //设置状态
            _status = UIPanelStatus.Leave;

            yield return OnLeaveEnd();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        internal IEnumerator Pause(bool useAnima)
        {
            //设置状态
            _status = UIPanelStatus.Pausing;

            //完成所有动画
            KillAllAnimations();

            yield return OnPauseStart();

            yield return OnPauseAnimation(useAnima);

            //设置状态
            _status = UIPanelStatus.Pause;

            yield return OnPauseEnd();
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        internal IEnumerator Resume(bool useAnima)
        {
            //设置状态
            _status = UIPanelStatus.Resuming;

            //完成所有动画
            KillAllAnimations();

            yield return OnResumeStart();

            yield return OnResumeAnimation(useAnima);

            //设置状态
            _status = UIPanelStatus.Resume;

            yield return OnResumeEnd();
        }

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

        #region Base Start End

        /// <summary>
        /// 进入开始
        /// </summary>
        protected virtual IEnumerator OnEnterStart()
        {
            yield break;
        }

        /// <summary>
        /// 进入结束
        /// </summary>
        protected virtual IEnumerator OnEnterEnd()
        {
            yield break;
        }

        /// <summary>
        /// 离开开始
        /// </summary>
        protected virtual IEnumerator OnLeaveStart()
        {
            yield break;
        }

        /// <summary>
        /// 离开结束
        /// </summary>
        protected virtual IEnumerator OnLeaveEnd()
        {
            yield break;
        }

        /// <summary>
        /// 暂停开始
        /// </summary>
        protected virtual IEnumerator OnPauseStart()
        {
            yield break;
        }

        /// <summary>
        /// 暂停结束
        /// </summary>
        protected virtual IEnumerator OnPauseEnd()
        {
            yield break;
        }

        /// <summary>
        /// 唤醒开始
        /// </summary>
        protected virtual IEnumerator OnResumeStart()
        {
            yield break;
        }

        /// <summary>
        /// 唤醒结束
        /// </summary>
        protected virtual IEnumerator OnResumeEnd()
        {
            yield break;
        }

        #endregion Base Start End

        #region Anima

        /// <summary>
        /// 强制完成所有动画
        /// </summary>
        protected virtual void KillAllAnimations()
        {
        }

        /// <summary>
        /// 进入动画
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        protected virtual IEnumerator OnEnterAnimation(bool useAnima)
        {
            yield break;
        }

        /// <summary>
        /// 离开动画
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        protected virtual IEnumerator OnLeaveAnimation(bool useAnima)
        {
            yield break;
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        protected virtual IEnumerator OnPauseAnimation(bool useAnima)
        {
            yield break;
        }

        /// <summary>
        /// 唤醒动画
        /// </summary>
        /// <param name="useAnima">是否使用动画</param>
        protected virtual IEnumerator OnResumeAnimation(bool useAnima)
        {
            yield break;
        }

        #endregion Anima
    }
}