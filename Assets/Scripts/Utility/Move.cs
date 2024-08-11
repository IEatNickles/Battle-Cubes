using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public Space space;
	public Vector3 axis;

	public float speed;

	void Update()
    {
        transform.Translate(speed * Time.deltaTime * axis, space);
    }
}
