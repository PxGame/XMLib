using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMEditor
{
    /// <summary>
    /// 公共工具
    /// </summary>
    public class CommonWindow : EditorWindow
    {
        [MenuItem("XM/Common")]
        public static void Display()
        {
            var win = GetWindow<CommonWindow>("公共工具");
            win.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("立即保存资源"))
            {
                AssetDatabase.SaveAssets();
            }

            GUILayout.EndVertical();
        }
    }
}