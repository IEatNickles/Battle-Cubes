using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PartList : MonoBehaviour
{
	[SerializeField] SkinCreator _skinCreator;
	[FormerlySerializedAs("_colorPicker")]
	public ColorPicker ColorPicker;
	Material _partMaterial;

	public PartCategory Category;
	SkinPart _part;
    
	[SerializeField] Transform _buttonContent;
	[SerializeField] Button _partButton;

	[SerializeField] Search _search;
	List<GameObject> _parts = new List<GameObject>();

	[SerializeField] Transform _display;

	public void Awake()
	{
		_part = new SkinPart(Category);
		_partMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
		GameObject[] resourceParts = Resources.LoadAll<GameObject>(GetPartPath(Category));

		if (resourceParts.Length > 0)
		{
			List<string> results = new List<string>();
			List<UnityAction> actions = new List<UnityAction>();

			int partIndex = -1;
			foreach (GameObject part in resourceParts)
			{
				GameObject newPart = Instantiate(part, _display);
				newPart.GetComponent<MeshRenderer>().material = _partMaterial;
				newPart.name = part.name;
				actions.Add(() => SetPart(newPart.transform.GetSiblingIndex()));
				results.Add(newPart.name);
				newPart.SetActive(false);
				_parts.Add(newPart);

				partIndex++;
				int t_index = partIndex;

				Button partButton = Instantiate(_partButton, _buttonContent);
				partButton.onClick.AddListener(() => SetPart(t_index));
				partButton.name = part.name;
				partButton.transform.GetChild(0).GetComponent<Image>().sprite = 
					Resources.Load<Sprite>(GetIconPath(Category) + "/" + newPart.name);
			}
			_search.Populate(results.ToArray(), actions.ToArray());
		}
	}

	public void SetPart(int index)
	{
		foreach (GameObject part in _parts)
		{
			part.SetActive(false);
		}

		if (index > -1)
		{
			_parts[index].SetActive(true);
			_part.Name = _parts[index].name;
			_part.Index = index;
			_skinCreator.SetPart(Category, _part);
		}
		else
		{
			_part.Name = "";
			_part.Index = -1;
		}
	}

	public void SetColor(Color color)
	{
		if (_partMaterial == null) return;

		_part.Color = color;
		_partMaterial.color = color;
		_skinCreator.SetPart(Category, _part);
	}

	public void SetMetallicAndRoughness(float metallic, float roughness)
	{
		if (_partMaterial == null) return;

		_part.Metallic = metallic;
		_part.Roughness = roughness;

		_partMaterial.SetFloat("_Metalic", metallic);
		_partMaterial.SetFloat("_Roughness", roughness);

		_skinCreator.SetPart(Category, _part);
	}

	public string GetPartPath(PartCategory partType)
	{
		return $"CustomParts/Parts/{partType}";
	}

	public string GetIconPath(PartCategory partType)
	{
		return $"CustomParts/Icons/{partType}";
	}
}
public enum PartCategory
{
	BackItems,
	EyeItems,
	Eyes,
	FaceItems,
	Hats,
	Mouths,
	NeckItems,
	Noses
}
