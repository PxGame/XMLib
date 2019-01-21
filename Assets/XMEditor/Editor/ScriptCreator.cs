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

namespace XMEditor
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
            CreateFile("XMLib", "XMEditor/Template/XMLib.cs.txt");
        }

        /// <summary>
        /// 创建编辑器脚本
        /// </summary>
        public static void CreateEditor()
        {
            CreateFile("XMEditor", "XMEditor/Template/XMEditor.cs.txt");
        }

        /// <summary>
        /// 创建测试脚本
        /// </summary>
        public static void CreateLibTest()
        {
            CreateFile("XMLibTest", "XMEditor/Template/XMLib.Test.cs.txt");
        }

        /// <summary>
        /// 创建测试运行脚本
        /// </summary>
        public static void CreateLibTestRunner()
        {
            CreateFile("XMLibTestRunner", "XMEditor/Template/XMLib.Test.TestRunner.cs.txt");
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="templatePath">模板资源路径</param>
        public static void CreateFile(string fileName, string templatePath)
        {
            string filePath = CreatePath(fileName);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<ScriptCreatorAction>(),
                filePath, null, templatePath
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

    }
}