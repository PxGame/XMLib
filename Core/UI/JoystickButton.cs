/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/3/2019 2:50:29 PM
 */

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace XMLib
{
    /// <summary>
    /// 摇杆按钮
    /// </summary>
    [AddComponentMenu("XMLib/UI/Joystick Button")]
    [RequireComponent(typeof(CanvasGroup))]
    public class JoystickButton : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler
    {
        /// <summary>
        /// 状态
        /// </summary>
        private enum Status
        {
            Down,
            Up,
            Drag,
            Reset
        }

        #region Event

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

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

        [SerializeField]
        protected RectTransform _background;

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent _onClick;

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

        #endregion Event

        private InputService _input;
        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();

            if (AppManager.Inst != null)
            {
                if (AppManager.CanMake<InputService>())
                {
                    _input = AppManager.Make<InputService>();
                }
            }

            _canvasGroup = GetComponent<CanvasGroup>();

            UpdateAxis(Status.Reset, Vector2.zero);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive())
                return;

            OnClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            //
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out point);
            UpdateAxis(Status.Down, point);

            //
            OnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            //
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out point);
            UpdateAxis(Status.Up, point);

            //
            OnUp();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out point);
            UpdateAxis(Status.Drag, point);
        }

        private Vector2 _centerPosition;

        /// <summary>
        /// 更新摇杆
        /// </summary>
        private void UpdateAxis(Status status, Vector2 position)
        {
            Vector2 handlerPosition;
            if (status == Status.Drag)
            {
                _canvasGroup.alpha = 1f;
                handlerPosition = position;
            }
            else if (status == Status.Down)
            {
                _canvasGroup.alpha = 1f;
                handlerPosition = position;

                _centerPosition = position;
                _background.SetCenterLocalPosition(position);
            }
            else
            {//Status.Reset,Status.Up
                _canvasGroup.alpha = 0f;
                handlerPosition = _centerPosition;
            }

            Vector2 offset = (Vector2)handlerPosition - _centerPosition;
            Vector2 normal = offset.normalized;
            float length = offset.magnitude;

            //校验
            if (length > _radius)
            {//限制半径
                length = _radius;

                offset = length * normal;
                handlerPosition = _centerPosition + offset;
            }

            //设置坐标
            _handler.SetCenterLocalPosition(handlerPosition);

            Vector2 value = offset / _radius;

            //设置值
            if (null != _input)
            {
                _input.SetAxis(_horizontalName, value.x);
                _input.SetAxis(_verticalName, value.y);
            }
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
        }

        private void OnClick()
        {
            _onClick.Invoke();
        }

        #endregion 事件
    }
}