using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Mission;
using SharedBaseLib.System;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

public static class CameraStateHelper
{
	public enum AnimationEventState
	{
		Off = 0,
		On = 1
	}

	private const float CLIMB_CANCELED_LOCK_TIME = 0.8f;

	private const float CLIMB_STARTED_LOCK_TIME = 0.8f;

	private static AnimationTaskBase ms_currentAnimationTask;

	private static AnimationTaskType ms_lastAnimationTaskType = AnimationTaskType.Unknown;

	private static float m_climbCanceledTimer = float.MinValue;

	private static float m_climbStartTimer = float.MinValue;

	private static bool m_climbStarted;

	private static AcierCameraController.ECameraState m_climbState;

	private static AcierCameraController.ECameraState m_roamingState = AcierCameraController.ECameraState.Roaming;

	public static AcierCameraController.ECameraState WantedState { get; private set; }

	public static AnimationEventState AnimationState { get; private set; }

	public static GameObject AssassinationTarget { get; private set; }

	public static AnimationTaskBase CurrentAnimationTask
	{
		get
		{
			return ms_currentAnimationTask;
		}
	}

	public static AnimationTaskType CachedAnimationTaskType
	{
		get
		{
			return ms_lastAnimationTaskType;
		}
	}

	public static void OnCameraStart()
	{
		AnimationState = AnimationEventState.On;
	}

	public static void OnCameraEnd()
	{
		AnimationState = AnimationEventState.Off;
	}

	public static void Initialize()
	{
		Reset();
		PlayerHelper.PlayerMain.StateChanged += PlayerStateChanged;
		LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited += TaskManagerOnAnimationTaskExited;
	}

	public static void Reset()
	{
		ms_currentAnimationTask = null;
		ms_lastAnimationTaskType = AnimationTaskType.Unknown;
		WantedState = AcierCameraController.ECameraState.None;
		AnimationState = AnimationEventState.Off;
		m_climbCanceledTimer = float.MinValue;
		m_climbStartTimer = float.MinValue;
		m_climbStarted = false;
		AssassinationTarget = null;
		m_climbState = AcierCameraController.ECameraState.None;
		m_roamingState = AcierCameraController.ECameraState.Roaming;
		RefreshRoamingState(LogicMain.PlayerEntity.Position);
	}

	public static void Refresh()
	{
		AnimationTaskManager taskManager = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager;
		if (taskManager != null)
		{
			ms_currentAnimationTask = taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		}
		else
		{
			ms_currentAnimationTask = null;
			ms_lastAnimationTaskType = AnimationTaskType.Unknown;
		}
		if (ms_currentAnimationTask != null)
		{
			ms_lastAnimationTaskType = ms_currentAnimationTask.TaskType;
		}
		WantedState = DetermineCurrentState();
	}

	private static void PlayerStateChanged(IStateMachine _stateMachine, EStateType _from, EStateType _to)
	{
		if (_from == EStateType.CombatCharacterMain_Combat)
		{
			RefreshRoamingState(AcierCameraController.NavMeshAgent.transform.position);
		}
	}

	public static AcierCameraController.ECameraState DetermineCurrentState()
	{
		if (Main.Instance.VrEnabled)
		{
			return AcierCameraController.ECameraState.VR;
		}
		if (CameraCustomAnimationHelper.ShouldActivateCamera)
		{
			return AcierCameraController.ECameraState.CustomAnimated;
		}
		if (IsDead())
		{
			return AcierCameraController.ECameraState.Dead;
		}
		if (IsIntroduction())
		{
			return AcierCameraController.ECameraState.Introduction;
		}
		if (IsOutro())
		{
			return AcierCameraController.ECameraState.Outro;
		}
		if (IsNPC())
		{
			return AcierCameraController.ECameraState.NPC;
		}
		if (IsStealthAssassination())
		{
			return AcierCameraController.ECameraState.StealthAssassination;
		}
		if (IsLeapAssassination())
		{
			return AcierCameraController.ECameraState.LeapAssassination;
		}
		if (IsAirAssassination())
		{
			return AcierCameraController.ECameraState.AirAssassination;
		}
		if (IsBenchAssassination())
		{
			return AcierCameraController.ECameraState.BenchAssassination;
		}
		if (IsHaystackAssassination())
		{
			return AcierCameraController.ECameraState.HaystackAssassination;
		}
		if (IsRooftopGardenAssassination())
		{
			return AcierCameraController.ECameraState.RooftopGardenAssassination;
		}
		AcierCameraController.ECameraState eCameraState = DetermineCombatState();
		if (eCameraState != 0)
		{
			return eCameraState;
		}
		if (IsHaystack())
		{
			return AcierCameraController.ECameraState.Haystack;
		}
		if (IsRooftopGarden())
		{
			return AcierCameraController.ECameraState.RooftopGarden;
		}
		if (IsBench())
		{
			return AcierCameraController.ECameraState.Bench;
		}
		AcierCameraController.ECameraState eCameraState2 = DetermineClimb();
		if (eCameraState2 != 0)
		{
			return eCameraState2;
		}
		if (IsJumpDown())
		{
			return AcierCameraController.ECameraState.JumpDown;
		}
		if (IsLeapOfFaith())
		{
			return AcierCameraController.ECameraState.LeapOfFaith;
		}
		AcierCameraController.ECameraState eCameraState3 = DetermineInteraction();
		if (eCameraState3 != 0)
		{
			return eCameraState3;
		}
		if (IsRoofEdge())
		{
			return AcierCameraController.ECameraState.RoofEdge;
		}
		return m_roamingState;
	}

