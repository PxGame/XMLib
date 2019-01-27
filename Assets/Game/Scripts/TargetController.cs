using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

public class TargetController : MonoBehaviour
{
	[SerializeField]
	private float _maxTime = 10f;

	[SerializeField]
	private float _speed = 1;
	[SerializeField]
	private float _maxSpeed = 10;

	[SerializeField]
	private AnimationCurve _speedCurve;

	private float _currentTimer = 0f;

	private float _currentSpeed = 0;

	public float currentSpeed { get { return _currentSpeed; } }

	private void Awake()
	{
		App.Instance<TargetController>(this);
	}

	private void OnDestroy()
	{
		App.Release<TargetController>();
	}

	private void Update()
	{
		_currentTimer += Time.deltaTime;
		_currentSpeed = _speed + (_maxSpeed - _speed) * _speedCurve.Evaluate(Mathf.Clamp(_currentTimer, 0, _maxTime) / _maxTime);

		float xMove = _currentSpeed * Time.deltaTime;

		transform.Translate(xMove, 0, 0);
	}

}