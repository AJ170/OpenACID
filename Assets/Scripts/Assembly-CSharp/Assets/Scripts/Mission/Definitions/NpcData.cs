using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class NpcData : UbiCore.Utils.Singleton<NpcData>, IDataManager<NpcData.NpcType, NpcData.INpcDefinition>
	{
		public class NpcType : DataType<NpcType, INpcDefinition>
		{
			protected override IDataManager<NpcType, INpcDefinition> DataManager
			{
				get
				{
					return UbiCore.Utils.Singleton<NpcData>.Instance;
				}
			}

			public NpcType(GameID _gameID)
				: base(_gameID)
			{
			}
		}

		public interface INpcDefinition : CharacterData.ICharacterDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, MapGeneratorData.IMapGeneratorEntityDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			string BehaviorTreePath { get; }

			string CombatBehaviorTreePath { get; }

			NpcType Type { get; }

			int ThreatLevel { get; }

			ENpcCategory Category { get; }

			NpcDefinition.ENpcInteractionType InteractionType { get; }

			IconMaterial InteractionIcon { get; }

			float RunSpeedOverwrite { get; }

			float[] DetectionDistances { get; }

			float[] DetectionAngles { get; }

			float[] DetectionAnglesVertical { get; }

			float CloseRangeDistanceStalking { get; }

			float CloseRangeFactorStrikingAssasination { get; }

			float CloseRangeFactorStealthAssasination { get; }

			float CloseRangeFactorCombat { get; }

			float CloseRangeFactorStalking { get; }

			float CloseRangeAudioDistance { get; }

			float[] NotoritiesContinuous { get; }

			float[] NotoritiesInteractions { get; }

			float StayAlarmedMinTime { get; }

			float MomentOfChaosTime { get; }

			GameID WeaponGameID { get; }

			GameID RangedWeaponGameID { get; }

			GameID HumanoidPrefabOverride { get; }

			float HidingSpotInspectionDistance { get; }

			EHidingSpotFlags HidingSpotInspectionMask { get; }
		}

		[Database("NpcDefinition")]
		public class NpcDefinition : CharacterData.CharacterDefinition, CharacterData.ICharacterDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, MapGeneratorData.IMapGeneratorEntityDefinition, MapGeneratorData.IMapGeneratorEntityDefinitionAccess, INpcDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			public enum ENpcInteractionType
			{
				Invalid = 0,
				Merchant = 1,
				Mission = 2
			}

			protected NpcType m_Type;

			protected string m_behaviorTreePath;

			protected string m_combatBehaviorTreePath;

			protected EntityTypes.EType m_entityType;

			protected BBSimpleList<IdleType> m_idleTypes = new BBSimpleList<IdleType>();

			protected ENpcInteractionType m_interactionType;

			protected IconMaterial m_interactionIcon;

			protected float[] m_detectionDistances;

			protected float[] m_detectionAngles;

			protected float[] m_detectionAnglesVertical;

			protected float[] m_notoritiesContinuous;

			protected float[] m_notoritiesInteractions;

			public override IdleType DefaultIdleType
			{
				get
				{
					return GetIdleType(ENpcIdleCategory.NpcTypeDefault);
				}
			}

			public override IdleType TransitionIdleType
			{
				get
				{
					return GetIdleType(ENpcIdleCategory.Neutral);
				}
			}

			public string BehaviorTreePath
			{
				get
				{
					return m_behaviorTreePath;
				}
			}

			public string CombatBehaviorTreePath
			{
				get
				{
					return m_combatBehaviorTreePath;
				}
			}

			public NpcType Type
			{
				get
				{
					return m_Type;
				}
			}

			public ENpcCategory Category { get; protected set; }

			public int ThreatLevel { get; protected set; }

			public int MapWeight { get; set; }

			public int UnlockLevel { get; set; }

			public ENpcInteractionType InteractionType
			{
				get
				{
					return m_interactionType;
				}
			}

			public IconMaterial InteractionIcon
			{
				get
				{
					return m_interactionIcon;
				}
			}

			public BBSimpleList<IdleType> IdleTypes
			{
				get
				{
					return m_idleTypes;
				}
			}

			public override EntityTypes.EType EntityType
			{
				get
				{
					return m_entityType;
				}
			}

			public float RunSpeedOverwrite { get; protected set; }

			public float[] DetectionDistances
			{
				get
				{
					return m_detectionDistances;
				}
			}

			public float[] DetectionAngles
			{
				get
				{
					return m_detectionAngles;
				}
			}

			public float[] DetectionAnglesVertical
			{
				get
				{
					return m_detectionAnglesVertical;
				}
			}

			public float CloseRangeDistanceStalking { get; protected set; }

			public float CloseRangeFactorStrikingAssasination { get; protected set; }

			public float CloseRangeFactorStealthAssasination { get; protected set; }

			public float CloseRangeFactorCombat { get; protected set; }

			public float CloseRangeFactorStalking { get; protected set; }

			public float CloseRangeAudioDistance { get; protected set; }

			public float[] NotoritiesContinuous
			{
				get
				{
					return m_notoritiesContinuous;
				}
			}

			public float[] NotoritiesInteractions
			{
				get
				{
					return m_notoritiesInteractions;
				}
			}

			public float StayAlarmedMinTime { get; protected set; }

			public float MomentOfChaosTime { get; protected set; }

			public GameID WeaponGameID { get; protected set; }

			public GameID RangedWeaponGameID { get; protected set; }

			public GameID HumanoidPrefabOverride { get; protected set; }

			public float HidingSpotInspectionDistance { get; protected set; }

			public EHidingSpotFlags HidingSpotInspectionMask { get; protected set; }

			public override Type InstanceType
			{
				get
				{
					return typeof(NpcCharacter);
				}
			}

			protected override Data.Definition DefaultTemplateDefinition
			{
				get
				{
					return (!UbiCore.Utils.Singleton<NpcData>.IsAvailable) ? null : UbiCore.Utils.Singleton<NpcData>.Instance.DefaultTemplateDefinition;
				}
			}

			private NpcDefinition NpcTemplateDefinition
			{
				get
				{
					return (NpcDefinition)base.TemplateDefinition;
				}
			}

			public NpcDefinition(AssetBundleInfo _assetBundleInfo)
				: base(StatsHelpers, _assetBundleInfo)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				Category = (ENpcCategory)_s.SerializeEnum("Category", Category.GetType(), (int)Category);
				m_entityType = (EntityTypes.EType)_s.SerializeEnum("EntityType", m_entityType.GetType(), (int)m_entityType);
				m_idleTypes = XmlHelpers.SerializeEnumMap<ENpcIdleCategory, IdleType>("IdleTypes", _s, m_idleTypes, (NpcTemplateDefinition != null) ? NpcTemplateDefinition.m_idleTypes : null);
				WeaponGameID = SerializeGameID("WeaponGameId", _s, this, WeaponGameID, (NpcTemplateDefinition == null) ? null : NpcTemplateDefinition.WeaponGameID, (NpcTemplateDefinition != null) ? NpcTemplateDefinition.WeaponGameID : null);
				RangedWeaponGameID = SerializeGameID("RangedWeaponGameId", _s, this, RangedWeaponGameID, (NpcTemplateDefinition == null) ? null : NpcTemplateDefinition.RangedWeaponGameID, (NpcTemplateDefinition != null) ? NpcTemplateDefinition.RangedWeaponGameID : null);
				HumanoidPrefabOverride = SerializeGameID("HumanoidPrefabOverrideGameId", _s, this, HumanoidPrefabOverride, (NpcTemplateDefinition == null) ? null : NpcTemplateDefinition.HumanoidPrefabOverride, (NpcTemplateDefinition != null) ? NpcTemplateDefinition.HumanoidPrefabOverride : null);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			public override IdleType GetIdleType(ENpcIdleCategory _category)
			{
				IdleType idleType = IdleType.Invalid;
				if ((int)_category < m_idleTypes.Count)
				{
					idleType = m_idleTypes[(int)_category];
				}
				else if (_category == ENpcIdleCategory.Invalid_ENpcIdleCategory)
				{
				}
				return (idleType != IdleType.Invalid) ? idleType : DefaultIdleType;
			}

			protected sealed override void PostConstructor()
			{
				base.PostConstructor();
				m_Type = new NpcType(base.GameID);
				m_behaviorTreePath = m_StatValueCollection.Get_String(StatTypes.EStatType.Npc_BehaviorTreePath);
				m_combatBehaviorTreePath = m_StatValueCollection.Get_String(StatTypes.EStatType.Npc_CombatBehaviorTreePath);
				m_interactionType = (ENpcInteractionType)m_StatValueCollection.Get_int(StatTypes.EStatType.Npc_InteractionType);
				m_interactionIcon = (IconMaterial)m_StatValueCollection.Get_int(StatTypes.EStatType.Npc_InteractionIcon);
				RunSpeedOverwrite = m_StatValueCollection.Get_float(StatTypes.EStatType.Npc_RunSpeedOverwrite);
				ThreatLevel = m_StatValueCollection.Get_int(StatTypes.EStatType.Enemy_ThreatLevel);
				MapGeneratorData.FillFromStats(this, m_StatValueCollection);
				m_detectionDistances = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.CharacterDetection_DetectionDistances).GetValuesCopy_float();
				m_detectionAngles = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.CharacterDetection_DetectionAngles).GetValuesCopy_float();
				m_detectionAnglesVertical = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.CharacterDetection_DetectionAnglesVertical).GetValuesCopy_float();
				m_notoritiesContinuous = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.CharacterDetection_NotoritiesContinuous).GetValuesCopy_float();
				m_notoritiesInteractions = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.CharacterDetection_NotoritiesInteractions).GetValuesCopy_float();
				CloseRangeAudioDistance = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeAudioDistance);
				CloseRangeFactorStrikingAssasination = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeFactorStrikingAssassination);
				CloseRangeFactorStealthAssasination = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeFactorStealthAssassination);
				CloseRangeFactorCombat = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeFactorCombat);
				CloseRangeFactorStalking = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeFactorStalking);
				CloseRangeDistanceStalking = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_CloseRangeDistanceStalking);
				StayAlarmedMinTime = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_StayAlarmedMinTime);
				MomentOfChaosTime = m_StatValueCollection.Get_float(StatTypes.EStatType.CharacterDetection_MomentOfChaosTime);
				HidingSpotInspectionDistance = m_StatValueCollection.Get_float(StatTypes.EStatType.Enemy_HidingSpotInspectionDistance);
				HidingSpotInspectionMask = (EHidingSpotFlags)m_StatValueCollection.Get_int(StatTypes.EStatType.Enemy_HidingSpotInspectionMask);
				if (UbiCore.Utils.Singleton<NpcData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<NpcData>.Instance.AddDefinition(this);
				}
			}

		}

		public static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(NpcStats.StatsHelpers).Concat(new IStatsHelper[3]
		{
			StatsHelper<EnemyStatsHelper>.Instance,
			StatsHelper<CharacterConflictStatsHelper>.Instance,
			StatsHelper<CharacterDetectionStatsHelper>.Instance
		}).ToArray();

		private readonly BBDictionary<GameID, NpcDefinition> m_DefinitionsByGameID = new BBDictionary<GameID, NpcDefinition>();

		private readonly BBDictionary<string, NpcDefinition> m_DefinitionsByName = new BBDictionary<string, NpcDefinition>();

		private readonly BBList<NpcType> m_Types = new BBList<NpcType>();

		private readonly ReadOnlyCollection<NpcType> m_TypesAccess;

		public ReadOnlyCollection<NpcType> Types
		{
			get
			{
				return m_TypesAccess;
			}
		}

		private NpcDefinition DefaultTemplateDefinition
		{
			get
			{
				return (!IsValid("__DefaultTemplate")) ? null : ((NpcDefinition)GetDefinitionByName("__DefaultTemplate"));
			}
		}

		public NpcData()
		{
			m_TypesAccess = new ReadOnlyCollection<NpcType>(m_Types);
		}

		public INpcDefinition GetDefinitionByName(string _name)
		{
			NpcDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByName.TryGetValue(_name, out value)))
			{
				return value;
			}
			return null;
		}

		public INpcDefinition GetDefinition(NpcType _type)
		{
			NpcDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByGameID.TryGetValue((GameID)_type, out value)))
			{
				return value;
			}
			return null;
		}

		public bool IsValid(NpcType _type)
		{
			return m_DefinitionsByGameID.ContainsKey((GameID)_type);
		}

		public bool IsValid(string _name)
		{
			return m_DefinitionsByName.ContainsKey(_name);
		}

		private void AddDefinition(NpcDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_DefinitionsByGameID.Add(_definition.GameID, _definition);
				m_DefinitionsByName.Add(_definition.Name, _definition);
				if (_definition.Name != "__DefaultTemplate")
				{
					m_Types.Add(_definition.Type);
				}
			}
		}
	}
}
