/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/18/2019 2:14:33 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.UIService
{
    /// <summary>
    /// 面板
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIPanel : MonoBehaviour
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        protected CanvasGroup canvasGroup { get; private set; }

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        #region Panel Event

        #region Enter

        /// <summary>
        /// 预进入
        /// </summary>
        public virtual void OnPreEnter()
        {
        }

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public virtual void OnEnter(Action onCompete)
        {
            if (null != onCompete)
            {
                onCompete();
            }
        }

        /// <summary>
        /// 完成进入
        /// </summary>
        public virtual void OnLateEnter()
        {
            //启用输入
            canvasGroup.interactable = true;
        }

        #endregion Enter

        #region Leave

        /// <summary>
        /// 预离开
        /// </summary>
        public virtual void OnPreLeave()
        {
            //禁用输入
            canvasGroup.interactable = false;
        }

        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public virtual void OnLeave(Action onCompete)
        {
            if (null != onCompete)
            {
                onCompete();
            }
        }

        /// <summary>
        /// 离开后
        /// </summary>
        public virtual void OnLateLeave()
        {
        }

        #endregion Leave

        #region Pause

        /// <summary>
        /// 预暂停
        /// </summary>
        public virtual void OnPrePause()
        {
            //禁用输入
            canvasGroup.interactable = false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public virtual void OnPause(Action onCompete)
        {
            if (null != onCompete)
            {
                onCompete();
            }
        }

        /// <summary>
        /// 暂停完成
        /// </summary>
        public virtual void OnLatePause()
        {
        }

        #endregion Pause

        #region Resume

        /// <summary>
        /// 预唤醒
        /// </summary>
        public virtual void OnPreResume()
        {
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public virtual void OnResume(Action onCompete)
        {
            if (null != onCompete)
            {
                onCompete();
            }
        }

        /// <summary>
        /// 唤醒完成
        /// </summary>
        public virtual void OnLateResume()
        {
            //启用输入
            canvasGroup.interactable = true;
        }

        #endregion Resume

        #endregion Panel Event
    }
}