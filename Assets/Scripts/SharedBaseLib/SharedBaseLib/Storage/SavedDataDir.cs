using System;
using UnityEngine;

namespace SharedBaseLib.Storage
{
	public class SavedDataDir
	{
		private static string ms_secondaryStorageDir = string.Empty;

		public static string GetPath()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				if (SharedMain.Instance == null)
				{
					throw new Exception("Using SavedDataDir before Init of SharedMain");
				}
				return GetExternalStorage();
			}
			return Application.persistentDataPath;
		}

		private static string GetExternalStorage()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				string text = PlayerPrefs.GetString("SAVED_PERSISTENT_DATA_PATH", "");
				if (string.IsNullOrEmpty(ms_secondaryStorageDir))
				{
					ms_secondaryStorageDir = SharedMain.Instance.AndroidExternalStorage.SecondaryExternalStorage;
				}
				if (SharedMain.Instance.AndroidExternalStorage.IsDirectoryWritable(ms_secondaryStorageDir))
				{
					text = ms_secondaryStorageDir;
				}
				else if (!SharedMain.Instance.AndroidExternalStorage.IsDirectoryWritable(text))
				{
					text = Application.persistentDataPath;
				}
				if (!text.Equals(PlayerPrefs.GetString("SAVED_PERSISTENT_DATA_PATH")))
				{
					PlayerPrefs.SetString("SAVED_PERSISTENT_DATA_PATH", text);
				}
				return text;
			}
			return Application.persistentDataPath;
		}
	}
}