	private static bool IsDead()
	{
		return PlayerHelper.IsDead;
	}

	private static bool IsIntroduction()
	{
		return UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.ShowIntroduction;
	}

	private static bool IsOutro()
	{
		return Outro.Instance != null && Outro.Instance.IsCameraActive;
	}

	private static bool IsNPC()
	{
		return AcierCameraController.Instance.CurrentPlayerState == EPlayerState.NpcInteraction || CachedAnimationTaskType == AnimationTaskType.PlayerHealAtDoctor;
	}

	private static bool IsStealthAssassination()
	{
		return AnimationState == AnimationEventState.On && CachedAnimationTaskType == AnimationTaskType.AssassinateStealthInstigator;
	}

	private static bool IsLeapAssassination()
	{
		return AnimationState == AnimationEventState.On && CachedAnimationTaskType == AnimationTaskType.AssassinateLeapInstigator;
	}

	private static bool IsAirAssassination()
	{
		if (CachedAnimationTaskType == AnimationTaskType.AssassinateAirInstigator)
		{
			AnimationTaskAssassinateAirInstigator animationTaskAssassinateAirInstigator = CurrentAnimationTask as AnimationTaskAssassinateAirInstigator;
			if (animationTaskAssassinateAirInstigator == null)
			{
				return false;
			}
			RefreshRoamingState(animationTaskAssassinateAirInstigator.Target.transform.position);
			return AnimationState == AnimationEventState.On;
		}
		return false;
	}

	private static bool IsBenchAssassination()
	{
		return CachedAnimationTaskType == AnimationTaskType.BenchKillInstigator;
	}

	private static bool IsHaystackAssassination()
	{
		return AnimationState == AnimationEventState.On && CachedAnimationTaskType == AnimationTaskType.HayStackKillInstigator;
	}

	private static bool IsRooftopGardenAssassination()
	{
		return AnimationState == AnimationEventState.On && CachedAnimationTaskType == AnimationTaskType.RooftopGardenKillInstigator;
	}

	private static AcierCameraController.ECameraState DetermineCombatState()
	{
		if (AcierCameraController.Instance.CurrentPlayerState != EPlayerState.Combat)
		{
			return AcierCameraController.ECameraState.None;
		}
		bool inCameraEvent = AnimationState == AnimationEventState.On;
		switch (CombatController.Instance.RecommendedCameraMode(inCameraEvent))
		{
		case ECombatCameraMode.Combat:
			return AcierCameraController.ECameraState.Combat;
		case ECombatCameraMode.Kill:
			return AcierCameraController.ECameraState.Kill;
		default:
			return AcierCameraController.ECameraState.None;
		}
	}

	private static bool IsHaystack()
	{
		bool flag = CachedAnimationTaskType == AnimationTaskType.HayStackEnter || CachedAnimationTaskType == AnimationTaskType.HayStackIdle || CachedAnimationTaskType == AnimationTaskType.HayStackExit || CachedAnimationTaskType == AnimationTaskType.HayStackKillInstigator;
		return UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current != null && flag;
	}

	private static bool IsRooftopGarden()
	{
		bool flag = CachedAnimationTaskType == AnimationTaskType.RooftopGardenEnter || CachedAnimationTaskType == AnimationTaskType.RooftopGardenIdle || CachedAnimationTaskType == AnimationTaskType.RooftopGardenExit || CachedAnimationTaskType == AnimationTaskType.RooftopGardenKillInstigator;
		return UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current != null && flag;
	}

	private static bool IsBench()
	{
		return CachedAnimationTaskType == AnimationTaskType.BenchEnter || CachedAnimationTaskType == AnimationTaskType.BenchIdle || CachedAnimationTaskType == AnimationTaskType.BenchKillInstigator;
	}

	private static void TaskManagerOnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
	{
		AnimationTaskClimb animationTaskClimb = _eventArgs.AnimationTask as AnimationTaskClimb;
		if (animationTaskClimb != null && animationTaskClimb.Canceled)
		{
			m_climbCanceledTimer = 0.8f;
			RefreshRoamingState(animationTaskClimb.OffMeshLinkEntity.StartPosition);
		}
		else
		{
			RefreshRoamingState(LogicMain.PlayerEntity.Position);
		}
	}

