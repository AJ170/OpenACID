using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;
using UbiCore.Logging;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class AvatarData : UbiCore.Utils.Singleton<AvatarData>, IDataManager<AvatarData.AvatarType, AvatarData.IAvatarDefinition>
	{
		public class AvatarType : DataType<AvatarType, IAvatarDefinition>
		{
			protected override IDataManager<AvatarType, IAvatarDefinition> DataManager
			{
				get
				{
					return UbiCore.Utils.Singleton<AvatarData>.Instance;
				}
			}

			public AvatarType(GameID _gameID)
				: base(_gameID)
			{
			}
		}

		public interface IAvatarDefinition : CharacterData.ICharacterDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			AvatarType Type { get; }

			string PortraitDisplayName { get; }

			int PortraitDisplayNameLocaKey { get; }

			GameID WeaponGameID { get; }

			GameID HeadGameID { get; }

			GameID HeadAccessoireGameID { get; }

			BBList<string> GetSkillNames();

			BBList<SkillData.ISkillDefinition> GetSkillDefinitions();
		}

		[Database("AvatarDefinition")]
		public class AvatarDefinition : CharacterData.CharacterDefinition, IAvatarDefinition, CharacterData.ICharacterDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			private EntityTypes.EType m_EntityType;

			public AvatarType Type { get; private set; }

			public string PortraitDisplayName { get; private set; }

			public int PortraitDisplayNameLocaKey { get; private set; }

			public GameID WeaponGameID { get; set; }

			public GameID HeadGameID { get; set; }

			public GameID HeadAccessoireGameID { get; set; }

			public override EntityTypes.EType EntityType
			{
				get
				{
					return m_EntityType;
				}
			}

			public override IdleType DefaultIdleType
			{
				get
				{
					return IdleType.HighProfile;
				}
			}

			public override IdleType TransitionIdleType
			{
				get
				{
					return DefaultIdleType;
				}
			}

			public override Type InstanceType
			{
				get
				{
					return typeof(PlayerCharacter);
				}
			}

			public StatValueCollection StatValueCollectionWriteAccess
			{
				get
				{
					return m_StatValueCollection;
				}
			}

			protected override Data.Definition DefaultTemplateDefinition
			{
				get
				{
					return (!UbiCore.Utils.Singleton<AvatarData>.IsAvailable) ? null : UbiCore.Utils.Singleton<AvatarData>.Instance.DefaultTemplateDefinition;
				}
			}

			private AvatarDefinition AvatarTemplateDefinition
			{
				get
				{
					return (AvatarDefinition)base.TemplateDefinition;
				}
			}

			public AvatarDefinition(AssetBundleInfo _assetBundleInfo)
				: base(PlayerStats.StatsHelpers, _assetBundleInfo)
			{
				WeaponGameID = GameID.Invalid;
				HeadGameID = GameID.Invalid;
				HeadAccessoireGameID = GameID.Invalid;
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				m_EntityType = (EntityTypes.EType)_s.SerializeEnum("EntityType", m_EntityType.GetType(), (int)m_EntityType);
				PortraitDisplayName = _s.SerializeString("PortraitDisplayName", PortraitDisplayName);
				PortraitDisplayNameLocaKey = _s.SerializeInt("PortraitDisplayNameLocaKey", PortraitDisplayNameLocaKey);
				WeaponGameID = SerializeGameID("WeaponGameID", _s, this, WeaponGameID, (AvatarTemplateDefinition == null) ? null : AvatarTemplateDefinition.WeaponGameID, (AvatarTemplateDefinition != null) ? AvatarTemplateDefinition.WeaponGameID : null);
				HeadGameID = SerializeGameID("HeadGameID", _s, this, HeadGameID, (AvatarTemplateDefinition == null) ? null : AvatarTemplateDefinition.HeadGameID, (AvatarTemplateDefinition != null) ? AvatarTemplateDefinition.HeadGameID : null);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			public BBList<string> GetSkillNames()
			{
				IStatCollectionValue statCollectionValue = m_StatValueCollection.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
				BBList<string> bBList = new BBList<string>();
				for (int i = 0; i < bBList.Count; i++)
				{
					string value = statCollectionValue.Get_string(i);
					if (!string.IsNullOrEmpty(value))
					{
						bBList.Add(statCollectionValue.Get_string(i));
					}
				}
				return bBList;
			}

			public BBList<SkillData.ISkillDefinition> GetSkillDefinitions()
			{
				IStatCollectionValue statCollectionValue = m_StatValueCollection.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
				BBList<SkillData.ISkillDefinition> bBList = new BBList<SkillData.ISkillDefinition>();
				for (int i = 0; i < bBList.Count; i++)
				{
					string value = statCollectionValue.Get_string(i);
					if (!string.IsNullOrEmpty(value))
					{
						bBList.Add(UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(default(Enum<SkillTypes>)));
					}
				}
				return bBList;
			}

			public override IdleType GetIdleType(ENpcIdleCategory _category)
			{
				return DefaultIdleType;
			}

			protected override void PostConstructor()
			{
				base.PostConstructor();
				Type = new AvatarType(base.GameID);
				if (UbiCore.Utils.Singleton<AvatarData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<AvatarData>.Instance.AddDefinition(this, false);
				}
			}

			protected override void AddToComplementedRuleCollection(string _skillStr, SkillData.ISkillDefinition _skillDefinition, IList<float> _parameter)
			{
				if (_skillDefinition.ActivationType == ESkillActivationType.PassiveHireling)
				{
					IAvatarDefinition hirelingDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetHirelingDefinition();
					if (hirelingDefinition != null && base.Name == hirelingDefinition.Name)
					{
						MoveToPlayerDefinition(_skillStr, _skillDefinition, _parameter);
					}
				}
				else
				{
					base.AddToComplementedRuleCollection(_skillStr, _skillDefinition, _parameter);
				}
			}

			internal void MoveToPlayerDefinition(string _skillStr, SkillData.ISkillDefinition _skillDefinition, IList<float> _parameter)
			{
				AvatarDefinition avatarDefinition = (AvatarDefinition)UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
				if (avatarDefinition != null)
				{
					avatarDefinition.m_StatValueCollection.AccessMode = Assets.Scripts.Mission.Logic.Stats.StatValueCollection.EAccessMode.Initialization;
					IStatCollectionValue statCollectionValue = avatarDefinition.m_StatValueCollection.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
					statCollectionValue.Add_string(_skillStr);
					avatarDefinition.m_StatValueCollection.AccessMode = Assets.Scripts.Mission.Logic.Stats.StatValueCollection.EAccessMode.WriteMutable;
					avatarDefinition.AddToComplementedRuleCollectionInternal(_skillDefinition, _parameter);
					m_StatValueCollection.AccessMode = Assets.Scripts.Mission.Logic.Stats.StatValueCollection.EAccessMode.Initialization;
					statCollectionValue = m_StatValueCollection.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
					statCollectionValue.Remove_string(_skillStr);
					m_StatValueCollection.AccessMode = Assets.Scripts.Mission.Logic.Stats.StatValueCollection.EAccessMode.WriteMutable;
				}
			}
		}

		public const string ColorVariationPseudoSkillGuid = "_ColorVariation";

		private const string MainPlayer = "<root AssetBundleAvailable=\"False\">\r\n  <Variations ItemCount=\"1\">\r\n    <RuntimeAsset Type=\"Assets.Scripts.Mission.Definitions.AvatarData+AvatarDefinition, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\" GameID=\"7AF4277F-6863-92B0-9298-30F1DED155CA\" Name=\"7AF4277F-6863-92B0-9298-30F1DED155CA\" Status=\"Production\" HumanoidGameID=\"2183e8fb-3d3e-4f20-bd68-1ed2657dba9c\" ColorSet=\"\" EntityType=\"PlayerCharacter\" WeaponGameID=\"7d63cb2a-eecd-4e8c-bf8d-cdfe658d3faf\" HeadGameID=\"77d5f907-1c42-41bd-a94e-283d1a274fff\">\r\n      <StatValueCollection ItemCount=\"0\">\r\n      </StatValueCollection>\r\n    </RuntimeAsset>    \r\n  </Variations>\r\n</root>";

		private const string MainHireling = "<root AssetBundleAvailable=\"False\">\r\n  <Variations ItemCount=\"1\">\r\n    <RuntimeAsset Type=\"Assets.Scripts.Mission.Definitions.AvatarData+AvatarDefinition, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\" GameID=\"7BC3176F-6562-31A0-9498-30F1EE2032DC\" Name=\"7BC3176F-6562-31A0-9498-30F1EE2032DC\" Status=\"Production\" HumanoidGameID=\"cebf5513-4796-46df-8de8-728e5280a916\" ColorSet=\"\" EntityType=\"HirelingCharacter\" WeaponGameID=\"7d63cb2a-eecd-4e8c-bf8d-cdfe658d3faf\" HeadGameID=\"77d5f907-1c42-41bd-a94e-283d1a274fff\">\r\n      <StatValueCollection ItemCount=\"9\">\r\n        <Element StatType=\"Character_Rules\" Type=\"StatCollectionValue`1\">\r\n          <CollectionValues ItemCount=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Skills\" Type=\"StatCollectionValue`1\">\r\n          <CollectionValues ItemCount=\"0\" />\r\n        </Element>\r\n        <Element StatType=\"Character_Conflict_BaseHealth\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"90\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseStrength\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"25\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BasePrecision\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"25\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseWeaponDamage\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"23\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseArmourRating\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"13\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Player_BaseIncognito\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"20\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Player_Level\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"1\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n      </StatValueCollection>\r\n    </RuntimeAsset>    \r\n  </Variations>\r\n</root>";

		private readonly BBDictionary<GameID, AvatarDefinition> m_DefinitionsByGameID = new BBDictionary<GameID, AvatarDefinition>();

		private readonly BBDictionary<string, AvatarDefinition> m_DefinitionsByName = new BBDictionary<string, AvatarDefinition>();

		private readonly BBList<AvatarType> m_Types = new BBList<AvatarType>();

		private readonly ReadOnlyCollection<AvatarType> m_TypesAccess;

		private readonly BBList<AvatarType> m_PlayerTypes = new BBList<AvatarType>();

		private readonly ReadOnlyCollection<AvatarType> m_PlayerTypesAccess;

		private readonly BBList<AvatarType> m_HirelingTypes = new BBList<AvatarType>();

		private readonly ReadOnlyCollection<AvatarType> m_HirelingTypesAccess;

		private GameID m_PlayerId;

		private GameID m_HirelingId;

		public ReadOnlyCollection<AvatarType> Types
		{
			get
			{
				return m_TypesAccess;
			}
		}

		public ReadOnlyCollection<AvatarType> PlayerTypes
		{
			get
			{
				return m_PlayerTypesAccess;
			}
		}

		public ReadOnlyCollection<AvatarType> HirelingTypes
		{
			get
			{
				return m_HirelingTypesAccess;
			}
		}

		private AvatarDefinition DefaultTemplateDefinition
		{
			get
			{
				return (!IsValid("__DefaultTemplate")) ? null : ((AvatarDefinition)GetDefinitionByName("__DefaultTemplate"));
			}
		}

		public AvatarData()
		{
			m_TypesAccess = new ReadOnlyCollection<AvatarType>(m_Types);
			m_PlayerTypesAccess = new ReadOnlyCollection<AvatarType>(m_PlayerTypes);
			m_HirelingTypesAccess = new ReadOnlyCollection<AvatarType>(m_HirelingTypes);
		}

		public IAvatarDefinition GetPlayerDefinition()
		{
			if (Main.IsInitialized && !string.IsNullOrEmpty(Main.Instance.BuildDefinition.Avatar) && (Main.Instance.BuildDefinition.AvatarFixed || !GameID.IsValid(m_PlayerId)))
			{
				return GetDefinitionByName(Main.Instance.BuildDefinition.Avatar);
			}
			return GetDefinition(m_PlayerId);
		}

		public IAvatarDefinition GetHirelingDefinition()
		{
			if (Main.IsInitialized && !string.IsNullOrEmpty(Main.Instance.BuildDefinition.Hireling) && (Main.Instance.BuildDefinition.AvatarFixed || !GameID.IsValid(m_HirelingId)))
			{
				return GetDefinitionByName(Main.Instance.BuildDefinition.Hireling);
			}
			if (m_HirelingId == GameID.Invalid)
			{
				return null;
			}
			return GetDefinition(m_HirelingId);
		}

		public IAvatarDefinition GetDefinition(GameID _gameId)
		{
			if (!GameID.IsValid(_gameId))
			{
				return null;
			}
			AvatarDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByGameID.TryGetValue(_gameId, out value), ChannelLogger.EChannel.Default, "FATAL ERROR: Lookup for avatar with game ID {0} failed.", _gameId.Guid))
			{
				return value;
			}
			return null;
		}

		public IAvatarDefinition GetDefinition(AvatarType _type)
		{
			return GetDefinition((GameID)_type);
		}

		public IAvatarDefinition GetDefinitionByName(string _name)
		{
			AvatarDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByName.TryGetValue(_name, out value), ChannelLogger.EChannel.Default, "FATAL ERROR: Lookup for avatar with name {0} failed.", _name))
			{
				return value;
			}
			return null;
		}

		public bool IsValid(AvatarType _type)
		{
			return m_DefinitionsByGameID.ContainsKey((GameID)_type);
		}

		public bool IsValid(string _name)
		{
			return m_DefinitionsByName.ContainsKey(_name);
		}

		public void PrepareMission()
		{
			AvatarDefinition avatarDefinition = (AvatarDefinition)GetPlayerDefinition();
			AvatarDefinition avatarDefinition2 = (AvatarDefinition)GetHirelingDefinition();
			avatarDefinition.BuildRulesCollection();
			if (avatarDefinition2 != null)
			{
				avatarDefinition2.BuildRulesCollection();
			}
		}

		public void PrepareMission(Assassin _player, HirelingAssassin _hireling, InventoryElement _module)
		{
			SetPlayerInternal(_player, false, _module);
			SetHireling(_hireling);
			AvatarDefinition avatarDefinition = (AvatarDefinition)GetPlayerDefinition();
			AvatarDefinition avatarDefinition2 = (AvatarDefinition)GetHirelingDefinition();
			avatarDefinition.InitializePass1();
			if (avatarDefinition2 != null)
			{
				avatarDefinition2.InitializePass1();
			}
			avatarDefinition.InitializePass2();
			if (avatarDefinition2 != null)
			{
				avatarDefinition2.InitializePass2();
			}
			avatarDefinition.BuildRulesCollection();
			if (avatarDefinition2 != null)
			{
				avatarDefinition2.BuildRulesCollection();
			}
		}

		public void SetPlayer(Assassin _player)
		{
			InventoryElement elementForSlot = _player.GetElementForSlot(EInventorySlot.SlotModule);
			SetPlayerInternal(_player, true, elementForSlot);
		}

		public static GameID TryGetItemModification(LootTemplate _lootTemplate)
		{
			GameID result = GameID.Invalid;
			if (!string.IsNullOrEmpty(_lootTemplate.guid))
			{
				GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_lootTemplate.guid);
				if (gameID != GameID.Invalid)
				{
					result = gameID;
				}
			}
			return result;
		}

		public static GameID TryGetItemColorModification(LootTemplate _lootTemplate)
		{
			GameID _colorDefId = GameID.Invalid;
			if (_lootTemplate != null && _lootTemplate.itemTemplate != null)
			{
				for (int i = 0; i < _lootTemplate.itemTemplate.presetMods.Length; i++)
				{
					LootTemplate.ItemTemplate.ModStaticTemplate modStaticTemplate = _lootTemplate.itemTemplate.presetMods[i];
					if (TryGetOutfitModification(modStaticTemplate.skillGuid, modStaticTemplate.minValue, out _colorDefId))
					{
						break;
					}
				}
			}
			return _colorDefId;
		}

		public static void TryGetOutfitModification(InventoryElement _item, out string _humanoidGuid, out string _colorDefGuid)
		{
			_humanoidGuid = string.Empty;
			_colorDefGuid = string.Empty;
			if (_item.IsItem())
			{
				TryGetOutfitModification(_item.itemData, _item.guid, out _humanoidGuid, out _colorDefGuid);
			}
		}

		public static void TryGetOutfitModification(InventoryElement _item, out GameID _humanoidId, out GameID _colorDefId)
		{
			_humanoidId = GameID.Invalid;
			_colorDefId = GameID.Invalid;
			if (_item.IsItem())
			{
				TryGetOutfitModification(_item.itemData, _item.guid, out _humanoidId, out _colorDefId);
			}
		}

		public static void TryGetOutfitModification(ItemData _item, string _guid, out string _humanoidGuid, out string _colorDefGuid)
		{
			_humanoidGuid = string.Empty;
			_colorDefGuid = string.Empty;
			if (_item.category == Item.EItemCategory.Outfit)
			{
				GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_guid);
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(gameID);
				if (item != null)
				{
					_humanoidGuid = _guid;
				}
			}
			for (int i = 0; i < _item.modifications.Length; i++)
			{
				Modification modification = _item.modifications[i];
				if (TryGetOutfitModification(modification.skillGuid, modification.effectValue, out _colorDefGuid))
				{
					break;
				}
			}
		}

		public static void TryGetOutfitModification(ItemData _item, string _guid, out GameID _humanoidId, out GameID _colorDefId)
		{
			_humanoidId = GameID.Invalid;
			_colorDefId = GameID.Invalid;
			if (_item.category == Item.EItemCategory.Outfit)
			{
				GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_guid);
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(gameID);
				if (item != null)
				{
					_humanoidId = gameID;
				}
			}
			for (int i = 0; i < _item.modifications.Length; i++)
			{
				Modification modification = _item.modifications[i];
				if (TryGetOutfitModification(modification.skillGuid, modification.effectValue, out _colorDefId))
				{
					break;
				}
			}
		}

		public static bool TryGetOutfitModification(string _skillGuid, int _effectValue, out string _colorDefGuid)
		{
			_colorDefGuid = string.Empty;
			if (_skillGuid.StartsWith("_ColorVariation"))
			{
				int num = -1;
				int length = "_ColorVariation".Length;
				int num2 = _skillGuid.IndexOf('[', length);
				while (num2 >= 0 && num <= _effectValue)
				{
					int num3 = _skillGuid.IndexOf(']', num2 + 1);
					string text = _skillGuid.Substring(num2 + 1, num3 - num2 - 1);
					if (!string.IsNullOrEmpty(text))
					{
						num++;
						if (num == _effectValue)
						{
							GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(text);
							ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(gameID);
							if (item != null)
							{
								_colorDefGuid = text;
								return true;
							}
							break;
						}
					}
					length = num3;
					num2 = _skillGuid.IndexOf('[', length);
				}
			}
			return false;
		}

		public static bool TryGetOutfitModification(string _skillGuid, int _effectValue, out GameID _colorDefId)
		{
			_colorDefId = GameID.Invalid;
			if (_skillGuid.StartsWith("_ColorVariation"))
			{
				int num = -1;
				int length = "_ColorVariation".Length;
				int num2 = _skillGuid.IndexOf('[', length);
				while (num2 >= 0 && num <= _effectValue)
				{
					int num3 = _skillGuid.IndexOf(']', num2 + 1);
					string text = _skillGuid.Substring(num2 + 1, num3 - num2 - 1);
					if (!string.IsNullOrEmpty(text))
					{
						num++;
						if (num == _effectValue)
						{
							GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(text);
							ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(gameID);
							if (item != null)
							{
								_colorDefId = gameID;
								return true;
							}
							break;
						}
					}
					length = num3;
					num2 = _skillGuid.IndexOf('[', length);
				}
			}
			return false;
		}

		private void SetPlayerInternal(Assassin _player, bool _initDefinitions, InventoryElement _module)
		{
			AvatarDefinition avatarDefinition = ReadAvatar("<root AssetBundleAvailable=\"False\">\r\n  <Variations ItemCount=\"1\">\r\n    <RuntimeAsset Type=\"Assets.Scripts.Mission.Definitions.AvatarData+AvatarDefinition, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\" GameID=\"7AF4277F-6863-92B0-9298-30F1DED155CA\" Name=\"7AF4277F-6863-92B0-9298-30F1DED155CA\" Status=\"Production\" HumanoidGameID=\"2183e8fb-3d3e-4f20-bd68-1ed2657dba9c\" ColorSet=\"\" EntityType=\"PlayerCharacter\" WeaponGameID=\"7d63cb2a-eecd-4e8c-bf8d-cdfe658d3faf\" HeadGameID=\"77d5f907-1c42-41bd-a94e-283d1a274fff\">\r\n      <StatValueCollection ItemCount=\"0\">\r\n      </StatValueCollection>\r\n    </RuntimeAsset>    \r\n  </Variations>\r\n</root>", delegate(AvatarDefinition _avatarDefinition)
			{
				AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType(_player.classTypeId);
				m_PlayerId = _avatarDefinition.GameID;
				_avatarDefinition.StatValueCollectionWriteAccess.AccessMode = StatValueCollection.EAccessMode.Initialization;
				_avatarDefinition.Name = _player.GetAssassinName();
				_avatarDefinition.Set_int(StatTypes.EStatType.Player_Level, _player.level);
				_avatarDefinition.Set_int(StatTypes.EStatType.Player_ItemPower, _player.TotalItemPower());
				_avatarDefinition.HumanoidGameID = Main.SafeGameDb.FindGameIdFromGuid(_player.humanoidGuid);
				_avatarDefinition.HeadGameID = Main.SafeGameDb.FindGameIdFromGuid(_player.headGuid);
				_avatarDefinition.WeaponGameID = Main.SafeGameDb.FindGameIdFromGuid(_player.weaponGuid);
				if (!string.IsNullOrEmpty(_player.colorSetting))
				{
					GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_player.colorSetting);
					if (GameID.IsValid(gameID))
					{
						_avatarDefinition.ColorSet = gameID;
					}
				}
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseArmourRating, _player.armoreRating);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseWeaponDamage, _player.baseWeapon);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseEvasion, _player.evasion);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseHealth, _player.hitPoints);
				_avatarDefinition.Set_float(StatTypes.EStatType.Player_BaseIncognito, _player.incognito);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BasePrecision, _player.precision);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseStrength, _player.strength);
				_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseCriticalMultiplier, definitionByType.criticalMultiplier);
				_avatarDefinition.Set_int(StatTypes.EStatType.Player_KillTokenRequiredForKill, definitionByType.killTokensRequiredForKill);
				IStatCollectionValue statCollectionValue = _avatarDefinition.StatValueCollectionWriteAccess.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
				if (_player.baseClassskillTiers != null || _player.extendedClassskillTiers != null)
				{
					Assets.Scripts.UI.Utils.Extensions.SkillData[][][] skills = _player.GetSkills();
					string[] valuesCopy_string = statCollectionValue.GetValuesCopy_string();
					statCollectionValue.Clear();
					BBList<int> bBList = new BBList<int>();
					for (int i = 0; i < skills.Length; i++)
					{
						if (skills[i] != null)
						{
							for (int j = 0; j < skills[i].Length; j++)
							{
								if (skills[i][j] != null)
								{
									for (int k = 0; k < skills[i][j].Length; k++)
									{
										if (skills[i][j][k] != null)
										{
											Assets.Scripts.UI.Utils.Extensions.SkillData skillData = skills[i][j][k];
											int _idx;
											if (HasEquipped(_player, skillData, out _idx))
											{
												if (_idx < 0)
												{
													SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(skillData.SkillId, skillData.Level);
													if (definition != null)
													{
														statCollectionValue.Add_string(definition.Type.Name);
													}
												}
												else
												{
													bBList.Add(_idx);
												}
											}
										}
									}
								}
							}
						}
					}
					bBList.Sort((int l, int r) => _player.skillWheel.entries[l].position.CompareTo(_player.skillWheel.entries[r].position));
					AvatarClassDefinitions.AvatarClass avatarClass = ((_player.extendedClassTypeId != Assassin.EClassTypeId.None) ? Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType(_player.extendedClassTypeId) : null);
					int num = 0;
					for (int m = 0; m < bBList.Count; m++)
					{
						SkillWheelEntry skillWheelEntry = _player.skillWheel.entries[bBList[m]];
						int tierId = skillWheelEntry.tierId;
						bool flag = skillWheelEntry.classType == definitionByType.classTypeId;
						string guid = ((!flag) ? avatarClass : definitionByType).GetSkillTier(tierId).activeSkill.guid;
						AssassinSkills skillTier = _player.GetSkillTier(tierId, flag);
						if (skillTier.activeSkill.level > 0)
						{
							for (int n = num + 1; n < skillWheelEntry.position; n++)
							{
								statCollectionValue.Add_string(string.Empty);
							}
							num = skillWheelEntry.position;
							SkillData.ISkillDefinition definition2 = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(guid, skillTier.activeSkill.level);
							if (definition2 != null)
							{
								statCollectionValue.Add_string(definition2.Type.Name);
							}
						}
					}
					for (int num2 = num + 1; num2 < 5; num2++)
					{
						statCollectionValue.Add_string(string.Empty);
					}
					for (int num3 = 0; num3 < valuesCopy_string.Length; num3++)
					{
						if ((!(valuesCopy_string[num3] == "Call_Hireling") || EntitlementExtensions.IsHirelingUnlocked()) && (!(valuesCopy_string[num3] == "Eagle_Vision") || EntitlementExtensions.IsEagleVisionUnlocked()))
						{
							statCollectionValue.Add_string(valuesCopy_string[num3]);
						}
					}
				}
				EquippedElement[] equippedElements = _player.equippedElements;
				foreach (EquippedElement equippedElement in equippedElements)
				{
					InventoryElement element = equippedElement.element;
					if (element.IsItem())
					{
						ItemData itemData = element.itemData;
						if (itemData.category.IsWeaponCategory())
						{
							_avatarDefinition.Set_float(StatTypes.EStatType.Character_Conflict_BaseWeaponDamage, itemData.baseValue);
						}
						else if (itemData.category.IsArmorCategory())
						{
							_avatarDefinition.AddFloat(StatTypes.EStatType.Character_Conflict_BaseArmourRating, itemData.baseValue);
						}
						GameID _gameId;
						if (itemData.category == Item.EItemCategory.Insignia && !string.IsNullOrEmpty(element.guid) && Main.Instance.GameDB.TryFindGameIdFromGuid(element.guid, out _gameId))
						{
							ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(_gameId);
							if (!string.IsNullOrEmpty(item.ItemPrefab.GetResourcePath()))
							{
								_avatarDefinition.HeadAccessoireGameID = _gameId;
							}
						}
						GameID _humanoidId;
						GameID _colorDefId;
						TryGetOutfitModification(element, out _humanoidId, out _colorDefId);
						if (GameID.IsValid(_humanoidId))
						{
							_avatarDefinition.HumanoidGameID = _humanoidId;
						}
						if (GameID.IsValid(_colorDefId))
						{
							_avatarDefinition.ColorSet = _colorDefId;
						}
						Modification[] modifications = itemData.modifications;
						foreach (Modification modification in modifications)
						{
							if (!modification.skillGuid.StartsWith("_ColorVariation") && !string.IsNullOrEmpty(modification.skillGuid))
							{
								string value = UbiCore.Utils.Singleton<SkillData>.Instance.MakeSkillString(modification.skillGuid, modification.effectValue);
								if (Assets.Scripts.Engine.Helpers.Verify(!string.IsNullOrEmpty(value)))
								{
									statCollectionValue.Add_string(value);
								}
							}
						}
					}
				}
				if (_module != null && _module.IsModule())
				{
					ModuleData moduleData = _module.moduleData;
					for (int num6 = 0; num6 < moduleData.modifications.Length; num6++)
					{
						Modification modification2 = moduleData.modifications[num6];
						if (modification2.moduleModifier == EModuleModifier.ClientModifier)
						{
							string value2 = UbiCore.Utils.Singleton<SkillData>.Instance.MakeSkillString(modification2.skillGuid, modification2.effectValue);
							if (Assets.Scripts.Engine.Helpers.Verify(!string.IsNullOrEmpty(value2)))
							{
								statCollectionValue.Add_string(value2);
							}
						}
					}
				}
				if (_player.extendedClassTypeId != Assassin.EClassTypeId.None)
				{
					AvatarClassDefinitions.AvatarClass.SecondaryClassData secondaryClassData = definitionByType.secondaryClassInfos.FirstOrDefault((AvatarClassDefinitions.AvatarClass.SecondaryClassData info) => info.classTypeId == _player.extendedClassTypeId);
					if (secondaryClassData != null)
					{
						for (int num7 = 0; num7 < secondaryClassData.skills.Length; num7++)
						{
							string value3 = UbiCore.Utils.Singleton<SkillData>.Instance.MakeSkillString(secondaryClassData.skills[num7].guid, secondaryClassData.skills[num7].parameter);
							if (Assets.Scripts.Engine.Helpers.Verify(!string.IsNullOrEmpty(value3)))
							{
								statCollectionValue.Add_string(value3);
							}
						}
					}
				}
				_avatarDefinition.StatValueCollectionWriteAccess.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
			});
			if (_initDefinitions)
			{
				avatarDefinition.InitializePass1();
				avatarDefinition.InitializePass2();
			}
		}

		private void SetHireling(HirelingAssassin _hireling)
		{
			if (_hireling == null)
			{
				m_HirelingId = GameID.Invalid;
				return;
			}
			ReadAvatar("<root AssetBundleAvailable=\"False\">\r\n  <Variations ItemCount=\"1\">\r\n    <RuntimeAsset Type=\"Assets.Scripts.Mission.Definitions.AvatarData+AvatarDefinition, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\" GameID=\"7BC3176F-6562-31A0-9498-30F1EE2032DC\" Name=\"7BC3176F-6562-31A0-9498-30F1EE2032DC\" Status=\"Production\" HumanoidGameID=\"cebf5513-4796-46df-8de8-728e5280a916\" ColorSet=\"\" EntityType=\"HirelingCharacter\" WeaponGameID=\"7d63cb2a-eecd-4e8c-bf8d-cdfe658d3faf\" HeadGameID=\"77d5f907-1c42-41bd-a94e-283d1a274fff\">\r\n      <StatValueCollection ItemCount=\"9\">\r\n        <Element StatType=\"Character_Rules\" Type=\"StatCollectionValue`1\">\r\n          <CollectionValues ItemCount=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Skills\" Type=\"StatCollectionValue`1\">\r\n          <CollectionValues ItemCount=\"0\" />\r\n        </Element>\r\n        <Element StatType=\"Character_Conflict_BaseHealth\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"90\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseStrength\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"25\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BasePrecision\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"25\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseWeaponDamage\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"23\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Character_Conflict_BaseArmourRating\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"13\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Player_BaseIncognito\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"20\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n\t\t<Element StatType=\"Player_Level\" Type=\"StatValue`1\">\r\n            <Value\r\n                value=\"1\"\r\n                levelIncrement=\"0\" />\r\n        </Element>\r\n      </StatValueCollection>\r\n    </RuntimeAsset>    \r\n  </Variations>\r\n</root>", delegate(AvatarDefinition _avatarDefinition)
			{
				m_HirelingId = _avatarDefinition.GameID;
				_avatarDefinition.StatValueCollectionWriteAccess.AccessMode = StatValueCollection.EAccessMode.Initialization;
				_avatarDefinition.Name = _hireling.GetHirelingName();
				_avatarDefinition.Set_int(StatTypes.EStatType.Player_Level, _hireling.level);
				_avatarDefinition.Set_int(StatTypes.EStatType.Player_ItemPower, _hireling.itemPower);
				_avatarDefinition.HumanoidGameID = Main.SafeGameDb.FindGameIdFromGuid(_hireling.humanoidGuid);
				_avatarDefinition.HeadGameID = Main.SafeGameDb.FindGameIdFromGuid(_hireling.headGuid);
				_avatarDefinition.WeaponGameID = Main.SafeGameDb.FindGameIdFromGuid(_hireling.weaponGuid);
				if (!string.IsNullOrEmpty(_hireling.colorSetting))
				{
					GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_hireling.colorSetting);
					if (GameID.IsValid(gameID))
					{
						_avatarDefinition.ColorSet = gameID;
					}
				}
				IStatCollectionValue statCollectionValue = _avatarDefinition.StatValueCollectionWriteAccess.GetStatCollectionValue(StatTypes.EStatType.Character_Rules);
				IStatCollectionValue statCollectionValue2 = _avatarDefinition.StatValueCollectionWriteAccess.GetStatCollectionValue(StatTypes.EStatType.Character_Skills);
				AvatarClassDefinitions.AvatarClass avatarClass = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.classes.First((AvatarClassDefinitions.AvatarClass c) => c.classTypeId == _hireling.classTypeId);
				string[] hirelingAbilityGuids = avatarClass.hirelingAbilityGuids;
				foreach (string guidString in hirelingAbilityGuids)
				{
					RuleData.IRuleDefinition definition = UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(guidString);
					if (definition != null)
					{
						statCollectionValue.Add_string(definition.Type.Name);
					}
					else
					{
						SkillData.ISkillDefinition definition2 = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(guidString, 0);
						if (definition2 != null)
						{
							statCollectionValue2.Add_string(definition2.Type.Name);
						}
					}
				}
				_avatarDefinition.StatValueCollectionWriteAccess.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
			});
		}

		private AvatarDefinition ReadAvatar(string _definition, Action<AvatarDefinition> _initAction)
		{
			AssetBundleInfo _assetBundleInfo = null;
			List<DBEntry> _dbEntries = null;
			GameDBSerializer.ReadGameDBFromString(_definition, out _dbEntries, out _assetBundleInfo);
			AvatarDefinition avatarDefinition = (AvatarDefinition)_dbEntries[0];
			_initAction(avatarDefinition);
			AddDefinition(avatarDefinition, true);
			UbiCore.Utils.Singleton<DefinitionManager>.Instance.RegisterOrReplaceDefinition(avatarDefinition);
			return avatarDefinition;
		}

		private bool HasEquipped(Assassin _assassin, Assets.Scripts.UI.Utils.Extensions.SkillData _skillData, out int _idx)
		{
			_idx = -1;
			if (_skillData.Level <= 0)
			{
				return false;
			}
			if (!_skillData.Active)
			{
				return true;
			}
			if (_assassin.skillWheel != null)
			{
				for (int i = 0; i < _assassin.skillWheel.entries.Length; i++)
				{
					SkillWheelEntry skillWheelEntry = _assassin.skillWheel.entries[i];
					if (_skillData.AvatarType == skillWheelEntry.classType && skillWheelEntry.tierId == _skillData.TierId)
					{
						_idx = i;
						return true;
					}
				}
			}
			return false;
		}

		private void AddDefinition(AvatarDefinition _definition, bool _replacing)
		{
			if (!_replacing && UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status != 0)
			{
				return;
			}
			m_DefinitionsByGameID[_definition.GameID] = _definition;
			m_DefinitionsByName[_definition.Name] = _definition;
			if (!(_definition.Name != "__DefaultTemplate"))
			{
				return;
			}
			if (!m_Types.Contains(_definition.Type))
			{
				m_Types.Add(_definition.Type);
			}
			switch (_definition.EntityType)
			{
			case EntityTypes.EType.PlayerCharacter:
				if (!m_PlayerTypes.Contains(_definition.Type))
				{
					m_PlayerTypes.Add(_definition.Type);
				}
				break;
			case EntityTypes.EType.HirelingCharacter:
				if (!m_HirelingTypes.Contains(_definition.Type))
				{
					m_HirelingTypes.Add(_definition.Type);
				}
				break;
			}
		}

		private void Clear()
		{
			m_DefinitionsByGameID.Clear();
			m_DefinitionsByName.Clear();
			m_Types.Clear();
			m_PlayerTypes.Clear();
			m_HirelingTypes.Clear();
		}
	}
}
