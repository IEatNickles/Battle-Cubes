using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timeToDestroy;

	public void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}
}
