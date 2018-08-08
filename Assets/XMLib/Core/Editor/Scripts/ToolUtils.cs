using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XMEditor
{
    /// <summary>
    /// 编辑器工具
    /// </summary>
    public static class ToolUtils
    {
        static ToolUtils()
        {
        }

        /// <summary>
        /// 模板路径
        /// </summary>
        public static string TemplatePath { get { return Path.Combine(Application.dataPath, "../XMTemp/"); } }

        /// <summary>
        /// 模板路径
        /// </summary>
        public static string ResourcePath { get { return Path.Combine(Application.dataPath, "Resources/"); } }

        /// <summary>
        /// 获取模板路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="createDir"></param>
        /// <returns></returns>
        public static string GetTemplatePath(string fileName, bool createDir = true)
        {
            string filePath = Path.Combine(TemplatePath, fileName);

            string dir = Path.GetDirectoryName(filePath);
            if (createDir && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return filePath;
        }

        /// <summary>
        /// 获取资源路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="createDir"></param>
        /// <returns></returns>
        public static string GetResourcePath(string fileName, bool createDir = true)
        {
            string filePath = Path.Combine(ResourcePath, fileName);

            string dir = Path.GetDirectoryName(filePath);
            if (createDir && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return filePath;
        }
    }
}