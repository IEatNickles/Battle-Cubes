using System;
using UnityEngine;

public abstract class Settings
{
	public void SetSetting(string name, object value)
	{
		GetType().GetField(name).SetValue(this, value);
	}

	public float GetSetting(string name)
	{
		return (float)Convert.ToDouble(GetType().GetField(name).GetValue(this));
	}
}
[Serializable]
public class MenuSettings : Settings
{
	public bool MotionBlur;
	public bool Vignette;

	public bool Bloom;
	[Range(0f, 15f)]
	public float BloomIntensity;

	public bool CameraShake;
	[Range(0.01f, 1f)]
	public float CameraShakeIntensity;

	[Range(-1f, 1f)]
	public float Brightness;

	public bool Fullscreen;

	public int ScreenWidth;
	public int ScreenHeight;

	public int ResolutionIndex;

	public bool Particles;
	public bool DeathParticles;
	public bool HurtParticles;
	public bool SpawnParticles;

	public bool UIAnimations;

	[Range(0, 100)]
	public float MasterVolume;
	[Range(0, 100)]
	public float MusicVolume;
	[Range(0, 100)]
	public float SoundVolume;
	public bool OldSounds;

	[Range(10, 320)]
	public float FPSLimit;
}
[Serializable]
public class MatchSettings : Settings
{
	[Min(1)] public float PlayerAmount;
	[Min(1)] public float Lives;
	public GameMode GameMode;
}