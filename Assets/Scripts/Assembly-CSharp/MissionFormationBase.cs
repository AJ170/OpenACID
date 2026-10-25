using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public abstract class MissionFormationBase<TData> : MissionBase<TData>, IMissionBase, IMissionEntity, IMissionFormationBase where TData : MissionFormationBaseData, new()
{
	private int m_lastChildCount;

	private MissionWaypoint[] m_waypoints;

	public MissionWaypoint[] Waypoints
	{
		get
		{
			return m_waypoints;
		}
	}

	public abstract EntityId EntityID { get; protected set; }

	public GameEntity GameEntity
	{
		get
		{
			return EntityID.GameEntity;
		}
	}

	public Vector3? LastKnownPosition { get; private set; }

	public float ReleaseTime { get; private set; }

	protected MissionFormationBase()
	{
		EntityID = EntityId.InvalidId;
	}

	public override void OnPreActivate()
	{
		ScanForWaypoints();
		base.OnPreActivate();
	}

	public abstract void OnWaypointLoaded(MissionWaypointData _waypoint);

	public abstract void OnWaypointUnload(MissionWaypointData _waypoint);

	public void ScanForWaypoints()
	{
		m_waypoints = base.gameObject.GetComponentsInChildren<MissionWaypoint>();
	}

	public void SetLastKnownPosition()
	{
		if (!LastKnownPosition.HasValue)
		{
			LastKnownPosition = GameEntity.Position;
			ReleaseTime = LogicMain.CurrentTime;
		}
	}

	public void PlayInteractionSound()
	{
	}

	public void PlaySuccessfullInteractionSound()
	{
	}
}
