using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 本地化信息
    /// </summary>
    [Serializable]
    public class LocalizationInfo
    {
        #region 设置

        /// <summary>
        /// 语言
        /// </summary>
        [SerializeField]
        protected LocalizationLanguage _language;

        /// <summary>
        /// 默认字体
        /// </summary>
        [SerializeField]
        protected Font _defaultFont;

        /// <summary>
        /// 本地化文本列表
        /// </summary>
        [SerializeField]
        protected List<LocalizationText> _texts = new List<LocalizationText>();

        #endregion 设置

        #region 公开

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LocalizationText GetText(string id)
        {
            UpdateTextDict();

            LocalizationText text;
            if (_textDict.TryGetValue(id, out text))
            {
            }

            return text;
        }

        #endregion 公开

        #region 不公开

        protected Dictionary<string, LocalizationText> _textDict;

        protected void UpdateTextDict(bool isForce = false)
        {
            if (!isForce && null != _textDict)
            {
                return;
            }

            int length = _texts.Count;

            _textDict = new Dictionary<string, LocalizationText>();
            LocalizationText text;
            for (int i = 0; i < length; i++)
            {
                text = _texts[i];

                _textDict.Add(text.ID, text);
            }
        }

        #endregion 不公开
    }
}