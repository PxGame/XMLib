using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 本地化文本
    /// </summary>
    [Serializable]
    public class LocalizationText
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID;

        /// <summary>
        /// 文本
        /// </summary>
        public string Text;

        /// <summary>
        /// 字体
        /// </summary>
        public Font Font;

        //字体大小
        public int FontSize;
    }
}