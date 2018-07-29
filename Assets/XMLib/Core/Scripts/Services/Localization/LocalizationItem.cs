using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 本地化服务元素
    /// </summary>
    public class LocalizationItem : MonoBehaviour
    {
        [SerializeField]
        private string _id;

        public string ID { get { return _id; } }

        protected virtual void Awake()
        {
            LocalizationService.Inst.AddItem(this);
        }

        protected virtual void OnDestroy()
        {
            if (null != LocalizationService.Inst)
            {
                LocalizationService.Inst.RemoveItem(this);
            }
        }

        protected virtual void OnUpdateText(string text)
        {
        }
    }
}