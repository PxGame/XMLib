using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM.Services;

namespace XMEditor
{
    [CustomEditor(typeof(PoolSetting))]
    public class PoolItemSettingEditor : Editor
    {
        private string _tip = "每个对象需有或继承于PoolItem的组件，且PoolName不能相同";
        private string _errorMsg = "";
        private SerializedProperty items;

        public override void OnInspectorGUI()
        {
            items = serializedObject.FindProperty("Items");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(items, true);

            if (GUILayout.Button("检查"))
            {
                CheckData(items);
            }

            EditorGUILayout.HelpBox(_tip, MessageType.None);

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                CheckData(items);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        private void CheckData(SerializedProperty items)
        {
            bool isError = false;
            int length = items.arraySize;
            GameObject obj;
            PoolItem item;
            Dictionary<string, int> poolNames = new Dictionary<string, int>();
            int existIndex;
            for (int i = 0; i < length; i++)
            {
                obj = (GameObject)items.GetArrayElementAtIndex(i).objectReferenceValue;
                if (null == obj)
                {
                    _errorMsg = string.Format("第{0}条对象为null.", i);
                    isError = true;
                    break;
                }

                item = obj.GetComponent<PoolItem>();
                if (null == item)
                {
                    _errorMsg = string.Format("第{0}条对象没有PoolItem组件", i);
                    isError = true;
                    break;
                }

                if (poolNames.TryGetValue(item.PoolName, out existIndex))
                {
                    _errorMsg = string.Format("第{0}条与第{1}条对象的PoolName重复.", i, existIndex);
                    isError = true;
                    break;
                }

                poolNames.Add(item.PoolName, i);
            }

            if (!isError)
            {
                _errorMsg = "";
            }
        }
    }
}