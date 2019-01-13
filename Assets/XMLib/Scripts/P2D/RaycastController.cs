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

namespace XMLib.P2D
{
    /// <summary>
    /// 2D物理控制
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour
    {
        #region 公开参数

        /// <summary>
        /// 设置
        /// </summary>
        public RaycastSetting setting { get { return _raycastSetting; } }

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
        protected RaycastSetting _raycastSetting;

        /// <summary>
        /// 碰撞体
        /// </summary>
        protected BoxCollider2D _boxCollider2D;

        #endregion 参数

        #region 共享参数

        /// <summary>
        /// 射线检测缓冲区
        /// </summary>
        protected RaycastHit2D[] _raycastResults;

        /// <summary>
        /// 用于在发生变化时重新计算相关数据
        /// </summary>
        protected Bounds _lastBounds;

        #region 射线检测

        /// <summary>
        /// 去皮包围盒
        /// </summary>
        protected Bounds _boundsNoSkin;

        /// <summary>
        /// 包围盒
        /// </summary>
        protected Bounds _bounds;

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
        protected RaycastOrigins _raycastOrigins;

        #endregion 射线检测

        #endregion 共享参数

        #region Mono

        protected virtual void Awake()
        {
            //初始化
            _raycastResults = new RaycastHit2D[_raycastSetting.rayBufferCount];

            //获取组件
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        #endregion Mono

        #region 私有方法

        /// <summary>
        /// 需要在所有检测之前调用，以更新相关参数
        /// </summary>
        protected void UpdateRaycastParameter()
        {
            //包围盒
            _bounds = _boxCollider2D.bounds;

            //去皮包围盒
            _boundsNoSkin = _boxCollider2D.bounds;
            _boundsNoSkin.Expand(_raycastSetting.skinWidth * -2);

            //计算射线起点
            _raycastOrigins.bottomLeft.x = _boundsNoSkin.min.x;
            _raycastOrigins.bottomLeft.y = _boundsNoSkin.min.y;

            _raycastOrigins.bottomRight.x = _boundsNoSkin.max.x;
            _raycastOrigins.bottomRight.y = _boundsNoSkin.min.y;

            _raycastOrigins.topLeft.x = _boundsNoSkin.min.x;
            _raycastOrigins.topLeft.y = _boundsNoSkin.max.y;

            _raycastOrigins.topRight.x = _boundsNoSkin.max.x;
            _raycastOrigins.topRight.y = _boundsNoSkin.max.y;

            if (_lastBounds != _bounds)
            {//包围盒大小改变时更新数据
                //横纵向射线数量
                _horizontalRayCount = Mathf.RoundToInt(_boundsNoSkin.size.y / _raycastSetting.raySpacing);
                _verticalRayCount = Mathf.RoundToInt(_boundsNoSkin.size.x / _raycastSetting.raySpacing);

                //横纵向射线间距
                _horizontalRaySpacing = _boundsNoSkin.size.y / (_horizontalRayCount - 1);
                _verticalRaySpacing = _boundsNoSkin.size.x / (_verticalRayCount - 1);
            }

            //
            _lastBounds = _bounds;
        }

        /// <summary>
        /// 检查纵向最近碰撞
        /// </summary>
        /// <param name="y">纵向移动距离</param>
        /// <param name="nearHit">最近的碰撞信息</param>
        /// <returns>是否发生碰撞</returns>
        public bool RaycastVerticalNear(float y, ref RaycastHit2D nearHit)
        {
            if (0 == y)
            {//未移动，不用检测
                return false;
            }

            float directionSign = Mathf.Sign(y);
            float distance = Mathf.Abs(y) + _raycastSetting.skinWidth;
            Vector2 direction = Vector2.up * directionSign;

            Vector2 rayOrigin = (directionSign == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;//由左往右

            RaycastHit2D raycastResult;
            bool isHitted = false;
            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 origin = rayOrigin + Vector2.right * (_verticalRaySpacing * i);

                Debug.DrawLine(origin, origin + direction * distance, Color.red);

                int count = Physics2D.Raycast(origin, direction, _raycastSetting.contactFilter2D, _raycastResults, distance);
                if (0 == count)
                {//没有检测到碰撞
                    continue;
                }

                raycastResult = _raycastResults[0];
                if (!isHitted)
                {//第一次检测到碰撞，直接设置值
                    nearHit = raycastResult;
                    isHitted = true;
                }
                else if (raycastResult.distance < nearHit.distance)
                {//后面检测结果更近时，更新值
                    nearHit = raycastResult;
                }
            }

            return isHitted;
        }

        /// <summary>
        /// 检查横向最近的碰撞
        /// </summary>
        /// <param name="x">横向移动距离</param>
        /// <param name="nearHit">最近的碰撞信息</param>
        /// <returns>是否发生碰撞</returns>
        public bool RaycastHorizontalNear(float x, ref RaycastHit2D nearHit)
        {
            if (0 == x)
            {//未移动，不用检测
                return false;
            }

            float directionSign = Mathf.Sign(x);
            float distance = Mathf.Abs(x) + _raycastSetting.skinWidth;
            Vector2 direction = Vector2.right * directionSign;

            Vector2 rayOrigin = (directionSign == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;//由下往上

            RaycastHit2D raycastResult;
            bool isHitted = false;
            for (int i = 0; i < _horizontalRayCount; i++)
            {
                Vector2 origin = rayOrigin + Vector2.up * (_horizontalRaySpacing * i);

                Debug.DrawLine(origin, origin + direction * distance, Color.blue);

                int count = Physics2D.Raycast(origin, direction, _raycastSetting.contactFilter2D, _raycastResults, distance);
                if (0 == count)
                {//没有检测到碰撞
                    continue;
                }

                raycastResult = _raycastResults[0];
                if (!isHitted)
                {//第一次检测到碰撞，直接设置值
                    nearHit = raycastResult;
                    isHitted = true;
                }
                else if (raycastResult.distance < nearHit.distance)
                {//后面检测结果更近时，更新值
                    nearHit = raycastResult;
                }
            }

            return isHitted;
        }

        #endregion 私有方法
    }
}