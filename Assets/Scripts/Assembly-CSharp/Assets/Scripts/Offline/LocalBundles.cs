using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AcierNetwork.AssetBundles;
using UnityEngine;

namespace Assets.Scripts.Offline
{
	// The game's bundles served without the CDN download path (Assets/BundleAssets, Tools/BundleImport).
	// Editor: the imported assets are loaded with the AssetDatabase and the level scenes by path, so the game runs in
	// Play mode without building any bundle. Player (PS Vita): the bundles rebuilt from those assets by
	// ACIDBundleBuild, one file per bundle in <DataRoot>/Bundles next to the same bundles.tsv, opened with
	// AssetBundle.LoadFromFile; assets are found by their project path, scenes by name.
	public static class LocalBundles
	{
		public const string ManifestPath = "Assets/BundleAssets/_import/bundles.tsv";

		public const string ManifestName = "bundles.tsv";

		private static readonly Regex s_platformSuffix = new Regex("_Android(Generic|DXT|ETC2)$|_(iOS|Win)$", RegexOptions.IgnoreCase);

		private static readonly Regex s_quality = new Regex("_(High|Low)(?=_|$)", RegexOptions.IgnoreCase);

		private static ManifestSource s_source;

		public static string PlayerBundleFolder
		{
			get
			{
				return OfflineMode.DataRoot + "/Bundles";
			}
		}

		public static void Install()
		{
			if (s_source != null)
			{
				return;
			}
#if UNITY_EDITOR
			if (File.Exists(ManifestPath))
			{
				s_source = new EditorSource(ManifestPath);
			}
#else
			if (File.Exists(PlayerBundleFolder + "/" + ManifestName))
			{
				s_source = new FileSource(PlayerBundleFolder);
			}
#endif
			if (s_source != null)
			{
				AssetBundleManager.LocalSource = s_source;
				Debug.Log("LocalBundles: serving " + s_source.BundleCount + " bundles (" + s_source.GetType().Name + ")");
			}
		}

		// One key for every variant of a bundle: the game asks for AndroidGeneric / High or Low, the import holds
		// the ETC2 High files. Also the file name of a rebuilt bundle.
		public static string BundleKey(string _bundleUrl)
		{
			string name = Path.GetFileNameWithoutExtension(_bundleUrl.Replace('\\', '/'));
			name = s_platformSuffix.Replace(name, string.Empty);
			return s_quality.Replace(name, string.Empty).ToLowerInvariant();
		}

		public static bool HasScene(string _scene)
		{
			return s_source != null && s_source.ScenePath(_scene) != null;
		}

		public static AsyncOperation LoadSceneAsync(string _scene)
		{
			return (s_source == null) ? null : s_source.LoadSceneAsync(_scene);
		}

		// bundles.tsv: bundle file, scene bundle (0/1), explicit asset name, type, project path
		private abstract class ManifestSource : ILocalBundleSource
		{
			protected struct Entry
			{
				public string Type;

				public string Path;
			}

			// bundle key -> asset name (full explicit name and file name, lower case) -> assets
			private readonly Dictionary<string, Dictionary<string, List<Entry>>> m_bundles = new Dictionary<string, Dictionary<string, List<Entry>>>();

			// scene name -> (bundle key, project path)
			private readonly Dictionary<string, KeyValuePair<string, string>> m_scenes = new Dictionary<string, KeyValuePair<string, string>>(StringComparer.OrdinalIgnoreCase);

			public int BundleCount
			{
				get
				{
					return m_bundles.Count;
				}
			}

			protected ManifestSource(string _manifest)
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
						m_scenes[System.IO.Path.GetFileNameWithoutExtension(f[4])] = new KeyValuePair<string, string>(key, f[4]);
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
				KeyValuePair<string, string> scene;
				return m_scenes.TryGetValue(_scene, out scene) ? scene.Value : null;
			}

