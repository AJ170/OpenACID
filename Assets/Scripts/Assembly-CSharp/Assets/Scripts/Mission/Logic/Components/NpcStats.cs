using UnityEngine.AI;
using System;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class NpcStats : CharacterStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(CharacterStats.StatsHelpers).Concat(new IStatsHelper[2]
		{
			StatsHelper<NpcStatsHelper>.Instance,
			StatsHelper<MapGeneratorStatsHelper>.Instance
		}).ToArray();

		private Assets.Scripts.Mission.Logic.Entities.Patrol m_Patrol;

		public Assets.Scripts.Mission.Logic.Entities.Patrol Patrol
		{
			get
			{
				return m_Patrol;
			}
			private set
			{
				if (m_Patrol != value)
				{
					if (m_Patrol != null)
					{
						m_Patrol.Remove(Entity.Id, false);
					}
					m_Patrol = value;
				}
			}
		}

		protected NpcStats(IEntity _entity, IStatsHelper[] _statHelpers)
			: base(_entity, _statHelpers)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			AddListener(OnStatValueChanged);
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			RemoveListener(OnStatValueChanged);
			Patrol = null;
		}

		public override void OnActivate()
		{
			base.OnActivate();
			float val = LogicMain.GameStats.Get_float(StatTypes.EStatType.Game_MaxDetectionRange);
			LogicMain.GameStats.Set_float(StatTypes.EStatType.Game_MaxDetectionRange, Math.Max(val, ((NpcDefinition)Entity.Definition).Definition.DetectionDistances[2]));
		}

		protected void OnStatValueChanged(IReadonlyStatValue _statValue, IStatValueWrapper _previousValue)
		{
			if (_statValue.StatType == StatTypes.EStatType.Npc_PatrolId)
			{
				EntityId value_EntityId = _statValue.Value_EntityId;
				if (value_EntityId == EntityId.InvalidId)
				{
					Patrol = null;
				}
				else
				{
					Patrol = value_EntityId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
				}
			}
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			NpcCharacter npcCharacter = (NpcCharacter)Entity;
			int avoidancePriority = ((npcCharacter.NpcDefinition.EntityType != EntityTypes.EType.EnemyCharacter) ? 75 : 50);
			NpcData.NpcDefinition.ENpcInteractionType interactionType = npcCharacter.NpcDefinition.Definition.InteractionType;
			if (interactionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant || interactionType == NpcData.NpcDefinition.ENpcInteractionType.Mission)
			{
				avoidancePriority = 25;
			}
			npcCharacter.UnityComponentCache.NavMeshAgent.avoidancePriority = avoidancePriority;
		}
	}
}
