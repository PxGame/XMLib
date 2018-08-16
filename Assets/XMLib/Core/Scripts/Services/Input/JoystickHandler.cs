using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XM.Services.Input
{
    /// <summary>
    /// 摇杆
    /// </summary>
    public class JoystickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region 设置

        [SerializeField]
        protected string _horizontalName = "JoystickHorizontal";

        [SerializeField]
        protected string _verticalName = "JoystickVertical";

        [SerializeField]
        protected string _buttonName = "JoystickButton";

        [SerializeField]
        protected RectTransform _background;

        [SerializeField]
        protected RectTransform _joystick;

        [SerializeField]
        protected float _maxValue = 0;

        #endregion 设置

        #region 参数

        protected Vector2 _backgroundPosition;

        protected Vector2 _backgroundMaxPosition;
        protected Vector2 _backgroundMinPosition;

        protected Vector2 _joystickSize;
        protected Vector2 _joystickPosition;

        protected Vector2 _currentValue = Vector2.zero;

        protected bool _isDown = false;

        private InputService _service;

        #endregion 参数

        private void Awake()
        {
            _service = AppEntry.Inst.Get<InputService>();
        }

        private void OnEnable()
        {
            _background.gameObject.SetActive(false);
            _joystick.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_isDown)
            {
                _service.SetAxis(_horizontalName, 0);
                _service.SetAxis(_verticalName, 0);
                _service.SetButton(_buttonName, false);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _joystickPosition = eventData.position;
            _currentValue = _joystickPosition - _backgroundPosition;
            if (_currentValue.magnitude > _maxValue)
            {
                _currentValue = _currentValue.normalized * _maxValue;
            }

            _joystickPosition = _backgroundPosition + _currentValue;
            _joystick.position = _joystickPosition;

            //计算轴值
            _currentValue /= _maxValue;

            //

            _service.SetAxis(_horizontalName, _currentValue.x);
            _service.SetAxis(_verticalName, _currentValue.y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _joystickPosition = eventData.position;
            _joystick.position = _joystickPosition;

            _backgroundPosition = _joystickPosition;
            _background.position = _backgroundPosition;

            _currentValue = Vector2.zero;

            //

            _service.SetAxis(_horizontalName, _currentValue.x);
            _service.SetAxis(_verticalName, _currentValue.y);
            _service.SetButton(_buttonName, true);

            _isDown = true;

            //
            _background.gameObject.SetActive(true);
            _joystick.gameObject.SetActive(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _joystickPosition = _backgroundPosition;

            _background.position = _backgroundPosition;
            _joystick.position = _joystickPosition;

            //计算轴值
            _currentValue = Vector2.zero;

            //

            _service.SetAxis(_horizontalName, _currentValue.x);
            _service.SetAxis(_verticalName, _currentValue.y);
            _service.SetButton(_buttonName, false);

            _isDown = false;

            //
            _background.gameObject.SetActive(false);
            _joystick.gameObject.SetActive(false);
        }
    }
}