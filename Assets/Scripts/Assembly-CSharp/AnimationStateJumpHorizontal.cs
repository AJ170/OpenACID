using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Motion;
using UbiCore.Messenger;
using UnityEngine;

public class AnimationStateJumpHorizontal : AnimationStateBase, IAnimation, IParkourAnimation, IAnimatorEventListener
{
	public static bool CompleteOnTaskEndEvent = true;

	public static float MatchYOffset = 0f;

	private Vector3 m_startPosition;

	private Vector3 m_endPosition;

	private Vector3 m_faceDirection;

	private Transform m_matchTransform;

	private AvatarTarget m_matchBodyPart = AvatarTarget.LeftHand;

	private bool m_jumpCompleted;

	private float m_completeJumpTime;

	private bool m_askMatch;

	private float m_targetLinkLength;

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

	public bool HasFallFinished
	{
		get
		{
			return m_jumpCompleted;
		}
	}

	public AnimationStateJumpHorizontal(AnimationController _animationController, Vector3 _startPosition, Vector3 _endPosition)
		: base(AnimationStateType.JumpHorizontal, _animationController)
	{
		m_startPosition = _startPosition;
		m_endPosition = _endPosition;
		m_faceDirection = m_endPosition - m_startPosition;
		m_faceDirection.y = 0f;
		m_faceDirection.Normalize();
	}

	public void OnParcourInputEvent(ParcourInputEvent _event)
	{
		AnimationController.Animator.SetCurrentMovementState((ParcourHelper.ComputeRelativeDirection(_event.Direction, m_faceDirection) != eDirection.Invalid) ? MovementState.Movement : MovementState.Idle);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		if (this.GetHandlesParkourInputEvents())
		{
			Messenger.Instance.AddListener<ParcourInputEvent>(OnParcourInputEvent);
		}
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		Vector3 vector = m_endPosition - m_startPosition;
		vector.y = 0f;
		float distance = m_startPosition.y - m_endPosition.y;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.JumpHorizontal);
		m_targetLinkLength = (m_startPosition - m_endPosition).magnitude;
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.Animator.SetCurrentJumpDistanceY(distance);
		AnimationController.Animator.SetCurrentJumpDistanceZ(vector.magnitude);
		m_completeJumpTime = float.MaxValue;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.speed = 1f;
		Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInputEvent);
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
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
			if (_stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_JumpHorizontal))
			{
				return;
			}
			_stateMachine = _stateMachine.ParentStateMachine;
		}
		base.IsFinished = true;
	}

	public override void OnUpdate()
	{
		if (LogicMain.CurrentTime > m_completeJumpTime && !m_jumpCompleted)
		{
			m_jumpCompleted = true;
		}
		float currentLinkLength = AnimationController.Animator.GetCurrentLinkLength();
		AnimationController.Animator.SetCurrentLinkLength(Damping.Damp(currentLinkLength, m_targetLinkLength, 0.1f, 1f));
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

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		bool matchTargetData = AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0);
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		if (matchTargetData)
		{
			AnimationController.Animator.MatchTarget(m_endPosition + Vector3.up * MatchYOffset, Quaternion.identity, m_matchBodyPart, s_weightMask, currentAnimatorStateInfo.normalizedTime, _matchEnd);
			m_askMatch = false;
			m_completeJumpTime = LogicMain.CurrentTime + (_matchEnd - currentAnimatorStateInfo.normalizedTime) * currentAnimatorStateInfo.length;
		}
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_JumpHorizontal))
		{
			AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
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
			m_matchTransform = AnimationController.Agent.transform.FindChildRecursive(m_matchBodyPart.ToString());
			m_askMatch = true;
			TryStartMatching();
			PlayerHelper.Entity.SetLastFallHeight(Height);
			break;
		case AnimatorEventType.MatchEnd:
			m_jumpCompleted = true;
			break;
		case AnimatorEventType.TaskEnd:
			if (m_jumpCompleted)
			{
				base.IsFinished |= CompleteOnTaskEndEvent;
			}
			break;
		}
		return false;
	}
}
