﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Localization
{
    /// <summary>
    /// 本地化元素
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class LocalizationItem : MonoBehaviour
    {
        /// <summary>
        /// ID
        /// </summary>
        [SerializeField]
        protected string _id = null;

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get { return _id; } }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="text"></param>
        public void UpdateText(LanguageType languageType, string text, Font font)
        {
            OnTextUpdate(text);
        }

        protected abstract void OnTextUpdate(string text);
    }
}