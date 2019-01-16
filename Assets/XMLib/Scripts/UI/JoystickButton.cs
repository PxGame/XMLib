/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/3/2019 2:50:29 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XMLib.InputDriver;

namespace XMLib.UI
{
    /// <summary>
    /// 摇杆按钮
    /// </summary>
    [AddComponentMenu("XM Tool/UI/Joystick Button")]
    public class JoystickButton : Selectable, IPointerClickHandler, ISubmitHandler, IDragHandler
    {
        #region Event

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [Serializable]
        public class ButtonDownEvent : UnityEvent { }

        [Serializable]
        public class ButtonUpEvent : UnityEvent { }

        [Serializable]
        public class ButtonAxisEvent : UnityEvent<Vector2> { }

        [SerializeField]
        private string _buttonName = "Joystick";

        [SerializeField]
        protected string _horizontalName = "Horizontal";

        [SerializeField]
        protected string _verticalName = "Vertical";

        [SerializeField]
        protected float _radius = 50f;

        [SerializeField]
        protected RectTransform _handler;

        protected RectTransform _self;

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent _onClick;

        [FormerlySerializedAs("onUp")]
        [SerializeField]
        private ButtonUpEvent _onUp;

        [FormerlySerializedAs("onDown")]
        [SerializeField]
        private ButtonDownEvent _onDown;

        [SerializeField]
        private ButtonAxisEvent _onAxis;

        /// <summary>
        /// 按下事件
        /// </summary>
        public ButtonClickedEvent onClick
        {
            get { return _onClick; }
            set { _onClick = value; }
        }

        /// <summary>
        /// 摇杆值事件
        /// </summary>
        public ButtonAxisEvent onAxis
        {
            get { return _onAxis; }
            set { _onAxis = value; }
        }

        /// <summary>
        /// 抬起
        /// </summary>
        public ButtonUpEvent onUp
        {
            get { return _onUp; }
            set { _onUp = value; }
        }

        /// <summary>
        /// 按下
        /// </summary>
        public ButtonDownEvent onDown
        {
            get { return _onDown; }
            set { _onDown = value; }
        }

        #endregion Event

        private IInputDriver _input;

        protected JoystickButton()
        {
        }

        protected override void Awake()
        {
            base.Awake();

            _self = (RectTransform)transform;

            if (App.Handler != null)
            {
                if (App.CanMake<IInputDriver>())
                {
                    _input = App.Make<IInputDriver>();
                }
            }

            UpdateAxis();
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

            _onClick.Invoke();
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

            //
            UpdateAxis(eventData.position);

            //
            if (null != _input && !string.IsNullOrEmpty(_buttonName))
            {
                _input.SetButtonDown(_buttonName);
            }

            _onDown.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            //
            UpdateAxis();

            //
            if (null != _input && !string.IsNullOrEmpty(_buttonName))
            {
                _input.SetButtonUp(_buttonName);
            }

            _onUp.Invoke();

            //
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            UpdateAxis(eventData.position);
        }

        /// <summary>
        /// 更新摇杆
        /// </summary>
        /// <param name="position"></param>
        private void UpdateAxis(Vector2? position = null)
        {
            Vector2 selfCenterPos = _self.GetCenterPosition();
            Vector2 handlerCenterPos;

            if (null == position)
            {//还原
                handlerCenterPos = selfCenterPos;
            }
            else
            {
                handlerCenterPos = (Vector2)position - selfCenterPos;

                //校验
                float length = handlerCenterPos.magnitude;
                if (length > _radius)
                {//限制半径
                    length = _radius;
                }

                handlerCenterPos = selfCenterPos + length * handlerCenterPos.normalized;
            }

            //设置坐标
            _handler.SetCenterPosition(handlerCenterPos);

            Vector2 offset = handlerCenterPos - selfCenterPos;
            Vector2 value = offset / _radius;

            //设置值
            if (null != _input)
            {
                _input.SetAxis(_horizontalName, value.x);
                _input.SetAxis(_verticalName, value.y);
            }

            _value = value;
            _offset = offset;
        }

        public Vector2 _value;
        public Vector2 _offset;
    }
}