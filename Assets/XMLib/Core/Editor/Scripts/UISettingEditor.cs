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
        private SerializedProperty Panels;

        public override void OnInspectorGUI()
        {
            Panels = serializedObject.FindProperty("Panels");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(Panels, true);

            if (GUILayout.Button("检查"))
            {
                CheckData(Panels);
            }

            EditorGUILayout.HelpBox(_tip, MessageType.None);

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                CheckData(Panels);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        private void CheckData(SerializedProperty panels)
        {
            bool isError = false;
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
                    _errorMsg = string.Format("第{0}条对象为null.", i);
                    isError = true;
                    break;
                }

                panel = obj.GetComponent<IUIPanel>();
                if (null == panel)
                {
                    _errorMsg = string.Format("第{0}条对象没有IUIPanel组件", i);
                    isError = true;
                    break;
                }

                if (panelNames.TryGetValue(panel.PanelName, out existIndex))
                {
                    _errorMsg = string.Format("第{0}条与第{1}条对象的PanelName重复.", i, existIndex);
                    isError = true;
                    break;
                }

                panelNames.Add(panel.PanelName, i);
            }

            if (!isError)
            {
                _errorMsg = "";
            }
        }
    }
}