using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XM.Services.Input
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class ButtonHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        protected string _buttonName;

        private InputService _service;

        private void Awake()
        {
            _service = AppEntry.Inst.Get<InputService>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _service.SetButton(_buttonName, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _service.SetButton(_buttonName, false);
        }
    }
}