/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/13/2019 2:59:43 PM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.P2D
{
    /// <summary>
    /// 刚体控制器
    /// </summary>
    public class RigidbodyController : RaycastController
    {
        /// <summary>
        /// 重力
        /// </summary>
        public float gravity { get { return Physics2D.gravity.y * _rigidbodySetting.gravityScale; } }

        /// <summary>
        /// 当前速度
        /// </summary>
        public Vector2 velocity { get { return _velocity; } set { _velocity = value; } }

        /// <summary>
        /// 设置
        /// </summary>
        [SerializeField]
        protected RigidbodySetting _rigidbodySetting;

        protected bool _isHorizontalHitted;
        protected bool _isVerticalHitted;
        protected RaycastHit2D _horizontalNearHit;
        protected RaycastHit2D _verticalNearHit;

        private Vector2 _velocity;

        protected void FixedUpdate()
        {
            //更新参数
            UpdateRaycastParameter();

            //添加重力
            _velocity.y += gravity * Time.fixedDeltaTime;

            //计算当前帧移动的距离
            Vector2 frameMove = _velocity * Time.fixedDeltaTime;

            //检测
            _isHorizontalHitted = RaycastHorizontalNear(frameMove.x, ref _horizontalNearHit);
            _isVerticalHitted = RaycastVerticalNear(frameMove.y, ref _verticalNearHit);

            if (_isHorizontalHitted)
            {//横向运动撞到物体
                _velocity.x = 0;//速度归零
                frameMove.x = (_horizontalNearHit.distance - _raycastSetting.skinWidth) * Mathf.Sign(frameMove.x);//校验碰撞时的移动，使之贴靠碰撞物体
            }

            if (_isVerticalHitted)
            {//纵向运动撞到物体
                _velocity.y = 0;//速度归零
                frameMove.y = (_verticalNearHit.distance - _raycastSetting.skinWidth) * Mathf.Sign(frameMove.y);//校验碰撞时的移动，使之贴靠碰撞物体
            }

            //移动物体
            transform.Translate(frameMove);
        }

        protected virtual void OnDrawGizmos()
        {
            if (_isHorizontalHitted)
            {//绘制碰撞点
                GizmosUtil.DrawCircle2D(_horizontalNearHit.point, 0.1f, Color.blue);
            }
            if (_isVerticalHitted)
            {//绘制碰撞点
                GizmosUtil.DrawCircle2D(_verticalNearHit.point, 0.1f, Color.red);
            }
        }
    }
}