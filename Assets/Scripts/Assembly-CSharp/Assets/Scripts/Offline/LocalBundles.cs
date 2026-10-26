using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AcierNetwork.AssetBundles;
using UnityEngine;

namespace Assets.Scripts.Offline
{
	// The game's bundles served from the project (Assets/BundleAssets, Tools/BundleImport) instead of bundle files.
	// Editor only: the imported assets are loaded with the AssetDatabase and the level scenes by path, so the game
	// runs in Play mode without building any bundle. The player loads the rebuilt bundles.
	public static class LocalBundles
	{
		public const string ManifestPath = "Assets/BundleAssets/_import/bundles.tsv";

		private static readonly Regex s_platformSuffix = new Regex("_Android(Generic|DXT|ETC2)$|_(iOS|Win)$", RegexOptions.IgnoreCase);

		private static readonly Regex s_quality = new Regex("_(High|Low)(?=_|$)", RegexOptions.IgnoreCase);

#if UNITY_EDITOR
		private static EditorSource s_source;
#endif

		public static void Install()
		{
#if UNITY_EDITOR
			if (s_source == null && File.Exists(ManifestPath))
			{
				s_source = new EditorSource(ManifestPath);
				AssetBundleManager.LocalSource = s_source;
				Debug.Log("LocalBundles: serving " + s_source.BundleCount + " bundles from " + ManifestPath);
			}
#endif
		}

		// One key for every variant of a bundle: the game asks for AndroidGeneric / High or Low, the import holds
		// the ETC2 High files.
		public static string BundleKey(string _bundleUrl)
		{
			string name = Path.GetFileNameWithoutExtension(_bundleUrl.Replace('\\', '/'));
			name = s_platformSuffix.Replace(name, string.Empty);
			return s_quality.Replace(name, string.Empty).ToLowerInvariant();
		}

		public static bool HasScene(string _scene)
		{
#if UNITY_EDITOR
			return s_source != null && s_source.ScenePath(_scene) != null;
#else
			return false;
#endif
		}

		public static AsyncOperation LoadSceneAsync(string _scene)
		{
#if UNITY_EDITOR
			string path = (s_source == null) ? null : s_source.ScenePath(_scene);
			if (path != null)
			{
				return UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(path);
			}
#endif
			return null;
		}

#if UNITY_EDITOR
		private class EditorSource : ILocalBundleSource
		{
			private struct Entry
			{
				public string Type;

				public string Path;
			}

			// bundle key -> asset name (full explicit name and file name, lower case) -> assets
			private readonly Dictionary<string, Dictionary<string, List<Entry>>> m_bundles = new Dictionary<string, Dictionary<string, List<Entry>>>();

			private readonly Dictionary<string, string> m_scenes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			public int BundleCount
			{
				get
				{
					return m_bundles.Count;
				}
			}

			public EditorSource(string _manifest)
			{
				foreach (string line in File.ReadAllLines(_manifest))
				{
					string[] f = line.Split('\t');
					if (f.Length < 5)
					{
						continue;
					}
					string key = BundleKey(f[0]);
					Dictionary<string, List<Entry>> assets;
					if (!m_bundles.TryGetValue(key, out assets))
					{
						assets = new Dictionary<string, List<Entry>>();
						m_bundles.Add(key, assets);
					}
					Entry entry = new Entry
					{
						Type = f[3],
						Path = f[4]
					};
					Add(assets, f[2].ToLowerInvariant(), entry);
					Add(assets, System.IO.Path.GetFileNameWithoutExtension(f[2]).ToLowerInvariant(), entry);
					if (f[1] == "1")
					{
						m_scenes[System.IO.Path.GetFileNameWithoutExtension(f[4])] = f[4];
					}
				}
			}

			private static void Add(Dictionary<string, List<Entry>> _assets, string _name, Entry _entry)
			{
				List<Entry> list;
				if (!_assets.TryGetValue(_name, out list))
				{
					list = new List<Entry>();
					_assets.Add(_name, list);
				}
				list.Add(_entry);
			}

			public string ScenePath(string _scene)
			{
				string path;
				return m_scenes.TryGetValue(_scene, out path) ? path : null;
			}

			public bool Contains(string _bundleUrl)
			{
				return m_bundles.ContainsKey(BundleKey(_bundleUrl));
			}

			public UnityEngine.Object LoadAsset(string _bundleUrl, string _name, Type _type)
			{
				Dictionary<string, List<Entry>> assets;
				List<Entry> entries;
				if (_name == null || !m_bundles.TryGetValue(BundleKey(_bundleUrl), out assets) || !assets.TryGetValue(_name.ToLowerInvariant(), out entries))
				{
					Debug.LogWarning("LocalBundles: " + _name + " not in " + _bundleUrl);
					return null;
				}
				foreach (Entry entry in entries)
				{
					UnityEngine.Object asset = (_type == typeof(UnityEngine.Object)) ? UnityEditor.AssetDatabase.LoadMainAssetAtPath(entry.Path) : UnityEditor.AssetDatabase.LoadAssetAtPath(entry.Path, _type);
					if (asset != null)
					{
						return asset;
					}
					foreach (UnityEngine.Object sub in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(entry.Path))
					{
						if (sub != null && _type.IsInstanceOfType(sub))
						{
							return sub;
						}
					}
				}
				Debug.LogWarning("LocalBundles: no " + _type.Name + " " + _name + " in " + _bundleUrl);
				return null;
			}
		}
#endif
	}
}
