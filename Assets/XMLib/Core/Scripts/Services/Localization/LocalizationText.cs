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

        /// <summary>
        /// 文本组件
        /// </summary>
        public Text Text { get { return _text; } }

        protected override void Awake()
        {
            if (null == _text)
            {//掉引用，重新获取
                _text = GetComponent<Text>();
            }

            base.Awake();
        }

        #region 重写

        protected override void OnTextUpdate(LanguageSetting setting, string text)
        {
            _text.text = text;
            _text.font = setting.Font;

#if UNITY_EDITOR
            //编辑器模式下必须设置，否则将无法及时显示修改
            //当前场景将标记为修改
            UnityEditor.EditorUtility.SetDirty(_text);
#endif
        }

        #endregion 重写

        #region 编辑器

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (null == _text)
            {
                _text = GetComponent<Text>();
            }
        }

#endif

        #endregion 编辑器
    }
}