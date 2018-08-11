using ProtoBuf;
using System;

namespace XM.Services.Localization
{
    /// <summary>
    /// 语言元素
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class LanguageItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [ProtoMember(1)]
        public string ID { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [ProtoMember(2)]
        public string Text { get; set; }

        public LanguageItem()
        {
        }

        public override string ToString()
        {
            return string.Format("[{0}]({1})", ID, Text);
        }
    }
}