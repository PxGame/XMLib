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
    }
}