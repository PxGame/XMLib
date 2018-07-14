using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XM.Services;

namespace XMEditor
{
    [CustomEditor(typeof(PoolSetting))]
    public class PoolSettingEditor : Editor
    {
        private string _tip = "每个对象需有或继承于PoolItem的组件，且PoolName不能相同";
        private string _errorMsg = "";
        private SerializedProperty _items;
        private SerializedProperty _debugType;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _debugType = serializedObject.FindProperty("_debugType");
            _items = serializedObject.FindProperty("_items");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_debugType);
            EditorGUILayout.PropertyField(_items, true);

            if (GUILayout.Button("检查"))
            {
                CheckData(_items);
            }

            EditorGUILayout.HelpBox(_tip, MessageType.None);

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                CheckData(_items);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        private void CheckData(SerializedProperty items)
        {
            bool isError = false;
            string tmpMsg = "";
            try
            {
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
                        tmpMsg = string.Format("第{0}条对象为null.", i);
                        throw new System.Exception(tmpMsg);
                    }

                    item = obj.GetComponent<PoolItem>();
                    if (null == item)
                    {
                        tmpMsg = string.Format("第{0}条对象没有PoolItem组件", i);
                        throw new System.Exception(tmpMsg);
                    }

                    if (poolNames.TryGetValue(item.PoolName, out existIndex))
                    {
                        tmpMsg = string.Format("第{0}条与第{1}条对象的PoolName重复.", i, existIndex);
                        throw new System.Exception(tmpMsg);
                    }

                    poolNames.Add(item.PoolName, i);
                }
            }
            catch (Exception ex)
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