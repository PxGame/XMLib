using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM;
using XM.Services;
using XM.Services.UI;

namespace XMEditor.Services.UI
{
    [CustomEditor(typeof(UISetting))]
    public class UISettingEditor : Editor
    {
        private string _tip = "每个对象需有或继承于IUIPanel的组件，且PanelName不能相同";
        private string _errorMsg = "";
        private SerializedProperty _root;
        private SerializedProperty _panels;
        private SerializedProperty _debugType;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _debugType = serializedObject.FindProperty("_debugType");
            _root = serializedObject.FindProperty("_root");
            _panels = serializedObject.FindProperty("_panels");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_debugType);
            EditorGUILayout.PropertyField(_root);
            EditorGUILayout.PropertyField(_panels, true);

            if (GUILayout.Button("检查"))
            {
                CheckData(_root, _panels);
            }

            EditorGUILayout.HelpBox(_tip, MessageType.None);

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                CheckData(_root, _panels);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        private void CheckData(SerializedProperty root, SerializedProperty panels)
        {
            bool isError = false;

            try
            {
                GameObject rootObj = (GameObject)root.objectReferenceValue;
                Checker.NotNull(rootObj, "根节点对象为null");

                UIRoot uiRoot = rootObj.GetComponent<UIRoot>();
                Checker.NotNull(uiRoot, "根节点对象没有UIRoot组件");

                int length = panels.arraySize;
                GameObject obj;
                IUIPanel panel;
                Dictionary<string, int> panelNames = new Dictionary<string, int>();
                int existIndex;
                for (int i = 0; i < length; i++)
                {
                    obj = (GameObject)panels.GetArrayElementAtIndex(i).objectReferenceValue;
                    Checker.NotNull(obj, "第{0}条对象为null.", i);

                    panel = obj.GetComponent<IUIPanel>();
                    Checker.NotNull(panel, "第{0}条对象没有IUIPanel组件", i);

                    bool isExist = panelNames.TryGetValue(panel.PanelName, out existIndex);
                    Checker.IsFalse(isExist, "第{0}条与第{1}条对象的PanelName重复.", i, existIndex);

                    panelNames.Add(panel.PanelName, i);
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