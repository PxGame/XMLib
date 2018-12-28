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

    private void Update()
    {
        float x = _input.GetAxis("Horizontal") * Time.deltaTime * _speed;
        float y = _input.GetAxis("Vertical") * Time.deltaTime * _speed;

        transform.Translate(x, y, 0);
    }
}