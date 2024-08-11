using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MethodExtensions
{
	const string dkfkk = "389429869";

    public static string EncryptDecrypt(string data)
	{
		string result = "";

		for (int i = 0; i < data.Length; i++)
		{
			result += (char)(data[i] ^ dkfkk[i % dkfkk.Length]);
		}

		return result;
	}

	public static bool PointInBounds(this Bounds bounds, Vector3 point)
	{
		float x = (bounds.size.x * 0.5f) + bounds.center.x;
		float y = (bounds.size.y * 0.5f) + bounds.center.y;
		float z = (bounds.size.z * 0.5f) + bounds.center.z;

		return
			point.x > x || point.x < -x || 
			point.y > y || point.y < -y || 
			point.z > z || point.z < -z;
	}

	public static Transform[] GetAllParents(this Transform transform)
	{
		List<Transform> parents = new List<Transform>();

		Transform[] children = transform.root.GetAllChildren();
		foreach (Transform child in children) 
		{
			if (transform.IsChildOf(child))
			{
				parents.Add(child);
			}
		}

		return parents.ToArray();
	}

	public static Transform[] GetAllChildren(this Transform transform)
	{
		Transform[] children = new Transform[0];
		foreach (Transform child in transform)
		{
			children = child.GetAllChildren();
		}

		return children;
	}
}
