using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放Sprites
/// </summary>
public class PlaySprites : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private List<Sprite> _sprites;

    [SerializeField]
    private float _interval = 0.1f;
    private int _index = 0;

    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _interval)
        {
            _index += 1;
            _index %= _sprites.Count;
            _timer -= _interval;

            _spriteRenderer.sprite = _sprites[_index];
        }
    }

}
