/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/21/2019 9:51:26 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XMEditor
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
        /// <param name="filePath"></param>
        /// <param name="resourceFile"></param>
        /// <returns>资源对象</returns>
        private Object CreateFile(string filePath, string resourceFile)
        {
            TextAsset text = (TextAsset) EditorGUIUtility.Load(resourceFile);
            if (null == text)
            {
                Debug.LogErrorFormat("未找到模板代码文件：{0}", resourceFile);
                return null;
            }

            string scriptStr = text.text;

            string className = Path.GetFileNameWithoutExtension(filePath).Replace(" ", "");

            string folderPath = Path.GetDirectoryName(filePath);
            int index = folderPath.LastIndexOfAny(new char[] { '/', '\\' }) + 1;
            string folderName = folderPath.Substring(index);

            //更新信息
            scriptStr = scriptStr.Replace("#SCRIPTNAME#", className);
            scriptStr = scriptStr.Replace("#AUTHOR#", "Peter Xiang");
            scriptStr = scriptStr.Replace("#CONTACT#", "565067150@qq.com");
            scriptStr = scriptStr.Replace("#DOC#", "https://github.com/xiangmu110/XMLib/wiki");
            scriptStr = scriptStr.Replace("#CREATEDATE#", DateTime.Now.ToString());
            scriptStr = scriptStr.Replace("#FOLDERNAME#", folderName);

            File.WriteAllText(filePath, scriptStr);

            //刷新本地资源
            AssetDatabase.ImportAsset(filePath);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
        }
    }
}