	private static AcierCameraController.ECameraState DetermineClimb()
	{
		if (m_climbStartTimer > 0f)
		{
			m_climbStartTimer -= UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
			return AcierCameraController.ECameraState.Climb;
		}
		if (CachedAnimationTaskType == AnimationTaskType.Climb && m_climbCanceledTimer < 0f)
		{
			AnimationTaskClimb animationTaskClimb = CurrentAnimationTask as AnimationTaskClimb;
			if (animationTaskClimb == null)
			{
				return m_climbState;
			}
			if (animationTaskClimb.Canceled)
			{
				m_climbCanceledTimer = 0.8f;
				RefreshRoamingState(animationTaskClimb.OffMeshLinkEntity.StartPosition);
				return m_roamingState;
			}
			if (animationTaskClimb.JumpingDownFromWall && animationTaskClimb.JumpingDownHeight > 1f)
			{
				return m_climbState = AcierCameraController.ECameraState.JumpDown;
			}
			RefreshRoamingState(animationTaskClimb.Destination);
			if (animationTaskClimb.Height < 5f && !animationTaskClimb.HorizontalJumpToWall)
			{
				return m_climbState = AcierCameraController.ECameraState.None;
			}
			if (m_climbStarted)
			{
				m_climbStartTimer = 0.8f;
				m_climbStarted = false;
			}
			return m_climbState = AcierCameraController.ECameraState.Climb;
		}
		m_climbStarted = true;
		float deltaTime = UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		m_climbCanceledTimer -= deltaTime;
		m_climbStartTimer -= deltaTime;
		return m_climbState = AcierCameraController.ECameraState.None;
	}

	private static bool IsRoofEdge()
	{
		if (AcierInputHandler.Instance.RoofEdgeDetection.AtRoofEdge)
		{
			return true;
		}
		return false;
	}

	private static bool IsJumpDown()
	{
		if (CachedAnimationTaskType == AnimationTaskType.AssassinateAirInstigator)
		{
			return true;
		}
		if (CachedAnimationTaskType == AnimationTaskType.RunPassOverDown)
		{
			AnimationTaskRunPassOverDown animationTaskRunPassOverDown = CurrentAnimationTask as AnimationTaskRunPassOverDown;
			if (animationTaskRunPassOverDown != null)
			{
				return animationTaskRunPassOverDown.Height > 2.5f;
			}
		}
		else if (CachedAnimationTaskType == AnimationTaskType.JumpDown || CachedAnimationTaskType == AnimationTaskType.JumpDownAnywhere)
		{
			AnimationTaskJumpDown animationTaskJumpDown = CurrentAnimationTask as AnimationTaskJumpDown;
			float height;
			AnimationStateJumpDown animationStateJumpDown;
			if (animationTaskJumpDown != null)
			{
				RefreshRoamingState(animationTaskJumpDown.OffMeshLinkEntity.EndPosition);
				height = animationTaskJumpDown.Height;
				animationStateJumpDown = animationTaskJumpDown.CurrentState as AnimationStateJumpDown;
			}
			else
			{
				AnimationTaskJumpDownAnywhere animationTaskJumpDownAnywhere = CurrentAnimationTask as AnimationTaskJumpDownAnywhere;
				if (animationTaskJumpDownAnywhere == null)
				{
					return false;
				}
				RefreshRoamingState(animationTaskJumpDownAnywhere.LandPosition);
				height = animationTaskJumpDownAnywhere.Height;
				animationStateJumpDown = animationTaskJumpDownAnywhere.CurrentState as AnimationStateJumpDown;
			}
			if (animationStateJumpDown == null)
			{
				return false;
			}
			if (height < 2.5f)
			{
				return false;
			}
			if (animationStateJumpDown.LandTime == float.MaxValue)
			{
				return true;
			}
			return LogicMain.CurrentTime <= animationStateJumpDown.LandTime - 0.5f;
		}
		return false;
	}

	private static bool IsLeapOfFaith()
	{
		if (CachedAnimationTaskType == AnimationTaskType.LeapOfFaith)
		{
			return true;
		}
		return false;
	}

	private static AcierCameraController.ECameraState DetermineInteraction()
	{
		if (AcierCameraController.Instance.CurrentPlayerState == EPlayerState.ObjectInteraction)
		{
			EntityId currentInteractionObject = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CurrentInteractionObject;
			if (currentInteractionObject != EntityId.InvalidId)
			{
				if (currentInteractionObject.Type == EntityTypes.EType.Codex)
				{
					return AcierCameraController.ECameraState.Codex;
				}
				if (AnimationState == AnimationEventState.On && currentInteractionObject.Type == EntityTypes.EType.InteractibleMissionObject)
				{
					return AcierCameraController.ECameraState.Chest;
				}
			}
		}
		return AcierCameraController.ECameraState.None;
	}

	private static void RefreshRoamingState(Vector3 p)
	{
		NavMeshHit _hit;
		if (NavigationHelper.CascadingSamplePosition(p, out _hit, -1))
		{
			if ((_hit.mask & NavMeshLayers.RoofSurfaceLayerMask) != 0)
			{
				m_roamingState = AcierCameraController.ECameraState.Roof;
			}
			else
			{
				m_roamingState = AcierCameraController.ECameraState.Roaming;
			}
		}
	}
}
