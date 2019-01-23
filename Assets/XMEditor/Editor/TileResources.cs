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

        public List<GameObject> T0;

        public List<GameObject> T1;
        public List<GameObject> T2;
        public List<GameObject> T3;
        public List<GameObject> T4;
        public List<GameObject> T5;
        public List<GameObject> T6;
        public List<GameObject> T7;
        public List<GameObject> T8;
        public List<GameObject> T9;


        public List<GameObject> T28;
        public List<GameObject> T46;
        public List<GameObject> T248;
        public List<GameObject> T268;
        public List<GameObject> T246;
        public List<GameObject> T468;

        public GameObject Get(string fieldName)
        {
            FieldInfo info = GetType().GetField(fieldName);
            List<GameObject> objs = (List<GameObject>)info.GetValue(this);
            GameObject obj = objs[Random.Range(0, objs.Count - 1)];//随机一个
            return obj;
        }
    }
}