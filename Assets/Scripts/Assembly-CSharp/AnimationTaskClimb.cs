using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Motion.Hotspots;
using SharedBaseLib.System;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class AnimationTaskClimb : AnimationTaskBase, IAnimation, IParkourAnimation, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.Climb)
		{
		}
	}

	public enum ECanceledBy
	{
		Player = 0,
		Damage = 1
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Climb;

	public static float ClimbingSpeed = 1f;

	public static float MaxPinDuration = 5f;

	public static float MinFallDistanceFromWall = 0.5f;

	public static float MaxFallDistanceFromWall = 1f;

	public static float MinHeightToJumpDown = 0.2f;

	public static float MinOverhang = 0.35f;

	public static float OverhangCheckOffsetY = 0.75f;

	public static bool AutoClimb = true;

	public static float MaxSpeedAfterClimbing = 0.4f;

	public static float MaxPitch = 40f;

	public static float PitchMultiplier = 1.2f;

	public static float PitchDamping = 0.2f;

	public static float EdgeDistanceXDamping = 0.4f;

	public static float TargetDistanceToWall = 0.5f;

	public static float MinHeightToKeepDistanceToWall = 1.5f;

	public static float KeepDistanceToWallDamping = 0.04f;

	public static float BlockClimbLinkDuration = 2f;

	public static float MinDistanceForHorizontalJumpToWall = 3.5f;

	public static Vector3 HorizontalJumpToWallHandOffset = new Vector3(0f, -0.075f, -0.05f);

	public static float TurnMinAngle = 75f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private ClimbRoute m_climbRoute;

	private ClimbRouteEntry m_targetEntry;

	private Vector3 m_turnFacing;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	private Vector3 m_landPosition;

	private Vector3 m_landFacing;

	private bool m_correctLandFacing;

	private Vector3 m_endPosition;

	private Vector3 m_endFacing;

	private eDirection m_climbDirection;

	private bool m_askMatch;

	private Vector3 m_matchPoint = Vector3.zero;

	private AvatarTarget m_matchBodyPart = AvatarTarget.LeftHand;

	private HumanBodyBones m_matchBone = HumanBodyBones.LeftHand;

	private static MatchTargetWeightMask s_weightMask = new MatchTargetWeightMask(new Vector3(1f, 1f, 1f), 0f);

	private Vector3 m_wallPoint = Vector3.zero;

	private bool m_catClimb;

	private bool m_freeHang;

	private float m_lastEdgeDistance;

	private bool m_climbStepCompleted;

	private bool m_lastClimbRightHand;

	private bool m_hasStartedClimbing;

	private bool m_reachedFirstPin;

	private ParcourCancelEvent.ECancelType m_cancelType;

	private bool m_skipTaskEndEvent;

	private float m_minAutoExitTime = float.MaxValue;

	private Vector3 m_faceDirection;

	private bool m_correctFacing = true;

	private List<AnimatorIK.PinData> m_pinData = new List<AnimatorIK.PinData>();

	private bool m_waitForIdlePose;

	private float m_targetEdgeDistanceX;

	private float m_climbOverhang;

	private float m_targetClimbOverhang;

	private bool m_resetOverhang = true;

	private bool m_keepDistanceToWall;

	private float m_pitchAngle;

	private float m_currentPitch;

	private float m_pitchEndTime = float.MaxValue;

	private bool m_canJumpDown;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Climb;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public Vector3 Destination { get; private set; }

	public OffMeshLinkEntity OffMeshLinkEntity { get; private set; }

	public float Height
	{
		get
		{
			return OffMeshLinkEntity.EndPosition.y - OffMeshLinkEntity.StartPosition.y;
		}
	}

	public bool HangingAtEdge
	{
		get
		{
			return m_climbRoute.IsLastEntry && !AnimationController.Animator.GetCurrentRoofEdge();
		}
	}

	public bool RouteCompleted { get; private set; }

	public bool Canceled { get; private set; }

	public bool JumpingDownFromWall { get; private set; }

	public float JumpingDownHeight { get; private set; }

	public bool HorizontalJumpToWall { get; private set; }

	public override EStartIdleAnimAfterTaskPolicy StartIdleWhenFinishedPolicy
	{
		get
		{
			if (Canceled)
			{
				return EStartIdleAnimAfterTaskPolicy.Yes;
			}
			return base.StartIdleWhenFinishedPolicy;
		}
	}

	public AnimationTaskClimb()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskClimb(Parameters _parameters)
		: base(_parameters)
	{
		OffMeshLinkEntity = _parameters.OffMeshLink;
		AnimatorIK component = AnimationController.Animator.gameObject.GetComponent<AnimatorIK>();
		m_pinData = ((!component) ? new List<AnimatorIK.PinData>() : component.PinDataList);
	}

	public void OnParcourInputEvent(ParcourInputEvent _event)
	{
		m_climbDirection = ParcourHelper.ComputeRelativeDirection(_event.Direction, m_faceDirection);
	}

	public void OnParcourCancelEvent(ParcourCancelEvent _event)
	{
		if (_event.TargetId == AnimationController.EntityId && m_canJumpDown)
		{
			m_cancelType = _event.CancelType;
			switch (_event.CancelType)
			{
			case ParcourCancelEvent.ECancelType.Hit:
				CancelClimbing(ECanceledBy.Damage);
				break;
			case ParcourCancelEvent.ECancelType.Die:
				CancelClimbing(ECanceledBy.Damage);
				break;
			}
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		if (OffMeshLinkEntity.ClimbRoute != null && OffMeshLinkEntity.ClimbRoute.ClimbRouteEntries.Count > 0)
		{
			m_climbRoute = OffMeshLinkEntity.ClimbRoute.Clone();
			if (OffMeshLinkEntity.OffMeshLink != null)
			{
				UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.BlockOffMeshLink(OffMeshLinkEntity.OffMeshLink, false, (base.Owner.Entity != LogicMain.PlayerEntity) ? BlockClimbLinkDuration : float.MaxValue);
			}
			if (this.GetHandlesParkourInputEvents())
			{
				Messenger.Instance.AddListener<ParcourInputEvent>(OnParcourInputEvent);
			}
			if (this.GetHandlesParkourCancelEvents())
			{
				Messenger.Instance.AddListener<ParcourCancelEvent>(OnParcourCancelEvent);
			}
			AnimationController.Animator.speed = ClimbingSpeed;
			AnimationController.ApplyRootMotion = true;
			AnimationController.Agent.updatePosition = false;
			AnimationController.Agent.updateRotation = false;
			AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
			AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
			Destination = OffMeshLinkEntity.EndPosition;
			Canceled = false;
			RouteCompleted = false;
			JumpingDownFromWall = false;
			m_canJumpDown = false;
			m_faceDirection = (OffMeshLinkEntity.EndPosition - OffMeshLinkEntity.StartPosition).Mask(new Vector3(1f, 0f, 1f)).normalized;
			m_landPosition = AnimationController.NextPosition;
			m_landFacing = m_faceDirection;
			m_reachedFirstPin = false;
			m_keepDistanceToWall = m_climbRoute.NextEntry.ClosestPoint.y - m_landPosition.y > MinHeightToKeepDistanceToWall;
			ClimbRouteEntry climbRouteEntry = m_climbRoute.ClimbRouteEntries[0];
			Vector3 vector = climbRouteEntry.Edge.ClosestPoint(m_landPosition);
			float num = vector.DistanceXZ(m_landPosition);
			if (num > MinDistanceForHorizontalJumpToWall)
			{
				Vector3 origin = vector - (vector - m_landPosition).normalized * MinFallDistanceFromWall;
				RaycastHit hitInfo;
				NavMeshHit _hit;
				if (Physics.Raycast(origin, Vector3.down, out hitInfo, float.MaxValue) && NavigationHelper.CascadingSamplePosition(hitInfo.point, out _hit, AnimationController.Agent.areaMask))
				{
					m_landPosition = _hit.position;
					m_landFacing = climbRouteEntry.Edge.Normals[0];
					m_keepDistanceToWall = false;
				}
			}
			m_matchPoint = AnimationController.NextPosition;
			m_wallPoint = m_matchPoint;
			RaycastHit hitInfo2;
			if (Physics.Raycast(m_matchPoint + Vector3.up, -m_climbRoute.ClimbRouteEntries[0].Edge.Normals[0], out hitInfo2, 3f, Layers.StaticCollidersLayersMaskPreset))
			{
				m_wallPoint = hitInfo2.point - Vector3.up;
				m_matchPoint = m_wallPoint;
			}
			m_turnFacing = m_faceDirection;
			float num2 = Vector3.Angle(m_turnFacing, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_turnFacing, AnimationController.transform.right));
			if (Mathf.Abs(num2) > TurnMinAngle)
			{
				float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
				float speed = Mathf.Min(currentSpeed, 0.4f);
				AnimationController.Animator.SetCurrentSpeed(speed);
				m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num2);
				m_turnCorrectionAngle = num2 - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
				AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
				AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
				m_waitForTurn = true;
			}
			else
			{
				StartClimbing();
			}
		}
		else
		{
			base.IsFinished = true;
		}
	}

	public void StartClimbing()
	{
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Climb);
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Up);
		AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
		AnimationController.Animator.SetCurrentRoofEdge(false);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.Animator.SetBool(AnimatorParameters.RoofEdgeJumpDownHash, false);
		StartClimbStep();
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Invalid);
		AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
		AnimationController.Animator.SetCurrentRoofEdge(false);
		AnimationController.Animator.SetClimbRightHand(false);
		AnimationController.Animator.speed = 1f;
		AnimationController.WarpCloseToDestination(Destination);
		m_climbRoute = null;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInputEvent);
		Messenger.Instance.RemoveListener<ParcourCancelEvent>(OnParcourCancelEvent);
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		AnimationController.NextRotation = Quaternion.LookRotation(AnimationController.NextRotation * Vector3.forward);
		if (OffMeshLinkEntity.OffMeshLink != null)
		{
			UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.UnblockOffMeshLink(OffMeshLinkEntity.OffMeshLink);
		}
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		CheckIsFinished(_event.StateMachine);
	}

	public override void OnDrawGizmos()
	{
		if (m_climbRoute == null || m_climbRoute.ClimbRouteEntries.Count <= 0)
		{
			return;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(OffMeshLinkEntity.StartPosition, 0.25f);
		Gizmos.color = ((m_climbRoute.CurrentIndex != -1) ? Color.blue : Color.green);
		Gizmos.DrawLine(OffMeshLinkEntity.StartPosition, m_climbRoute.ClimbRouteEntries[0].ClosestPoint);
		for (int i = 0; i < m_climbRoute.ClimbRouteEntries.Count; i++)
		{
			Gizmos.color = ((m_climbRoute.CurrentIndex != i || !m_climbStepCompleted) ? Color.blue : Color.green);
			Gizmos.DrawSphere(m_climbRoute.ClimbRouteEntries[i].ClosestPoint, 0.1f);
			if (i < m_climbRoute.ClimbRouteEntries.Count - 1)
			{
				Gizmos.color = ((m_climbRoute.CurrentIndex != i || m_climbStepCompleted) ? Color.blue : Color.green);
				Gizmos.DrawLine(m_climbRoute.ClimbRouteEntries[i].ClosestPoint, m_climbRoute.ClimbRouteEntries[i + 1].ClosestPoint);
			}
		}
		Gizmos.color = ((m_climbRoute.CurrentIndex != m_climbRoute.ClimbRouteEntries.Count - 1 || m_climbStepCompleted) ? Color.blue : Color.green);
		Gizmos.DrawLine(m_climbRoute.ClimbRouteEntries[m_climbRoute.ClimbRouteEntries.Count - 1].ClosestPoint, OffMeshLinkEntity.EndPosition);
		Gizmos.color = ((!m_climbRoute.IsLastEntry || !RouteCompleted) ? Color.blue : Color.green);
		Gizmos.DrawSphere(OffMeshLinkEntity.EndPosition, 0.25f);
	}

	public void CheckIsFinished()
	{
		CheckIsFinished(AnimationController.StateMachine.GetCurrentStateMachine());
	}

	public void CheckIsFinished(AnimatorStateMachineStatic.StateMachine _stateMachine)
	{
		if (LogicMain.CurrentTime < m_minAutoExitTime)
		{
			return;
		}
		while (_stateMachine != null)
		{
			if (_stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_JumpDown) || _stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_ClimbingSystem))
			{
				return;
			}
			_stateMachine = _stateMachine.ParentStateMachine;
		}
		base.IsFinished = m_hasStartedClimbing;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		CheckIsFinished();
		if (base.IsFinished)
		{
			return;
		}
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		float currentNormalizedOverhang = AnimationController.Animator.GetCurrentNormalizedOverhang();
		float b = Mathf.Max((m_targetClimbOverhang - 0.3f) / 1.7f, (!(m_targetClimbOverhang > 0f)) ? 0f : float.Epsilon);
		float value = Mathf.Lerp(currentNormalizedOverhang, b, LogicMain.FrameTime / (0.1f + LogicMain.FrameTime));
		AnimationController.Animator.SetCurrentNormalizedOverhang(value);
		AnimationController.Animator.SetCurrentMovementState((m_climbDirection != eDirection.Invalid && m_climbDirection != eDirection.Down) ? MovementState.Movement : MovementState.Idle);
		float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
		float to = Mathf.Min(currentSpeed, MaxSpeedAfterClimbing);
		AnimationController.Animator.SetCurrentSpeed(Damping.Damp(currentSpeed, to, 0.5f, 1f));
		if (m_correctFacing && (m_targetEntry != null || Vector3.Angle(m_faceDirection, AnimationController.Agent.transform.forward) > 5f))
		{
			AnimationController.FaceDirection(m_faceDirection, (!HorizontalJumpToWall) ? 0.1f : 0.3f);
		}
		else
		{
			m_correctFacing = false;
		}
		if (m_correctLandFacing)
		{
			AnimationController.FaceDirection(m_landFacing);
		}
		if (m_hasStartedClimbing)
		{
			float b2 = Mathf.Min(m_pitchAngle * PitchMultiplier, MaxPitch);
			m_currentPitch = Mathf.Lerp(m_currentPitch, b2, LogicMain.FrameTime / (PitchDamping + LogicMain.FrameTime));
			float num2 = Mathf.Clamp01((m_pitchEndTime - LogicMain.CurrentTime) * 2f);
			m_currentPitch = Mathf.Min(m_currentPitch, num2 * MaxPitch);
			Vector3 forward = AnimationController.NextRotation * Vector3.forward;
			forward.y = 0f;
			AnimationController.NextRotation = Quaternion.LookRotation(forward) * Quaternion.Euler(m_currentPitch, 0f, 0f);
			float currentEdgeDistanceX = AnimationController.Animator.GetCurrentEdgeDistanceX();
			currentEdgeDistanceX = Mathf.Lerp(currentEdgeDistanceX, m_targetEdgeDistanceX, LogicMain.FrameTime / (EdgeDistanceXDamping + LogicMain.FrameTime));
			AnimationController.Animator.SetCurrentEdgeDistanceX(currentEdgeDistanceX);
		}
		if (!m_reachedFirstPin && m_keepDistanceToWall)
		{
			AnimationController.KeepDistance(m_wallPoint, -m_faceDirection, TargetDistanceToWall, !m_hasStartedClimbing, KeepDistanceToWallDamping);
		}
		if (m_canJumpDown && m_climbDirection == eDirection.Down)
		{
			CancelClimbing(ECanceledBy.Player);
			return;
		}
		TryStartClimbStep();
		if (m_askMatch)
		{
			TryStartMatching();
		}
		if (Canceled)
		{
			Vector3 landPosition = m_landPosition;
			landPosition.y = AnimationController.NextPosition.y;
			AnimationController.CorrectPosition(landPosition, 0.2f);
		}
	}

	public void CancelClimbing(ECanceledBy _canceledBy)
	{
		Canceled = true;
		RouteCompleted = true;
		m_climbStepCompleted = true;
		m_minAutoExitTime = LogicMain.CurrentTime + 0.5f;
		m_pitchEndTime = LogicMain.CurrentTime + 1f;
		m_targetEntry = null;
		m_canJumpDown = false;
		m_skipTaskEndEvent = false;
		AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Animator.SetTrigger(AnimatorParameters.CancelClimbHash);
		ReactionType reactionType = ((_canceledBy != 0) ? ReactionType.CancelClimbByDamage : ReactionType.CancelClimbByPlayer);
		float num = AnimationController.NextPosition.y - m_landPosition.y;
		AnimationController.Animator.SetCurrentJumpDistanceY(num);
		AnimationController.Animator.SetCurrentJumpDistanceZ(0f);
		AnimationController.Animator.SetCurrentReactionType(reactionType);
		float distance = ParcourHelper.ClearDistanceForward(m_landPosition, m_faceDirection, 2f);
		AnimationController.Animator.SetCurrentEdgeDistanceZ(distance);
		Vector3 nextPosition = AnimationController.NextPosition;
		nextPosition.y = m_landPosition.y;
		if (_canceledBy == ECanceledBy.Player)
		{
			nextPosition += AnimationController.NextRotation * Vector3.forward * Mathf.Lerp(0f - MinFallDistanceFromWall, 0f - MaxFallDistanceFromWall, num);
		}
		NavMeshHit hit;
		if (NavMesh.SamplePosition(nextPosition, out hit, 1.5f, NavMeshLayers.WalkableLayerMaskPreset))
		{
			m_landPosition = hit.position;
		}
		m_pinData.Clear();
		AnimationController.Animator.InterruptMatchTarget(false);
		Destination = m_landPosition;
		PlayerHelper.Entity.SetLastFallHeight(num);
		m_landFacing = AnimationController.NextRotation * Vector3.forward;
		m_endFacing = m_landFacing;
		m_endPosition = m_landPosition;
	}

	private void TryStartClimbStep()
	{
		AssassinCharacter assassinCharacter = AnimationController.EntityId.TryGetEntity<AssassinCharacter>();
		if (!RouteCompleted && m_climbStepCompleted && ((!m_climbRoute.IsLastEntry && AutoClimb) || m_climbDirection == eDirection.Up || (assassinCharacter != null && assassinCharacter.GetIsApproachingTarget()) || (m_climbRoute.IsLastEntry && m_climbRoute.ClimbRouteEntries.Count == 1)))
		{
			StartClimbStep();
		}
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		if (!AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0))
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		m_askMatch = false;
		AnimationController.Animator.InterruptMatchTarget(false);
		AnimationController.Animator.MatchTarget(m_matchPoint, Quaternion.identity, m_matchBodyPart, s_weightMask, currentAnimatorStateInfo.normalizedTime % 1f, _matchEnd);
		float num = (_matchEnd - currentAnimatorStateInfo.normalizedTime % 1f) * currentAnimatorStateInfo.length;
		if (Canceled)
		{
			float num2 = Mathf.Max(AnimationController.NextPosition.y - m_matchPoint.y, 0f);
			float num3 = 6f;
			float num4 = 9.8f;
			float num5 = Mathf.Abs((Mathf.Sqrt(2f * num4 * num2 + num3 * num3) - num3) / num4);
			AnimationController.Animator.speed = Mathf.Clamp(num / num5, 0.01f, 5f);
		}
		float num6 = LogicMain.CurrentTime + num;
		for (int i = 0; i < m_pinData.Count; i++)
		{
			AnimatorIK.PinData pinData = m_pinData[i];
			if (!pinData.Enabled && pinData.EndTime > LogicMain.CurrentTime)
			{
				pinData.StartTime = Mathf.Min(pinData.StartTime, num6);
				pinData.EndTime = Mathf.Min(num6 + MaxPinDuration);
			}
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.ClimbStart:
			m_canJumpDown = true;
			break;
		case AnimatorEventType.ClimbEnd:
			m_canJumpDown = false;
			m_landPosition = m_endPosition;
			m_landFacing = m_endFacing;
			break;
		case AnimatorEventType.FindNewEdge:
			m_resetOverhang = true;
			if (m_waitForIdlePose)
			{
				StartClimbStep();
				m_waitForIdlePose = false;
			}
			if (m_resetOverhang)
			{
				m_targetClimbOverhang = m_climbOverhang;
				m_resetOverhang = false;
			}
			break;
		case AnimatorEventType.MatchEnd:
		{
			m_reachedFirstPin = true;
			for (int l = 0; l < m_pinData.Count; l++)
			{
				AnimatorIK.PinData pinData4 = m_pinData[l];
				if (!pinData4.Enabled)
				{
					pinData4.StartTime = Mathf.Min(pinData4.StartTime, LogicMain.CurrentTime);
					pinData4.EndTime = LogicMain.CurrentTime + MaxPinDuration;
					pinData4.Enabled = true;
				}
			}
			if (Canceled && m_cancelType == ParcourCancelEvent.ECancelType.Die)
			{
				base.IsFinished = true;
			}
			if (Canceled)
			{
				AnimationController.Animator.speed = 1f;
			}
			break;
		}
		case AnimatorEventType.PinStart:
			StartOrRefreshPins();
			break;
		case AnimatorEventType.PinEnd:
		{
			for (int i = 0; i < m_pinData.Count; i++)
			{
				AnimatorIK.PinData pinData = m_pinData[i];
				pinData.EndTime = 0f;
			}
			break;
		}
		case AnimatorEventType.SwitchToLeftHand:
		{
			m_matchPoint = GetMatchPoint(m_targetEntry, 0f);
			for (int m = 0; m < m_pinData.Count; m++)
			{
				AnimatorIK.PinData pinData5 = m_pinData[m];
				if (pinData5.AvatarIKGoal == AvatarIKGoal.LeftHand)
				{
					pinData5.EndTime = 0f;
				}
			}
			m_matchBodyPart = AvatarTarget.LeftHand;
			m_matchBone = HumanBodyBones.LeftHand;
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.LeftHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = m_matchPoint
			});
			break;
		}
		case AnimatorEventType.SwitchToTwoHandsFromLeft:
		{
			m_matchPoint = GetMatchPoint(m_targetEntry, (0f - ClimbRouteValues.HandSpacing) * 0.5f);
			for (int j = 0; j < m_pinData.Count; j++)
			{
				AnimatorIK.PinData pinData2 = m_pinData[j];
				if (pinData2.AvatarIKGoal == AvatarIKGoal.LeftHand)
				{
					pinData2.EndTime = 0f;
				}
			}
			m_matchBodyPart = AvatarTarget.LeftHand;
			m_matchBone = HumanBodyBones.LeftHand;
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.LeftHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = m_matchPoint
			});
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.RightHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = GetMatchPoint(m_targetEntry, ClimbRouteValues.HandSpacing * 0.5f)
			});
			break;
		}
		case AnimatorEventType.SwitchToRightHand:
		{
			m_matchPoint = GetMatchPoint(m_targetEntry, 0f);
			for (int num = 0; num < m_pinData.Count; num++)
			{
				AnimatorIK.PinData pinData7 = m_pinData[num];
				if (pinData7.AvatarIKGoal == AvatarIKGoal.RightHand)
				{
					pinData7.EndTime = 0f;
				}
			}
			m_matchBodyPart = AvatarTarget.RightHand;
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.RightHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = m_matchPoint
			});
			break;
		}
		case AnimatorEventType.SwitchToTwoHandsFromRight:
		{
			m_matchPoint = GetMatchPoint(m_targetEntry, ClimbRouteValues.HandSpacing * 0.5f);
			for (int n = 0; n < m_pinData.Count; n++)
			{
				AnimatorIK.PinData pinData6 = m_pinData[n];
				if (pinData6.AvatarIKGoal == AvatarIKGoal.RightHand)
				{
					pinData6.EndTime = 0f;
				}
			}
			m_matchBodyPart = AvatarTarget.RightHand;
			m_matchBone = HumanBodyBones.RightHand;
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.LeftHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = GetMatchPoint(m_targetEntry, (0f - ClimbRouteValues.HandSpacing) * 0.5f)
			});
			m_pinData.Add(new AnimatorIK.PinData
			{
				AvatarIKGoal = AvatarIKGoal.RightHand,
				EndTime = LogicMain.CurrentTime + 10f,
				StartTime = LogicMain.CurrentTime + 10f,
				Point = m_matchPoint
			});
			break;
		}
		case AnimatorEventType.SwitchToLeftFoot:
			m_matchPoint = GetMatchPoint(m_targetEntry, 0f);
			m_matchBodyPart = AvatarTarget.LeftFoot;
			m_matchBone = HumanBodyBones.LeftFoot;
			break;
		case AnimatorEventType.SwitchToRightFoot:
			m_matchPoint = GetMatchPoint(m_targetEntry, 0f);
			m_matchBodyPart = AvatarTarget.RightFoot;
			m_matchBone = HumanBodyBones.RightFoot;
			break;
		case AnimatorEventType.MatchStart:
		{
			CompleteClimbStep();
			TryStartClimbStep();
			for (int k = 0; k < m_pinData.Count; k++)
			{
				AnimatorIK.PinData pinData3 = m_pinData[k];
				if (LogicMain.CurrentTime >= pinData3.StartTime)
				{
					pinData3.EndTime = 0f;
				}
			}
			m_askMatch = true;
			m_hasStartedClimbing = true;
			TryStartMatching();
			break;
		}
		case AnimatorEventType.TaskEnd:
			if (RouteCompleted)
			{
				if (m_skipTaskEndEvent)
				{
					m_skipTaskEndEvent = false;
				}
				else
				{
					base.IsFinished = true;
				}
			}
			break;
		case AnimatorEventType.TurnEnd:
			if (m_waitForTurn)
			{
				m_waitForTurn = false;
				StartClimbing();
			}
			break;
		}
		return false;
	}

	private void StartOrRefreshPins()
	{
		for (int i = 0; i < m_pinData.Count; i++)
		{
			AnimatorIK.PinData pinData = m_pinData[i];
			if (pinData.Enabled)
			{
				bool flag = false;
				for (int j = i + 1; j < m_pinData.Count; j++)
				{
					if (m_pinData[j].AvatarIKGoal == pinData.AvatarIKGoal)
					{
						flag = true;
						break;
					}
				}
				if (!flag && LogicMain.CurrentTime < pinData.EndTime)
				{
					pinData.EndTime = LogicMain.CurrentTime + MaxPinDuration;
				}
			}
			else
			{
				pinData.StartTime = LogicMain.CurrentTime - 1f;
				pinData.EndTime = LogicMain.CurrentTime + MaxPinDuration;
				pinData.Enabled = true;
			}
		}
	}

	private Vector3 GetMatchPoint(ClimbRouteEntry entry, float offset)
	{
		if (RouteCompleted)
		{
			return m_landPosition;
		}
		if (entry == null)
		{
			return m_matchPoint;
		}
		Vector3 vector = Slide(entry.ClosestPoint, entry.Edge, !HorizontalJumpToWall);
		Vector3 vector2 = entry.Edge.Normals[0];
		vector2.y = 0f;
		vector2.Normalize();
		Vector3 vector3 = entry.Edge.End - entry.Edge.Start;
		vector3.Normalize();
		float num = Mathf.Sign(Vector3.Dot(vector3, Vector3.Cross(vector2, Vector3.up)));
		vector3 *= num;
		float num2 = ((!(num * Mathf.Sign(offset) > 0f)) ? (entry.Edge.Start - vector).magnitude : (entry.Edge.End - vector).magnitude);
		offset = Mathf.Clamp(offset, 0f - num2, num2);
		Vector3 vector4 = ((!HorizontalJumpToWall) ? AnimationController.HandOffset : HorizontalJumpToWallHandOffset);
		return vector + vector3 * offset + Quaternion.LookRotation(-vector2) * vector4;
	}

	private Vector3 Slide(Vector3 closestPoint, EdgeObject edge, bool slideFirstPoint = true)
	{
		Vector3 nextPosition = AnimationController.NextPosition;
		nextPosition.y = closestPoint.y;
		closestPoint = ((!slideFirstPoint) ? edge.ClosestPoint(closestPoint) : edge.ClosestPoint(nextPosition));
		float magnitude = (closestPoint - edge.Start).magnitude;
		float num = ClimbRouteValues.HandSpacing * 0.5f;
		float num2 = edge.Length - ClimbRouteValues.HandSpacing * 0.5f;
		return Vector3.Lerp(t: ((!(num2 < num)) ? Mathf.Clamp(magnitude, num, num2) : ((num + num2) * 0.5f)) / edge.Length, a: edge.Start, b: edge.End);
	}

	private void StartClimbStep()
	{
		m_climbStepCompleted = false;
		m_waitForIdlePose = false;
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Up);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Climb);
		if (m_climbRoute.IsLastEntry)
		{
			AnimationController.Animator.SetCurrentClimbingStance(m_freeHang ? 2 : 0);
			AnimationController.Animator.SetCurrentRoofEdge(true);
			AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
			m_targetEdgeDistanceX = 0f;
			Vector3 vector = OffMeshLinkEntity.EndPosition;
			Vector3 vector2 = vector - m_matchPoint;
			float num = 0f - vector2.y;
			vector2.y = 0f;
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(vector, vector - vector2, out _hit, -1))
			{
				Vector3 vector3 = AnimationController.NextRotation * Vector3.forward;
				vector3.y = 0f;
				vector3.Normalize();
				Vector3 position = _hit.position + vector3 * Vector3.Dot(vector3, vector2) * (_hit.distance / vector2.magnitude);
				if (NavigationHelper.CascadingSamplePosition(position, out _hit, -1))
				{
					vector = _hit.position;
					vector2 = vector - m_matchPoint;
					num = 0f - vector2.y;
					vector2.y = 0f;
				}
			}
			float distance = ParcourHelper.ClearDistanceForward(vector, m_faceDirection, 2f);
			if (num > MinHeightToJumpDown)
			{
				AnimationController.Animator.SetCurrentJumpDistanceY(num);
				AnimationController.Animator.SetCurrentJumpDistanceZ(vector2.magnitude);
				AnimationController.Animator.SetBool(AnimatorParameters.RoofEdgeJumpDownHash, true);
				m_skipTaskEndEvent = true;
				JumpingDownFromWall = true;
				JumpingDownHeight = num;
				m_endFacing = vector2.normalized;
				m_correctLandFacing = true;
				AnimationController.Animator.SetCurrentEdgeDistanceZ(0f);
			}
			else
			{
				AnimationController.Animator.SetCurrentEdgeDistanceZ(distance);
			}
			m_targetEntry = null;
			m_endPosition = vector;
			RouteCompleted = true;
			m_climbStepCompleted = true;
			m_minAutoExitTime = LogicMain.CurrentTime + 0.5f;
			m_pitchEndTime = LogicMain.CurrentTime + 1f;
			return;
		}
		ClimbRouteEntry currentEntry = m_climbRoute.CurrentEntry;
		ClimbRouteEntry climbRouteEntry = (m_targetEntry = m_climbRoute.NextEntry);
		if (m_targetEntry == null)
		{
		}
		m_faceDirection = -m_targetEntry.Edge.Normals[0];
		HorizontalJumpToWall = currentEntry == null && (climbRouteEntry.ClosestPoint - m_matchPoint).Mask(new Vector3(1f, 0f, 1f)).magnitude > MinDistanceForHorizontalJumpToWall;
		Vector3 vector4 = Slide(climbRouteEntry.ClosestPoint, climbRouteEntry.Edge, !HorizontalJumpToWall);
		Vector3 lhs = vector4 - m_matchPoint;
		Vector3 vector5 = climbRouteEntry.Edge.Normals[0];
		vector5.y = 0f;
		vector5.Normalize();
		Vector3 rhs = Vector3.Cross(vector5, Vector3.up);
		float num2 = Vector3.Dot(lhs, vector5);
		float num3 = (m_targetEdgeDistanceX = Vector3.Dot(lhs, rhs));
		m_climbOverhang = num2;
		if (m_resetOverhang)
		{
			m_targetClimbOverhang = m_climbOverhang;
			m_resetOverhang = false;
		}
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(vector4 + Vector3.down * OverhangCheckOffsetY + vector5, -vector5, out hitInfo, 1f + MinOverhang, Layers.StaticCollidersLayersMaskPreset);
		if (flag)
		{
			float num4 = Mathf.Clamp01(1f - hitInfo.distance);
			float pitchAngle = 90f - Mathf.Atan(OverhangCheckOffsetY / num4) * 57.29578f;
			m_pitchAngle = pitchAngle;
		}
		else
		{
			m_pitchAngle = 0f;
		}
		if (currentEntry == null)
		{
			AnimationController.Animator.SetCurrentEdgeDistanceX(m_targetEdgeDistanceX);
			if (!flag)
			{
				AnimationController.Animator.SetCurrentClimbingStance(2);
				m_freeHang = true;
			}
			else
			{
				AnimationController.Animator.SetCurrentClimbingStance(0);
			}
			m_lastEdgeDistance = Mathf.Clamp(lhs.y, AnimationController.GroundJump.Min, AnimationController.GroundJump.Max);
			m_lastEdgeDistance = Mathf.Clamp(m_lastEdgeDistance, 0f, 4.49f);
			AnimationController.Animator.SetCurrentEdgeDistance(m_lastEdgeDistance);
			m_catClimb = false;
			if (HorizontalJumpToWall)
			{
				AnimationController.Animator.SetCurrentLinkType(eLinkType.JumpHorizontalToWall);
				AnimationController.Animator.SetCurrentJumpDistanceY(Mathf.Clamp(lhs.y, 0f, 0f));
				AnimationController.Animator.SetCurrentJumpDistanceZ(Mathf.Clamp(0f - num2, 0.01f, 7.99f));
			}
			if (m_climbRoute.ClimbRouteEntries.Count == 1)
			{
				AnimationController.Animator.SetCurrentRoofEdge(true);
			}
			return;
		}
		m_lastEdgeDistance = Mathf.Clamp(lhs.y, AnimationController.WallJump.Min, AnimationController.WallJump.Max);
		m_climbOverhang = Mathf.Max(m_climbOverhang, 0.01f);
		m_targetClimbOverhang = m_climbOverhang;
		AnimationController.Animator.SetCurrentNormalizedOverhang(Mathf.Max(AnimationController.Animator.GetCurrentNormalizedOverhang(), 0.01f));
		AnimationController.Animator.SetCurrentRoofEdge(false);
		if (!flag || num2 > MinOverhang)
		{
			if (m_catClimb)
			{
				AnimationController.Animator.SetCurrentClimbingStance(0);
				AnimationController.Animator.SetCurrentEdgeDistance(m_lastEdgeDistance);
				m_catClimb = false;
				m_waitForIdlePose = true;
			}
			else
			{
				AnimationController.Animator.SetCurrentClimbingStance(2);
				m_lastEdgeDistance = Mathf.Clamp(m_lastEdgeDistance, 0.91f, 2.49f);
				AnimationController.Animator.SetCurrentEdgeDistance(m_lastEdgeDistance);
				m_freeHang = true;
			}
			return;
		}
		m_freeHang = false;
		if (m_catClimb && ((num3 > 0.1f && m_lastClimbRightHand) || (num3 < -0.1f && !m_lastClimbRightHand)))
		{
			m_catClimb = false;
			AnimationController.Animator.SetCurrentClimbingStance(1);
			AnimationController.Animator.SetCurrentEdgeDistance(m_lastEdgeDistance);
			m_waitForIdlePose = true;
			return;
		}
		if (m_catClimb)
		{
			AnimationController.Animator.SetClimbRightHand(!m_lastClimbRightHand);
		}
		else
		{
			AnimationController.Animator.SetClimbRightHand(num3 > 0f);
			m_lastClimbRightHand = !AnimationController.Animator.GetClimbRightHand();
		}
		AnimationController.Animator.SetCurrentClimbingStance(4);
		m_lastEdgeDistance = Mathf.Clamp(m_lastEdgeDistance, 0f, 2.09f);
		AnimationController.Animator.SetCurrentEdgeDistance(m_lastEdgeDistance);
		m_catClimb = true;
	}

	private void CompleteClimbStep()
	{
		m_climbStepCompleted = true;
		m_lastClimbRightHand = AnimationController.Animator.GetClimbRightHand();
		eDirection direction = ((!RouteCompleted) ? eDirection.Invalid : eDirection.Up);
		AnimationController.Animator.SetCurrentClimbDirection(direction);
		m_climbRoute.AdvanceToNextEntry();
		if (m_climbRoute.IsLastEntry)
		{
			AnimationController.Animator.SetCurrentClimbingStance(m_freeHang ? 2 : 0);
		}
	}
}
