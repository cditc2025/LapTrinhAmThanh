using UnityEngine;
using System.IO;

namespace KienChi
{
	public static class JSONReader
	{
		public static T ReadFromJson<T>(string levelData)
		{
			try
			{
				// string jsonData = File.ReadAllText(levelData);

				T data = JsonUtility.FromJson<T>(levelData);

				return data;
			}
			catch (System.Exception ex)
			{
				Debug.LogError("JSONReader Err: " + ex.Message);
				return default(T);
			}
		}
	}
}
