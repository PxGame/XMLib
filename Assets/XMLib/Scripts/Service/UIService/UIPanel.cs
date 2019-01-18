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
    public abstract class UIPanel : MonoBehaviour
    {
        #region Panel Event

        #region Enter

        /// <summary>
        /// 预进入
        /// </summary>
        public abstract void OnPreEnter();

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public abstract void OnEnter(Action onCompete);

        /// <summary>
        /// 完成进入
        /// </summary>
        public abstract void OnLateEnter();

        #endregion Enter

        #region Leave

        /// <summary>
        /// 预离开
        /// </summary>
        public abstract void OnPreLeave();

        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public abstract void OnLeave(Action onCompete);

        /// <summary>
        /// 离开后
        /// </summary>
        public abstract void OnLateLeave();

        #endregion Leave

        #region Pause

        /// <summary>
        /// 预暂停
        /// </summary>
        public abstract void OnPrePause();

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public abstract void OnPause(Action onCompete);

        /// <summary>
        /// 暂停完成
        /// </summary>
        public abstract void OnLatePause();

        #endregion Pause

        #region Resume

        /// <summary>
        /// 预唤醒
        /// </summary>
        public abstract void OnPreResume();

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="onCompete">完成回调</param>
        public abstract void OnResume(Action onCompete);

        /// <summary>
        /// 唤醒完成
        /// </summary>
        public abstract void OnLateResume();

        #endregion Resume

        #endregion Panel Event
    }
}