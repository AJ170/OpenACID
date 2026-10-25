using System;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public static class UIAtlasHelper
	{
		public static string GetDisplayName(this UIAtlas _atlas)
		{
			if (_atlas.replacement == null || _atlas.replacement == _atlas)
			{
				return _atlas.name;
			}
			return string.Format("{0} -> {1}", _atlas.name, _atlas.replacement.GetDisplayName());
		}

		public static bool IsLoaded(string _atlas)
		{
			UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
			if (array == null)
			{
				return false;
			}
			UIAtlas uIAtlas = Array.Find(array, (UIAtlas atlas) => atlas.name.ToLowerInvariant().Equals(_atlas.ToLowerInvariant()));
			return uIAtlas != null;
		}

		public static void ForceUnloadAnimusAtlasIfLoaded()
		{
			UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
			if (array == null)
			{
				return;
			}
			string text = "animus";
			foreach (UIAtlas uIAtlas in array)
			{
				if (uIAtlas.name.ToLowerInvariant().Contains(text.ToLowerInvariant()))
				{
					if (uIAtlas.texture != null)
					{
						Resources.UnloadAsset(uIAtlas.texture);
					}
					if (uIAtlas.spriteMaterial != null)
					{
						Resources.UnloadAsset(uIAtlas.spriteMaterial);
					}
				}
			}
		}

		public static UIAtlas[] GetLoadedAtlases()
		{
			return Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
		}

		public static string[] GetLoadedAtlasNames()
		{
			UIAtlas[] loadedAtlases = GetLoadedAtlases();
			string[] array = new string[loadedAtlases.Length];
			for (int i = 0; i < loadedAtlases.Length; i++)
			{
				array[i] = loadedAtlases[i].name;
			}
			return array;
		}
	}
}
