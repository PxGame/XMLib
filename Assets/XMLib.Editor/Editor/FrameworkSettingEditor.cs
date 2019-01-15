/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/16/2019 12:03:42 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMLib.Editor
{
    [CustomEditor(typeof(FrameworkSetting))]
    public class FrameworkSettingEditor : UnityEditor.Editor
    {
        SerializedProperty _settings;
        private void OnEnable()
        {
            _settings = serializedObject.FindProperty("_settings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            int size = _settings.arraySize;
            for (int i = 0; i < size; i++)
            {
                SerializedProperty setting = _settings.GetArrayElementAtIndex(i);
                EditorGUILayout.ObjectField(setting);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}