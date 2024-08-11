using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMultiple : MonoBehaviour
{
    public Obj[] objs;
	List<Obj> spawnables = new List<Obj>();

    [Space]

    public bool randomTime;

    [Space]

    public float minTime;
    public float maxTime;

    [Space]

    public float time;
    float currentTime;

    [Space]

    public float minX;
    public float maxX;

    [Space]

    public float minZ;
    public float maxZ;

	public Bounds bounds;

    void Awake()
	{
        currentTime = randomTime ? Random.Range(minTime, maxTime) : time;
		for (int i = 0; i < objs.Length; i++)
		{
            //objs[i].canSpawn = PlayerPrefsUtility.GetBool("Spawnable" + i, true);
			if (objs[i].canSpawn)
			{
                spawnables.Add(objs[i]);
			}
		}
	}

    void Update()
	{
		if (spawnables.Count > 0)
		{
			currentTime -= Time.deltaTime;
			if (currentTime <= 0f)
			{
				float rTime = Random.Range(minTime, maxTime);
				int f = Random.Range(0, spawnables.Count);
				for (int i = 0; i < objs.Length; i++)
				{
					if (spawnables[f] == objs[i])
					{
						float x = transform.position.x + (Random.Range(-bounds.size.x, bounds.size.x) * 0.5f);
						float y = transform.position.y + (Random.Range(-bounds.size.y, bounds.size.y) * 0.5f);
						float z = transform.position.z + (Random.Range(-bounds.size.z, bounds.size.z) * 0.5f);

						GameObject objGO = Instantiate(objs[i].obj, new Vector3(x, y, z), objs[f].obj.transform.rotation);
						objGO.transform.SetParent(transform);
					}
				}
				currentTime = randomTime ? rTime : time;
			}
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
	}
}
[System.Serializable]
public class Obj
{
    public GameObject obj;
    public bool canSpawn;
}