			public AsyncOperation LoadSceneAsync(string _scene)
			{
				KeyValuePair<string, string> scene;
				return m_scenes.TryGetValue(_scene, out scene) ? LoadSceneAsync(scene.Key, scene.Value) : null;
			}

			public bool Contains(string _bundleUrl)
			{
				return m_bundles.ContainsKey(BundleKey(_bundleUrl));
			}

			public UnityEngine.Object LoadAsset(string _bundleUrl, string _name, Type _type)
			{
				string key = BundleKey(_bundleUrl);
				Dictionary<string, List<Entry>> assets;
				List<Entry> entries;
				if (_name == null || !m_bundles.TryGetValue(key, out assets) || !assets.TryGetValue(_name.ToLowerInvariant(), out entries))
				{
					Debug.LogWarning("LocalBundles: " + _name + " not in " + _bundleUrl);
					return null;
				}
				foreach (Entry entry in entries)
				{
					UnityEngine.Object asset = LoadEntry(key, entry.Path, _type);
					if (asset != null)
					{
						return asset;
					}
				}
				Debug.LogWarning("LocalBundles: no " + _type.Name + " " + _name + " in " + _bundleUrl);
				return null;
			}

			protected abstract UnityEngine.Object LoadEntry(string _key, string _path, Type _type);

			protected abstract AsyncOperation LoadSceneAsync(string _key, string _path);
		}

		// Player: the rebuilt bundle files, each opened once and kept (LZ4, so only the headers stay in memory).
		private class FileSource : ManifestSource
		{
			private readonly string m_folder;

			private readonly Dictionary<string, AssetBundle> m_open = new Dictionary<string, AssetBundle>();

			public FileSource(string _folder)
				: base(_folder + "/" + ManifestName)
			{
				m_folder = _folder;
			}

			private AssetBundle Open(string _key)
			{
				AssetBundle bundle;
				if (!m_open.TryGetValue(_key, out bundle))
				{
					bundle = AssetBundle.LoadFromFile(m_folder + "/" + _key);
					if (bundle == null)
					{
						Debug.LogError("LocalBundles: cannot open " + m_folder + "/" + _key);
					}
					m_open[_key] = bundle;
				}
				return bundle;
			}

			protected override UnityEngine.Object LoadEntry(string _key, string _path, Type _type)
			{
				AssetBundle bundle = Open(_key);
				if (bundle == null)
				{
					return null;
				}
				string name = _path.ToLowerInvariant();
				UnityEngine.Object asset = bundle.LoadAsset(name, _type);
				if (asset != null)
				{
					return asset;
				}
				foreach (UnityEngine.Object sub in bundle.LoadAssetWithSubAssets(name))
				{
					if (sub != null && _type.IsInstanceOfType(sub))
					{
						return sub;
					}
				}
				return null;
			}

			protected override AsyncOperation LoadSceneAsync(string _key, string _path)
			{
				return (Open(_key) == null) ? null : UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(System.IO.Path.GetFileNameWithoutExtension(_path));
			}
		}

#if UNITY_EDITOR
		private class EditorSource : ManifestSource
		{
			public EditorSource(string _manifest)
				: base(_manifest)
			{
			}

			protected override UnityEngine.Object LoadEntry(string _key, string _path, Type _type)
			{
				UnityEngine.Object asset = (_type == typeof(UnityEngine.Object)) ? UnityEditor.AssetDatabase.LoadMainAssetAtPath(_path) : UnityEditor.AssetDatabase.LoadAssetAtPath(_path, _type);
				if (asset != null)
				{
					return asset;
				}
				foreach (UnityEngine.Object sub in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(_path))
				{
					if (sub != null && _type.IsInstanceOfType(sub))
					{
						return sub;
					}
				}
				return null;
			}

			protected override AsyncOperation LoadSceneAsync(string _key, string _path)
			{
				return UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(_path);
			}
		}
#endif
	}
}
