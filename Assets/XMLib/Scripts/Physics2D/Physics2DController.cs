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
        public Physics2DSetting setting { get { return _setting; } }

        /// <summary>
        /// 碰撞体
        /// </summary>
        public BoxCollider2D boxCollider2D { get { return _boxCollider2D; } }

        /// <summary>
        /// 包围盒
        /// </summary>
        public Bounds bounds { get { return _boxCollider2D.bounds; } }

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
        protected BoxCollider2D _boxCollider2D;

        #endregion 参数

        #region 共享参数

        /// <summary>
        /// 射线检测缓冲区
        /// </summary>
        protected RaycastHit2D[] _raycastBuffer;

        /// <summary>
        /// 移动偏移
        /// </summary>
        protected Vector2 _velocity;

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

        /// <summary>
        /// bound
        /// </summary>
        protected Bounds _boundsNoSkin;

        /// <summary>
        /// 横向射线数量
        /// </summary>
        protected int _horizontalRayCount;

        /// <summary>
        /// 纵向射线数量
        /// </summary>
        protected int _verticalRayCount;

        /// <summary>
        /// 横向射线间距
        /// </summary>
        protected float _horizontalRaySpacing;

        /// <summary>
        /// 纵向射线间距
        /// </summary>
        protected float _verticalRaySpacing;

        /// <summary>
        /// 射线起点
        /// </summary>
        protected RaycastOrigins raycastOrigins;

        #endregion 射线检测

        #endregion 共享参数

        #region Mono

        private void Awake()
        {
            //初始化
            _raycastBuffer = new RaycastHit2D[_setting.rayBufferCount];
            _horizontalCheckItems = new List<CheckItem>();
            _verticalCheckItems = new List<CheckItem>();
            _horizontalCheckResults = new List<CheckResult>();
            _verticalCheckResults = new List<CheckResult>();

            //获取组件
            _boxCollider2D = GetComponent<BoxCollider2D>();
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
        /// <param name="velocity">偏移</param>
        public void Move(Vector2 velocity)
        {
            _velocity = velocity;
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
            //去皮包围盒
            _boundsNoSkin = _boxCollider2D.bounds;
            _boundsNoSkin.Expand(_setting.skinWidth * -2);

            //计算射线起点
            raycastOrigins = new RaycastOrigins();
            raycastOrigins.bottomLeft = new Vector2(_boundsNoSkin.min.x, _boundsNoSkin.min.y);
            raycastOrigins.bottomRight = new Vector2(_boundsNoSkin.max.x, _boundsNoSkin.min.y);
            raycastOrigins.topLeft = new Vector2(_boundsNoSkin.min.x, _boundsNoSkin.max.y);
            raycastOrigins.topRight = new Vector2(_boundsNoSkin.max.x, _boundsNoSkin.max.y);

            //横纵向射线数量
            _horizontalRayCount = Mathf.RoundToInt(_boundsNoSkin.size.y / _setting.raySpacing);
            _verticalRayCount = Mathf.RoundToInt(_boundsNoSkin.size.x / _setting.raySpacing);

            //横纵向射线间距
            _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);

            //更新检查项
            UpdateCheckItem();
        }

        /// <summary>
        /// 更新检查项
        /// </summary>
        protected void UpdateCheckItem()
        {
            //设置容器大小
            _horizontalCheckItems.FixedCountWithInstance(_horizontalRayCount);
            _verticalCheckItems.FixedCountWithInstance(_verticalRayCount);

            float directionX = Mathf.Sign(_velocity.x);
            float directionY = Mathf.Sign(_velocity.y);

            //计算横向检测项
            if (_velocity.y != 0)
            {
            }

            //计算纵向检测项
            if (_velocity.x != 0)
            {
            }
        }

        #endregion 私有方法
    }
}