using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 语言信息
    /// </summary>
    [System.Serializable]
    public class LanguageInfo
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        public LanguageType Language { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public List<LanguageItem> Items { get; set; }
    }
}