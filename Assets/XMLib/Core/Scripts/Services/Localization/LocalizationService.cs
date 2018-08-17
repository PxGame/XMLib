using System.Collections.Generic;
using UnityEngine;

namespace XM.Services.Localization
{
    /// <summary>
    /// 本地化服务
    /// </summary>
    public class LocalizationService : SimpleService<AppEntry, LocalizationSetting>
    {
        #region 属性

        private LanguageSetting _languageSetting;
        private LanguageInfo _languageInfo;

        private List<LocalizationItem> _items = new List<LocalizationItem>();

        #endregion 属性

        #region 重写

        public override void CreateService(AppEntry appEntry)
        {
            base.CreateService(appEntry);

            UpdateLanguage(Setting.DefualtLanguage);
        }

        #endregion 重写

        #region 元素操作

        /// <summary>
        /// 注册元素
        /// </summary>
        /// <param name="item">元素</param>
        public void RegistItem(LocalizationItem item)
        {
            Checker.IsFalse(_items.Contains(item), "元素重复注册:{0}", item);

            _items.Add(item);

            UpdateItem(item);
        }

        /// <summary>
        /// 反注册元素
        /// </summary>
        /// <param name="item">元素</param>
        /// <returns>是否成功</returns>
        public bool UnRegistItem(LocalizationItem item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// 更新元素
        /// </summary>
        /// <param name="item">元素</param>
        public void UpdateItem(LocalizationItem item)
        {
            string text = _languageInfo.Get(item.ID);

            item.UpdateText(_languageSetting, text);
        }

        /// <summary>
        /// 更新所有元素
        /// </summary>
        public void UpdateAllItem()
        {
            int length = _items.Count;
            LocalizationItem item;

            for (int i = 0; i < length; i++)
            {
                item = _items[i];
                if (item == null)
                {
                    _items.RemoveAt(i);

                    i--;
                    length--;
                    continue;
                }

                item.UpdateText(_languageSetting, _languageInfo.Get(item.ID));
            }
        }

        /// <summary>
        /// 更新语言
        /// </summary>
        /// <param name="languageType">语言类型</param>
        public void UpdateLanguage(LanguageType languageType)
        {
            UpdateData(languageType);
            UpdateAllItem();
        }

        #endregion 元素操作

        #region 读取配置

        /// <summary>
        /// 更新语言数据
        /// </summary>
        /// <param name="languageType">语言类型</param>
        private void UpdateData(LanguageType languageType)
        {
            _languageSetting = Setting.Get(languageType);
            Checker.NotNull(_languageSetting, "获取语言设置为空:{0}", languageType);

            TextAsset textAsset = Resources.Load<TextAsset>(_languageSetting.SrcPath);
            Checker.NotNull(textAsset, "获取语言资源为空:{0}", _languageSetting.SrcPath);

            _languageInfo = SerializerUtils.Deserialize<LanguageInfo>(textAsset.bytes);
            Checker.NotNull(_languageInfo, "反序列化语言资源为空:{0}", languageType);
        }

        #endregion 读取配置
    }
}