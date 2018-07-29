using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 本地化服务
    /// </summary>
    /// <typeparam name="AE"></typeparam>
    public class LocalizationService : SimpleService<AppEntry, LocalizationSetting>
    {
        #region Base

        protected override void OnClearService()
        {
        }

        protected override void OnCreateService()
        {
            Inst = this;
        }

        protected override void OnInitService()
        {
        }

        protected override void OnDisposeService()
        {
            Inst = null;
        }

        #endregion Base

        #region private members

        private List<LocalizationItem> _items = new List<LocalizationItem>();

        #endregion private members

        #region public function

        public static LocalizationService Inst { get; protected set; }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item">元素</param>
        public void AddItem(LocalizationItem item)
        {
            if (_items.Contains(item))
            {
                Debug(DebugType.Warning, "元素重复添加");
                return;
            }

            _items.Add(item);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="item">元素</param>
        public void RemoveItem(LocalizationItem item)
        {
            _items.Remove(item);
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public string GetText(string id)
        {
            return "";
        }

        #endregion public function
    }
}