using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 语言元素
    /// </summary>
    [System.Serializable]
    public class LanguageItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
    }
}