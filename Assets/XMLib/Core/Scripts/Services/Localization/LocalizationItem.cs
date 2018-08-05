using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XM.Services
{
    /// <summary>
    /// 本地化元素
    /// </summary>
    [DisallowMultipleComponent]
    public class LocalizationItem : MonoBehaviour
    {
        /// <summary>
        /// ID
        /// </summary>
        [SerializeField]
        protected string _id = null;
    }
}