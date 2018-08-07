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
            string[] enumNames = Enum.GetNames(typeof(LanguageType));
            int length = enumNames.Length;
            string enumName;

            string filePath = SpecialUtils.Language.GetConfigPath();
            Debug.Log("Export:" + filePath);

            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("Language");

                sheet.SetValue(1, 1, "ID");
                for (int i = 0; i < length; i++)
                {
                    enumName = enumNames[i];
                    sheet.SetValue(1, i + 2, enumName);
                }

                excel.SaveAs(new FileInfo(filePath));
            }
        }

        private void ExportLanguage()
        {
            string filePath = SpecialUtils.Language.GetConfigPath();
            if (!File.Exists(filePath))
            {
                Debug.LogErrorFormat("文件不存在:{0}", filePath);
                return;
            }

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            List<string> indexs = new List<string>();
            string header;
            string indexName;
            List<string> items;
            using (var excel = new ExcelPackage(new FileInfo(filePath)))
            {
                var sheet = excel.Workbook.Worksheets["Language"];

                ExcelRange range;
                string id;
                string text;
                int col = 1;
                int row = 1;

                //header
                do
                {
                    range = sheet.Cells[row, col];
                    header = range.Text;
                    if (string.IsNullOrEmpty(header))
                    {
                        break;
                    }

                    indexs.Add(header);
                    dict[header] = new List<string>();

                    col++;
                } while (true);

                if (!dict.ContainsKey("ID"))
                {
                    throw new Exception(string.Format("配置表中没有ID字段:{0}", filePath));
                }

                //取id
                col = 1;
                row = 2;
                do
                {
                    range = sheet.Cells[row, col];
                    id = range.Text;
                    if (string.IsNullOrEmpty(id))
                    {
                        break;
                    }

                    dict["ID"].Add(id);

                    row++;
                } while (true);

                //取值
                int maxCol = indexs.Count;
                int maxRow = dict["ID"].Count;

                for (col = 2; col <= maxCol; col++)
                {
                    indexName = indexs[col - 1];
                    items = dict[indexName];
                    for (row = 2; row <= maxRow + 1; row++)
                    {
                        range = sheet.Cells[row, col];
                        text = range.Text;
                        if (string.IsNullOrEmpty(text))
                        {
                            Debug.LogWarningFormat("[{0},{1}] 数据为空", row, col);
                        }

                        items.Add(text);
                    }
                }
            }

            //解析
            List<LanguageInfo> infos = new List<LanguageInfo>();
            LanguageInfo info;
            int length = indexs.Count;
            List<string> ids = dict["ID"];
            int itemSize = ids.Count;
            LanguageType type;
            for (int i = 1; i < length; i++)
            {
                header = indexs[i];
                items = dict[header];

                if (!Enum.TryParse<LanguageType>(header, out type))
                {
                    throw new Exception(string.Format("{0} 不是 LanguageType", header));
                }

                info = new LanguageInfo();
                info.Language = type;

                info.Items = new List<LanguageItem>();

                for (int j = 0; j < itemSize; j++)
                {
                    info.Items.Add(new LanguageItem()
                    {
                        ID = ids[j],
                        Text = items[j]
                    });
                }

                infos.Add(info);
            }

            //导出
            length = infos.Count;
            for (int i = 0; i < length; i++)
            {
                info = infos[i];
                ExportLanguageInfo(info);
            }
        }

        private void ExportLanguageInfo(LanguageInfo info)
        {
            string filePath = SpecialUtils.Language.GetTempPath(info.Language);
            Debug.LogFormat("导出语言文件:{0}", filePath);
            byte[] data = SerializationUtils.Serialize(info);
            File.WriteAllBytes(filePath, data);
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