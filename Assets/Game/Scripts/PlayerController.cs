using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.InputService;

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

    private XMLib.P2D.PlayerController _controller;

    private void Awake()
    {
        App.Instance<PlayerController>(this);

        //获取组件
        _controller = GetComponent<XMLib.P2D.PlayerController>();

        //获取输入服务
        _input = App.Make<IInputService>();
    }

    private void Start() { }

    private void OnDestroy()
    {
        App.Release<PlayerController>();
    }

    private bool _isRight = true;

    private void Update()
    {
        //更新输入
        if (_input.GetButtonDown("Jump"))
        {
            _controller.JumpBegin();
        }
        else if (_input.GetButtonUp("Jump"))
        {
            _controller.JumpEnd();
        }

        _controller.Move(_input.GetAxisRaw("Horizontal"));

        //动画
        _animator.SetBool("isRun", _controller.velocity.x != 0);
        _animator.SetBool("isGround", _controller.velocity.y == 0);
        _animator.SetBool("isJump", _controller.velocity.y > 0);

        float rotateY = 0;
        if ((!_isRight && _controller.velocity.x == 0) ||
            _controller.velocity.x < 0)
        {
            rotateY = 180;
            _isRight = false;
        }
        else
        {
            _isRight = true;
        }

        _body.transform.localRotation = Quaternion.Euler(0, rotateY, 0);
    }
}