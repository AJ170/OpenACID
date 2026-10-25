using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveParkour : ObjectiveBase<ObjectiveParkourData>, IObjectiveTimer, IObjectiveValue
{
	private class ChallengerData
	{
		public MissionNpcFormation Formation { get; set; }

		public Assets.Scripts.Mission.Logic.Components.Patrol PatrolComponent { get; set; }

		public List<MissionWaypoint> Waypoints { get; set; }
	}

	private const int c_CountdownDuration = 3;

	public List<MissionNpcSpawn> Challengers = new List<MissionNpcSpawn>();

	[StaticObjectTypeGuid]
	public int WaypointMarker;

	public List<MissionTarget> Waypoints = new List<MissionTarget>();

	private Dictionary<MissionNpcSpawn, ChallengerData> m_challengerContent = new Dictionary<MissionNpcSpawn, ChallengerData>();

	private bool m_countdownStarted;

	private StaticObjectData.IStaticObjectDefinition m_entityDefinition;

	private int m_furthestReachedChallengerWaypoint;

	private HighlightBehaviour m_marker;

	private double m_startTime = double.MaxValue;

	private int m_value;

	public bool ChallengerReachedLastWaypoint
	{
		get
		{
			for (int i = 0; i < Challengers.Count; i++)
			{
				MissionNpcSpawn key = Challengers[i];
				Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = m_challengerContent[key].PatrolComponent;
				Vector3 vector = patrolComponent.Formation.CalcCurrentMemberCenter();
				Vector3 vector2 = LastWaypoint.Position - vector;
				if (FurthestReachedChallengerWaypoint == Waypoints.Count - 1 && LastWaypoint.Data.Range * LastWaypoint.Data.Range >= vector2.sqrMagnitude)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool HasTimer
	{
		get
		{
			return VisualTrackerState == EProgressState.NotStarted && GetTime() >= 0;
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
				OnValueChanged();
			}
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.NotStarted;
		}
	}

	private MissionTarget CurrentWaypoint
	{
		get
		{
			if (Value >= Waypoints.Count)
			{
				return null;
			}
			return Waypoints[Value];
		}
	}

	private MissionTarget LastWaypoint
	{
		get
		{
			return Waypoints[Waypoints.Count - 1];
		}
	}

	private bool ReachedCurrentWaypoint
	{
		get
		{
			Vector3 position = LogicMain.Player.transform.position;
			Vector3 vector = CurrentWaypoint.Position - position;
			return CurrentWaypoint.Data.Range * CurrentWaypoint.Data.Range >= vector.sqrMagnitude;
		}
	}

	private int FurthestReachedChallengerWaypoint
	{
		get
		{
			return m_furthestReachedChallengerWaypoint;
		}
		set
		{
			if (m_furthestReachedChallengerWaypoint != value)
			{
				m_furthestReachedChallengerWaypoint = value;
				OnValueChanged();
				UpdateDescription();
			}
		}
	}

	public event ValueChangedDelegate ValueChanged;

	private ObjectiveParkour()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7746, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void FrameUpdateImpl()
	{
		if (!IsActive)
		{
			return;
		}
		UpdateTargetPositions();
		switch (VisualTrackerState)
		{
		case EProgressState.NotStarted:
			if (ReachedCurrentWaypoint && !m_countdownStarted)
			{
				StartCountdown();
			}
			else if (ReachedCurrentWaypoint && m_countdownStarted && GetTime() <= 0)
			{
				StartMatch();
			}
			break;
		case EProgressState.InProgress:
			if (ReachedCurrentWaypoint)
			{
				Value += 1;
				if (CurrentWaypoint != null)
				{
					m_marker.transform.position = CurrentWaypoint.Position;
				}
				else
				{
					m_marker.Activate(false);
				}
				UpdateDescription();
			}
			if (CurrentWaypoint == null)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
				break;
			}
			FurthestReachedChallengerWaypoint = ComputeFurthestChallengerWaypoint();
			if (ChallengerReachedLastWaypoint)
			{
				base.VisualTrackerState = EProgressState.Failed;
				base.Result = Mission.EObjectiveResult.Failed;
				Complete();
			}
			break;
		}
	}

	public int GetTime()
	{
		return 3 - Mathf.RoundToInt((float)((double)LogicMain.CurrentTime - m_startTime));
	}

	public override void OnActivate()
	{
		base.OnActivate();
		SetupChallengers();
		if (Waypoints.Count > 0)
		{
			m_marker = SetupMarker(Waypoints[0].transform);
		}
		UpdateDescription();
	}

	public override void OnDeactivate()
	{
		AudioManager.Instance.EventManager.OnLeaveParkour();
		LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_SprintInHighProfileOverride, false);
		RemoveAllWaypoints();
		base.OnDeactivate();
	}

	public override void OnPostRead(MissionBaseData _nodeContent)
	{
		base.OnPostRead(_nodeContent);
		StaticObjectData.ObjectType objectType = MissionFactory.StaticObjectFromGuid(base.Data.TypeGuid);
		if (objectType != null)
		{
			m_entityDefinition = UbiCore.Utils.Singleton<StaticObjectData>.Instance.GetDefinition(objectType);
			WaypointMarker = objectType.GameID.Guid;
		}
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		Challengers = ReadMissionEntities<MissionNpcSpawn>(base.Data.Challengers);
		Waypoints = ReadMissionEntities<MissionTarget>(base.Data.Waypoints);
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current_waypoint", Value);
		stringMapper.Add("waypoint_count", Waypoints.Count);
		stringMapper.Add("challenger_waypoint", FurthestReachedChallengerWaypoint);
		stringMapper.Add("Current", Value);
		stringMapper.Add("Count", Waypoints.Count);
		stringMapper.Add("Threshold", FurthestReachedChallengerWaypoint);
		return stringMapper;
	}

	protected virtual void OnValueChanged()
	{
		ValueChangedDelegate valueChanged = this.ValueChanged;
		if (valueChanged != null)
		{
			valueChanged();
		}
	}

	private int ComputeFurthestChallengerWaypoint()
	{
		int num = 0;
		for (int i = 0; i < Challengers.Count; i++)
		{
			MissionNpcSpawn key = Challengers[i];
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = m_challengerContent[key].PatrolComponent;
			int currentWaypointIndex = patrolComponent.CurrentWaypointIndex;
			num = Math.Max(num, currentWaypointIndex);
		}
		return num;
	}

	private void RemoveAllWaypoints()
	{
		foreach (MissionNpcSpawn challenger in Challengers)
		{
			ChallengerData challengerData = m_challengerContent[challenger];
			for (int i = 0; i < challengerData.Waypoints.Count; i++)
			{
				MissionWaypoint missionWaypoint = challengerData.Waypoints[i];
				GameObject obj = missionWaypoint.gameObject;
				UnityEngine.Object.Destroy(missionWaypoint);
				UnityEngine.Object.Destroy(obj);
			}
		}
		Challengers.Clear();
	}

	private void SetupChallengers()
	{
		foreach (MissionNpcSpawn challenger in Challengers)
		{
			SetupWaypoint(0, challenger.transform, challenger);
		}
	}

	private HighlightBehaviour SetupMarker(Transform _transform)
	{
		HighlightBehaviour highlightBehaviour = MissionFactory.InstantiateStaticObject(m_entityDefinition, _transform);
		if (highlightBehaviour != null)
		{
			highlightBehaviour.Activate(true);
		}
		return highlightBehaviour;
	}

	private void SetupWaypoint(int _i, Transform _parent, MissionNpcSpawn _challenger)
	{
		bool flag = _i >= Waypoints.Count - 1;
		GameObject gameObject = new GameObject("Waypoint_" + _i);
		MissionWaypoint missionWaypoint = gameObject.AddComponent<MissionWaypoint>();
		missionWaypoint.IsDynamic = true;
		missionWaypoint.Data.WarpTarget = true;
		missionWaypoint.Data.MinSpeed = EMovementSpeed.Run;
		missionWaypoint.Data.MaxSpeed = EMovementSpeed.Run;
		missionWaypoint.Data.ActivationIterationStartType = ERouteIterationStart.Nearest_ERouteIterationStart;
		missionWaypoint.Data.ActivationIterationType = EFormationIterationType.BackAndForth;
		missionWaypoint.Data.IdleTime = base.Data.WaypointWaitingDuration;
		missionWaypoint.Data.RotationType = ((!flag) ? ERotationAtWaypoint.RotationOfWaypoint : ERotationAtWaypoint.None);
		gameObject.transform.parent = _parent;
		MissionNpcFormation component = _challenger.GetComponent<MissionNpcFormation>();
		Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = component.EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>().PatrolComponent;
		Vector3 position = Waypoints[_i].Position;
		Vector3 vector = (flag ? position : Waypoints[_i + 1].Position);
		if (_i == 0)
		{
			_challenger.transform.position = position;
			missionWaypoint.Data.IdleCategory = ENpcIdleCategory.Wait;
			m_challengerContent[_challenger] = new ChallengerData
			{
				Waypoints = new List<MissionWaypoint>(),
				Formation = component,
				PatrolComponent = patrolComponent
			};
		}
		m_challengerContent[_challenger].Waypoints.Add(missionWaypoint);
		if (_i == Waypoints.Count - 1)
		{
			missionWaypoint.Data.IdleTime = -1f;
		}
		gameObject.transform.position = position;
		Vector3 forward = vector - position;
		forward.y = 0f;
		gameObject.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
		component.ScanForWaypoints();
		patrolComponent.TriggerReloadMainRoute();
	}

	private void StartCountdown()
	{
		m_startTime = LogicMain.CurrentTime;
		m_countdownStarted = true;
	}

	private void StartMatch()
	{
		AudioManager.Instance.EventManager.OnEnterParkour();
		LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_SprintInHighProfileOverride, true);
		LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerProfile.ChosenSpeed = EMovementSpeed.Run;
		foreach (MissionNpcSpawn challenger in Challengers)
		{
			for (int i = 1; i < Waypoints.Count; i++)
			{
				SetupWaypoint(i, challenger.transform, challenger);
			}
		}
		base.VisualTrackerState = EProgressState.InProgress;
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0)
		{
			TargetPositions.Add(Waypoints[Value].Position);
		}
	}
}
