using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkinSelecter : MonoBehaviour
{
	[SerializeField] Transform _skinDisplay;
	[SerializeField] TMP_Text _displayName;

	[Space]

    SkinsSO _skinsSO;
	List<Skin> _skins = new List<Skin>();
	[SerializeField] int _index;

	[Space]

	[SerializeField] TMP_InputField _gamertag;
	[SerializeField] ProgressBar _characterProgress;

	public void Start()
	{
		_skinsSO = SkinsSO.Instance();
		Refresh();
	}

	public void SelectCharacter(int ind)
	{
		ind = Mathf.Clamp(ind, 0, _skins.Count - 1);
		ResetActiveCharacters();

		_skinDisplay.GetChild(ind).gameObject.SetActive(true);
		
		_index = ind;
		_displayName.SetText(_skins[ind].Name);

		_characterProgress.SetValue(ind);

		PlayerPrefs.SetInt("Character" + transform.GetSiblingIndex(), ind);
	}

	public void Next()
	{
		SelectCharacter(_index + 1);
	}

	public void Previous()
	{
		SelectCharacter(_index - 1);
	}

	public void SetGamerTag(string gamertag)
	{
		PlayerPrefs.SetString("Gamertag" + transform.GetSiblingIndex(), gamertag);
	}

	public void Refresh()
	{
		StartCoroutine(Refresh_C());
	}

	IEnumerator Refresh_C()
	{
		foreach (Transform skin in _skinDisplay)
		{
			Destroy(skin.gameObject);
		}
		_skins.Clear();

		yield return new WaitForEndOfFrame();

		foreach (Skin skin in _skinsSO.Skins)
		{
			SkinObject skinObject = Instantiate(skin.Prefab, _skinDisplay);
			skinObject.Name = skin.Name;
			skinObject.Initialize();
			skinObject.transform.localPosition = Vector3.zero;
			skinObject.name = skin.Name;
			_skins.Add(skin);
			skinObject.gameObject.SetActive(false);
		}

		_characterProgress.SetMax(_skins.Count - 1);

		SelectCharacter(PlayerPrefs.GetInt("Character" + transform.GetSiblingIndex(), _index));

		string gamertag = PlayerPrefs.GetString("Gamertag" + transform.GetSiblingIndex(), _displayName.text);
		_gamertag.SetTextWithoutNotify(gamertag);
	}

	void ResetActiveCharacters()
	{
		foreach (Transform skin in _skinDisplay)
		{
			skin.gameObject.SetActive(false);
		}
	}
}
