using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FCEditor
{
    public class SkinView : EditorWindow
    {
        private static List<GUIStyle> styles = null;

        [MenuItem("XM/Skin Style View")]
        public static void Test()
        {
            EditorWindow.GetWindow<SkinView>("styles");
        }

        public Vector2 scrollPosition = Vector2.zero;

        private void OnEnable()
        {
            styles = new List<GUIStyle>();
            foreach (PropertyInfo fi in typeof(EditorStyles).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object o = fi.GetValue(null, null);
                if (o.GetType() == typeof(GUIStyle))
                {
                    styles.Add(o as GUIStyle);
                }
            }
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < styles.Count; i++)
            {
                GUILayout.Label("EditorStyles." + styles[i].name, styles[i]);
            }
            GUILayout.EndScrollView();
        }
    }
}