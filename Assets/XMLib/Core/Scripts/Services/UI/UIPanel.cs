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

        internal override void Enter()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
            _canvasGroup.interactable = true;
        }

        internal override void Leave()
        {
            _canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }

        internal override void Pause()
        {
            _canvasGroup.interactable = false;
        }

        internal override void Resume()
        {
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