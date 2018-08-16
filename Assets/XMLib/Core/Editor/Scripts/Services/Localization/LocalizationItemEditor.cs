using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM.Services;
using XM.Services.Localization;

namespace XMEditor.Services.Localization
{
    /// <summary>
    /// LocalizationItem 编辑器
    /// </summary>
    [CustomEditor(typeof(LocalizationItem))]
    [CanEditMultipleObjects]
    public class LocalizationItemEditor : Editor
    {
        protected SerializedProperty _id;

        protected LanguageType _oldLanguage;

        protected virtual void UpdateMembers()
        {
            serializedObject.Update();
            _id = serializedObject.FindProperty("_id");
        }

        public override void OnInspectorGUI()
        {
            UpdateMembers();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_id);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged()
        {
        }
    }
}