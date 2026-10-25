using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public abstract class AnimationStateHideoutEnterBase : AnimationStateBase, IAnimatorEventListener
{
	public static float TurnMinAngle = 90f;

	private bool m_RootMotion;

	protected Vector3 m_CurrentMatchTarget = Vector3.zero;

	protected HideoutObject m_hideoutObject;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	protected Vector3 m_faceDirection;

	private bool m_askMatch;

	private bool m_jumpPhase;

	private AvatarTarget m_CurrentMatchTargetIK;

	private static MatchTargetWeightMask s_WeightMask = new MatchTargetWeightMask(Vector3.one, 0f);

	public AnimationStateHideoutEnterBase(AnimationStateType _stateType, AnimationController _animationController, HideoutObject _hideout, InteractionLocators.Locator _selectedLocator)
		: base(_stateType, _animationController)
	{
		m_jumpPhase = false;
		m_hideoutObject = _hideout;
		m_CurrentMatchTarget = _selectedLocator.Step.transform.position;
		m_faceDirection = _selectedLocator.Orientation * Vector3.forward;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateExited += OnStateExited;
		AnimationController.DataController.RegisterEventListener(this);
	}

	protected Vector3 Slide(Vector3 edgePoint, Vector3 targetCenter, Vector3 referencePoint, float range)
	{
		targetCenter.y = edgePoint.y;
		referencePoint.y = edgePoint.y;
		Vector3 normalized = (targetCenter - edgePoint).normalized;
		Vector3 vector = Vector3.Cross(normalized, Vector3.up);
		Vector3 lhs = targetCenter - referencePoint;
		float num = Vector3.Dot(lhs, normalized);
		float num2 = 0f - Vector3.Dot(lhs, vector);
		float num3 = (targetCenter - edgePoint).magnitude / num;
		float value = num3 * num2;
		value = Mathf.Clamp(value, 0f - range, range);
		return edgePoint + vector * value;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_RootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.Stop();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.enabled = false;
		float num = Vector3.Angle(m_faceDirection, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_faceDirection, AnimationController.transform.right));
		if (Mathf.Abs(num) > TurnMinAngle)
		{
			float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
			float speed = Mathf.Min(currentSpeed, 0f);
			AnimationController.Animator.SetCurrentSpeed(speed);
			m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
			m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
			m_waitForTurn = true;
		}
		else
		{
			StartEnterHideout();
		}
	}

	private void StartEnterHideout()
	{
		AnimationController.Animator.SetCurrentActionType(ActionType.ToHayStack);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_CurrentMatchTargetIK = ((!AnimationController.GetRightFoot()) ? AvatarTarget.LeftFoot : AvatarTarget.RightFoot);
	}

	public override void OnUpdate()
	{
		if (base.IsFinished)
		{
			return;
		}
		base.OnUpdate();
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		AnimationController.FaceDirection(m_faceDirection);
		if (m_askMatch)
		{
			TryStartMatching();
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
			AnimationController.Animator.MatchTarget(m_CurrentMatchTarget, Quaternion.identity, m_CurrentMatchTargetIK, s_WeightMask, _matchStart, Mathf.Max(currentAnimatorStateInfo.normalizedTime + 0.01f, _matchEnd));
			m_askMatch = false;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = m_RootMotion;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.NextPosition = GetHideoutCenter();
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
	}

	public override bool CanCancel()
	{
		return false;
	}

	protected virtual void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_HayStack_Idle))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}
	}

	private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_HayStackEnter_Left) || _event.State.CompareNameHash(AnimationStateHashes.State_HayStackEnter_Right))
		{
			AnimationController.StateMachine.StateExited -= OnStateExited;
			OnStartJump();
		}
	}

	protected abstract Vector3 GetHideoutCenter();

	protected virtual void OnStartJump()
	{
	}

	public virtual bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.TurnEnd:
			if (m_waitForTurn)
			{
				StartEnterHideout();
			}
			m_waitForTurn = false;
			break;
		case AnimatorEventType.MatchStart:
			if (m_jumpPhase)
			{
				m_CurrentMatchTarget = GetHideoutCenter();
				m_CurrentMatchTargetIK = AvatarTarget.Root;
			}
			m_askMatch = true;
			TryStartMatching();
			break;
		case AnimatorEventType.MatchEnd:
			m_jumpPhase = true;
			break;
		}
		return false;
	}
}
