/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2/21/2019 10:25:32 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.P2D
{
    /// <summary>
    /// 路径创建者
    /// </summary>
    public class PathCreator : MonoBehaviour
    {
        /// <summary>
        /// 路径点
        /// </summary>
        [SerializeField]
        protected List<Vector3> _points = new List<Vector3>();

        /// <summary>
        /// 是否循环
        /// </summary>
        [SerializeField]
        protected bool _isClose = false;

        /// <summary>
        /// 是否是本地坐标
        /// </summary>
        [SerializeField]
        protected bool _isLocal = false;

        /// <summary>
        /// 获取世界坐标路径
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetWorldPaths()
        {
            int length = _points.Count;
            List<Vector3> paths = new List<Vector3>(length);

            if (_isLocal)
            {
                Vector3 point;

                for (int i = 0; i < length; i++)
                {
                    point = transform.TransformPoint(_points[i]);
                    paths.Add(point);
                }
            }
            else
            {
                paths.AddRange(_points);
            }

            return paths;
        }

        /// <summary>
        /// 获取本地路径
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetLocalPaths()
        {
            int length = _points.Count;
            List<Vector3> paths = new List<Vector3>(length);

            if (!_isLocal)
            {
                Vector3 point;

                for (int i = 0; i < length; i++)
                {
                    point = transform.InverseTransformPoint(_points[i]);
                    paths.Add(point);
                }
            }
            else
            {
                paths.AddRange(_points);
            }

            return paths;
        }
    }
}