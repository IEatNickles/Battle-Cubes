using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using MilkShake;
using System;

public class MainMenu : MonoBehaviour
{
	public const string SETTINGS_EXTENTION = "bc_settings";
	public static MainMenu Instance;

	public event Action OnSettingsChanged;

	public ShakePreset CamShakePreset;

	[Space]

	public MenuSettings MenuSettings;
	public static readonly MenuSettings DefaultMenuSettings = new MenuSettings
	{
		MotionBlur = true,
		Vignette = true,

		Bloom = true,
		BloomIntensity = 1,

		CameraShake = true,
		CameraShakeIntensity = 0.1f,

		Particles = true,
		HurtParticles = true,
		SpawnParticles = true,
		DeathParticles = true,

		Fullscreen = true,
		ScreenHeight = 1080,
		ScreenWidth = 1920,
		ResolutionIndex = 1,
		
		FPSLimit = 60,
		
		Brightness = 1,
		
		MasterVolume = 100,
		MusicVolume = 100,
		SoundVolume = 100,
		OldSounds = false,
		
		UIAnimations = true,
	};

	[Space]

	public MatchSettings MatchSettings;
	public static readonly MatchSettings SefaultMatchSettings = new MatchSettings
	{
		PlayerAmount = 2,
		Lives = 1,
		GameMode = GameMode.PVP
	};

	private void Awake()
	{
		Instance = this;
		SkinsSO.Instance().Initialize();

		string path = $"{Application.persistentDataPath}/Settings";

		Directory.CreateDirectory(path);
		if (!File.Exists($"{path}/settings.{SETTINGS_EXTENTION}"))
		{
			FileWriter.WriteToJson(DefaultMenuSettings, path + $"/settings.{SETTINGS_EXTENTION}");
			MenuSettings = DefaultMenuSettings;
		}
		else
		{
			MenuSettings = FileWriter.ReadFromJson<MenuSettings>(path + $"/settings.{SETTINGS_EXTENTION}");
		}
		
		if (!File.Exists($"{path}/matchSettings.bc_settings"))
		{
			FileWriter.WriteToJson(SefaultMatchSettings, path + $"/matchSettings.{SETTINGS_EXTENTION}");
			MatchSettings = SefaultMatchSettings;
		}
		else
		{
			MatchSettings = FileWriter.ReadFromJson<MatchSettings>(path + $"/matchSettings.{SETTINGS_EXTENTION}");
		}
	}

	public void LoadLevel(int level)
	{
		FileWriter.WriteToJson(MatchSettings, $"{Application.persistentDataPath}/Settings/matchSettings.{SETTINGS_EXTENTION}");
		StartCoroutine(LoadLevelAsync(level));
	}

	IEnumerator LoadLevelAsync(int level)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(level);

		while (!operation.isDone)
		{
			yield return null;
		}
	}

	public void SetSetting(SettingsType type, string settingName, object value)
	{
		switch (type)
		{
			case SettingsType.Menu:
				MenuSettings.SetSetting(settingName, value);
				break;
			case SettingsType.Match:
				MatchSettings.SetSetting(settingName, value);
				break;
		}
	}

	public float GetSetting(SettingsType type, string settingName)
	{
		switch (type)
		{
			case SettingsType.Menu:
				return MenuSettings.GetSetting(settingName);
			case SettingsType.Match:
				return MatchSettings.GetSetting(settingName);
		}

		return 0;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void DefaultSettings()
	{
		MenuSettings = DefaultMenuSettings;
		OnSettingsChanged?.Invoke();
	}

	public void ResetSettings()
	{
		string path = $"{Application.persistentDataPath}/Settings/settings.{SETTINGS_EXTENTION}";
		if(File.Exists(path))
			MenuSettings = FileWriter.ReadFromJson<MenuSettings>(path);

		OnSettingsChanged?.Invoke();
	}

	public void SaveSettings()
	{
		FileWriter.WriteToJson(MenuSettings, $"{Application.persistentDataPath}/Settings/settings.{SETTINGS_EXTENTION}");
		OnSettingsChanged?.Invoke();
	}
}
public enum GameMode
{
	PVP,
	Endless
}
public enum SettingsType
{
	Menu,
	Match
}