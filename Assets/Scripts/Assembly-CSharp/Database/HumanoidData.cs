using System;
using AcierNetwork.AssetBundles;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Utilities;
using Humanoids;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Database
{
	[ExecuteInEditMode]
	public static class HumanoidData
	{
		public const string CharacterAtlasFolderInAssets = "/RuntimeAssets/Character/Atlases";

		public const string CharacterAtlasFolder = "Assets/RuntimeAssets/Character/Atlases";

		public const string CharacterAtlasMaterialFolder = "Assets/RuntimeAssets/Character/Atlases/Materials";

		private static bool ms_Initialized;

		private static bool ms_Preloaded;

		private static EEra ms_PreloadedEra;

		private static HumanoidMeshCache ms_MeshCache;

		private static BBDictionary<uint, Material> ms_OverrideMatCache;

		public static HumanoidInfo[] Data { get; private set; }

		public static bool Preloaded
		{
			get
			{
				return ms_Preloaded;
			}
		}

		public static EEra PreloadedEra
		{
			get
			{
				return ms_PreloadedEra;
			}
		}

		internal static bool IsInitialized
		{
			get
			{
				return ms_Initialized;
			}
		}

		static HumanoidData()
		{
			ms_PreloadedEra = EEra.Common;
			Initialize();
		}

		private static void Initialize()
		{
			int[] array = (int[])Enum.GetValues(typeof(HumanoidDef.ECharType));
			Data = new HumanoidInfo[array.Length];
			int num = array.Length;
			Data = new HumanoidInfo[num];
			for (int i = 0; num > i; i++)
			{
				Data[i] = new HumanoidInfo();
			}
			for (int j = 0; num > j; j++)
			{
				switch ((HumanoidDef.ECharType)array[j])
				{
				case HumanoidDef.ECharType.Player:
					InitDataPlayer(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.Enemy:
					InitDataEnemy(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.Male:
					InitDataMale(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.Female:
					InitDataFemale(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.Doctor:
					InitDataDoctor(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.Courtesan:
					InitDataCourtesan(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				case HumanoidDef.ECharType.SpecialNPC:
					InitDataEnemy(GetInfo((HumanoidDef.ECharType)array[j]));
					break;
				}
			}
		}

		public static HumanoidInfo GetInfo(HumanoidDef.ECharType _type)
		{
			switch (_type)
			{
			case HumanoidDef.ECharType.Player:
				return Data[0];
			case HumanoidDef.ECharType.Enemy:
				return Data[1];
			case HumanoidDef.ECharType.Male:
				return Data[2];
			case HumanoidDef.ECharType.Female:
				return Data[3];
			case HumanoidDef.ECharType.Doctor:
				return Data[4];
			case HumanoidDef.ECharType.Courtesan:
				return Data[5];
			case HumanoidDef.ECharType.SpecialNPC:
				return Data[6];
			default:
				return null;
			}
		}

		public static void ClearAllPreloads()
		{
			int[] array = (int[])Enum.GetValues(typeof(HumanoidDef.ECharType));
			int[] array2 = array;
			foreach (int type in array2)
			{
				GetInfo((HumanoidDef.ECharType)type).ClearPreloads();
			}
			HumanoidFactoryHelpers.ClearGoCache();
			Data = null;
		}

		public static void PreloadAdditionalPrefab(GameID _animatorPrefab)
		{
			AnimatorDef item = Main.SafeGameDb.GetItem<AnimatorDef>(_animatorPrefab);
			GameObject @object = item.AnimatorPrefab.GetObject<GameObject>();
			HumanoidFactoryHelpers.AddGoToCache(_animatorPrefab, @object);
		}

		public static void OnSceneLoad(EEra _era)
		{
			if (ms_Initialized)
			{
				return;
			}
			ms_Preloaded = true;
			if (_era == EEra.Abstergo)
			{
				_era = EEra.Italy;
			}
			ms_PreloadedEra = _era;
			UbiCore.Utils.Singleton<AnimationMain>.Initialize();
			Initialize();
			GetInfo(HumanoidDef.ECharType.Player).PreloadPrefab();
			ms_MeshCache = new HumanoidMeshCache();
			ms_OverrideMatCache = new BBDictionary<uint, Material>();
			if (_era != EEra.Common)
			{
				GameDB safeGameDb = Main.SafeGameDb;
				HumanoidDef item = safeGameDb.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_femaleCrowd);
				HumanoidDef item2 = safeGameDb.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_maleCrowd);
				HumanoidDef item3 = safeGameDb.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_enemy);
				GetInfo(HumanoidDef.ECharType.Courtesan).PreloadPrefab();
				GetInfo(HumanoidDef.ECharType.Doctor).PreloadPrefab();
				GetInfo(HumanoidDef.ECharType.Female).Preload(item.AssetBundleInfo, HumanoidDef.EAtlasPage.Female, _era);
				GetInfo(HumanoidDef.ECharType.Male).Preload(item2.AssetBundleInfo, HumanoidDef.EAtlasPage.Male, _era);
				GetInfo(HumanoidDef.ECharType.Enemy).Preload(item3.AssetBundleInfo, HumanoidDef.EAtlasPage.Enemy, _era);
				GetInfo(HumanoidDef.ECharType.SpecialNPC).Preload(item3.AssetBundleInfo, HumanoidDef.EAtlasPage.Enemy, _era);
				BBDictionary<GameID, DBEntry> data = safeGameDb.GetSubDB(typeof(HumanoidDef)).Data;
				foreach (GameID key in data.Keys)
				{
					HumanoidDef humanoidDef = data[key] as HumanoidDef;
					if (humanoidDef != null && humanoidDef.AtlasPage != 0 && humanoidDef.Age.Contains(_era))
					{
						PreloadBodyMaterials(humanoidDef);
						PreloadHeadMaterials(humanoidDef);
						PreloadAccessoryMaterials(humanoidDef);
					}
				}
			}
			ms_Initialized = true;
		}

		private static void PreloadAccessoryMaterials(HumanoidDef hd)
		{
			AccessoryDef accessoryDef = hd.GetAccessoryDef();
			if (accessoryDef == null)
			{
				return;
			}
			foreach (string accessoryMaterial in accessoryDef.AccessoryMaterials)
			{
				Material material = LookupMaterial(accessoryDef, accessoryMaterial.ToLower());
				uint key = StringCRC32.ComputeLowerCase(accessoryMaterial);
				if (material != null && !ms_OverrideMatCache.ContainsKey(key))
				{
					ms_OverrideMatCache.Add(key, material);
				}
				else if (!(material == null))
				{
				}
			}
		}

		private static void PreloadHeadMaterials(HumanoidDef hd)
		{
			if (!hd.RandomHeadMaterial || !(hd.Head != null))
			{
				return;
			}
			HeadDef item = Main.SafeGameDb.GetItem<HeadDef>(hd.Head);
			if (item == null)
			{
				return;
			}
			int count = item.MaterialVariations.Count;
			for (int i = 0; count > i; i++)
			{
				string value = PreloadMaterialHelper(item.MaterialVariations[i].Material.GetObject<Material>(), hd);
				if (!string.IsNullOrEmpty(value))
				{
				}
			}
		}

		private static void PreloadBodyMaterials(HumanoidDef hd)
		{
			foreach (string bodyAtlasMaterial in hd.BodyAtlasMaterials)
			{
				Material material = LookupMaterial(hd, bodyAtlasMaterial.ToLower());
				uint key = StringCRC32.ComputeLowerCase(bodyAtlasMaterial);
				if (material != null && !ms_OverrideMatCache.ContainsKey(key))
				{
					ms_OverrideMatCache.Add(key, material);
				}
				else if (!(material == null))
				{
				}
			}
		}

		private static string PreloadMaterialHelper(Material _mat, HumanoidDef _hd)
		{
			string text = HumanoidFactoryHelpers.MaterialOverrideSafeName(_mat, _hd.AtlasPage, _hd.Age.Fix());
			uint key = StringCRC32.ComputeLowerCase(text);
			if (!string.IsNullOrEmpty(text) && !ms_OverrideMatCache.ContainsKey(key))
			{
				Material material = LookupMaterial(_hd, text);
				if (!(material != null))
				{
					return text;
				}
				ms_OverrideMatCache.Add(key, material);
			}
			return string.Empty;
		}

		private static Material LookupMaterial(DBEntry _dbe, string _materialName)
		{
			AssetBundleInfo assetBundleInfo = null;
			assetBundleInfo = _dbe.AssetBundleInfo;
			return AssetBundleManager.Instance.LoadAsset<Material>(assetBundleInfo, _materialName);
		}

		public static void OnSceneUnload()
		{
			if (ms_Initialized)
			{
				UbiCore.Utils.Singleton<AnimationMain>.Shutdown();
				ClearAllPreloads();
				ms_MeshCache = null;
				ms_Initialized = false;
				ms_OverrideMatCache = null;
				ms_Preloaded = false;
			}
		}

		public static Mesh FindCachedMesh(string _name)
		{
			return ms_MeshCache.Find(_name);
		}

		public static void AddMeshToCache(string _name, Mesh _mesh)
		{
			if (ms_MeshCache != null)
			{
				ms_MeshCache.Add(_name, _mesh);
			}
		}

		public static bool TryFindMaterial(string _name, out Material _outMat)
		{
			return ms_OverrideMatCache.TryGetValue(StringCRC32.ComputeLowerCase(_name), out _outMat);
		}

		private static void InitDataPlayer(HumanoidInfo _info)
		{
			_info.GamePrefabGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayer;
		}

		private static void InitDataCourtesan(HumanoidInfo _info)
		{
			_info.GamePrefabGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorCourtesan;
		}

		private static void InitDataDoctor(HumanoidInfo _info)
		{
			_info.GamePrefabGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorDoctor;
		}

		private static void InitDataFemale(HumanoidInfo _info)
		{
			_info.GamePrefabGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorFemale;
		}

		private static void InitDataMale(HumanoidInfo _info)
		{
			InitDataMale(_info, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorMale);
		}

		private static void InitDataMale(HumanoidInfo _info, GameID animusPrefabGameId)
		{
			_info.GamePrefabGameID = animusPrefabGameId;
		}

		private static void InitDataEnemy(HumanoidInfo _info)
		{
			_info.GamePrefabGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorEnemy;
		}
	}
}
