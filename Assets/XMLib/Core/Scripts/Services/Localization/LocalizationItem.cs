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
        /// 服务
        /// </summary>
        protected LocalizationService _service;

        protected virtual void Awake()
        {
            _service = AppEntry.Inst.Get<LocalizationService>();
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="text">文本</param>
        public void UpdateText(LanguageType languageType, string text, Font font)
        {
            OnTextUpdate(languageType, text, font);
        }

        protected abstract void OnTextUpdate(LanguageType languageType, string text, Font font);
    }
}