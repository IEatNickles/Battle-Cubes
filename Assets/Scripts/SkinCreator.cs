using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkinCreator : MonoBehaviour
{
	public const string EXTENTION = "bc_skin";

	[SerializeField] ColorPicker _colorPicker; 

	[Space]

	[SerializeField] TMP_Text _nameDisplay;
	[SerializeField] Transform _skinDisplay;
	[SerializeField] MeshRenderer _skinRenderer;
	[SerializeField] Material _skinMaterial;

	[Space]

	[SerializeField] GameObject _saveMenu;
	[SerializeField] TMP_InputField _skinNameValue;

	[Space]

	[SerializeField] GameObject _skinList; 
	[SerializeField] Transform _skinContent;
	[SerializeField] Button _skinButtonPrefab;

	[Space]

	[SerializeField] List<PartList> _partLists;

	string _skinsPath;

	CustomSkin _skin;
	List<Skin> _skins = new List<Skin>();
	SkinsSO _skinsSO;

	void Start()
	{
		_skinsSO = SkinsSO.Instance();
		_skinMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
		_skinMaterial.color = _colorPicker.RGBColor;
		_skinRenderer.material = _skinMaterial;
		_skin = new CustomSkin("");
		_skinsPath = Application.persistentDataPath + "/Skins";
		if (!Directory.Exists(_skinsPath)) Directory.CreateDirectory(_skinsPath);
		LoadSkins();
	}

	public void Save()
	{
		Save(_skinNameValue.text);
		ToggleSaveMenu(false);
	}

	public void Save(string name)
	{
		if (string.IsNullOrEmpty(name)) return;

		string path = $"{_skinsPath}/{name}.{EXTENTION}";
		_skin.Name = name;

		FileWriter.WriteToJson(_skin, path);
		Load(name);
		LoadSkins();
	}

	public void ToggleSaveMenu()
	{
		_saveMenu.SetActive(!_saveMenu.activeSelf);
		ToggleLoadMenu(false);
	}

	public void ToggleSaveMenu(bool enabled)
	{
		_saveMenu.SetActive(enabled);
		if (enabled) ToggleLoadMenu(false);
	}

	public void Delete()
	{
		Delete(_skin.Name);
	}

	public void Delete(string name)
	{
		if (string.IsNullOrEmpty(name)) return;
		if (!File.Exists($"{_skinsPath}/{name}.{EXTENTION}")) throw new Exception($"The skin '{name}' does not exist.");

		File.Delete($"{_skinsPath}/{name}.{EXTENTION}");
		_skinNameValue.SetTextWithoutNotify("");
		LoadSkins();
		ResetSkin();
	}

	public void Load(string name)
	{
		_skin = FileWriter.ReadFromJson<CustomSkin>($"{_skinsPath}/{name}.{EXTENTION}");
		_skinMaterial.color = _skin.Color;
		_colorPicker.RGBColor = _skin.Color;
		_nameDisplay.SetText(_skin.Name);

		foreach (PartList list in _partLists)
		{
			SkinPart part = _skin.GetSkinPart(list.Category);
			list.SetPart(part.Index);
			list.SetColor(part.Color);
			list.ColorPicker.RGBColor = part.Color;
		}

		_skinNameValue.SetTextWithoutNotify(name);
		ToggleLoadMenu(false);
	}

	public void ToggleLoadMenu()
	{
		_skinList.SetActive(!_skinList.activeSelf);
		if (_skinList.activeSelf) _skinDisplay.DOLocalMoveY(175, 0.25f);
		else _skinDisplay.DOLocalMoveY(0, 0.25f);

		ToggleSaveMenu(false);
	}

	public void ToggleLoadMenu(bool enabled)
	{
		_skinList.SetActive(enabled);
		if (enabled) _skinDisplay.DOLocalMoveY(175, 0.25f);
		else _skinDisplay.DOLocalMoveY(0, 0.25f);

		if (enabled) ToggleSaveMenu(false);
	}

	public void LoadSkins()
	{
		_skins.Clear();
		if (_skinContent.childCount > 0)
		{
			foreach (Transform t in _skinContent)
			{
				Destroy(t.gameObject);
			}
		}

		string[] files = Directory.GetFiles(_skinsPath);
		if (files.Length > 0)
		{
			foreach (string file in files)
			{
				CustomSkin newSkin = FileWriter.ReadFromJson<CustomSkin>(file);
				if (!_skinsSO.ContainsName(newSkin.Name)) 
					_skins.Add(new Skin(newSkin.Name, Resources.Load<SkinObject>("CustomSkin")));

				Button skinButton = Instantiate(_skinButtonPrefab, _skinContent);
				skinButton.onClick.AddListener(() =>
				{
					Load(newSkin.Name);
				});
				skinButton.GetComponentInChildren<TMP_Text>().SetText(newSkin.Name);
			}
		}
		_skinsSO.Skins.AddRange(_skins);
	}

	public void ResetSkin()
	{
		_nameDisplay.SetText("");
		foreach (PartList list in _partLists)
		{
			list.SetPart(-1);
		}
	}

	public void SetPart(PartCategory category, SkinPart part)
	{
		_skin.SetSkinPart(category, part);
	}

	public void SetColor(Color color)
	{
		if (_skinMaterial == null) return;

		_skin.Color = color;
		_skinMaterial.color = color;
	}

	public void SetMetallicAndRoughness(float metallic, float roughness)
	{
		if (_skinMaterial == null) return;
		
		_skin.Metallic = metallic;
		_skin.Roughness = roughness;

		_skinMaterial.SetFloat("_Metallic", metallic);
		_skinMaterial.SetFloat("_Smoothness", roughness);
	}

	public static string GetSkinPath(string name)
	{
		return $"{Application.persistentDataPath}/Skins/{name}.{EXTENTION}";
	}
}
[Serializable]
public struct CustomSkin
{
	public string Name;

