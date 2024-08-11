using System.IO;
using UnityEngine;

public static class FileWriter
{
	public static void WriteToJson<T>(T type, string path)
	{
		string jsonExport = JsonUtility.ToJson(type);
		File.WriteAllText(path, jsonExport);
	}

	public static T ReadFromJson<T>(string path)
	{
		string jsonImport = File.ReadAllText(path);
		return JsonUtility.FromJson<T>(jsonImport);
	}
}