using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins", menuName = "SkinSO/Skins")]
public class SkinsSO : ScriptableObject
{
	public List<Skin> Skins;

	public void Initialize()
	{
		if (Application.isPlaying)
		{
			Skins = new List<Skin>();

			SkinObject[] skins = Resources.LoadAll<SkinObject>("Skins");
			foreach (SkinObject skin in skins)
			{
				Skins.Add(new Skin(skin.Name, skin));
			}
		}
	}

	public bool ContainsName(string name)
	{
		foreach (Skin skin in Skins)
		{
			if (skin.Name == name)
			{
				return true;
			}
		}

		return false;
	}

	public int FindWithName(string name)
	{
		int index = -1;
		foreach (Skin skin in Skins)
		{
			if (skin.Name == name)
			{
				index = Skins.IndexOf(skin);
			}
		}
		return index;
	}

	public static SkinsSO Instance()
	{
		return Resources.Load<SkinsSO>("Skins");
	}
}
[Serializable]
public struct Skin
{
	public string Name;
	public SkinObject Prefab;

	public Skin(string name, SkinObject prefab)
	{
		Name = name;
		Prefab = prefab;
	}
}
