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
        /// 初始化数据
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            //计算跳跃速度
            float tmp = 2.0f * Mathf.Abs(gravity);
            _preJumpSpeedRange.x = Mathf.Sqrt(tmp * _playerSetting.jumpHeightRange.x);
            _preJumpSpeedRange.y = Mathf.Sqrt(tmp * _playerSetting.jumpHeightRange.y);
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

            //物理计算
            base.Update();
        }
    }
}