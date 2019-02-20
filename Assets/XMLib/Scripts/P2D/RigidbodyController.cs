/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/13/2019 2:59:43 PM
 */

using System;
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
        #region 公开参数

        /// <summary>
        /// 重力
        /// </summary>
        public float gravity { get { return Physics2D.gravity.y * rigidbodySetting.gravityScale; } }

        /// <summary>
        /// 当前速度
        /// </summary>
        public Vector2 velocity { get { return _velocity; } set { _velocity = value; } }

        /// <summary>
        /// 刚体设置
        /// </summary>
        public RigidbodySetting rigidbodySetting { get { return _rigidbodySetting; } }

        #endregion 公开参数

        /// <summary>
        /// 刚体设置
        /// </summary>
        [SerializeField]
        protected RigidbodySetting _rigidbodySetting;

        /// <summary>
        /// 横向运动是否发生碰撞
        /// </summary>
        protected bool _isHorizontalHitted;

        /// <summary>
        /// 纵向运动是否发生碰撞
        /// </summary>
        protected bool _isVerticalHitted;

        /// <summary>
        /// 横向运动碰撞信息
        /// </summary>
        protected RaycastHit2D _horizontalNearHit;

        /// <summary>
        /// 纵向运动碰撞信息
        /// </summary>
        protected RaycastHit2D _verticalNearHit;

        /// <summary>
        /// 当前速度
        /// </summary>
        protected Vector2 _velocity;

        protected virtual void Update()
        {
            UpdateRigidBody(Time.deltaTime, true);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            _velocity = Vector2.zero;
        }

        /// <summary>
        /// 更新刚体
        /// </summary>
        /// <param name="deltaTime">时间间隔</param>
        /// <param name="useGravity">使用重力</param>
        protected void UpdateRigidBody(float deltaTime, bool useGravity)
        {
            //更新参数
            UpdateRaycastParameter();

            //添加重力
            if (useGravity)
            {
                _velocity.y += gravity * deltaTime;
            }

            //计算当前帧移动的距离
            Vector2 frameMove = _velocity * deltaTime;

            //检测
            _isVerticalHitted = RaycastVerticalNear(frameMove.y, ref _verticalNearHit);
            _isHorizontalHitted = RaycastHorizontalNear(frameMove.x, ref _horizontalNearHit);

            if (_isHorizontalHitted)
            { //横向运动撞到物体
                _velocity.x = 0; //速度归零
                frameMove.x = (_horizontalNearHit.distance - _raycastSetting.skinWidth) * Mathf.Sign(frameMove.x); //校验碰撞时的移动，使之贴靠碰撞物体
            }

            if (_isVerticalHitted)
            { //纵向运动撞到物体
                _velocity.y = 0; //速度归零
                frameMove.y = (_verticalNearHit.distance - _raycastSetting.skinWidth) * Mathf.Sign(frameMove.y); //校验碰撞时的移动，使之贴靠碰撞物体
            }

            if (_isVerticalHitted && ProcessMovePlatform(_verticalNearHit.collider, deltaTime, ref frameMove))
            { //移动中遇到障碍物时，未经行处理，移动平台移动过快时存在问题

            }

            //移动物体
            transform.Translate(frameMove);
        }

        /// <summary>
        /// 处理移动平台
        /// </summary>
        /// <param name="collider">当前移动平台</param>
        /// <param name="deltaTime">当前帧间隔</param>
        /// <param name="frameMove">当前帧移动距离</param>
        /// <returns>是否处理</returns>
        protected bool ProcessMovePlatform(Collider2D collider, float deltaTime, ref Vector2 frameMove)
        {
            if (!collider.CompareTag(_rigidbodySetting.movePlatformTag))
            { //不是移动平台
                return false;
            }

            IMovePlatform movePlatform = collider.GetComponent<IMovePlatform>();
            if (null == movePlatform)
            { //未发现移动平台脚本
                return false;
            }

            frameMove += movePlatform.velocity * deltaTime;

            return true;
        }

        protected virtual void OnDrawGizmos()
        {
            if (_isHorizontalHitted)
            { //绘制碰撞点
                GizmosUtil.DrawCircle2D(_horizontalNearHit.point, 0.1f, Color.blue);
            }
            if (_isVerticalHitted)
            { //绘制碰撞点
                GizmosUtil.DrawCircle2D(_verticalNearHit.point, 0.1f, Color.red);
            }
        }
    }
}