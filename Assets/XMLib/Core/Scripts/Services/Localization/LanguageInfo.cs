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

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>文本</returns>
        public string Get(string id)
        {
            string str = "";

            LanguageItem item = Items.Find((t) => { return t.ID == id; });
            Checker.NotNull(item, "{0} 语言中不存在 ID:{1}", Language, id);
            str = item.Text;

            return str;
        }

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