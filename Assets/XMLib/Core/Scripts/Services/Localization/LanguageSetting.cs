using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Localization
{
    /// <summary>
    /// 语言设置
    /// </summary>
    [System.Serializable]
    public class LanguageSetting
    {
        [SerializeField]
        protected LanguageType _language;

        [SerializeField]
        protected Font _font;

        [SerializeField]
        protected string _srcPath;

        /// <summary>
        /// 语言类型
        /// </summary>
        public LanguageType Language { get { return _language; } }

        /// <summary>
        /// 语言字体
        /// </summary>
        public Font Font { get { return _font; } }

        /// <summary>
        /// 语言资源路径
        /// </summary>
        public string SrcPath { get { return _srcPath; } }
    }
}