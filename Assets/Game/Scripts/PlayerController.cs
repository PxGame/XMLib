using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;
using XMLib.InputDriver;

public class PlayerController : MonoBehaviour
{
    private IInput _input;

    [SerializeField]
    private float _speed = 3f;

    private Physics2DController _controller;

    private void Awake()
    {
        Debug.Log("Player Awake");

        _controller = GetComponent<Physics2DController>();
        _input = App.Make<IInput>();
    }

    private bool _isPress = false;

    private void Update()
    {
        float x = _input.GetAxis("Horizontal") * Time.deltaTime * _speed;
        float y = _input.GetAxis("Vertical") * Time.deltaTime * _speed;

        if (_input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump Down:" + Time.frameCount);
        }
        if (_input.GetButtonUp("Jump"))
        {
            Debug.Log("Jump Up:" + Time.frameCount);
        }
        if (!_isPress && _input.GetButton("Jump"))
        {
            _isPress = true;
            Debug.Log("Jump Holding:" + Time.frameCount);
        }
        else if (_isPress && !_input.GetButton("Jump"))
        {
            _isPress = false;
            Debug.Log("Jump Holded:" + Time.frameCount);
        }

        _controller.Move(new Vector2(x, y));
    }
}