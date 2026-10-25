using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

public class AnimationTaskIdle : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public IdleType IdleType { get; set; }

		public IdleType IdleTypeOnTransition { get; set; }

		public AnimatorStateMachineStatic.StateHashValue CrossFadeToState { get; set; }

		public float CrossFadeDuration { get; set; }

		public int AnimatorNpcId { get; set; }

		public Parameters()
			: base(AnimationTaskType.Idle)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Idle;

	private const float ForceIdleInterval = 2f;

	public float PushedVelocityThreshold = 1f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private IdleType m_idleType = IdleType.Invalid;

	private IdleType m_idleTypeOnTransition = IdleType.Invalid;

	private int m_animatorNpcId;

	private bool m_exitCalled;

	private bool m_isBeingPushed;

	private SimpleGameTimer m_forceIdleTimer = new SimpleGameTimer();

	private bool m_canBePushed;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Idle;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskIdle()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskIdle(Parameters _parameters)
		: base(_parameters)
	{
		m_idleType = _parameters.IdleType;
		m_idleTypeOnTransition = _parameters.IdleTypeOnTransition;
		m_animatorNpcId = _parameters.AnimatorNpcId;
		if (!_parameters.CrossFadeToState.IsInvalid)
		{
			AnimationController.Animator.CrossFade(_parameters.CrossFadeToState.NameHash, _parameters.CrossFadeDuration);
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		SetIdleType(m_idleType, m_idleTypeOnTransition, m_animatorNpcId);
		if (AnimationController.Agent.enabled)
		{
			AnimationController.Agent.Stop();
			AnimationController.Agent.ResetPath();
			AnimationController.Agent.velocity = Vector3.zero;
		}
	}

	public void OnUpdateIdleType(IdleType _idleType, IdleType _idleTypeOnTransition)
	{
		if (m_idleType != _idleType)
		{
			SetIdleType(_idleType, _idleTypeOnTransition, m_animatorNpcId);
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_forceIdleTimer.IsFinished)
		{
			AnimatorStateMachineStatic.StateMachine currentStateMachine = AnimationController.StateMachine.GetCurrentStateMachine();
			ForceIdleIfWalk(currentStateMachine);
			m_forceIdleTimer.Start(2f);
		}
		float magnitude = AnimationController.Agent.velocity.magnitude;
		if (!m_isBeingPushed && m_canBePushed && magnitude > PushedVelocityThreshold && AnimationController.StateMachineStatic.StateMachines.ContainsKey(AnimationStateMachineHashes.StateMachine_PushedByWalk.NameHash) && !AnimationController.Animator.IsInTransition(0) && AnimationController.StateMachine.GetCurrentState() != AnimationStateHashes.State_Npc_Idle_Sit_Crossed_Legs.NameHash)
		{
			m_isBeingPushed = true;
			m_canBePushed = false;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetPushedByWalk);
			AnimationController.Animator.SetTargetDirection(AnimationHelper.DetermineDirection6Ways(AnimationController.Agent.transform.forward, AnimationController.Agent.velocity));
		}
		if (magnitude < PushedVelocityThreshold * 0.75f)
		{
			m_canBePushed = true;
		}
		if (!AnimationController.CorrectPosition())
		{
			if (!AnimationController.Agent.enabled)
			{
				AnimationController.Agent.Warp(AnimationController.Agent.transform.position);
				AnimationController.Agent.enabled = true;
			}
			AnimationController.Agent.updatePosition = true;
			AnimationController.Agent.updateRotation = true;
		}
		if (AnimationController.IsLeavingCrouch() && AnimationController.Animator.GetCurrentIdleType() == IdleType.Crouch)
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.LowProfile);
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		m_exitCalled = true;
		if (AnimationController.ReferenceIdleType == IdleType.Invalid)
		{
			IdleType type = m_idleTypeOnTransition;
			if (m_idleTypeOnTransition == IdleType.Invalid && AnimationController.EntityId.Entity is NpcCharacter)
			{
				type = AnimationController.EntityId.Entity.Cast<NpcCharacter>().NpcDefinition.Definition.TransitionIdleType;
			}
			AnimationController.Animator.SetCurrentIdleType(type);
		}
		if (m_isBeingPushed && AnimationController.Animator.GetCurrentReactionType() == ReactionType.GetPushedByWalk)
		{
			m_isBeingPushed = false;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		}
	}

	public override bool CanCancel()
	{
		return true;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (m_isBeingPushed && _event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_PushedByWalk))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		}
		if (!m_exitCalled && !TrySetFinalIdleType(_event.StateMachine))
		{
			ForceIdleIfWalk(_event.StateMachine);
		}
	}

	private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (m_isBeingPushed && _event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_PushedByWalk))
		{
			m_isBeingPushed = false;
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
	}

	public override string ToString()
	{
		return string.Format("{0} ({1}) itm:{2} it:{3}", GetType().ToString(), base.ID, m_idleType, AnimationController.Animator.GetCurrentIdleType());
	}

	private void SetIdleType(IdleType _idleType, IdleType _idleTypeOnTransition, int _animatorNpcId)
	{
		m_idleTypeOnTransition = _idleTypeOnTransition;
		m_idleType = _idleType;
		AnimationController.ApplyRootMotion = m_idleType.HasRootMotion();
		AnimationController.Animator.SetCurrentNpcId(_animatorNpcId);
		ForceIdle();
		m_forceIdleTimer.Start(2f);
	}

	private void ForceIdle()
	{
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimatorStateMachineStatic.StateMachine currentStateMachine = AnimationController.StateMachine.GetCurrentStateMachine();
		if (m_idleTypeOnTransition == IdleType.Invalid)
		{
			if (m_idleType == IdleType.Crouch)
			{
				AnimationController.Animator.SetCurrentSpeed(AnimationController.GetSpeedFactor(EMovementSpeed.Walk));
			}
			AnimationController.Animator.SetCurrentIdleType(m_idleType);
		}
		else if (!TrySetFinalIdleType(currentStateMachine) && AnimationController.ReferenceIdleType == IdleType.Invalid)
		{
			AnimationController.Animator.SetCurrentIdleType(m_idleTypeOnTransition);
		}
	}

	private bool TrySetFinalIdleType(AnimatorStateMachineStatic.StateMachine _stateMachine)
	{
		if (m_idleType != m_idleTypeOnTransition && _stateMachine != null && _stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait))
		{
			if (AnimationController.ReferenceIdleType == IdleType.Invalid)
			{
				AnimationController.Animator.SetCurrentIdleType(m_idleType);
			}
			return true;
		}
		return false;
	}

	private void ForceIdleIfWalk(AnimatorStateMachineStatic.StateMachine _stateMachine)
	{
		if (m_idleType != m_idleTypeOnTransition && _stateMachine != null && _stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_WalkRun))
		{
			ForceIdle();
		}
	}
}
