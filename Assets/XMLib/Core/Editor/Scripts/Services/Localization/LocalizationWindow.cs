using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using XM.Services.Localization;

namespace XMEditor.Services.Localization
{
    /// <summary>
    /// 本地化设置窗口
    /// </summary>
    public class LocalizationWindow : EditorWindow
    {
        [MenuItem("XM/Localization")]
        public static void Display()
        {
            var win = GetWindow<LocalizationWindow>("本地化设置");
            win.Show();
        }

        #region 属性

        [System.Serializable]
        private class Data
        {
            public List<string> FontPaths = new List<string>();
            public LanguageType Language = LanguageType.Chinese;
        }

        private SerializedObject _serializedObject;
        private Data _data = new Data();

        [SerializeField]
        private List<Font> _fonts = new List<Font>();

        protected SerializedProperty _fontsProp;
        protected ReorderableList _fontsPropList;

        protected const string DataFlag = "XM_LocalizationWindow";

        #endregion 属性

        #region 编辑器数据

        /// <summary>
        /// 当前语言
        /// </summary>
        public static LanguageType Language
        {
            get
            {
                Data data = new Data();
                if (EditorPrefs.HasKey(DataFlag))
                {
                    string json = EditorPrefs.GetString(DataFlag);
                    EditorJsonUtility.FromJsonOverwrite(json, data);
                }

                return data.Language;
            }
        }

        private void LoadData()
        {
            if (EditorPrefs.HasKey(DataFlag))
            {
                string json = EditorPrefs.GetString(DataFlag);
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
            EditorPrefs.SetString(DataFlag, json);
        }

        #endregion 编辑器数据

        #region Unity 函数

        private void OnEnable()
        {
            LoadData();
        }

        private void OnDisable()
        {
            SaveData();
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

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            //_fontsPropList.DoLayoutList();

            if (GUILayout.Button("导出语言"))
            {
                ExportLanguage();
            }

            if (GUILayout.Button("创建模板"))
            {
                CreateTemplate();
            }

            _data.Language = (LanguageType)EditorGUILayout.EnumPopup("选择语言:", _data.Language);

            if (GUILayout.Button("更新当前场景语言"))
            {
                UpdateScene();
            }

            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
                SaveData();
            }
        }

        #endregion Unity 函数

        #region 函数

        private void UpdateScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            LocalizationUtils.UpdateScene(scene, _data.Language);

            if (!EditorApplication.isPlaying)
            {//标记修改
                EditorSceneManager.MarkSceneDirty(scene);
            }
        }

        private void CreateTemplate()
        {
            LocalizationUtils.CreateConfigFile();
        }

        private void ExportLanguage()
        {
            LocalizationUtils.ExportConfigFile();
        }

        #endregion 函数
    }
}