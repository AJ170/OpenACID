using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveIdentify : ObjectiveTarget<ObjectiveIdentifyData>
{
	private readonly HashSet<EntityId> m_identifiedEntities = new HashSet<EntityId>();

	private readonly BBDictionary<IMissionEntity, MissionArea> m_targetAreaMap = new BBDictionary<IMissionEntity, MissionArea>();

	private readonly List<EntityId> m_trackedTarget = new List<EntityId>();

	public BBDictionary<IMissionEntity, MissionArea> TargetAreaMap
	{
		get
		{
			return m_targetAreaMap;
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

	private ObjectiveIdentify()
	{
	}

	public override void FrameUpdateImpl()
	{
		UpdateTargetPositions();
		if (PlayerHelper.IsUsingEagleEye || base.Data.UsePlainSight)
		{
			DetectTargets();
		}
		if (base.Data.FailIfTargetDies && IsAnyTargetInvalid())
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = EObjectiveResult.Failed;
			Complete();
		}
		else if (AreSufficientTargetsIdentified())
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.VisualSummaryState = true;
			base.Result = EObjectiveResult.Fulfilled;
			Complete();
		}
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		for (int i = 0; i < m_trackedTarget.Count; i++)
		{
			if (m_trackedTarget[i].IsValid && !m_trackedTarget[i].GetEntity<EnemyCharacter>().GetIsDead())
			{
				TargetPositions.Add(m_trackedTarget[i].GameObjectEntity.Position);
			}
		}
	}

	protected override bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		bool flag = _entity is Character || _entity is StaticObject;
		if (!flag)
		{
		}
		return flag && base.IsEntityToBeRegistered(_entity, _missionEntity);
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		IList<MissionArea> list = ((base.Data.TargetAreas == null || base.Data.TargetAreas.Count <= 0) ? new MissionArea[Targets.Count] : ((base.Data.TargetAreas.Count == base.Data.Targets.Count) ? ((IList<MissionArea>)ReadMissionEntities<MissionArea>(base.Data.TargetAreas, true)) : ((IList<MissionArea>)new MissionArea[Targets.Count])));
		for (int i = 0; i < Targets.Count; i++)
		{
			TargetAreaMap[Targets[i]] = list[i];
		}
		m_identifiedEntities.Clear();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_identifiedEntities.Clear();
		m_trackedTarget.ClearIfNotEmpty();
	}

	private bool AreSufficientTargetsIdentified()
	{
		int num = 0;
		for (int i = 0; i < Targets.Count; i++)
		{
			EntityId entityID = Targets[i].EntityID;
			if (m_identifiedEntities.Contains(entityID))
			{
				num++;
			}
			else if (entityID == EntityId.InvalidId)
			{
				num++;
			}
			else if (!entityID.IsValid)
			{
				SetEntityDetected(entityID);
			}
			else if (entityID.Type == EntityTypes.EType.EnemyCharacter)
			{
				EnemyCharacter enemyCharacter = entityID.TryGetEntity<EnemyCharacter>();
				if (enemyCharacter.GetIsDead())
				{
					SetEntityDetected(enemyCharacter.Id);
				}
			}
		}
		return (base.Data.Minimum != 0) ? (num >= base.Data.Minimum) : (num >= Targets.Count);
	}

	private bool IsAnyTargetInvalid()
	{
		for (int i = 0; i < Targets.Count; i++)
		{
			IMissionEntity missionEntity = Targets[i];
			if ((MonoBehaviour)missionEntity == null)
			{
				return true;
			}
			if (!missionEntity.EntityID.IsValid)
			{
				return true;
			}
			EnemyCharacter enemyCharacter = base.RelevantEntities[i].TryGetEntity<EnemyCharacter>();
			if (enemyCharacter != null && enemyCharacter.GetIsDead())
			{
				return true;
			}
		}
		return false;
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current", m_identifiedEntities.Count);
		stringMapper["count"] = ((base.Data.Minimum == 0) ? base.Data.Targets.Count : base.Data.Minimum);
		return stringMapper;
	}

	private void DetectTargets()
	{
		for (int i = 0; i < base.RelevantEntities.Count; i++)
		{
			EntityId entityId = base.RelevantEntities[i];
			if (m_identifiedEntities.Contains(entityId))
			{
				continue;
			}
			if (!entityId.IsValid)
			{
				SetEntityDetected(entityId);
				continue;
			}
			GameObjectEntity gameObjectEntity = entityId.GameObjectEntity;
			if (!gameObjectEntity.IsAttached)
			{
				continue;
			}
			if (entityId.GameEntity is Character)
			{
				if (LogicMain.EntityTracking.IsCharacterInLineOfSight(entityId, true, 1f))
				{
					SetEntityDetected(entityId);
				}
				continue;
			}
			GameObject instance = gameObjectEntity.GameObject;
			if (LogicMain.EntityTracking.IsInLineOfSight(instance, true, 1f))
			{
				SetEntityDetected(entityId);
			}
		}
	}

	private void SetEntityDetected(EntityId _relevantEntity)
	{
		if (m_identifiedEntities.Contains(_relevantEntity))
		{
			return;
		}
		m_identifiedEntities.Add(_relevantEntity);
		base.Value = Value + 1;
		MissionArea missionArea = null;
		foreach (KeyValuePair<IMissionEntity, MissionArea> item in m_targetAreaMap)
		{
			if (item.Key.EntityID != _relevantEntity)
			{
				continue;
			}
			missionArea = item.Value;
			break;
		}
		foreach (KeyValuePair<IMissionEntity, MissionArea> item2 in m_targetAreaMap)
		{
			if (item2.Key.EntityID != _relevantEntity && item2.Value == missionArea && !m_identifiedEntities.Contains(item2.Key.EntityID))
			{
				missionArea = null;
			}
		}
		if (_relevantEntity != null)
		{
			EnemyCharacter enemyCharacter = _relevantEntity.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter != null && !enemyCharacter.GetIsDead())
			{
				m_trackedTarget.Add(_relevantEntity);
			}
			else
			{
				m_trackedTarget.Remove(_relevantEntity);
			}
			StaticObject staticObject = _relevantEntity.GameEntity as StaticObject;
			if (staticObject != null)
			{
				staticObject.Highlight.PlayAppearEffect();
			}
		}
		if (missionArea != null)
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.RemoveMissionEntry(missionArea);
		}
	}
}
