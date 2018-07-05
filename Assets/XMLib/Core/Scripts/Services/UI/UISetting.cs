using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// UI设置
    /// </summary>
    [CreateAssetMenu]
    public class UISetting : ScriptableObject
    {
        [SerializeField]
        protected List<GameObject> Panels;

        /// <summary>
        /// 获取面板字典
        /// </summary>
        public Dictionary<string, GameObject> GetPanelDict()
        {
            //转换成字典
            int length = Panels.Count;
            Dictionary<string, GameObject> itemDict = new Dictionary<string, GameObject>(length);

            GameObject obj;
            IUIPanel panel;
            for (int i = 0; i < length; i++)
            {
                obj = Panels[i];
                panel = obj.GetComponent<IUIPanel>();
                itemDict.Add(panel.PanelName, obj);
            }

            return itemDict;
        }
    }
}