/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/21/2019 9:50:16 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// 脚本创建者
    /// </summary>
    public static class ScriptCreator
    {
        /// <summary>
        /// 创建脚本
        /// </summary>
        public static void CreateLib()
        {
            CreateFile("XMLib", XMLib_cs);
        }

        /// <summary>
        /// 创建编辑器脚本
        /// </summary>
        public static void CreateEditor()
        {
            CreateFile("XMEditor", XMEditor_cs);
        }

        /// <summary>
        /// 创建测试脚本
        /// </summary>
        public static void CreateLibTest()
        {
            CreateFile("XMLibTest", XMLib_Test_cs);
        }

        /// <summary>
        /// 创建测试运行脚本
        /// </summary>
        public static void CreateLibTestRunner()
        {
            CreateFile("XMLibTestRunner", XMLib_Test_TestRunner_cs);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="templateClass">类模板</param>
        public static void CreateFile(string fileName, string templateClass)
        {
            string filePath = CreatePath(fileName);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<ScriptCreatorAction>(),
                filePath, null, templateClass
            );
        }

        /// <summary>
        /// 创建文件路径
        /// </summary>
        /// <returns>相对路径</returns>
        private static string CreatePath(string fileName)
        {
            string dir = "Assets";

            //是否选择路径
            string[] guids = Selection.assetGUIDs;
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);

                if (File.Exists(path))
                { //文件
                    dir = Path.GetDirectoryName(path);
                }
                else if (Directory.Exists(path))
                { //文件夹
                    dir = path;
                }
            }

            //文件路径优化
            dir = dir.Replace('\\', '/');
            if (!dir.EndsWith("/"))
            { //添加末尾斜线
                dir += '/';
            }

            fileName += ".cs";
            string filePath = Path.Combine(dir, fileName);

            return filePath;
        }

        public const string XMEditor_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing UnityEditor;\n\nnamespace XMLib\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME#\n    {\n    }\n}";
        public const string XMLib_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\n\nnamespace XMLib\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME# \n    {\n    }\n}";
        public const string XMLib_Test_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing System;\n\nnamespace XMLib.Test.#FOLDERNAME#Test\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME#\n    {\n    }\n}";
        public const string XMLib_Test_TestRunner_cs = "/*\n * 作者：#AUTHOR#\n * 联系方式：#CONTACT#\n * 文档: #DOC#\n * 创建时间: #CREATEDATE#\n */\n\nusing System.Collections;\nusing System.Collections.Generic;\nusing System;\nusing UnityEngine.TestTools;\nusing NUnit.Framework;\nusing System.Diagnostics;\nusing Debug = UnityEngine.Debug;\n\nnamespace XMLib.Test.#FOLDERNAME#Test\n{\n    /// <summary>\n    /// #SCRIPTNAME#\n    /// </summary>\n    public class #SCRIPTNAME#\n    {\n        [Test]\n        public void Run()\n        {\n        }\n\n        [UnityTest]\n        public IEnumerator RunSync()\n        {\n            yield break;\n        }\n    }\n}";
    }
}