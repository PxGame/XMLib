using System;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 基于UGUI的面板
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : IUIPanel
    {
        #region protected members

        protected CanvasGroup _canvasGroup;

        #endregion protected members

        #region IUIPanel

        protected override void OnCreate()
        {
            base.OnCreate();

            gameObject.SetActive(false);
            _canvasGroup.interactable = false;
        }

        internal override void OnPreEnter()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            base.OnPreEnter();
        }

        internal override void OnLateEnter()
        {
            base.OnLateEnter();
            _canvasGroup.interactable = true;
        }

        internal override void OnPreLeave()
        {
            _canvasGroup.interactable = false;
            base.OnPreLeave();
        }

        internal override void OnLateLeave()
        {
            base.OnLateLeave();
            gameObject.SetActive(false);
        }

        internal override void OnPrePause()
        {
            _canvasGroup.interactable = false;
            base.OnPrePause();
        }

        internal override void OnLateResume()
        {
            base.OnLateResume();
            _canvasGroup.interactable = true;
        }

        #endregion IUIPanel

        #region Editor

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            if (null == _canvasGroup)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

#endif

        #endregion Editor
    }
}