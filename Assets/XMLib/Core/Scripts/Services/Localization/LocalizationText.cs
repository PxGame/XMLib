using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XM.Services.Localization
{
    /// <summary>
    /// 本地化Text组件
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizationText : LocalizationItem
    {
        [SerializeField]
        protected Text _text;

        private void Awake()
        {
            if (null == _text)
            {//掉引用，重新获取
                _text = GetComponent<Text>();
            }
        }

        protected override void OnTextUpdate(string text)
        {
            _text.text = text;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (null == _text)
            {
                _text = GetComponent<Text>();
            }
        }

#endif
    }
}