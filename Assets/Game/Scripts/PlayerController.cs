using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.InputService;
using XMLib.P2D;

/// <summary>
/// 角色控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    private IInputService _input;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Transform _body;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private Vector2 _jumpHeightRange = new Vector2(1f, 3f);

    private RigidbodyController _controller;
    protected Vector2 _preJumpSpeedRange;

    private void Awake()
    {
        Debug.Log("Player Awake");
        //获取组件
        _controller = GetComponent<RigidbodyController>();

        //计算跳跃速度
        float tmp = 2.0f * Mathf.Abs(_controller.gravity);
        _preJumpSpeedRange.x = Mathf.Sqrt(tmp * _jumpHeightRange.x);
        _preJumpSpeedRange.y = Mathf.Sqrt(tmp * _jumpHeightRange.y);

        //获取输入服务
        _input = App.Make<IInputService>();
    }

    private bool _isPress = false;
    private Vector2 _velocity;
    private bool _stJumpDown;
    private bool _stJumpUp;

    private void Update()
    {
        if (!_isPress && _input.GetButton("Jump"))
        {
            _stJumpDown = true;
            _isPress = true;
        }
        else if (_isPress && !_input.GetButton("Jump"))
        {
            _stJumpUp = true;
            _isPress = false;
        }
    }

    private void FixedUpdate()
    {
        _velocity = _controller.velocity;

        _velocity.x = _input.GetAxisRaw("Horizontal") * _speed;
        //_velocity.y = _input.GetAxis("Vertical") * _speed;

        if (_stJumpDown)
        {
            _velocity.y = _preJumpSpeedRange.y;
            _stJumpDown = false;
        }

        if (_stJumpUp)
        {
            if (_velocity.y > _preJumpSpeedRange.x)
            {
                _velocity.y = _preJumpSpeedRange.x;
            }
            _stJumpUp = false;
        }

        _controller.velocity = _velocity;


        //动画
        _animator.SetBool("isRun", _controller.velocity.x != 0);
        _animator.SetBool("isGround", _controller.velocity.y == 0);
        _animator.SetBool("isJump", _controller.velocity.y > 0);

        float rotateY = _controller.velocity.x >= 0 ? 1 : 180;
        _body.transform.localRotation = Quaternion.Euler(0, rotateY, 0);
    }
}