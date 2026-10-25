using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Motion;
using UbiCore.Messenger;
using UnityEngine;

public class AnimationStateJumpDown : AnimationStateBase, IAnimation, IParkourAnimation, IAnimatorEventListener
{
	public static bool CompleteOnTaskEndEvent = true;

	public static float MatchYOffset = 0.2f;

	public static float TurnMinAngle = 90f;

	private Vector3 m_startPosition;

	private Vector3 m_endPosition;

	private Vector3 m_faceDirection;

	private float m_fallDuration;

	private Transform m_matchTransform;

	private MovementState m_nextMovementState = MovementState.Movement;

	private AvatarTarget m_matchBodyPart = AvatarTarget.LeftHand;

	private bool m_jumpCompleted;

	private bool m_jumpStarted;

	private float m_completeJumpTime;

	private float m_jumpStartTime;

	private bool m_askMatch;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	private static MatchTargetWeightMask s_weightMask = new MatchTargetWeightMask(Vector3.one, 0f);

	public Vector3 Destination
	{
		get
		{
			return m_endPosition;
		}
	}

	public float Height
	{
		get
		{
			return (m_startPosition - m_endPosition).y;
		}
	}

	public float LandTime
	{
		get
		{
			return m_completeJumpTime;
		}
	}

	public float StartTime
	{
		get
		{
			return m_jumpStartTime;
		}
	}

