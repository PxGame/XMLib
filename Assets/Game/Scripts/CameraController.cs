using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    protected Vector2 _offset;

    [SerializeField]
    protected float _maxDistance = 4;

    [SerializeField]
    protected float _moveSpeed = 13;

    [SerializeField]
    protected Transform _target;

    [SerializeField]
    protected AnimationCurve _moveSpeedCurve;

    private void LateUpdate()
    {
        if (null == _target)
        {
            return;
        }

        Vector2 targetPos = (Vector2)_target.position - _offset;
        Vector2 selfPos = transform.position;
        Vector3 pos;
        float distance = Vector2.Distance(targetPos, selfPos);
        if (distance > _maxDistance)
        {
            pos = (selfPos - targetPos).normalized * _maxDistance + targetPos;
        }
        else if (distance > float.Epsilon)
        {
            float ratio = 1;
            if (_maxDistance > 0)
            {
                ratio = Mathf.Clamp01(distance / _maxDistance);
            }
            float speed = _moveSpeedCurve.Evaluate(ratio) * _moveSpeed;
            selfPos = Vector2.MoveTowards(
                selfPos,
                targetPos,
                speed * Time.deltaTime);
            pos = selfPos;
        }
        else
        {
            pos = selfPos;
        }

        pos.z = transform.position.z;

        transform.position = pos;
    }
}