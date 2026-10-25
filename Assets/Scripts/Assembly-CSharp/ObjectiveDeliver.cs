using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;

public class ObjectiveDeliver : ObjectiveTarget<ObjectiveDeliverData>
{
	private class RelevantUnitData
	{
		public IMissionSpawnEntity MissionEntity { get; private set; }

		public MissionAreaMoving AreaMoving { get; private set; }

		public RelevantUnitData(IMissionSpawnEntity _missionEntity, MissionAreaMoving _areaMoving)
		{
			MissionEntity = _missionEntity;
			AreaMoving = _areaMoving;
		}
	}

	private BBList<EntityId> m_allEntities = new BBList<EntityId>();

	private int m_mimicCount;

	private int m_relevantTargetCount;

	private readonly BBDictionary<EntityId, RelevantUnitData> m_relevantUnitData = new BBDictionary<EntityId, RelevantUnitData>();

	private int m_totalCount;

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.NotStarted;
		}
	}

	private int MimicCount
	{
		get
		{
			return m_mimicCount;
		}
		set
		{
			if (m_mimicCount != value)
			{
				m_mimicCount = value;
				ShowMimicMessage();
			}
		}
	}

	public int TotalCount
	{
		get
		{
			return m_totalCount;
		}
		set
		{
			if (m_totalCount != value)
			{
				m_totalCount = value;
				UpdateTotalCount();
			}
		}
	}

	private ObjectiveDeliver()
	{
	}

	public override void OnActivate()
	{
		m_allEntities.ClearIfNotEmpty();
		m_mimicCount = 0;
		m_totalCount = 0;
		base.OnActivate();
		m_relevantTargetCount = 0;
		for (int i = 0; i < Targets.Count; i++)
		{
			MissionSpawnObject missionSpawnObject = Targets[i] as MissionSpawnObject;
			if (missionSpawnObject == null || !missionSpawnObject.Data.IsMimic)
			{
				m_relevantTargetCount++;
			}
		}
		FindMovingAreas();
		UpdateDescription();
		ActivateInteraction();
	}

	private void ActivateInteraction()
	{
		for (int i = 0; i < m_allEntities.Count; i++)
		{
			EntityId entityId = m_allEntities[i];
			GameEntity gameEntity = entityId.GameEntity;
			InteractionCharacter interactionCharacter = gameEntity as InteractionCharacter;
			if (interactionCharacter != null)
			{
				interactionCharacter.InteractionCharacterStateMachines.CharacterInteraction.ScheduleStateChange(EStateType.CharacterInteraction_Enabled);
				continue;
			}
			InteractableMissionObject interactableMissionObject = gameEntity as InteractableMissionObject;
			if (interactableMissionObject != null)
			{
				interactableMissionObject.InteractableObjectStateMachines.ObjectInteraction.ScheduleStateChange(EStateType.Object_Enabled);
			}
		}
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		foreach (IMissionEntity target in Targets)
		{
			bool flag = !(target is MissionNpcFormation);
		}
	}

	public override string ToString()
	{
		return string.Format("Real: {0}, Fakes: {1}\r\n{2}", m_relevantTargetCount, Targets.Count - m_relevantTargetCount, base.ToString());
	}

	private void FindMovingAreas()
	{
		BBList<IMissionAreaBase> areas = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.GetAreas();
		m_relevantUnitData.Clear();
		foreach (IMissionEntity target in Targets)
		{
			IMissionSpawnEntity missionSpawnEntity = target as IMissionSpawnEntity;
			if (missionSpawnEntity == null)
			{
				continue;
			}
			MissionAreaMoving areaMoving = null;
			foreach (IMissionAreaBase item in areas)
			{
				if (item is MissionAreaMoving && GetMissionObject(((MissionAreaMoving)item).Data.Target) == missionSpawnEntity)
				{
					areaMoving = (MissionAreaMoving)item;
					break;
				}
			}
			m_relevantUnitData[missionSpawnEntity.EntityID] = new RelevantUnitData(missionSpawnEntity, areaMoving);
		}
	}

	public override void FrameUpdateImpl()
	{
		if (IsActive)
		{
			UpdateTargetPositions();
			UpdateDeliveredResult();
			if (Value >= base.RelevantEntities.Count)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
			}
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("count", m_relevantTargetCount);
		return stringMapper;
	}

	protected override bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		m_allEntities.Add(_entity.Id);
		MissionSpawnObject missionSpawnObject = _missionEntity as MissionSpawnObject;
		return missionSpawnObject == null || !missionSpawnObject.Data.IsMimic;
	}

	protected override void OnValueChanged()
	{
		base.OnValueChanged();
		if (Targets.Count > 0)
		{
			Targets[0].PlaySuccessfullInteractionSound();
		}
		UpdateDescription();
	}

	private void ShowMimicMessage()
	{
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.WrongChest);
	}

	private void UpdateDeliveredResult()
	{
		int num = Value;
		int num2 = MimicCount;
		for (int num3 = m_allEntities.Count - 1; num3 >= 0; num3--)
		{
			EntityId entityId = m_allEntities[num3];
			GameEntity gameEntity = entityId.GameEntity;
			if (gameEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
			{
				bool flag = gameEntity is Patrol || gameEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_IsRelevant);
				if (m_allEntities.Remove(entityId))
				{
					if (flag)
					{
						num++;
					}
					else
					{
						num2++;
					}
					entityId.GameEntity.Highlight.Deactivate();
					RelevantUnitData relevantUnitData = m_relevantUnitData[entityId];
					if (relevantUnitData != null)
					{
						relevantUnitData.MissionEntity.SetLastKnownPosition();
						if (!(relevantUnitData.AreaMoving == null))
						{
							m_relevantUnitData.Remove(entityId);
							UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.RemoveMissionEntry(relevantUnitData.AreaMoving);
						}
					}
				}
			}
		}
		MimicCount = num2;
		base.Value = num;
		TotalCount = num2 + num;
	}

	private void UpdateTotalCount()
	{
		if (Targets.Count > 0)
		{
			Targets[0].PlayInteractionSound();
		}
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon == EMapIconType.Invalid_EMapIconType)
		{
			return;
		}
		for (int i = 0; i < Targets.Count; i++)
		{
			GameEntity gameEntity = Targets[i].GameEntity;
			if (!gameEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
			{
				TargetPositions.Add(gameEntity.Position);
			}
		}
	}
}
