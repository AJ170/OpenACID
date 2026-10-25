using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using SharedBaseLib.Core;

public abstract class ObjectiveTarget<TData> : ObjectiveBase<TData>, IObjectiveTarget, IObjectiveValue where TData : ObjectiveTargetData, new()
{
	private readonly BBList<EntityId> m_relevantEntities = new BBList<EntityId>();

	private BBList<IMissionEntity> m_targets = new BBList<IMissionEntity>();

	private int m_value;

	public List<EntityId> RelevantEntities
	{
		get
		{
			return m_relevantEntities;
		}
	}

	public List<IMissionEntity> Targets
	{
		get
		{
			return m_targets;
		}
	}

	public int Value
	{
		get
		{
			return m_value;
		}
		protected set
		{
			if (m_value != value)
			{
				m_value = value;
				UpdateDescription();
				OnValueChanged();
			}
		}
	}

	public event ValueChangedDelegate ValueChanged;

	public override void OnActivate()
	{
		base.OnActivate();
		RelevantEntities.ClearIfNotEmpty();
		m_value = 0;
		for (int i = 0; i < m_targets.Count; i++)
		{
			RegisterGameEntity(m_targets[i]);
		}
		UpdateDescription();
		MissionSpawnEntity.EntitySpawned += MissionSpawnEntityOnEntitySpawned;
	}

	private void MissionSpawnEntityOnEntitySpawned(IMissionSpawnEntity _spawnEntity)
	{
		if (m_targets.Contains(_spawnEntity) && RegisterGameEntity(_spawnEntity))
		{
			UpdateDescription();
		}
	}

	public override void OnDeactivate()
	{
		for (int i = 0; i < RelevantEntities.Count; i++)
		{
			EntityId entityId = RelevantEntities[i];
			Helper.DemandNavAgentSurrogateFrameUpdate(entityId, false);
			if (!(entityId.Entity is Patrol) && base.Data.IsVisibleInEagleVision)
			{
				LogicMain.PlayerEntity.Vision.RemoveEntity(entityId);
			}
		}
		RelevantEntities.Clear();
		MissionSpawnEntity.EntitySpawned -= MissionSpawnEntityOnEntitySpawned;
		base.OnDeactivate();
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		m_targets.Clear();
		BBList<IMissionEntity> targets = m_targets;
		TData data = base.Data;
		targets.AddRange(ReadMissionEntities<IMissionEntity>(data.Targets));
	}

	public override string ToString()
	{
		return string.Format("Targets:\n{0}", ObjectiveBase<TData>.FormatEntityList(Targets));
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current", Value);
		TData data = base.Data;
		stringMapper.Add("count", data.Targets.Count);
		return stringMapper;
	}

	protected override bool IsConditionToBeRemoved(string _statusText, bool _inverse)
	{
		if (_statusText != "Multi")
		{
			return base.IsConditionToBeRemoved(_statusText, _inverse);
		}
		TData data = base.Data;
		bool flag = data.Targets.Count <= 1;
		if (_inverse)
		{
			flag = !flag;
		}
		return flag;
	}

	protected virtual bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		return true;
	}

	protected virtual void OnValueChanged()
	{
		ValueChangedDelegate valueChanged = this.ValueChanged;
		if (valueChanged != null)
		{
			valueChanged();
		}
	}

	private bool RegisterGameEntity(IMissionEntity _missionEntity)
	{
		if (_missionEntity == null)
		{
			return false;
		}
		EntityId entityID = _missionEntity.EntityID;
		if (!entityID.IsValid)
		{
			return false;
		}
		GameEntity gameEntity = entityID.GameEntity;
		if (gameEntity == null)
		{
			return false;
		}
		if (!IsEntityToBeRegistered(gameEntity, _missionEntity))
		{
			return false;
		}
		if (RelevantEntities.Contains(entityID))
		{
			return false;
		}
		RelevantEntities.Add(entityID);
		Helper.DemandNavAgentSurrogateFrameUpdate(gameEntity, true);
		if (!(gameEntity is Patrol) && base.Data.IsVisibleInEagleVision)
		{
			LogicMain.PlayerEntity.Vision.AddEntity(entityID);
		}
		return true;
	}
}
