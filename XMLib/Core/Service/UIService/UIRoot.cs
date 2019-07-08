/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/18/2019 1:05:34 PM
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace XMLib.UIService
{
    /// <summary>
    /// UI 根节点
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        /// <summary>
        /// 层元素
        /// </summary>
        [Serializable]
        private class LayerItem
        {
            /// <summary>
            /// 层名
            /// </summary>
            public string layerName { get { return _layerName; } }

            /// <summary>
            /// 层节点
            /// </summary>
            public RectTransform layerRoot { get { return _layerRoot; } }

            /// <summary>
            /// 层名
            /// </summary>
            [SerializeField]
            private string _layerName;

            /// <summary>
            /// 层节点
            /// </summary>
            [SerializeField]
            private RectTransform _layerRoot;
        }

        [SerializeField]
        private Camera _uiCamera;

        /// <summary>
        /// 层元素列表
        /// </summary>
        [SerializeField]
        private List<LayerItem> _layers;

        /// <summary>
        /// 缓存字典
        /// </summary>
        private Dictionary<string, RectTransform> _layerDict;

        /// <summary>
        /// 获取对应节点
        /// </summary>
        /// <param name="layerName">节点名</param>
        /// <returns>节点</returns>
        public RectTransform this[string layerName] { get { return Get(layerName); } }

        /// <summary>
        /// ui 相机
        /// </summary>
        public Camera uiCamera { get { return _uiCamera; } }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Init()
        {
            _layerDict = new Dictionary<string, RectTransform>();

            foreach (var layer in _layers)
            {
                _layerDict.Add(layer.layerName, layer.layerRoot);
            }
        }

        /// <summary>
        /// 获取对应节点
        /// </summary>
        /// <param name="layerName">节点名</param>
        /// <returns>节点</returns>
        public RectTransform Get(string layerName)
        {
            if (null == _layerDict)
            {
                Init();
            }

            RectTransform rectTransform = null;
            if (!_layerDict.TryGetValue(layerName, out rectTransform))
            {
                Debug.LogWarningFormat("UIRoot上未找到改节点:{0}", layerName);
            }

            return rectTransform;
        }

        ///// <summary>
        ///// 屏幕坐标转换到本地坐标
        ///// </summary>
        ///// <param name="rect"></param>
        ///// <param name="screenPoint"></param>
        ///// <param name="localPoint"></param>
        ///// <returns></returns>
        //public bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, out Vector2 localPoint)
        //{
        //    return RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, _uiCamera, out localPoint);
        //}
    }
}