using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public override string ToString()
        {
            int cnt = 0;
            if (null != Items)
            {
                cnt = Items.Count;
            }

            return string.Format("[{0}]({1})", Language, cnt);
        }
    }
}