/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/3/2019 11:17:16 AM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XMLib.InputService;

namespace XMLib.UI
{
    /// <summary>
    /// 按钮
    /// </summary>
    [AddComponentMenu("XM Tool/UI/Button")]
    public class Button : Selectable, IPointerClickHandler, ISubmitHandler
    {
        #region Event

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [Serializable]
        public class ButtonDownEvent : UnityEvent { }

        [Serializable]
        public class ButtonUpEvent : UnityEvent { }

        [SerializeField]
        private string _buttonName = "Button";

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent _onClick;

        [FormerlySerializedAs("onUp")]
        [SerializeField]
        private ButtonUpEvent _onUp;

        [FormerlySerializedAs("onDown")]
        [SerializeField]
        private ButtonDownEvent _onDown;

        public ButtonClickedEvent onClick
        {
            get { return _onClick; }
            set { _onClick = value; }
        }

        public ButtonUpEvent onUp
        {
            get { return _onUp; }
            set { _onUp = value; }
        }

        public ButtonDownEvent onDown
        {
            get { return _onDown; }
            set { _onDown = value; }
        }

        #endregion Event

        private IInputService _input;

        protected Button()
        {
        }

        protected override void Awake()
        {
            base.Awake();

            if (App.Handler != null)
            {
                if (App.CanMake<IInputService>())
                {
                    _input = App.Make<IInputService>();
                }
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnClick();
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            OnDown();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            OnUp();
        }

        #region 事件

        private void OnUp()
        {
            if (!string.IsNullOrEmpty(_buttonName))
            {
                if (null != _input)
                {
                    _input.SetButtonUp(_buttonName);
                }
            }

            _onUp.Invoke();
        }

        private void OnDown()
        {
            if (!string.IsNullOrEmpty(_buttonName))
            {
                if (null != _input)
                {
                    _input.SetButtonDown(_buttonName);
                }
            }

            _onDown.Invoke();
        }

        private void OnClick()
        {
            _onClick.Invoke();
        }

        #endregion 事件
    }
}