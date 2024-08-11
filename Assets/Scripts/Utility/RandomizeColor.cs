using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColor : MonoBehaviour
{
    public Renderer[] renderers;

    void Start()
    {
		foreach (Renderer rend in renderers)
		{
            rend.material = new Material(rend.material);
            rend.material.color = Random.ColorHSV();
		}
    }
}
