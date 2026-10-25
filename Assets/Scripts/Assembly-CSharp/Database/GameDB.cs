using System;
using System.Collections.Generic;
using System.Text;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Definitions;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

namespace Database
{
	public class GameDB
	{
		public class SubDB
		{
			private BBDictionary<GameID, DBEntry> m_data = new BBDictionary<GameID, DBEntry>();

			public string Name { get; private set; }

			public Type Type { get; private set; }

			public BBDictionary<GameID, DBEntry> Data
			{
				get
				{
					return m_data;
				}
			}

			public SubDB(string _name, Type _type)
			{
				Name = _name;
				Type = _type;
			}
		}

		private BBDictionary<GameID, DBEntry> m_dictionary = new BBDictionary<GameID, DBEntry>();

		private BBDictionary<Type, SubDB> m_subDBs = new BBDictionary<Type, SubDB>();

		private static readonly Type[] s_SubDbTypes = new Type[34]
		{
			typeof(AnimatorDef),
			typeof(AudioControllerDef),
			typeof(AudioMixingGroupDef),
			typeof(AudioMixStateDef),
			typeof(ColorDef),
			typeof(ColorTableDef),
			typeof(EffectPackage),
			typeof(HeadDef),
			typeof(HumanoidDef),
			typeof(AccessoryDef),
			typeof(ItemDef),
			typeof(SoundDef),
			typeof(TranslationsDef),
			typeof(RegionDefinition),
			typeof(CodexDefinition),
			typeof(AvatarData.AvatarDefinition),
			typeof(BuffData.BuffDefinition),
			typeof(GameData.GameDefinition),
			typeof(InteractableObjectData.InteractableObjectDefinition),
			typeof(NpcData.NpcDefinition),
			typeof(PatrolData.PatrolDefinition),
			typeof(ProjectileData.ProjectileDefinition),
			typeof(RuleData.RuleDefinition),
			typeof(SkillData.SkillDefinition),
			typeof(StaticObjectData.StaticObjectDefinition),
			typeof(DownloadScreenDef),
			typeof(AchievementDefinition),
			typeof(AchievementCategoryDefinition),
			typeof(UIAtlasDefinition),
			typeof(CTADefinitionBase),
			typeof(EntitlementDefinition),
			typeof(CampaignDefinition),
			typeof(BodyPartTableDefinition),
			typeof(BodyPartDefinition)
		};

		private BBDictionary<int, AssetBundleInfo> m_assetBundleInfos = new BBDictionary<int, AssetBundleInfo>();

		public GameDB(string _filename, EQualityAssetBundle _quality)
		{
			Profiling.Start("GameDB(string)", float.MaxValue);
			DefinitionManager.Initialize();
			m_dictionary.Clear();
			m_assetBundleInfos.Clear();
			CreateSubDBs(null);
			GameDBSerializer.ReadGameDBFromFile(_filename, m_assetBundleInfos, _quality, m_dictionary, GameDBSerializer.SerializationFormat.kFormat_Binary);
			foreach (KeyValuePair<GameID, DBEntry> item in m_dictionary)
			{
				m_subDBs[GetSubDbType(item.Value)].Data.Add(item.Key, item.Value);
			}
			UbiCore.Utils.Singleton<DefinitionManager>.Instance.InitializeDefinitions();
			Profiling.Stop();
			Profiling.DumpLastFrame();
		}

		public bool TryFindGameIdFromGuid(string _guid, out GameID _gameId)
		{
			GameID key = GameID.CreateDummy(_guid);
			DBEntry value;
			if (m_dictionary.TryGetValue(key, out value))
			{
				_gameId = value.GameID;
				return true;
			}
			_gameId = GameID.Invalid;
			return false;
		}

		public GameID FindGameIdFromGuid(string _guid)
		{
			GameID key = GameID.CreateDummy(_guid);
			DBEntry value;
			if (m_dictionary.TryGetValue(key, out value))
			{
				return value.GameID;
			}
			return GameID.Invalid;
		}

		public bool FindGameIdFromGuidHash(int _guidHash, ref GameID _result)
		{
			GameID key = GameID.CreateDummy(_guidHash);
			DBEntry value;
			if (!m_dictionary.TryGetValue(key, out value))
			{
				return false;
			}
			_result = value.GameID;
			return true;
		}

