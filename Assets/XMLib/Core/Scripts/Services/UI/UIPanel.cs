using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 基于UGUI的面板
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : IUIPanel
    {
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        #region Base

        protected override IEnumerator OnEnterStart()
        {
            yield return base.OnEnterStart();

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        protected override IEnumerator OnEnterEnd()
        {
            yield return base.OnEnterEnd();

            _canvasGroup.interactable = true;
        }

        protected override IEnumerator OnLeaveStart()
        {
            yield return base.OnLeaveStart();

            _canvasGroup.interactable = false;
        }

        protected override IEnumerator OnLeaveEnd()
        {
            yield return base.OnLeaveEnd();

            gameObject.SetActive(false);
        }

        protected override IEnumerator OnPauseStart()
        {
            yield return base.OnPauseStart();

            _canvasGroup.interactable = false;
        }

        protected override IEnumerator OnPauseEnd()
        {
            yield return base.OnPauseEnd();
        }

        protected override IEnumerator OnResumeStart()
        {
            yield return base.OnResumeStart();
        }

        protected override IEnumerator OnResumeEnd()
        {
            yield return base.OnResumeEnd();

            _canvasGroup.interactable = true;
        }

        #endregion Base

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
    }
}