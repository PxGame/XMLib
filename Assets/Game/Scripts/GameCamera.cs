using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

public class GameCamera : MonoBehaviour
{
    PlayerController _target;
    private void Awake()
    {
        App.Instance<GameCamera>(this);
    }

    private void Start()
    {
        _target = App.Make<PlayerController>();
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = _target.transform.position.x;
        transform.position = position;
    }

    private void OnDestroy()
    {
        App.Release<GameCamera>();
    }
}