		public T GetItem<T>(GameID _gameID) where T : DBEntry
		{
			DBEntry value = null;
			if (!GameID.IsValid(_gameID))
			{
				return (T)null;
			}
			if (m_dictionary.TryGetValue(_gameID, out value))
			{
				return value as T;
			}
			return (T)null;
		}

		public List<KeyValuePair<GameID, DBEntry>> GetItems<T>() where T : DBEntry
		{
			BBDictionary<GameID, DBEntry> dict = ((typeof(T) != typeof(DBEntry)) ? m_subDBs[typeof(T)].Data : m_dictionary);
			return dict.ToList();
		}

		public bool Contains(DBEntry _dbEntry)
		{
			return m_dictionary.ContainsKey(_dbEntry.GameID);
		}

		private void CreateSubDBs(Type[] _dbEntryTypes)
		{
			if (_dbEntryTypes == null)
			{
				_dbEntryTypes = s_SubDbTypes;
			}
			Type[] array = _dbEntryTypes;
			foreach (Type type in array)
			{
				if (type.IsClass && typeof(DBEntry).IsAssignableFrom(type))
				{
					DatabaseAttribute[] array2 = (DatabaseAttribute[])type.GetCustomAttributes(typeof(DatabaseAttribute), false);
					if (array2.Length > 0)
					{
						SubDB value = new SubDB(array2[0].Name, type);
						m_subDBs.Add(type, value);
					}
				}
			}
		}

		public void Dump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<GameID, DBEntry> item in m_dictionary)
			{
				stringBuilder.AppendLine(item.Value.GameID.ToString());
			}
			Debug.Log(stringBuilder.ToString());
		}

		public void ClearCachedResources()
		{
			foreach (KeyValuePair<GameID, DBEntry> item in m_dictionary)
			{
				item.Value.ClearCache();
			}
		}

		public AssetBundleInfo GetAssetBundleInfo(GameID _gameID)
		{
			DBEntry item = GetItem<DBEntry>(_gameID);
			return item.AssetBundleInfo;
		}

		public IEnumerable<AssetBundleInfo> GetEnforcedLoadingAssetBundleInfos()
		{
			List<AssetBundleInfo> list = new List<AssetBundleInfo>();
			foreach (KeyValuePair<int, AssetBundleInfo> assetBundleInfo in m_assetBundleInfos)
			{
				if (assetBundleInfo.Value.EnforceLoading)
				{
					list.Add(assetBundleInfo.Value);
				}
			}
			return list;
		}

		public HashSet<AssetBundleInfo> GetAssetBundleInfos()
		{
			HashSet<AssetBundleInfo> hashSet = new HashSet<AssetBundleInfo>();
			foreach (KeyValuePair<int, AssetBundleInfo> assetBundleInfo in m_assetBundleInfos)
			{
				hashSet.Add(assetBundleInfo.Value);
			}
			return hashSet;
		}

		public AssetBundleInfo GetAssetBundleInfoByBundleId(int _id)
		{
			AssetBundleInfo value = null;
			if (!m_assetBundleInfos.TryGetValue(_id, out value))
			{
			}
			return value;
		}

		public SubDB GetSubDB(Type _type)
		{
			return m_subDBs[_type];
		}

		private Type GetSubDbType(DBEntry _dbEntry)
		{
			Type type = _dbEntry.GetType();
			DatabaseAttribute[] array = (DatabaseAttribute[])type.GetCustomAttributes(typeof(DatabaseAttribute), false);
			while (array.Length == 0 && type != null)
			{
				type = type.BaseType;
				array = (DatabaseAttribute[])type.GetCustomAttributes(typeof(DatabaseAttribute), false);
			}
			return type;
		}

		public static string ConvertGUIDToName(int _guidInput)
		{
			GameID _result = GameID.Invalid;
			if (Main.Instance.GameDB.FindGameIdFromGuidHash(_guidInput, ref _result))
			{
				NpcData.NpcType npcType = new NpcData.NpcType(_result);
				if (npcType.IsValid)
				{
					return npcType.Definition.Name;
				}
			}
			return "Undefined";
		}
	}
}
