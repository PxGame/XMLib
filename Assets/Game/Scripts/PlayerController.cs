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

    private void Awake()
    {
        Debug.Log("Player Awake");
        _input = App.Make<IInput>();
    }

    private bool _isPress = false;

    private void Update()
    {
        float x = _input.GetAxis("Horizontal") * Time.deltaTime * _speed;
        float y = _input.GetAxis("Vertical") * Time.deltaTime * _speed;

        if (_input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 Down:" + Time.frameCount);
        }
        if (_input.GetButtonUp("Fire1"))
        {
            Debug.Log("Fire1 Up:" + Time.frameCount);
        }
        if (!_isPress && _input.GetButton("Fire1"))
        {
            _isPress = true;
            Debug.Log("Fire1 Holding:" + Time.frameCount);
        }
        else if (_isPress && !_input.GetButton("Fire1"))
        {
            _isPress = false;
            Debug.Log("Fire1 Holded:" + Time.frameCount);
        }

        transform.Translate(x, y, 0);
    }
}