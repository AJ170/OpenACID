using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using SharedBaseLib.Core;

public class ObjectiveInteraction : ObjectiveTarget<ObjectiveInteractionData>
{
	private BBDictionary<EntityId, IMissionEntity> m_missionEntityMapping = new BBDictionary<EntityId, IMissionEntity>();

	private BBList<EntityId> m_allEntities = new BBList<EntityId>();

	private int m_interactionCounter;

	public int InteractionCounter
	{
		get
		{
			return m_interactionCounter;
		}
		set
		{
			if (value != m_interactionCounter)
			{
				m_interactionCounter = value;
				UpdateDescription();
			}
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.NotStarted;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override EObjectiveResult InitialResult
	{
		get
		{
			return EObjectiveResult.Undefined;
		}
	}

	private ObjectiveInteraction()
	{
	}

	public override void FrameUpdateImpl()
	{
		UpdateInteractionResult();
		UpdateTargetPositions();
		if (m_allEntities.Count == 0)
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.VisualSummaryState = true;
			base.Result = EObjectiveResult.Fulfilled;
			Complete();
			return;
		}
		for (int i = 0; i < m_allEntities.Count; i++)
		{
			bool flag = false;
			Patrol patrol = (Patrol)m_allEntities[i].GameEntity;
			if (patrol.CaptiveComponent != null && patrol.CaptiveComponent.IsVictimHit)
			{
				flag = true;
			}
			else
			{
				for (int j = 0; j < patrol.PatrolComponent.Formation.Entities.Count; j++)
				{
					NpcCharacter entity = patrol.PatrolComponent.Formation.Entities[j].m_id.TryGetEntity<NpcCharacter>();
					if (entity.GetIsDead())
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				base.VisualTrackerState = EProgressState.Failed;
				base.VisualSummaryState = true;
				base.Result = EObjectiveResult.Failed;
				Complete();
			}
		}
	}

	public override void OnActivate()
	{
		m_allEntities.ClearIfNotEmpty();
		m_missionEntityMapping.Clear();
		base.OnActivate();
		m_interactionCounter = 0;
		ActivateInteraction();
	}

	private void ActivateInteraction()
	{
		for (int i = 0; i < m_allEntities.Count; i++)
		{
			EntityId entityId = m_allEntities[i];
			GameEntity gameEntity = entityId.GameEntity;
			Patrol patrol = gameEntity as Patrol;
			if (patrol != null)
			{
				patrol.EntityStats.Set_bool(StatTypes.EStatType.Patrol_Enabled, true);
			}
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Remove("current");
		stringMapper.Remove("count");
		stringMapper.Add("current", InteractionCounter);
		stringMapper.Add("count", base.RelevantEntities.Count);
		return stringMapper;
	}

	protected override bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		bool flag = _entity is Patrol;
		if (flag)
		{
			m_allEntities.Add(_entity.Id);
			m_missionEntityMapping.Add(_entity.Id, _missionEntity);
		}
		return flag;
	}

	private void UpdateInteractionResult()
	{
		for (int num = m_allEntities.Count - 1; num >= 0; num--)
		{
			EntityId entityId = m_allEntities[num];
			if (entityId.GameEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
			{
				if (m_allEntities.Remove(entityId))
				{
					InteractionCounter++;
				}
				IMissionEntity missionEntity = m_missionEntityMapping[entityId];
				missionEntity.SetLastKnownPosition();
			}
		}
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0)
		{
			for (int i = 0; i < m_allEntities.Count; i++)
			{
				EntityId entityId = m_allEntities[i];
				if (entityId != EntityId.InvalidId)
				{
					TargetPositions.Add(entityId.GameEntity.Position);
				}
			}
		}
		base.Value = base.Data.Targets.Count - TargetPositions.Count;
	}
}
