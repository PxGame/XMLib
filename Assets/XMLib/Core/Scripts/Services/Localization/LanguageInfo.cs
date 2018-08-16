using ProtoBuf;
using System;
using System.Collections.Generic;

namespace XM.Services.Localization
{
    /// <summary>
    /// 语言信息
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class LanguageInfo
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        [ProtoMember(1)]
        public LanguageType Language { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [ProtoMember(2)]
        public List<LanguageItem> Items { get; set; }

        public LanguageInfo()
        {
        }

        #region 数据操作

        private Dictionary<string, string> _dict;

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>文本</returns>
        public string Get(string id)
        {
            if (null == _dict)
            {
                UpdateDict();
            }

            string str = "";

            if (!_dict.TryGetValue(id, out str))
            {
                Checker.IsTrue(false, "{0} 语言中不存在 ID:{1}", Language, id);
            }

            return str;
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        public void UpdateDict()
        {
            _dict = new Dictionary<string, string>();

            LanguageItem item;
            int length = Items.Count;
            for (int i = 0; i < length; i++)
            {
                item = Items[i];

                _dict.Add(item.ID, item.Text);
            }
        }

        #endregion 数据操作

        public override string ToString()
        {
            int cnt = 0;
            if (null != Items)
            {
                cnt = Items.Count;
            }

            return string.Format("[{0}](cnt={1})", Language, cnt);
        }
    }
}