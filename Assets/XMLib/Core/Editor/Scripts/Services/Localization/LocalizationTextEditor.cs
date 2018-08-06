using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XM.Services;
using XM.Services.Localization;

namespace XMEditor.Services.Localization
{
    /// <summary>
    /// 本地化文本编辑器
    /// </summary>
    [CustomEditor(typeof(LocalizationText))]
    [CanEditMultipleObjects]
    public class LocalizationTextEditor : LocalizationItemEditor
    {
        protected override void OnValueChanged()
        {
            if (null != targets)
            {
                /*
                Text text;
                int length = targets.Length;
                for (int i = 0; i < length; i++)
                {
                    text = ((LocalizationText)targets[i]).GetComponent<Text>();
                }
                */
            }
        }
    }
}