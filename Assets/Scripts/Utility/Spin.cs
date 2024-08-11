using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
	public float speed;

	[Space]

	public Vector3 axis;

	[Space]

	public Space space;

	private void FixedUpdate()
	{
		transform.Rotate(axis, Time.deltaTime * speed, space);
	}
}
