using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveSpawn : ObjectiveBase<ObjectiveSpawnData>
{
	private int m_spawnFrameIndex = -1;

	private float m_elapsedTime;

	private List<MissionTarget> m_spawnSpots = new List<MissionTarget>();

	private MissionTarget m_destination;

	private BBList<EntityId> m_forcedUpdateUnits = new BBList<EntityId>();

	private ObjectiveSpawnData.SpawnFrame CurrentFrame
	{
		get
		{
			return base.Data.SpawnFrames[m_spawnFrameIndex];
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Fulfilled;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return true;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.Fulfilled;
		}
	}

	private ObjectiveSpawn()
	{
	}

	public override void FrameUpdateImpl()
	{
		base.FrameUpdateImpl();
		if (m_spawnFrameIndex < 0)
		{
			m_elapsedTime = 0f;
			m_spawnFrameIndex = 0;
			return;
		}
		m_elapsedTime += LogicMain.FrameTime;
		if (base.Data.SpawnFrames.Length > m_spawnFrameIndex && m_elapsedTime >= base.Data.SpawnFrames[m_spawnFrameIndex].Delay)
		{
			SpawnWave();
			m_elapsedTime = 0f;
			m_spawnFrameIndex++;
		}
	}

	private void SpawnWave()
	{
		m_spawnSpots = ReadMissionEntities<MissionTarget>(CurrentFrame.SpawnPoints);
		m_destination = ReadMissionEntity<MissionTarget>(CurrentFrame.DestinationSpot);
		for (int i = 0; i < m_spawnSpots.Count; i++)
		{
			MissionTarget source = m_spawnSpots[i];
			ConstructUnit(source, i);
		}
	}

	private void ConstructUnit(MissionTarget _source, int _index)
	{
		GameObject gameObject = new GameObject("Wave_" + m_spawnFrameIndex + "_Unit_" + _index);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = _source.Position;
		Vector3 forward = (gameObject.transform.position.To2D() - m_destination.transform.position.To2D()).normalized.To3D(0f);
		gameObject.transform.rotation = Quaternion.LookRotation(forward);
		MissionNpcFormation missionNpcFormation = gameObject.AddComponent<MissionNpcFormation>();
		missionNpcFormation.Data.ActivateAtId = base.Data.ActivateAtId;
		missionNpcFormation.Data.IterationDirection = EFormationIterationDirection.Up;
		missionNpcFormation.Data.IterationType = EFormationIterationType.BackAndForth;
		missionNpcFormation.Data.Type = EPatrolType.Military;
		MissionNpcSpawn missionNpcSpawn = gameObject.AddComponent<MissionNpcSpawn>();
		missionNpcSpawn.Data.ActivateAtId = base.Data.ActivateAtId;
		missionNpcSpawn.Data.ClampMapIconOnBorder = true;
		missionNpcSpawn.Data.MapIcon = base.Data.MapIcon;
		missionNpcSpawn.Data.TypeGuid = CurrentFrame.Guid;
		m_forcedUpdateUnits.Add(missionNpcSpawn.EntityID);
		Helper.DemandNavAgentSurrogateFrameUpdate(missionNpcSpawn.EntityID, true);
		MissionWaypoint missionWaypoint = CreateWaypoint(_source, gameObject);
		MissionWaypoint missionWaypoint2 = CreateWaypoint(m_destination, gameObject, -1);
		UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.AddMissionEntries(new IMissionBase[4] { missionNpcSpawn, missionNpcFormation, missionWaypoint, missionWaypoint2 });
	}

	private MissionWaypoint CreateWaypoint(MissionTarget _source, GameObject _unit, int _duration = 0)
	{
		GameObject gameObject = new GameObject("Waypoint");
		gameObject.transform.parent = _unit.transform;
		gameObject.transform.position = _source.Position;
		MissionWaypoint missionWaypoint = gameObject.AddComponent<MissionWaypoint>();
		missionWaypoint.Data.ActivateAtId = base.Data.ActivateAtId;
		missionWaypoint.Data.ActivationIterationStartType = ERouteIterationStart.Nearest_ERouteIterationStart;
		missionWaypoint.Data.ActivationIterationType = EFormationIterationType.BackAndForth;
		missionWaypoint.Data.IdleCategory = ENpcIdleCategory.NpcTypeDefault;
		missionWaypoint.Data.IdleTime = _duration;
		missionWaypoint.Data.RotationType = ERotationAtWaypoint.RotationOfWaypoint;
		missionWaypoint.Data.MinSpeed = EMovementSpeed.Sprint;
		missionWaypoint.Data.MaxSpeed = EMovementSpeed.Sprint;
		return missionWaypoint;
	}

	public override void OnActivate()
	{
		base.OnActivate();
		LogicMain.EntityManager.OnEntityUnregistered += EntityUnregistered;
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		for (int num = m_forcedUpdateUnits.Count - 1; num >= 0; num--)
		{
			UnregisterEntity(m_forcedUpdateUnits[num]);
		}
	}

	private void EntityUnregistered(IEntity _sender)
	{
		EntityId id = _sender.Id;
		UnregisterEntity(id);
	}

	private void UnregisterEntity(EntityId _entityId)
	{
		if (m_forcedUpdateUnits.Remove(_entityId))
		{
			Helper.DemandNavAgentSurrogateFrameUpdate(_entityId, false);
		}
	}

	public override string ToString()
	{
		return "Spawning waves: " + base.Data.SpawnFrames;
	}
}
