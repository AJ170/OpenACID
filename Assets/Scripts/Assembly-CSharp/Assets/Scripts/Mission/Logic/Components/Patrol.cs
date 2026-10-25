using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Patrol : TurnUpdateableComponent, IComponent, IIconPositionCarrier
	{
		public class Waypoint
		{
			public long ID { get; private set; }

			public Vector3 Position { get; internal set; }

			public Quaternion Rotation { get; private set; }

			public ENpcIdleCategory IdleCategory { get; private set; }

			public float IdleTime { get; private set; }

			public ERotationAtWaypoint RotationType { get; private set; }

			public EMovementSpeed MinSpeed { get; set; }

			public EMovementSpeed MaxSpeed { get; set; }

			public bool WarpTarget { get; internal set; }

			public Waypoint()
			{
				ID = -1L;
				IdleCategory = ENpcIdleCategory.Invalid_ENpcIdleCategory;
				IdleTime = 0f;
				RotationType = ERotationAtWaypoint.None;
				MinSpeed = EMovementSpeed.Walk;
				MaxSpeed = EMovementSpeed.WalkFast;
				WarpTarget = false;
			}

			public Waypoint(MissionWaypoint _waypoint)
			{
				ID = _waypoint.Data.ID;
				Transform transform = _waypoint.transform;
				Position = transform.position;
				Rotation = transform.rotation;
				IdleCategory = _waypoint.Data.IdleCategory;
				IdleTime = _waypoint.Data.IdleTime;
				RotationType = _waypoint.Data.RotationType;
				MinSpeed = _waypoint.Data.MinSpeed;
				MaxSpeed = _waypoint.Data.MaxSpeed;
				WarpTarget = _waypoint.Data.WarpTarget;
			}

			public Waypoint(Vector3 _position, Quaternion _rotation)
				: this()
			{
				Position = _position;
				Rotation = _rotation;
			}
		}

		public const float CloseToWaypointDeviationSqr = 1f;

		public const int IterationDirectionDown = -1;

		public const int IterationDirectionUp = 1;

		private const float CloseToWayPointDistanceSqrDefault = 1f;

		private const float CloseToWayPointDistanceSqrShocked = 4f;

		private const int FollowAttachedIntervalTurns = 1;

		private const int FollowNotAttachedIntervalTurns = 9;

		private const int IdleAttachedIntervalTurns = 11;

		private const int IdleNotAttachedIntervalTurns = 19;

		private const int MaxApproacherUpdateIterations = 6;

		private const float DeltaSqrTurnThreshold = 0.01f;

		private int m_TurnUpdateIntervalTurns;

		private readonly SimpleGameTimer m_idleTimer = new SimpleGameTimer();

		private readonly BBList<Waypoint> m_mainRoute = new BBList<Waypoint>(1);

		private readonly BBList<Waypoint> m_sidetrack = new BBList<Waypoint>(1);

		private bool m_sidetrackApproachMovingTarget;

		private bool m_sidetrackHidingEntityShouldExit;

		private int m_sidetrackTimeout = -1;

		private BBList<Waypoint> m_currentRoute;

		private int m_currentWaypointIndex;

		private int m_lastWaypointIndex;

		private int m_oldCurrentWaypointIndex;

		private int m_oldLastWaypointIndex;

		private int m_currentDirection = 1;

		private bool m_forceReassignPositionIndices;

		private int m_lastMemberCount;

		private EFormationIterationType m_formationIterationType;

		private bool m_membersShouldUpdate;

		private bool m_triggerReloadRoute;

		private bool m_triggerEvaluateWaypoint;

		private ERouteIterationStart m_reloadIterationStart = ERouteIterationStart.Reset_ERouteIterationStart;

		private bool m_sidetrackByShouldLeave;

		private bool m_despawned;

		public PatrolTargetApproacher Approacher { get; private set; }

		public sealed override int Priority
		{
			get
			{
				return 8;
			}
		}

		public Assets.Scripts.Mission.Logic.Entities.Patrol PatrolEntity
		{
			get
			{
				return (Assets.Scripts.Mission.Logic.Entities.Patrol)Entity;
			}
		}

		public IFormation Formation { get; private set; }

		public MissionNpcFormation MissionData { get; private set; }

		public bool IsOnSidetrack
		{
			get
			{
				return CurrentRoute == m_sidetrack;
			}
		}

		public bool HidingEntityShouldExit
		{
			get
			{
				return IsOnSidetrack && m_sidetrackHidingEntityShouldExit;
			}
		}

		public bool Idles { get; private set; }

		public bool HasAttachedMembers { get; private set; }

		public EFormationIterationType IterationType
		{
			get
			{
				return m_formationIterationType;
			}
		}

		public Vector3 Position
		{
			get
			{
				return GameEntity.Position;
			}
			private set
			{
				GameEntity.Position = value;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return GameEntity.Rotation;
			}
			private set
			{
				GameEntity.Rotation = value;
			}
		}

		public Vector3 IconPosition
		{
			get
			{
				return Formation.CalcCurrentMemberCenter();
			}
		}

		public Waypoint CurrentWaypoint
		{
			get
			{
				return CurrentRoute[CurrentWaypointIndex];
			}
		}

		public Waypoint LastWaypoint
		{
			get
			{
				return CurrentRoute[m_lastWaypointIndex];
			}
		}

		public int WaypointCount
		{
			get
			{
				return CurrentRoute.Count;
			}
		}

		public EntityId EntityAtHidingSpot
		{
			get
			{
				return Formation.EntityAtHidingSpot;
			}
		}

		public BBList<Waypoint> CurrentRoute
		{
			get
			{
				return m_currentRoute;
			}
			private set
			{
				m_currentRoute = value;
				m_forceReassignPositionIndices = true;
				CurrentWaypointIndex = 0;
				m_lastWaypointIndex = 0;
				ScheduleTurnUpdate(false);
				if (LogicConstants.AreDefaultSpeedsInitialized)
				{
					UpdateReferenceSpeed();
				}
			}
		}

		public float RemainingIdleTime
		{
			get
			{
				return m_idleTimer.TimeToFinish;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return m_TurnUpdateIntervalTurns;
			}
		}

		public int CurrentWaypointIndex
		{
			get
			{
				return m_currentWaypointIndex;
			}
			private set
			{
				m_lastWaypointIndex = Mathf.Clamp(m_currentWaypointIndex, 0, WaypointCount - 1);
				m_currentWaypointIndex = value;
			}
		}

		private bool ShouldLeave
		{
			get
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrolEntity = PatrolEntity;
				return patrolEntity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered) && !EntityAtHidingSpot.IsValid && patrolEntity.EscortComponent == null && !patrolEntity.IsSeducing() && patrolEntity.PatrolDefinition.Definition.InteractionReason != EInteractionReason.Invalid && (patrolEntity.PatrolDefinition.Definition.InteractionReason != EInteractionReason.FreeCapturedNpcs || MissionData.Waypoints == null || MissionData.Waypoints.Length <= 1);
			}
		}

		public bool IsTriggerReloadRoute
		{
			get
			{
				return m_triggerReloadRoute;
			}
		}

		public Patrol(IEntity _entity)
			: base(_entity)
		{
			Approacher = new PatrolTargetApproacher((Assets.Scripts.Mission.Logic.Entities.Patrol)_entity);
			Idles = true;
			Waypoint item = new Waypoint();
			m_sidetrack.Add(item);
		}

		public void TriggerReloadMainRoute(EFormationIterationType _newIterationType, ERouteIterationStart _newRouteIterationStart)
		{
			TriggerReloadMainRoute();
			if (!m_triggerEvaluateWaypoint)
			{
				m_triggerEvaluateWaypoint = true;
				m_formationIterationType = _newIterationType;
				m_reloadIterationStart = _newRouteIterationStart;
			}
		}

		public void TriggerReloadMainRoute()
		{
			m_triggerReloadRoute = true;
			ScheduleTurnUpdate(false);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			MissionData = GameEntity.GameObjectParent.gameObject.GetComponent<MissionNpcFormation>();
			Formation = FormationFactory.CreateFormation(GameEntity.Id, ((IPatrolDefinitionCarrier)Entity).PatrolDefinition.Definition, MissionData.Data.FixedLeaderSlot);
			switch (MissionData.Data.IterationDirection)
			{
			case EFormationIterationDirection.Down:
				m_currentDirection = -1;
				break;
			case EFormationIterationDirection.Up:
				m_currentDirection = 1;
				break;
			}
			m_formationIterationType = MissionData.Data.IterationType;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			PatrolEntity.HeadIcon.IsVisible = PlayerHelper.AssassinInteraction.CanInteractWithNpc(PatrolEntity.Id);
			if (m_despawned || HandleLeaveAndDespawn())
			{
				return;
			}
			if (m_sidetrackTimeout > 0)
			{
				m_sidetrackTimeout -= TurnsSinceTurnUpdate;
				if (m_sidetrackTimeout <= 1)
				{
					StopSidetrack();
				}
			}
			bool isTriggerReloadRoute = IsTriggerReloadRoute;
			if (IsTriggerReloadRoute)
			{
				ReloadMainRoute();
			}
			if (m_triggerEvaluateWaypoint)
			{
				EvaluateCurrentWaypoint();
			}
			int memberCount = Formation.MemberCount;
			m_membersShouldUpdate = memberCount != 0;
			if (!m_membersShouldUpdate)
			{
				return;
			}
			int currentDirection = m_currentDirection;
			bool flag = false;
			bool _hasAttachedMembers = false;
			float _deltaTime = Mathf.Min(TimeSinceTurnUpdate, (float)TurnUpdateIntervalTurns * 0.1f);
			for (int i = 0; i < 6; i++)
			{
				bool _closeToTarget;
				Approacher.DoTurnUpdate(IsOnSidetrack && m_sidetrackApproachMovingTarget, isTriggerReloadRoute, out _closeToTarget, out _hasAttachedMembers, ref _deltaTime);
				bool flag2 = Approacher.MinMemberDeviationSqr < CloseToWayPointDistance(CurrentWaypoint.IdleCategory);
				flag = _closeToTarget && flag2;
				if (flag)
				{
					flag = HandleBeingAtWaypoint();
				}
				if (flag || _closeToTarget || _deltaTime <= MathHelper.Epsilon || CurrentWaypoint.WarpTarget)
				{
					break;
				}
				if (i >= 5)
				{
				}
			}
			bool flag3 = HasAttachedMembers && (m_forceReassignPositionIndices || currentDirection != m_currentDirection || (!flag && Idles));
			if (flag3)
			{
				Vector3 forward = CurrentWaypoint.Position - Position;
				if (forward.sqrMagnitude > 0.01f)
				{
					Rotation = Quaternion.LookRotation(forward);
				}
			}
			if (flag3 || memberCount != m_lastMemberCount)
			{
				m_lastMemberCount = memberCount;
				m_forceReassignPositionIndices = false;
				Formation.ReassignPositionIndices();
			}
			if (flag != Idles || _hasAttachedMembers != HasAttachedMembers)
			{
				CalcNumberTurnsBetweenUpdates(flag, _hasAttachedMembers);
				if (_hasAttachedMembers)
				{
					ScheduleTurnUpdate(false);
				}
				else if (!flag)
				{
					ScheduleTurnUpdate(true);
				}
			}
			Idles = flag;
			HasAttachedMembers = _hasAttachedMembers;
			base.DoTurnUpdate(_deltaTurns);
		}

		public void StartSidetrack(Vector3 _target, bool _sidetrackApproachMovingTarget, bool _hidingEntityShouldExit = true, int _timeout = -1, bool _warpTarget = false, EMovementSpeed _speed = EMovementSpeed.Invalid_EMovementSpeed)
		{
			m_sidetrackApproachMovingTarget = _sidetrackApproachMovingTarget;
			m_sidetrackTimeout = _timeout;
			m_sidetrack[0].Position = _target;
			if (_speed != EMovementSpeed.Invalid_EMovementSpeed)
			{
				Waypoint waypoint = m_sidetrack[0];
				m_sidetrack[0].MaxSpeed = _speed;
				waypoint.MinSpeed = _speed;
			}
			m_sidetrack[0].WarpTarget = _warpTarget;
			m_sidetrackHidingEntityShouldExit = _hidingEntityShouldExit;
			if (!IsOnSidetrack)
			{
				m_oldCurrentWaypointIndex = CurrentWaypointIndex;
				m_oldLastWaypointIndex = m_lastWaypointIndex;
				CurrentRoute = m_sidetrack;
			}
		}

		public void StopSidetrack()
		{
			CurrentRoute = m_mainRoute;
			CurrentWaypointIndex = m_oldCurrentWaypointIndex;
			m_lastWaypointIndex = m_oldLastWaypointIndex;
			m_sidetrackTimeout = -1;
		}

		public bool GetTargetWorldPosition(EntityId _entityId, out Vector3 _position)
		{
			return Formation.GetTargetWorldPosition(_entityId, out _position);
		}

		public bool ShouldMembersPerformUpdate()
		{
			if (!HasAttachedMembers)
			{
				return false;
			}
			return m_membersShouldUpdate;
		}

		public override void OnActivate()
		{
			CalcNumberTurnsBetweenUpdates(true, false);
			ReloadMainRoute();
			WarpPatrolAndMembers();
			m_triggerEvaluateWaypoint = true;
		}

		public float ComputePathLength()
		{
			int count = CurrentRoute.Count;
			float num = 0f;
			for (int i = 0; i < count - 1; i++)
			{
				Waypoint waypoint = CurrentRoute[i];
				Waypoint waypoint2 = CurrentRoute[i + 1];
				float num2 = waypoint.Position.DistanceXZ(waypoint2.Position);
				num += num2;
			}
			return num;
		}

		public float ComputeTraveledLength()
		{
			int count = CurrentRoute.Count;
			float num = 0f;
			for (int i = 0; i < count - 1; i++)
			{
				Waypoint waypoint = CurrentRoute[i];
				Waypoint waypoint2 = CurrentRoute[i + 1];
				if (waypoint == CurrentWaypoint)
				{
					break;
				}
				if (waypoint2 == CurrentWaypoint)
				{
					float num2 = waypoint.Position.DistanceXZ(Position);
					num += num2;
				}
				else
				{
					float num3 = waypoint.Position.DistanceXZ(waypoint2.Position);
					num += num3;
				}
			}
			return num;
		}

		private void Despawn()
		{
			m_despawned = true;
			for (int i = 0; i < Formation.MemberCount; i++)
			{
				Character entity = Formation.Entities[i].m_id.GetEntity<Character>();
				entity.Despawn();
			}
		}

		private void WarpPatrolAndMembers()
		{
			Position = CurrentWaypoint.Position;
			Rotation = CurrentWaypoint.Rotation;
			MissionNpcSpawn[] components = MissionData.GetComponents<MissionNpcSpawn>();
			if (components.Length > Formation.MaxCount)
			{
			}
			int num = components.Length;
			Formation.MaxCount = num;
			for (int i = 0; i < num; i++)
			{
				MissionNpcSpawn missionNpcSpawn = components[i];
				Formation.Add(missionNpcSpawn.GameEntity.Id);
			}
			for (int j = 0; j < num; j++)
			{
				MissionNpcSpawn missionNpcSpawn2 = components[j];
				Vector3 _position;
				Formation.GetTargetWorldPosition(missionNpcSpawn2.EntityID, out _position);
				missionNpcSpawn2.EntityID.GameEntity.Position = _position;
				NpcCharacter npcCharacter = missionNpcSpawn2.GameEntity as NpcCharacter;
				if (npcCharacter != null)
				{
					PatrolHelper.ForceRotationOfWaypoint(npcCharacter, this);
					if (MissionData.Data.Type == EPatrolType.CapturedNpcs)
					{
						CharacterCondition characterCondition = npcCharacter.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
						characterCondition.ExternalChange(EStateType.CharacterCondition_Captured);
					}
				}
			}
		}

		private void ReloadMainRoute()
		{
			m_triggerReloadRoute = false;
			m_mainRoute.ClearIfNotEmpty();
			int num = ((MissionData.Waypoints != null) ? MissionData.Waypoints.Length : 0);
			m_mainRoute.Capacity = num;
			Waypoint waypoint = null;
			if (num >= 1)
			{
				waypoint = new Waypoint(MissionData.Waypoints[0]);
			}
			for (int i = 0; i < num; i++)
			{
				MissionWaypoint missionWaypoint = MissionData.Waypoints[i];
				if (UbiCore.Utils.Singleton<MissionManager>.Instance.IsEntryActive(missionWaypoint))
				{
					Waypoint item = new Waypoint(missionWaypoint);
					m_mainRoute.Add(item);
				}
			}
			if (m_mainRoute.Count == 0)
			{
				Waypoint item2 = waypoint ?? new Waypoint(MissionData.gameObject.transform.position, MissionData.gameObject.transform.rotation);
				m_mainRoute.Add(item2);
			}
			CurrentRoute = m_mainRoute;
		}

		private void EvaluateCurrentWaypoint()
		{
			m_triggerEvaluateWaypoint = false;
			int currentWaypointIndex = CurrentWaypointIndex;
			int count = CurrentRoute.Count;
			switch (m_reloadIterationStart)
			{
			case ERouteIterationStart.Reset_ERouteIterationStart:
				m_currentDirection = 1;
				CurrentWaypointIndex = 0;
				break;
			case ERouteIterationStart.Up_ERouteIterationStart:
				m_currentDirection = 1;
				CurrentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, count - 1);
				break;
			case ERouteIterationStart.Nearest_ERouteIterationStart:
				SetNearestWaypointAsTarget(0);
				break;
			case ERouteIterationStart.NearestUp_ERouteIterationStart:
				m_currentDirection = 1;
				SetNearestWaypointAsTarget(currentWaypointIndex);
				break;
			}
		}

		private void SetNearestWaypointAsTarget(int _startIndex)
		{
			int count = m_mainRoute.Count;
			float num = float.MaxValue;
			_startIndex = Mathf.Clamp(_startIndex, 0, count - 1);
			for (int i = _startIndex; i < count; i++)
			{
				float magnitude = (m_mainRoute[i].Position - Position).magnitude;
				if (magnitude < num)
				{
					CurrentWaypointIndex = i;
					num = magnitude;
				}
			}
		}

		private bool HandleBeingAtWaypoint()
		{
			Waypoint currentWaypoint = CurrentWaypoint;
			if (m_idleTimer.IsFinished)
			{
				return IterateWaypoint();
			}
			if (m_idleTimer.IsStarted)
			{
				return true;
			}
			if (!m_idleTimer.IsStarted && currentWaypoint.IdleTime > 0f)
			{
				m_idleTimer.Start(currentWaypoint.IdleTime);
				return true;
			}
			if (MathHelper.Approximately(currentWaypoint.IdleTime, 0f))
			{
				return IterateWaypoint();
			}
			if (currentWaypoint.IdleTime < 0f)
			{
				return true;
			}
			return false;
		}

		private bool IterateWaypoint()
		{
			if (WaypointCount > 1)
			{
				m_idleTimer.Reset();
				CurrentWaypointIndex += m_currentDirection;
				EFormationIterationType iterationType = IterationType;
				if (iterationType == EFormationIterationType.Circeling)
				{
					UpdateCirceling();
				}
				else
				{
					UpdateBackAndForth();
				}
				UpdateReferenceSpeed();
				return false;
			}
			return true;
		}

		private void UpdateBackAndForth()
		{
			if (m_currentDirection > 0 && CurrentWaypointIndex >= WaypointCount)
			{
				m_currentDirection *= -1;
				CurrentWaypointIndex = WaypointCount - 2;
			}
			else if (m_currentDirection < 0 && CurrentWaypointIndex < 0)
			{
				m_currentDirection *= -1;
				CurrentWaypointIndex = 1;
			}
		}

		private void UpdateCirceling()
		{
			if (CurrentWaypointIndex < 0)
			{
				CurrentWaypointIndex += WaypointCount;
			}
			else
			{
				CurrentWaypointIndex %= WaypointCount;
			}
		}

		private void CalcNumberTurnsBetweenUpdates(bool _idles, bool _hasAttachedMembers)
		{
			if (_idles)
			{
				m_TurnUpdateIntervalTurns = ((!_hasAttachedMembers) ? LogicMain.Random.Rand(17, 21) : 11);
			}
			else
			{
				m_TurnUpdateIntervalTurns = (_hasAttachedMembers ? 1 : LogicMain.Random.Rand(7, 11));
			}
		}

		private void UpdateReferenceSpeed()
		{
			Approacher.ReferenceSpeed = CurrentWaypoint.MinSpeed.GetDefaultSpeedValue();
		}

		private bool HandleLeaveAndDespawn()
		{
			if (ShouldLeave)
			{
				if (m_sidetrackByShouldLeave && IsOnSidetrack)
				{
					if (CurrentWaypoint.Position.DistanceXZSquared(Formation.CalcCurrentMemberCenter()) < 1f)
					{
						Despawn();
						return true;
					}
				}
				else
				{
					NavMeshPath _path;
					Vector3 target = SpawnHelper.ComputeSpawnPos(PatrolEntity.Position, Quaternion.LookRotation(PatrolEntity.Position), 30f, 0f, NavMeshLayers.WalkableLayerMaskPreset, false, out _path);
					if (_path == null)
					{
						Despawn();
						return true;
					}
					EMovementSpeed speed = ((MissionData.Data.Type != EPatrolType.CapturedNpcs) ? EMovementSpeed.Invalid_EMovementSpeed : EMovementSpeed.Sprint);
					StartSidetrack(target, false, false, -1, true, speed);
					m_sidetrackByShouldLeave = true;
				}
			}
			return false;
		}

		private static float CloseToWayPointDistance(ENpcIdleCategory _idleCategory)
		{
			if (_idleCategory == ENpcIdleCategory.Shocked)
			{
				return 4f;
			}
			return 1f;
		}
	}
}
