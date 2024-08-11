using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public const string SCORE_EXTENTION = "bc_score";

	public UnityEvent<PlayerMovement> OnPlayerDeath;
	public static GameManager instance;
	public Bounds bounds;

	[Space]

	Camera cam;
	public ushort score;
	public TMP_Text scoreText;
	public GameObject enemySpawner;

	public GameMode GameMode;

	[Space]

	public int players;
	public GameObject playerPrefab;
	public List<PlayerMovement> activePlayers;

	[Space]

	public GameObject winUI;
	public GameObject loseUI;
	bool roundOver;

	[Space]

	List<Skin> _skins;

	public AudioSource music;

	public SkinsSO skinsSO;

	Volume volume;
	Bloom bloom;
	MotionBlur motionBlur;
	Vignette vignette;

	[Space]

	public MenuSettings settings;
	public MatchSettings matchSettings;
	public ushort _highScore;

	public bool usePP = true;

	void Awake()
	{
		settings = FileWriter.ReadFromJson<MenuSettings>
			($"{Application.persistentDataPath}/Settings/settings.{MainMenu.SETTINGS_EXTENTION}");
		matchSettings = FileWriter.ReadFromJson<MatchSettings>
			($"{Application.persistentDataPath}/Settings/matchSettings.{MainMenu.SETTINGS_EXTENTION}");

		instance = this;

		GameMode = matchSettings.GameMode;

		switch (GameMode)
		{
			case GameMode.Endless:
				Directory.CreateDirectory(Application.persistentDataPath + $"/Scores");

				if (!File.Exists(Application.persistentDataPath + 
					$"/Scores/{SceneManager.GetActiveScene().name}.{SCORE_EXTENTION}"))
				{
					_highScore = 0;
					string jsonExport = JsonUtility.ToJson(_highScore);
					jsonExport = MethodExtensions.EncryptDecrypt(jsonExport);
					File.WriteAllText(Application.persistentDataPath + 
						$"/Scores/{SceneManager.GetActiveScene().name}.{SCORE_EXTENTION}", jsonExport);
				}
				else
				{
					string _jsonImport = File.ReadAllText(Application.persistentDataPath + 
						$"/Scores/{SceneManager.GetActiveScene().name}.{SCORE_EXTENTION}");
					_jsonImport = MethodExtensions.EncryptDecrypt(_jsonImport);
					_highScore = JsonUtility.FromJson<ushort>(_jsonImport);
				}

				enemySpawner.SetActive(true);
				break;
		}

		OnPlayerDeath.AddListener(pm => RemovePlayer(pm));

		winUI.SetActive(false);
		loseUI.SetActive(false);

		skinsSO = SkinsSO.Instance();
		_skins = skinsSO.Skins;

		cam = Camera.main;

		if (cam.GetComponent<MilkShake.Shaker>() != null)
			cam.GetComponent<MilkShake.Shaker>().enabled = settings.CameraShake;

		if(usePP)
		{
			volume = cam.transform.GetComponentInChildren<Volume>();
			volume.profile.TryGet(out bloom);
			volume.profile.TryGet(out motionBlur);
			volume.profile.TryGet(out vignette);

			bloom.active = settings.Bloom;
			bloom.intensity.value = settings.BloomIntensity;

			motionBlur.active = settings.MotionBlur;

			vignette.active = settings.Vignette;
		}

		players = (int)matchSettings.PlayerAmount;

		for (int i = 0; i < players; i++)
		{
			GameObject playerGO = Instantiate(playerPrefab);
			playerGO.transform.position = new Vector3((i * (14f / players)) - 7f, 0f, 0f);
			playerGO.name = $"Player{i + 1}";

			PlayerMovement pm = playerGO.GetComponent<PlayerMovement>();
			activePlayers.Add(pm);
			pm.pInd = i;

			Health health = playerGO.GetComponent<Health>();

			health.OnDeath.AddListener(h => OnPlayerDeath?.Invoke(pm));

			Skin skin = _skins[PlayerPrefs.GetInt("Character" + i, i)];
			SkinObject skinGO = Instantiate(skin.Prefab, playerGO.transform);
			skinGO.transform.Find("Hurt").parent = playerGO.transform.Find("Effects");
			skinGO.transform.Find("Die").parent = playerGO.transform.Find("Effects");
			if (!settings.Particles)
			{
				playerGO.transform.Find("Effects/Hurt").gameObject.SetActive(false);
				playerGO.transform.Find("Effects/Die").gameObject.SetActive(false);
			}
		}

		if (music != null)
			music.volume = settings.MusicVolume;

		if (!settings.Particles)
		{
			foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
			{
				ps.Stop();
				ps.gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			SceneManager.LoadScene("MainMenu");
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (GameMode != GameMode.Endless)
		{
			if (activePlayers.Count == 1)
			{
				if (!roundOver)
				{
					Win();
				}
			}
		}

		if (activePlayers.Count <= 0)
		{
			if (!roundOver)
			{
				Lose();
			}
		}
	}

	public void SetTimer(float duration)
	{

	}

	public void RemovePlayer(PlayerMovement pm)
	{
		activePlayers.Remove(pm);
	}

	public void Win()
	{
		winUI.SetActive(true);
		if (GameMode != GameMode.Endless)
		{
			winUI.transform.GetComponentInChildren<TMP_Text>().SetText(
			$"{activePlayers[0].GetComponentInChildren<TMP_Text>().text} WON \n Press 'Backspace' to leave or \n 'Space' to play again");
		}
		else
		{
			if(score > _highScore)
			{
				_highScore = score;
			}

			string jsonExport = JsonUtility.ToJson(_highScore);
			jsonExport = MethodExtensions.EncryptDecrypt(jsonExport);
			File.WriteAllText(Application.persistentDataPath + 
				$"/Scores/{SceneManager.GetActiveScene().name}.{SCORE_EXTENTION}", jsonExport);
		}

		roundOver = true;
	}

	public void Lose()
	{
		loseUI.SetActive(true);

		if (GameMode == GameMode.Endless)
		{
			if (score > _highScore)
			{
				_highScore = score;
			}

			string jsonExport = JsonUtility.ToJson(_highScore);
			File.WriteAllText(Application.persistentDataPath + 
				$"/Scores/{SceneManager.GetActiveScene().name}. {SCORE_EXTENTION}", jsonExport);
		}

		roundOver = true;
	}

	public void AddScore()
	{
		score++;
		scoreText.SetText("Score: " + score);
		if (score == ushort.MaxValue)
		{
			if (!roundOver)
			{
				Win();
				winUI.transform.GetComponentInChildren<TMP_Text>().SetText($"YOU WIN! \n Score: {score}");
			}
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 0f, 1f, 0.1f);
		Gizmos.DrawCube(bounds.center, bounds.size);
	}
}