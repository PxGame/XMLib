using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> _backgrounds;

    [SerializeField]
    private float _scale = 0.1f;

    [SerializeField]
    private float[] _offsets;

    PlayerController _player;
    private void Awake()
    {
        _offsets = new float[_backgrounds.Count];
    }

    private void Start()
    {
        _player = App.Make<PlayerController>();
    }

    private void Update()
    {
        float currentSpeed = _player.transform.position.x - transform.position.x;

        Vector3 position = transform.position;
        position.x = _player.transform.position.x;
        transform.position = position;

        for (int i = 0; i < _backgrounds.Count; i++)
        {
            _offsets[i] += currentSpeed * _scale * (i + 1);
            _backgrounds[i].material.SetFloat("_xOffset", _offsets[i]);
        }
    }

}