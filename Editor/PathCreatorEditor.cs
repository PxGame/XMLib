/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2/21/2019 10:26:45 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace XMLib.P2D
{
    /// <summary>
    /// PathCreatorEditor
    /// </summary>
    [CustomEditor(typeof(PathCreator))]
    public class PathCreatorEditor: Editor
    {
        private SerializedProperty _points;
        private SerializedProperty _isClose;
        private SerializedProperty _isLocal;
        private ReorderableList _pointsEx;

        private GUIStyle _textStyle;

        private void OnEnable()
        {
            _points = serializedObject.FindProperty("_points");
            _isClose = serializedObject.FindProperty("_isClose");
            _isLocal = serializedObject.FindProperty("_isLocal");
            _pointsEx = new ReorderableList(serializedObject, _points, true, true, true, true);

            _pointsEx.drawElementCallback += OnDrawElement;
            _pointsEx.drawHeaderCallback += OnDrawHeader;
            _pointsEx.onChangedCallback += OnPointsChanged;

            _textStyle = new GUIStyle();
            _textStyle.fontSize = 20;
            _textStyle.normal.background = Texture2D.whiteTexture;
        }

        private void OnPointsChanged(ReorderableList list)
        {
        }

        private void OnDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "节点列表");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty itemData = _pointsEx.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, itemData, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            PathCreator pathController = (PathCreator)target;
            Transform transform = pathController.transform;

            serializedObject.Update();

            int length = _points.arraySize;
            bool isLocal = _isLocal.boolValue;
            bool isLoop = _isClose.boolValue;

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(4);
            _pointsEx.DoLayoutList();

            EditorGUILayout.PropertyField(_isClose);
            EditorGUILayout.PropertyField(_isLocal);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {
            PathCreator pathCreator = (PathCreator)target;
            Transform transform = pathCreator.transform;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            int length = _points.arraySize;
            bool isLocal = _isLocal.boolValue;
            bool isLoop = _isClose.boolValue;

            if (length > 1)
            {
                Vector3 point;
                Vector3 lastPoint;
                Vector3 nextPoint;

                //
                point = _points.GetArrayElementAtIndex(0).vector3Value;
                if (isLocal)
                {
                    point = transform.TransformPoint(point);
                }
                point = Handles.DoPositionHandle(point, Quaternion.identity);
                DrawNumber(point, 0);
                nextPoint = point;
                if (isLocal)
                {
                    point = transform.InverseTransformPoint(point);
                }
                _points.GetArrayElementAtIndex(0).vector3Value = point;
                lastPoint = nextPoint;
                //

                for (int i = 1; i < length; i++)
                {
                    //
                    point = _points.GetArrayElementAtIndex(i).vector3Value;
                    if (isLocal)
                    {
                        point = transform.TransformPoint(point);
                    }
                    point = Handles.DoPositionHandle(point, Quaternion.identity);
                    DrawNumber(point, i);
                    nextPoint = point;
                    if (isLocal)
                    {
                        point = transform.InverseTransformPoint(point);
                    }
                    _points.GetArrayElementAtIndex(i).vector3Value = point;
                    //

                    //Draw
                    DrawLine(lastPoint, nextPoint);
                    //

                    lastPoint = nextPoint;
                }

                if (isLoop)
                {
                    //
                    point = _points.GetArrayElementAtIndex(0).vector3Value;
                    if (isLocal)
                    {
                        point = transform.TransformPoint(point);
                    }
                    nextPoint = point;
                    //

                    //Draw

                    DrawLine(lastPoint, nextPoint);

                    //
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawNumber(Vector3 pos, int num)
        {
            Color oldColor = Handles.color;
            Handles.color = Color.yellow;
            Handles.Label(pos, "" + num, _textStyle);
            Handles.color = oldColor;
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            Color oldColor = Handles.color;
            Handles.color = Color.yellow;
            Handles.DrawLine(start, end);
            Handles.color = oldColor;
        }
    }
}