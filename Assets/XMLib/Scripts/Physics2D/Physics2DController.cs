/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/4/2019 11:13:49 AM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XMLib
{
    /// <summary>
    /// 2D物理控制
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Physics2DController : MonoBehaviour
    {
        #region 公开参数

        /// <summary>
        /// 设置
        /// </summary>
        public Physics2DSetting Setting { get { return _setting; } }

        /// <summary>
        /// 碰撞体
        /// </summary>
        public BoxCollider2D Collider2D { get { return _collider2D; } }

        #endregion 公开参数

        #region 参数

        /// <summary>
        /// 设置
        /// </summary>
        [SerializeField]
        protected Physics2DSetting _setting;

        /// <summary>
        /// 碰撞体
        /// </summary>
        protected BoxCollider2D _collider2D;

        #endregion 参数

        #region 共享参数

        /// <summary>
        /// 射线检测缓冲区
        /// </summary>
        protected RaycastHit2D[] _raycastBuffer;

        /// <summary>
        /// 移动偏移
        /// </summary>
        protected Vector2 _moveOffset;

        #region 射线检测

        /// <summary>
        /// 横向检查项
        /// </summary>
        protected List<CheckItem> _horizontalCheckItems;

        /// <summary>
        /// 纵向检查项
        /// </summary>
        protected List<CheckItem> _verticalCheckItems;

        /// <summary>
        /// 横向检查结果
        /// </summary>
        protected List<CheckResult> _horizontalCheckResults;

        /// <summary>
        /// 纵向检查结果
        /// </summary>
        protected List<CheckResult> _verticalCheckResults;

        #endregion 射线检测

        #endregion 共享参数

        #region Mono

        private void Awake()
        {
            //初始化
            _raycastBuffer = new RaycastHit2D[_setting.raycastBufferCount];
            _horizontalCheckItems = new List<CheckItem>();
            _verticalCheckItems = new List<CheckItem>();
            _horizontalCheckResults = new List<CheckResult>();
            _verticalCheckResults = new List<CheckResult>();

            //获取组件
            _collider2D = GetComponent<BoxCollider2D>();
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
        }

#endif

        #endregion Editor

        #endregion Mono

        #region 公开方法

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="offset">偏移</param>
        public void Move(Vector2 offset)
        {
            _moveOffset = offset;
            RaycastCheck();
        }

        #endregion 公开方法

        #region 私有方法

        /// <summary>
        /// 射线检测
        /// </summary>
        protected void RaycastCheck()
        {
            //处理数据
            Precompute();

            using (var scope = new IgnoreRaycastScope(gameObject))
            {//忽略当前物体上的碰撞
            }
        }

        /// <summary>
        /// 预处理数据
        /// </summary>
        protected void Precompute()
        {
            //校验数据
            InitListSize(_horizontalCheckItems, _setting.horizontalRayCount);
            InitListSize(_verticalCheckItems, _setting.verticalRayCount);

            Bounds bounds = _collider2D.bounds;
        }

        /// <summary>
        /// 初始化数组大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        private void InitListSize<T>(List<T> list, int count) where T : new()
        {
            int offset = list.Count - count;
            if (0 == offset)
            {
                return;
            }

            if (offset > 0)
            {//移除多余对象
                list.RemoveRange(0, offset);
            }
            else
            {//添加新对象
                while (offset < 0)
                {
                    list.Add(new T());
                    offset++;
                }
            }

            if (list.Count != count)
            {
                throw new InvalidOperationException("初始化List大小错误");
            }
        }

        #endregion 私有方法
    }
}