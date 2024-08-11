using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class LevelButton : MonoBehaviour
{
    MainMenu menu;
	[SerializeField] GameMode _gameMode;
    public int level;
	public TMP_Text levelNameVal;
	public TMP_Text scoreVal;

	public void Start()
	{
		menu = MainMenu.Instance;
		GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => LoadLevel());
	}

	void LoadLevel()
	{
		menu.MatchSettings.GameMode = _gameMode;
		menu.LoadLevel(level);
	}

	public void OnValidate()
	{
		if (levelNameVal)
			levelNameVal.SetText(name);
		if (scoreVal)
			scoreVal.gameObject.SetActive(_gameMode == GameMode.Endless);
	}
}
