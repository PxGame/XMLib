using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

/// <summary>
/// 移动平台
/// </summary>
public class MovePlatform : MonoBehaviour, IMovePlatform
{
	[SerializeField]
	protected Vector2 _velocity;

	/// <summary>
	/// 移动速度
	/// </summary>
	/// <value></value>
	public Vector2 velocity { get { return _velocity; } }

	void Update()
	{
		transform.Translate(_velocity * Time.deltaTime);
	}
}