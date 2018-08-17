using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using XM;
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
            public string ImportPath = "Resources/Languages/";
            public string SettingPath = "";
        }

        private Data _data = new Data();

        protected const string DataFlag = "XM_LocalizationWindow";

        private LocalizationSetting _localizationSetting;

        #endregion 属性

        #region 编辑器数据

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

            if (!string.IsNullOrEmpty(_data.SettingPath))
            {
                _localizationSetting = AssetDatabase.LoadAssetAtPath<LocalizationSetting>(_data.SettingPath);
            }
        }

        private void SaveData()
        {
            if (null != _localizationSetting)
            {
                _data.SettingPath = AssetDatabase.GetAssetPath(_localizationSetting);
            }

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

        private void OnGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                _localizationSetting = (LocalizationSetting)EditorGUILayout.ObjectField("本地化配置:", _localizationSetting, typeof(LocalizationSetting), false);
                if (null == _localizationSetting)
                {
                    GUILayout.Label("必须先配置本地化设置");
                    return;
                }

                _data.ImportPath = EditorGUILayout.TextField("导入目录:", _data.ImportPath);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("导出语言"))
                {
                    ExportLanguage();
                }

                if (GUILayout.Button("创建模板"))
                {
                    CreateTemplate();
                }
                GUILayout.EndHorizontal();

                _data.Language = (LanguageType)EditorGUILayout.EnumPopup("选择语言:", _data.Language);
                GUILayout.BeginHorizontal();

                if (EditorApplication.isPlaying)
                {
                    if (GUILayout.Button("更新游戏内语言"))
                    {
                        UpdateGame();
                    }
                }
                else
                {
                    if (GUILayout.Button("更新当前场景语言"))
                    {
                        UpdateScene();
                    }
                }

                GUILayout.EndHorizontal();

                if (check.changed)
                {
                    SaveData();
                }
            }
        }

        #endregion Unity 函数

        #region 函数

        /// <summary>
        /// 更新游戏内语言
        /// </summary>
        private void UpdateGame()
        {
            Checker.NotNull(AppEntry.Inst, "AppEntry 未创建");

            LocalizationService service = AppEntry.Inst.Get<LocalizationService>();
            Checker.NotNull(service, "LocalizationService 未创建");

            service.UpdateLanguage(_data.Language);
        }

        /// <summary>
        /// 更新当前场景语言
        /// </summary>
        private void UpdateScene()
        {
            if (null == _localizationSetting)
            {
                EditorUtility.DisplayDialog("错误", "本地化设置不能为空！", "确定");
                return;
            }

            Scene scene = SceneManager.GetActiveScene();

            LanguageSetting languageSetting = _localizationSetting.Get(_data.Language);
            Checker.NotNull(languageSetting, "语言配置为空:{0}", _data.Language);

            LocalizationUtils.UpdateScene(scene, languageSetting);

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

            if (!string.IsNullOrEmpty(_data.ImportPath))
            {//导出到目录
                //
                //检查目录
                string outPath = Path.Combine("Assets", _data.ImportPath);
                if (Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }

                Debug.LogFormat("复制语言文件到该目录:{0}", outPath);

                Array types = Enum.GetValues(typeof(LanguageType));

                foreach (LanguageType type in types)
                {
                    string path = LocalizationUtils.GetPath(type);
                    if (File.Exists(path))
                    {
                        string fileName = Path.GetFileName(path);
                        string filePath = Path.Combine(outPath, fileName);
                        File.Copy(path, filePath, true);
                    }
                }

                AssetDatabase.Refresh();
            }
        }

        #endregion 函数
    }
}