﻿/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 2:34:42 PM
 */

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XMLib.Editor
{
    /// <summary>
    /// 脚本创建事件
    /// </summary>
    public class ScriptCreatorAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            //创建资源
            Object obj = CreateFile(pathName, resourceFile);
            //高亮显示该资源
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">      </param>
        /// <param name="resourceFile">  </param>
        /// <returns>  </returns>
        private static Object CreateFile(string filePath, string resourceFile)
        {
            TextAsset text = (TextAsset)EditorGUIUtility.Load(resourceFile);
            if (null == text)
            {
                Debug.LogErrorFormat("未找到模板代码文件：{0}", resourceFile);
                return null;
            }

            string scriptStr = text.text;

            string className = Path.GetFileNameWithoutExtension(filePath);

            //更新信息
            scriptStr = scriptStr.Replace("#SCRIPTNAME#", className);
            scriptStr = scriptStr.Replace("#AUTHOR#", "Peter Xiang");
            scriptStr = scriptStr.Replace("#CONTACT#", "565067150@qq.com");
            scriptStr = scriptStr.Replace("#DOC#", "https://github.com/xiangmu110/XMLib/wiki");
            scriptStr = scriptStr.Replace("#CREATEDATE#", DateTime.Now.ToString());

            File.WriteAllText(filePath, scriptStr);

            //刷新本地资源
            AssetDatabase.ImportAsset(filePath);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
        }
    }

    /// <summary>
    /// 脚本处理
    /// </summary>
    public class ScriptHandler
    {
        public static string ScriptTemplate = "XMLib/Template/XMLib.cs.txt";
        public static string ScriptEditorTemplate = "XMLib/Template/XMLibEditor.cs.txt";
        public static string CheckEditorPath = ".*/Editor/.*";

        /// <summary>
        /// 创建新脚本
        /// </summary>
        [MenuItem("Assets/XMLib/Create/Script")]
        public static void CreateScript()
        {
            string filePath = GetFilePath();
            string templatePath = ScriptTemplate;

            //
            Regex reg = new Regex(CheckEditorPath, RegexOptions.IgnoreCase);
            if (reg.IsMatch(filePath))
            {//编辑器脚本
                templatePath = ScriptEditorTemplate;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<ScriptCreatorAction>(),
                filePath, null, templatePath
                );
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <returns>  </returns>
        private static string GetFilePath()
        {
            string dir = "Assets";

            //是否选择路径
            string[] guids = Selection.assetGUIDs;
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);

                if (File.Exists(path))
                {//文件
                    dir = Path.GetDirectoryName(path);
                }
                else if (Directory.Exists(path))
                {//文件夹
                    dir = path;
                }
            }

            //文件路径
            string filePath = Path.Combine(dir, "XMLib.cs").Replace('\\', '/');

            return filePath;
        }
    }
}