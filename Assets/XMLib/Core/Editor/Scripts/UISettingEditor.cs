using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM.Services;

namespace XMEditor
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
            EditorGUILayout.PropertyField(_root, true);
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
            string tmpMsg = "";

            try
            {
                GameObject rootObj = (GameObject)root.objectReferenceValue;
                if (null == rootObj)
                {
                    tmpMsg = string.Format("根节点对象为null.");
                    throw new System.Exception(tmpMsg);
                }

                UIRoot uiRoot = rootObj.GetComponent<UIRoot>();
                if (null == uiRoot)
                {
                    tmpMsg = string.Format("根节点对象没有UIRoot组件");
                    throw new System.Exception(tmpMsg);
                }

                int length = panels.arraySize;
                GameObject obj;
                IUIPanel panel;
                Dictionary<string, int> panelNames = new Dictionary<string, int>();
                int existIndex;
                for (int i = 0; i < length; i++)
                {
                    obj = (GameObject)panels.GetArrayElementAtIndex(i).objectReferenceValue;
                    if (null == obj)
                    {
                        tmpMsg = string.Format("第{0}条对象为null.", i);
                        throw new System.Exception(tmpMsg);
                    }

                    panel = obj.GetComponent<IUIPanel>();
                    if (null == panel)
                    {
                        tmpMsg = string.Format("第{0}条对象没有IUIPanel组件", i);
                        throw new System.Exception(tmpMsg);
                    }

                    if (panelNames.TryGetValue(panel.PanelName, out existIndex))
                    {
                        tmpMsg = string.Format("第{0}条与第{1}条对象的PanelName重复.", i, existIndex);
                        throw new System.Exception(tmpMsg);
                    }

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