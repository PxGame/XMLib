using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XM.Services.Localization;

namespace XMEditor.Services.Localization
{
    /// <summary>
    /// 本地化设置
    /// </summary>
    [CustomEditor(typeof(LocalizationSetting))]
    public class LocalizationSettingEditor : Editor
    {
        private SerializedProperty _languages;
        private SerializedProperty _debugType;
        private ReorderableList _languagesReorder;

        private void OnEnable()
        {
            _languages = serializedObject.FindProperty("_languages");
            _debugType = serializedObject.FindProperty("_debugType");
            _languagesReorder = new ReorderableList(serializedObject, _languages, false, true, true, true);
            _languagesReorder.drawElementCallback = OnDrawElement;
            _languagesReorder.drawHeaderCallback = OnDrawHeader;
            _languagesReorder.onAddDropdownCallback = OnAddDropdown;
            _languagesReorder.elementHeight = 60;
        }

        private void OnAddDropdown(Rect buttonRect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();

            List<LanguageType> types = new List<LanguageType>();

            Array allTypes = Enum.GetValues(typeof(LanguageType));

            foreach (var type in allTypes)
            {
                types.Add((LanguageType)type);
            }

            int length = _languages.arraySize;
            for (int i = 0; i < length; i++)
            {
                LanguageType type = (LanguageType)_languages.GetArrayElementAtIndex(i).FindPropertyRelative("_language").enumValueIndex;
                types.Remove(type);
            }

            foreach (var type in types)
            {
                menu.AddItem(new GUIContent("" + type), false, OnAdd, type);
            }

            menu.ShowAsContext();
        }

        private void OnAdd(object obj)
        {
            LanguageType type = (LanguageType)obj;

            var index = _languages.arraySize;
            _languages.arraySize++;
            _languagesReorder.index = index;

            var element = _languages.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("_language").enumValueIndex = (int)type;
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "语言列表");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty property = _languages.GetArrayElementAtIndex(index);

            rect.y += 2;
            rect.x += 5;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width -= 10;

            Rect rt = new Rect(rect)
            {
                y = rect.y + 56,
                height = 1,
                width = rect.width
            };

            EditorGUI.DrawRect(rt, Color.black);

            SerializedProperty language = property.FindPropertyRelative("_language");
            SerializedProperty font = property.FindPropertyRelative("_font");
            SerializedProperty srcPath = property.FindPropertyRelative("_srcPath");

            Rect languageRt = new Rect(rect)
            {
                y = rect.y + 2
            };
            Rect fontRt = new Rect(rect)
            {
                y = languageRt.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect srcRt = new Rect(rect)
            {
                y = fontRt.y + EditorGUIUtility.singleLineHeight + 2
            };

            EditorGUI.LabelField(languageRt, "" + (LanguageType)language.enumValueIndex);
            EditorGUI.PropertyField(fontRt, font);
            EditorGUI.PropertyField(srcRt, srcPath);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_debugType);

            _languagesReorder.DoLayoutList();

            if (EditorGUI.EndChangeCheck())
            {
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}