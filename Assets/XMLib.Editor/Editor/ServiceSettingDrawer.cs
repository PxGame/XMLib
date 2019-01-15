/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/15/2019 11:33:20 PM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMLib.Editor
{

    [CustomPropertyDrawer(typeof(ServiceSetting))]
    public class ServiceSettingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            object obj = property.objectReferenceValue;

            position.height *= 2;

            EditorGUI.LabelField(position, obj.GetType().Name);

            EditorGUI.EndProperty();
        }

    }
}