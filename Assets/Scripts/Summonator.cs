using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summonator : MonoBehaviour
{
    public GameObject truck;

    public float timer;

    public float minTime;
    public float maxTime;

    public bool doDestroy;
    public float destroyTime;

    public bool affectScore = true;

    GameManager gameManager;

	private void Start()
	{
        timer = Random.Range(minTime, maxTime);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
		{
            Summinate();
		}

        if(affectScore)
            if (gameManager.score >= short.MaxValue)
                Destroy(this);
    }

    public void Summinate()
	{
        GameObject truckGO = Instantiate(truck, transform.position, transform.rotation);
		if (doDestroy)
		{
            Destroy(truckGO, destroyTime);
		}

        timer = Random.Range(minTime, maxTime);
	}
}