	public Color Color;
	public float Metallic;
	public float Roughness;

	public SkinPart BackItem;
	public SkinPart EyeItem;
	public SkinPart Eyes;
	public SkinPart FaceItem;
	public SkinPart Hat;
	public SkinPart Mouth;
	public SkinPart NeckItem;
	public SkinPart Nose;

	public CustomSkin(string name)
	{
		Name = name;
		Color = Color.white;
		Metallic = 0;
		Roughness = 0;

		BackItem = new SkinPart(PartCategory.BackItems);
		EyeItem = new SkinPart(PartCategory.EyeItems);
		Eyes = new SkinPart(PartCategory.Eyes);
		FaceItem = new SkinPart(PartCategory.FaceItems);
		Hat = new SkinPart(PartCategory.Hats);
		Mouth = new SkinPart(PartCategory.Mouths);
		NeckItem = new SkinPart(PartCategory.NeckItems);
		Nose = new SkinPart(PartCategory.Noses);
	}

	public SkinPart GetSkinPart(PartCategory type)
	{
		switch (type)
		{
			case PartCategory.BackItems: return BackItem;
			case PartCategory.EyeItems: return EyeItem;
			case PartCategory.Eyes: return Eyes;
			case PartCategory.FaceItems: return FaceItem;
			case PartCategory.Hats: return Hat;
			case PartCategory.Mouths: return Mouth;
			case PartCategory.NeckItems: return NeckItem;
			case PartCategory.Noses: return Nose;
		}

		return BackItem;
	}

	public void SetSkinPart(PartCategory type, SkinPart part)
	{
		switch (type)
		{
			case PartCategory.BackItems:
				BackItem = part;
				break;
			case PartCategory.EyeItems:
				EyeItem = part;
				break;
			case PartCategory.Eyes:
				Eyes = part;
				break;
			case PartCategory.FaceItems:
				FaceItem = part;
				break;
			case PartCategory.Hats:
				Hat = part;
				break;
			case PartCategory.Mouths:
				Mouth = part;
				break;
			case PartCategory.NeckItems:
				NeckItem = part;
				break;
			case PartCategory.Noses:
				Nose = part;
				break;
		}
	}
}
[Serializable]
public struct SkinPart
{
	public PartCategory Category;
	public string Name;
	public int Index;

	public Color Color;
	public float Metallic;
	public float Roughness;

	public SkinPart(PartCategory category)
	{
		Category = category;
		Name = "";
		Index = -1;
		Color = Color.white;
		Metallic = 0;
		Roughness = 0;
	}
}
