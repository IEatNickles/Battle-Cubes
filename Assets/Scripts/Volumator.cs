using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volumator : MonoBehaviour
{
    public AudioSource autioScource;
    public bool isMusic = true;

    MenuSettings settings;

    void Start()
    {
        string jsonImport = File.ReadAllText($"{Application.persistentDataPath}/Settings/settings.json");
        settings = JsonUtility.FromJson<MenuSettings>(jsonImport);

        if (isMusic)
        {
            autioScource.volume = settings.MusicVolume;
        }
        else
        {
            autioScource.volume = settings.SoundVolume;
        }
    }

	private void OnValidate()
    {
        if (GetComponent<AudioSource>() != null)
            autioScource = GetComponent<AudioSource>();
    }
}
