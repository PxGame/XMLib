using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XM.Services;
using XM.Services.Localization;
using XM.Tools;

namespace XMEditor.Services.Localization
{
    /// <summary>
    /// 本地化
    /// </summary>
    public class LocalizationWindow : EditorWindow
    {
        [MenuItem("XM/Localization")]
        public static void Display()
        {
            var win = GetWindow<LocalizationWindow>("本地化设置");
            win.Show();
        }

        [System.Serializable]
        private class Data
        {
            public List<string> FontPaths = new List<string>();
        }

        private SerializedObject _serializedObject;
        private Data _data = new Data();

        [SerializeField]
        private List<Font> _fonts = new List<Font>();

        protected SerializedProperty _fontsProp;
        protected ReorderableList _fontsPropList;

        private void OnEnable()
        {
            LoadData();
        }

        private void OnDisable()
        {
            SaveData();
        }

        private void LoadData()
        {
            if (EditorPrefs.HasKey("XM_LocalizationWindow"))
            {
                string json = EditorPrefs.GetString("XM_LocalizationWindow");
                EditorJsonUtility.FromJsonOverwrite(json, _data);
                if (null == _data)
                {
                    _data = new Data();
                }
            }

            _fonts.Clear();

            List<string> fontPaths = _data.FontPaths;
            Font font;
            string path;
            int length = fontPaths.Count;
            for (int i = 0; i < length; i++)
            {
                path = fontPaths[i];
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                font = AssetDatabase.LoadAssetAtPath<Font>(path);
                _fonts.Add(font);
            }

            _serializedObject = new SerializedObject(this);

            _fontsProp = _serializedObject.FindProperty("_fonts");
            _fontsPropList = new ReorderableList(_serializedObject, _fontsProp, true, true, true, true);
            _fontsPropList.drawElementCallback += OnDrawFontElement;
            _fontsPropList.drawHeaderCallback += OnDrawFontHeader;
        }

        private void OnDrawFontHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "字体列表");
        }

        private void OnDrawFontElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty itemData = _fontsProp.GetArrayElementAtIndex(index);
            rect.y += 2;
            rect.height = EditorGUIUtility.singleLineHeight;

            using (var gor = new EditorGUILayout.HorizontalScope())
            {
                EditorGUI.PropertyField(rect, itemData, new GUIContent(index + ""));
            }
        }

        private void SaveData()
        {
            List<string> fontPaths = new List<string>();
            Font font;
            string path;
            int length = _fonts.Count;
            for (int i = 0; i < length; i++)
            {
                font = _fonts[i];
                if (null == font)
                {
                    continue;
                }

                path = AssetDatabase.GetAssetPath(font);
                fontPaths.Add(path);
            }
            _data.FontPaths = fontPaths;

            string json = EditorJsonUtility.ToJson(_data);
            EditorPrefs.SetString("XM_LocalizationWindow", json);
        }

        private void ExportTemplate()
        {
            LocalizationUtils.CreateConfigFile();
        }

        private void ExportLanguage()
        {
            LocalizationUtils.ExportConfigFile();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            _fontsPropList.DoLayoutList();

            if (GUILayout.Button("导出语言"))
            {
                ExportLanguage();
            }

            if (GUILayout.Button("导出模板"))
            {
                ExportTemplate();
            }

            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
            }
        }
    }
}