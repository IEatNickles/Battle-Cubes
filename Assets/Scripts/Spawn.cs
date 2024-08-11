using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject biohazard;

    public float maxTime;

    [Space]

    float timer;

    [Space]

    public float minX = -5f;
    public float maxX = 5f;
    
    [Space]

    public float minZ = 0f;
    public float maxZ = 0f;

	private void Awake()
	{
        timer = maxTime;
	}

	void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
		{
            Instantiate(biohazard, new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ)), Quaternion.identity);
            timer = maxTime;
        }
    }
}