	public AnimationStateJumpDown(AnimationController _animationController, Vector3 _startPosition, Vector3 _endPosition)
		: base(AnimationStateType.JumpDown, _animationController)
	{
		m_startPosition = _startPosition;
		m_endPosition = _endPosition;
		m_faceDirection = m_endPosition - m_startPosition;
		m_faceDirection.y = 0f;
		m_faceDirection.Normalize();
		if (this.GetHandlesParkourInputEvents())
		{
			Messenger.Instance.AddListener<ParcourInputEvent>(OnParcourInputEvent);
		}
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public void OnParcourInputEvent(ParcourInputEvent _event)
	{
		m_nextMovementState = ((ParcourHelper.ComputeRelativeDirection(_event.Direction, m_faceDirection) != eDirection.Invalid) ? MovementState.Movement : MovementState.Idle);
		if (!m_waitForTurn)
		{
			AnimationController.Animator.SetCurrentMovementState(m_nextMovementState);
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.ResetPath();
		float num = Vector3.Angle(m_faceDirection, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_faceDirection, AnimationController.transform.right));
		if (Mathf.Abs(num) > TurnMinAngle)
		{
			float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
			float speed = Mathf.Min(currentSpeed, 0.4f);
			AnimationController.Animator.SetCurrentSpeed(speed);
			m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
			m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
			AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			m_waitForTurn = true;
		}
		else
		{
			StartJump();
		}
	}

	private void StartJump()
	{
		Vector3 vector = m_endPosition - m_startPosition;
		vector.y = 0f;
		float num = m_startPosition.y - m_endPosition.y;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.JumpDown);
		AnimationController.Animator.SetCurrentLinkLength((m_startPosition - m_endPosition).magnitude);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.Animator.SetCurrentJumpDistanceY(num);
		AnimationController.Animator.SetCurrentJumpDistanceZ(vector.magnitude);
		AnimationController.Animator.SetCurrentMovementState(m_nextMovementState);
		float distance = ParcourHelper.ClearDistanceForward(m_endPosition, m_faceDirection, 2f);
		AnimationController.Animator.SetCurrentEdgeDistanceZ(distance);
		m_fallDuration = Mathf.Sqrt(Mathf.Abs(num / -9.8f));
		m_completeJumpTime = LogicMain.CurrentTime + 3f;
		m_jumpStartTime = LogicMain.CurrentTime;
		m_waitForTurn = false;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.speed = 1f;
		AnimationController.Agent.Warp(AnimationController.NextPosition);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInputEvent);
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
	}

	public void CheckIsFinished()
	{
		CheckIsFinished(AnimationController.StateMachine.GetCurrentStateMachine());
	}

	public void CheckIsFinished(AnimatorStateMachineStatic.StateMachine _stateMachine)
	{
		if (!m_jumpCompleted)
		{
			return;
		}
		while (_stateMachine != null)
		{
			if (_stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_JumpDown))
			{
				return;
			}
			_stateMachine = _stateMachine.ParentStateMachine;
		}
		base.IsFinished = true;
	}

	public override void OnUpdate()
	{
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		if (LogicMain.CurrentTime > m_completeJumpTime && !m_jumpCompleted)
		{
			AnimationController.Animator.speed = 1f;
			AnimationController.Animator.SetCurrentMovementState(m_nextMovementState);
			CompleteJump();
		}
		CheckIsFinished();
		if (!base.IsFinished)
		{
			AnimationController.FaceDirection(m_faceDirection);
			if (AnimationController.Animator.isMatchingTarget && m_matchTransform != null)
			{
				Vector3 position = m_matchTransform.position;
				Vector3 vector = m_endPosition - position;
			}
			if (m_askMatch)
			{
				TryStartMatching();
			}
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		CheckIsFinished(_event.StateMachine);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.SwitchToLeftHand:
		case AnimatorEventType.SwitchToTwoHandsFromLeft:
			m_matchBodyPart = AvatarTarget.LeftHand;
			break;
		case AnimatorEventType.SwitchToRightHand:
		case AnimatorEventType.SwitchToTwoHandsFromRight:
			m_matchBodyPart = AvatarTarget.RightHand;
			break;
		case AnimatorEventType.SwitchToLeftFoot:
			m_matchBodyPart = AvatarTarget.LeftFoot;
			break;
		case AnimatorEventType.SwitchToRightFoot:
			m_matchBodyPart = AvatarTarget.RightFoot;
			break;
		case AnimatorEventType.MatchStart:
			AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
			m_askMatch = true;
			m_jumpStarted = true;
			TryStartMatching();
			PlayerHelper.Entity.SetLastFallHeight(Height);
			break;
		case AnimatorEventType.MatchEnd:
			if (m_jumpStarted)
			{
				AnimationController.Animator.speed = 1f;
				if (!m_jumpCompleted)
				{
					CompleteJump();
				}
			}
			break;
		case AnimatorEventType.PinEnd:
			if (m_jumpStarted)
			{
				AnimationController.Animator.speed = 1f;
				if (!m_jumpCompleted)
				{
					CompleteJump();
				}
			}
			break;
		case AnimatorEventType.TaskEnd:
			if (m_jumpCompleted)
			{
				base.IsFinished |= CompleteOnTaskEndEvent;
			}
			break;
		case AnimatorEventType.TurnEnd:
			if (m_waitForTurn)
			{
				StartJump();
			}
			m_waitForTurn = false;
			break;
		}
		return false;
	}

	private void CompleteJump()
	{
		m_jumpCompleted = true;
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		bool matchTargetData = AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0);
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		if (matchTargetData)
		{
			m_completeJumpTime = LogicMain.CurrentTime + (_matchEnd - currentAnimatorStateInfo.normalizedTime) * currentAnimatorStateInfo.length;
			m_matchTransform = AnimationController.Agent.transform.FindChildRecursive(m_matchBodyPart.ToString());
			AnimationController.Animator.MatchTarget(m_endPosition + Vector3.up * MatchYOffset, Quaternion.identity, m_matchBodyPart, s_weightMask, _matchStart, _matchEnd);
			if (m_fallDuration > 0.5f)
			{
				AnimationController.Animator.speed = Mathf.Min(AnimationController.Animator.speed, (_matchEnd - _matchStart) * currentAnimatorStateInfo.length / m_fallDuration);
			}
			m_askMatch = false;
			float distance = ParcourHelper.ClearDistanceForward(m_endPosition, m_faceDirection, 2f);
			AnimationController.Animator.SetCurrentEdgeDistanceZ(distance);
		}
	}
}
