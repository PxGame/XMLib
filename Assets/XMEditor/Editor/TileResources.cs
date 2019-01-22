/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/23/2019 1:16:26 PM
 */

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XMEditor
{
    /// <summary>
    /// 地图资源设置
    /// </summary>
    [CreateAssetMenu(menuName = "XMLib/横版地图编辑器资源配置", fileName = "TileResourses")]
    [System.Serializable]
    public class TileResources : ScriptableObject
    {
        /// <summary>
        /// 元素名
        /// </summary>
        public string TypeName;

        /// <summary>
        /// 是否是简单模式
        /// </summary>
        public bool IsSimpleMode;

        /// <summary>
        /// 是否合并碰撞
        /// </summary>
        public bool IsCombatPhysic;

        /// <summary>
        /// 物体层级
        /// </summary>
        public string NewLayerName;

        public GameObject T0;

        public GameObject T1;
        public GameObject T2;
        public GameObject T3;
        public GameObject T4;
        public GameObject T5;
        public GameObject T6;
        public GameObject T7;
        public GameObject T8;
        public GameObject T9;


        public GameObject T28;
        public GameObject T46;
        public GameObject T248;
        public GameObject T268;
        public GameObject T246;
        public GameObject T468;


        public GameObject Get(string fieldName)
        {
            FieldInfo info = GetType().GetField(fieldName);
            return (GameObject)info.GetValue(this);
        }
    }
}