/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 3:42:24 PM
 */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMLib.Editor
{
    /// <summary>
    /// 公共工具
    /// </summary>
    public class CommonWindow : EditorWindow
    {
        [MenuItem("XM Tool/Common Tool")]
        public static void DisplayWindow()
        {
            var win = GetWindow<CommonWindow>();
            win.titleContent = new GUIContent("公共工具");
            win.Show();
        }

        [System.Serializable]
        public class Settings
        {
            public string assetGUID = "";
            internal string shaderName;

            public string prefsKey = "";
            public string prefsValue = "";
        }

        private Settings _setting;
        private readonly string SettingName;

        public CommonWindow()
        {
            SettingName = GetType().FullName;
        }

        private void OnEnable()
        {
            _setting = new Settings();
            string jsonData;
            if (!EditorPrefs.HasKey(SettingName))
            {
                jsonData = EditorJsonUtility.ToJson(_setting);
                EditorPrefs.SetString(SettingName, jsonData);
            }
            else
            {
                jsonData = EditorPrefs.GetString(SettingName);
                EditorJsonUtility.FromJsonOverwrite(jsonData, _setting);
            }
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            ToolView();
            if (EditorGUI.EndChangeCheck())
            {
                string jsonData = EditorJsonUtility.ToJson(_setting);
                EditorPrefs.SetString(SettingName, jsonData);
            }
        }

        private void ToolView()
        {
            FindRefView();
            PlayerPrefsView();
            OtherView();
        }

        private void OtherView()
        {
            GUILayout.BeginVertical(GUI.skin.box);

            if (GUILayout.Button("立即保存资源"))
            {
                AssetDatabase.SaveAssets();
            }

            GUILayout.EndVertical();
        }

        private void PlayerPrefsView()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Unity PlayerPrefs");
            _setting.prefsKey = EditorGUILayout.TextField("Key", _setting.prefsKey);
            _setting.prefsValue = EditorGUILayout.TextField("Value", _setting.prefsValue);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("获取"))
            {
                if (PlayerPrefs.HasKey(_setting.prefsKey))
                {
                    _setting.prefsValue = PlayerPrefs.GetString(_setting.prefsKey);
                }
                else
                {
                    Debug.LogWarningFormat("PlayerPrefs 不存在 {0}", _setting.prefsKey);
                }
            }

            if (GUILayout.Button("设置"))
            {
                PlayerPrefs.SetString(_setting.prefsKey, _setting.prefsValue);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void FindRefView()
        {//Selection
            GUILayout.BeginVertical(GUI.skin.box);

            _setting.assetGUID = EditorGUILayout.TextField("资源GUID", _setting.assetGUID);

            if (GUILayout.Button("查找GUID"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(_setting.assetGUID);
                Debug.LogFormat("资源路径: {0}", assetPath);
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
            }

            _setting.shaderName = EditorGUILayout.TextField("Shader名字", _setting.shaderName);
            if (GUILayout.Button("查找引用材质"))
            {
                string[] guids = AssetDatabase.FindAssets("t:Material");
                List<string> outPaths = new List<string>();

                int startIndex = 0;
                int maxIndex = guids.Length;
                string targetShaderName = _setting.shaderName;
                EditorApplication.update = delegate ()
                {
                    try
                    {
                        if (startIndex >= maxIndex)
                        {
                            throw null;
                        }

                        string guid = guids[startIndex];
                        bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中:" + targetShaderName, guid, (startIndex + 1) / (float)maxIndex);
                        if (isCancel)
                        {
                            throw null;
                        }

                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                        if (mat)
                        {
                            string shaderName = mat.shader.name;
                            if (0 == string.Compare(shaderName, targetShaderName, true))
                            {
                                outPaths.Add(path);
                            }
                        }

                        startIndex++;
                    }
                    catch (Exception)
                    {
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;

                        string formatMsg = "";
                        foreach (var item in outPaths)
                        {
                            formatMsg += item + "\n";
                        }

                        Debug.LogFormat("查找结果:\n{0}", formatMsg);
                    }
                };
            }

            GUILayout.EndVertical();
        }
    }
}