using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Localization
{
    /// <summary>
    /// 本地化设置
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "XMLib/Localization Setting")]
    public class LocalizationSetting : SimpleSetting
    {
        [SerializeField]
        protected List<LanguageSetting> _languages = new List<LanguageSetting>();

        [SerializeField]
        protected LanguageType _defualtLanguage = LanguageType.Chinese;

        /// <summary>
        /// 获取语言设置
        /// </summary>
        /// <param name="languageType">语言类型</param>
        /// <returns>语言设置</returns>
        public LanguageSetting Get(LanguageType languageType)
        {
            LanguageSetting languageSetting = _languages.Find(t => t.Language == languageType);
            return languageSetting;
        }

        /// <summary>
        /// 默认语言类型
        /// </summary>
        public LanguageType DefualtLanguage { get { return _defualtLanguage; } }
    }
}