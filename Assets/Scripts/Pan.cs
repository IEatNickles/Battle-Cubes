using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public Vector3 direction = Vector3.left;
	public float maxDist = 5f;

	Vector3 startingPoint;

	public void Start()
	{
		startingPoint = transform.position;
	}

	void Update()
    {
		if (Vector3.Distance(transform.position, startingPoint) <= maxDist)
		{
			transform.position += direction * Time.deltaTime;
		}
		else
		{
			transform.position = startingPoint;
		}
    }
}
