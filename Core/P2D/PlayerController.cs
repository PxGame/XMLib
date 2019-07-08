/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2/18/2019 7:57:12 PM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMLib.P2D
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public class PlayerController : RigidbodyController
    {
        #region 公开参数

        /// <summary>
        /// 玩家设置
        /// </summary>
        public PlayerSetting playerSetting { get { return _playerSetting; } }

        #endregion

        /// <summary>
        /// 玩家设置
        /// </summary>
        [SerializeField]
        protected PlayerSetting _playerSetting;

        /// <summary>
        /// 跳跃速度
        /// </summary>
        private Vector2 _preJumpSpeedRange;

        /// <summary>
        /// 开始跳跃
        /// </summary>
        private bool _jumpBegin;

        /// <summary>
        /// 结束跳跃
        /// </summary>
        private bool _jumpEnd;

        /// <summary>
        /// 移动方向
        /// </summary>
        private float _moveX;

        /// <summary>
        /// 猛冲
        /// </summary>
        private bool _dash;

        /// <summary>
        /// 猛冲中
        /// </summary>
        private bool _dashing;

        /// <summary>
        /// 猛冲速度
        /// </summary>
        private float _preDashSpeed;

        /// <summary>
        /// 猛冲计时器
        /// </summary>
        private float _dashTimer;

        /// <summary>
        /// 最后的朝向
        /// </summary>
        private float _lastLookAt;

        /// <summary>
        /// 初始化数据
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            //计算跳跃速度
            float tmp = 2.0f * Mathf.Abs(gravity);
            _preJumpSpeedRange.x = Mathf.Sqrt(tmp * _playerSetting.jumpHeightRange.x);
            _preJumpSpeedRange.y = Mathf.Sqrt(tmp * _playerSetting.jumpHeightRange.y);

            _jumpBegin = false;
            _jumpEnd = false;
            _moveX = 0f;
            _dash = false;
            _dashing = false;
            _preDashSpeed = _playerSetting.dashDistance / _playerSetting.dashTime;
            _dashTimer = 0f;
            _lastLookAt = 1;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="x">移动方向</param>
        public void Move(float x)
        {
            _moveX = x;
        }

        /// <summary>
        /// 开始跳跃
        /// </summary>
        public void JumpBegin()
        {
            _jumpBegin = true;
        }

        /// <summary>
        /// 结束跳跃
        /// </summary>
        public void JumpEnd()
        {
            _jumpEnd = true;
        }

        /// <summary>
        /// 冲刺
        /// </summary>
        public void Dash()
        {
            _dash = true;
        }

        protected override void Update()
        {
            _velocity.x = _moveX * _playerSetting.speed;

            if (_jumpBegin)
            { //开始跳跃
                _jumpBegin = false;

                _velocity.y = _preJumpSpeedRange.y;
            }

            if (_jumpEnd)
            { //结束跳跃
                _jumpEnd = false;

                if (_velocity.y > _preJumpSpeedRange.x)
                {
                    _velocity.y = _preJumpSpeedRange.x;
                }
            }

            if (_dash)
            {
                _dash = false;
                _dashing = true;
                _dashTimer = 0f;
            }

            if (_dashing)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer >= _playerSetting.dashTime || _isHorizontalHitted)
                {
                    _dashing = false;
                }

                _velocity.x = _preDashSpeed * _lastLookAt;
                _velocity.y = 0f;
            }

            //物理计算
            UpdateRigidBody(Time.deltaTime, !_dashing);

            if (0 != _velocity.x)
            {
                _lastLookAt = Mathf.Sign(_velocity.x);
            }
        }
    }
}