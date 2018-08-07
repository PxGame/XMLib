using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XM.Services.Localization;

namespace XMEditor
{
    /// <summary>
    /// 特殊工具
    /// </summary>
    public static class SpecialUtils
    {
#if XM_USE_PROTOBUF
        public static string Extension = ".bytes";
#else
        public static string Extension = ".json";
#endif

        /// <summary>
        /// 语言
        /// </summary>
        public static class Language
        {
            /// <summary>
            /// 获取语言配置
            /// </summary>
            /// <param name="languageType"></param>
            /// <returns></returns>
            public static string GetTempPath(LanguageType languageType)
            {
                string fileName = string.Format("Language/{0}{1}", languageType, Extension);
                string filePath = ToolUtils.GetTemplatePath(fileName);
                return filePath;
            }

            /// <summary>
            /// 获取语言配置文件
            /// </summary>
            /// <returns></returns>
            public static string GetConfigPath()
            {
                string filePath = ToolUtils.GetTemplatePath("Language/Language.xlsx");
                return filePath;
            }
        }
    }
}