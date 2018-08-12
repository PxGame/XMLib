using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM;
using XM.Services;

namespace XMEditor.Services
{
    /// <summary>
    /// 服务设置编辑器
    /// </summary>
    [CustomEditor(typeof(ServiceSettings))]
    public class ServiceSettingsEditor : Editor
    {
        private string _tip = "存放服务使用的配置文件，同类型配置不能重复";
        private string _errorMsg = "";
        private SerializedProperty _settings;
        private SerializedProperty _debugType;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _debugType = serializedObject.FindProperty("_debugType");
            _settings = serializedObject.FindProperty("_settings");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_debugType);
            EditorGUILayout.PropertyField(_settings, true);

            if (GUILayout.Button("检查"))
            {
                CheckData(_settings);
            }

            EditorGUILayout.HelpBox(_tip, MessageType.None);

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                CheckData(_settings);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="settings">设置</param>
        private void CheckData(SerializedProperty settings)
        {
            bool isError = false;

            try
            {
                int length = settings.arraySize;
                SimpleSetting setting;
                Type settingType;
                Dictionary<Type, int> settingTypes = new Dictionary<Type, int>();
                int existIndex;
                for (int i = 0; i < length; i++)
                {
                    setting = (SimpleSetting)settings.GetArrayElementAtIndex(i).objectReferenceValue;
                    Checker.NotNull(setting, "第{0}条设置为null.", i);

                    settingType = setting.GetType();

                    bool isExist = settingTypes.TryGetValue(settingType, out existIndex);
                    Checker.IsFalse(isExist, "第{0}条与第{1}条设置类型重复.", i, existIndex);

                    settingTypes.Add(settingType, i);
                }
            }
            catch (System.Exception ex)
            {
                isError = true;
                _errorMsg = ex.Message;
            }

            if (!isError)
            {
                _errorMsg = "";
            }
        }
    }
}