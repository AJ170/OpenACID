using System;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class PatrolData : UbiCore.Utils.Singleton<PatrolData>
	{
		public interface IPatrolDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IReadonlyStatValueCollection
		{
			EPatrolType Type { get; }

			EFormationType FormationType { get; }

			float MaxExtend { get; }

			float MaxRandomDeviation { get; }

			int MaxCount { get; }

			EInteractionReason InteractionReason { get; }

			UnityResource AudioSourcePrefab { get; }
		}

		[Database("PatrolDefinition")]
		public class PatrolDefinition : EntityData.EntityDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IPatrolDefinition, IReadonlyStatValueCollection
		{
			private EPatrolType m_type;

			private EFormationType m_formationType;

			private int m_maxCount;

			private float m_maxExtend;

			private float m_maxRandomDeviation;

			private EInteractionReason m_interactionReason;

			private UnityResource m_audioSourcePrefab;

			public EPatrolType Type
			{
				get
				{
					return m_type;
				}
			}

			public EFormationType FormationType
			{
				get
				{
					return m_formationType;
				}
			}

			public float MaxExtend
			{
				get
				{
					return m_maxExtend;
				}
			}

			public float MaxRandomDeviation
			{
				get
				{
					return m_maxRandomDeviation;
				}
			}

			public int MaxCount
			{
				get
				{
					return m_maxCount;
				}
			}

			public EInteractionReason InteractionReason
			{
				get
				{
					return m_interactionReason;
				}
			}

			public UnityResource AudioSourcePrefab
			{
				get
				{
					return m_audioSourcePrefab;
				}
			}

			public override Type InstanceType
			{
				get
				{
					return typeof(Assets.Scripts.Mission.Logic.Entities.Patrol);
				}
			}

			public override EntityTypes.EType EntityType
			{
				get
				{
					return EntityTypes.EType.Patrol;
				}
			}

			public PatrolDefinition(AssetBundleInfo _assetBundleInfo)
				: base(PatrolStats.StatsHelpers, _assetBundleInfo)
			{
				m_audioSourcePrefab = new UnityResource(_assetBundleInfo);
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				if (_s.Mode == EMode.EMode_Read)
				{
					m_type = (EPatrolType)(int)Enum.Parse(typeof(EPatrolType), base.Name);
					m_formationType = (EFormationType)m_StatValueCollection.Get_int(StatTypes.EStatType.Patrol_FormationType);
					m_maxCount = m_StatValueCollection.Get_int(StatTypes.EStatType.Patrol_MaxCount);
					m_maxExtend = m_StatValueCollection.Get_float(StatTypes.EStatType.Patrol_MaxExtend);
					m_maxRandomDeviation = m_StatValueCollection.Get_float(StatTypes.EStatType.Patrol_MaxRandomDeviation);
					m_interactionReason = (EInteractionReason)m_StatValueCollection.Get_int(StatTypes.EStatType.Patrol_InteractionReason);
					if (UbiCore.Utils.Singleton<PatrolData>.IsAvailable)
					{
						UbiCore.Utils.Singleton<PatrolData>.Instance.AddDefinition(this);
					}
				}
				AudioSourcePrefab.Serialize("AudioSourcePrefab", _s, base.AssetBundleInfo);
			}
		}

		private readonly PatrolDefinition[] m_Definitions = new PatrolDefinition[Enum.GetValues(typeof(EPatrolType)).Length];

		public IPatrolDefinition GetDefinition(EPatrolType _type)
		{
			if (_type == EPatrolType.Invalid_EPatrolType)
			{
				return null;
			}
			return m_Definitions[(int)_type];
		}

		public static bool WillHidingLimitFailOnIncrease(EPatrolType _patrolType)
		{
			EHotspot _hotspotType;
			ELimitType _limitType;
			return Assets.Scripts.Mission.Logic.Components.MissionResult.ConvertTrackedHotspotType(_patrolType, out _hotspotType, out _limitType) && Assets.Scripts.Mission.Logic.Components.MissionResult.WillHidingLimitFailOnIncrease(_limitType);
		}

		private void AddDefinition(PatrolDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_Definitions[(int)_definition.Type] = _definition;
			}
		}
	}
}
