using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.LoadoutScreen;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Mission;
using SharedBaseLib.Core;
using UnityEngine;

public class ObjectiveLoot : ObjectiveTarget<ObjectiveLootData>
{
	private BBList<EntityId> m_allEntities = new BBList<EntityId>();

	private int m_mimicCount;

	private int m_relevantTargetCount;

	private readonly HashSet<EntityId> m_identifiedEntities = new HashSet<EntityId>();

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

	private ObjectiveLoot()
	{
	}

	public override void OnActivate()
	{
		m_allEntities.ClearIfNotEmpty();
		base.OnActivate();
		m_relevantTargetCount = 0;
		m_identifiedEntities.Clear();
		m_mimicCount = 0;
		m_relevantTargetCount = 0;
		m_totalCount = 0;
		for (int i = 0; i < Targets.Count; i++)
		{
			MissionSpawnObject missionSpawnObject = Targets[i] as MissionSpawnObject;
			if (missionSpawnObject == null || !missionSpawnObject.Data.IsMimic)
			{
				m_relevantTargetCount++;
			}
		}
		UpdateDescription();
		ActivateInteraction();
		UIMissionGoals.GoalData challengeDescription = MissionMetaDataManager.Instance.GetChallengeDescription(base.Data.ID);
		if (challengeDescription == null || !Main.Instance.IsNetworkEnabled)
		{
			return;
		}
		int id = challengeDescription.Id;
		MissionGoalLoot[] missionGoalLoots = Singleton<StateManager>.Instance.ProfileManager.MissionToken.missionGoalLoots;
		for (int j = 0; j < missionGoalLoots.Length; j++)
		{
			if (missionGoalLoots[j].missionGoalId == id)
			{
				UpdateRelevantHeaderIcon(IconMaterial.InteractLockpick);
			}
		}
	}

	private void ActivateInteraction()
	{
		bool requiresLockPick = false;
		UIMissionGoals.GoalData challengeDescription = MissionMetaDataManager.Instance.GetChallengeDescription(base.Data.ID);
		if (challengeDescription != null && Main.Instance.IsNetworkEnabled)
		{
			int id = challengeDescription.Id;
			MissionGoalLoot[] missionGoalLoots = Singleton<StateManager>.Instance.ProfileManager.MissionToken.missionGoalLoots;
			for (int i = 0; i < missionGoalLoots.Length; i++)
			{
				if (missionGoalLoots[i].missionGoalId == id)
				{
					requiresLockPick = true;
					break;
				}
			}
		}
		for (int j = 0; j < m_allEntities.Count; j++)
		{
			EntityId entityId = m_allEntities[j];
			InteractableMissionObject entity = entityId.GetEntity<InteractableMissionObject>();
			if (entity != null)
			{
				entity.InteractableObjectStateMachines.ObjectInteraction.ScheduleStateChange(EStateType.Object_Enabled);
				entity.InteractableObjectStateMachines.ObjectInteraction.RequiresLockPick = requiresLockPick;
			}
		}
	}

	private void UpdateRelevantHeaderIcon(IconMaterial _materialType)
	{
		for (int i = 0; i < base.RelevantEntities.Count; i++)
		{
			InteractableObject entity = base.RelevantEntities[i].GetEntity<InteractableObject>();
			if (entity.InteractableObjectDefinition.Definition.SubCategory == EObjectSubCategory.Chest)
			{
				entity.HeadIcon.IconAttachment.SetIcon(_materialType, 5);
			}
		}
	}

	public override string ToString()
	{
		return string.Format("Real: {0}, Fakes: {1}\r\n{2}", m_relevantTargetCount, Targets.Count - m_relevantTargetCount, base.ToString());
	}

	public override void FrameUpdateImpl()
	{
		if (IsActive)
		{
			UpdateTargetPositions();
			UpdateDeliveredResult();
			if (PlayerHelper.IsUsingEagleEye)
			{
				DetectTargets();
			}
			if (Value >= base.RelevantEntities.Count)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
			}
		}
	}

	public InteractableObject GetInteractableObjectChallengeLootChest()
	{
		MissionGoalLoot _goalLoot = null;
		return GetChallengeLootChest(ref _goalLoot);
	}

	public InteractableObject GetChallengeLootChest(ref MissionGoalLoot _goalLoot)
	{
		UIMissionGoals.GoalData challengeDescription = MissionMetaDataManager.Instance.GetChallengeDescription(base.Data.ID);
		if (challengeDescription == null)
		{
			return null;
		}
		int id = challengeDescription.Id;
		MissionGoalLoot[] missionGoalLoots = Singleton<StateManager>.Instance.ProfileManager.MissionToken.missionGoalLoots;
		foreach (MissionGoalLoot missionGoalLoot in missionGoalLoots)
		{
			if (missionGoalLoot.missionGoalId != id)
			{
				continue;
			}
			for (int j = 0; j < base.RelevantEntities.Count; j++)
			{
				InteractableObject entity = base.RelevantEntities[j].GetEntity<InteractableObject>();
				if (entity.InteractableObjectDefinition.Definition.SubCategory == EObjectSubCategory.Chest)
				{
					_goalLoot = missionGoalLoot;
					return entity;
				}
			}
			break;
		}
		return null;
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Remove("count");
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

	protected override void ChallengeSuccessChanged(bool _success)
	{
		base.ChallengeSuccessChanged(_success);
		if (_success)
		{
			MissionGoalLoot _goalLoot = null;
			InteractableObject challengeLootChest = GetChallengeLootChest(ref _goalLoot);
			if (challengeLootChest != null)
			{
				LogicMain.MissionResult.IncreaseTreasureChestsOpenedInMission();
				LogicMain.MissionResult.IncreaseConsumedLockPicks(EntitlementExtensions.GetHeroicRankMapping(Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank).requiredLockPicks);
				LogicMain.LogicInterface.Combat_MissionGoalLoot(challengeLootChest, _goalLoot);
			}
		}
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
			if (!gameEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered) && (!base.Data.IdentityActivatesMapIcon || m_identifiedEntities.Contains(gameEntity.Id)))
			{
				TargetPositions.Add(gameEntity.Position);
			}
		}
	}

	private void DetectTargets()
	{
		for (int i = 0; i < m_allEntities.Count; i++)
		{
			EntityId entityId = m_allEntities[i];
			if (m_identifiedEntities.Contains(entityId))
			{
				continue;
			}
			if (!entityId.IsValid)
			{
				m_identifiedEntities.Add(entityId);
				continue;
			}
			GameObjectEntity gameObjectEntity = entityId.GameObjectEntity;
			if (gameObjectEntity.IsAttached)
			{
				GameObject instance = gameObjectEntity.GameObject;
				if (LogicMain.EntityTracking.IsInLineOfSight(instance, true, 0f))
				{
					m_identifiedEntities.Add(entityId);
				}
			}
		}
	}
}
