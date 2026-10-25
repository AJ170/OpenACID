using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationStatePushTarget : AnimationStateBase, IAnimatorEventListener
{
	private readonly EntityId m_instigator;

	private AnimationController.Direction m_targetDir;

	private bool m_startedReaction;

	public AnimationStatePushTarget(AnimationController _animationController, EntityId _instigator)
		: base(AnimationStateType.PushTarget, _animationController)
	{
		m_instigator = _instigator;
		CalcTargetDirectionAndRotation();
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_instigator.GameEntity.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.SetCurrentIdleType(AnimationController.ReferenceIdleType);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		if (m_instigator.IsValid && m_instigator.GameEntity.IsAttached)
		{
			m_instigator.GameEntity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.ReactionStart)
		{
			OnStartReaction();
		}
		return false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimationController.CorrectPosition();
		if (!m_instigator.IsValid && !m_startedReaction)
		{
			OnStartReaction();
		}
	}

	public override bool CanCancel()
	{
		return true;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Stumble))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Stumble))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		}
	}

	private void OnStartReaction()
	{
		m_startedReaction = true;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.StumbleBackwardHeavy);
		AnimationController.Animator.SetTargetDirection(m_targetDir);
		if (m_instigator.IsValid && m_instigator.GameEntity.IsAttached)
		{
			m_instigator.GameEntity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
	}

	private void CalcTargetDirectionAndRotation()
	{
		GameEntity gameEntity = AnimationController.EntityId.GameEntity;
		GameEntity gameEntity2 = m_instigator.GameEntity;
		Vector3 direction = gameEntity2.Position - gameEntity.Position;
		float num = direction.AngleZX();
		float y = gameEntity.Rotation.eulerAngles.y;
		float num2 = Mathf.Abs(Mathf.DeltaAngle(num, y));
		m_targetDir = ((num2 < 90f) ? AnimationController.Direction.Front : AnimationController.Direction.Back);
		float num3 = ((m_targetDir != AnimationController.Direction.Back) ? 0f : 180f);
		gameEntity.Rotation = Quaternion.Euler(0f, num3 + num, 0f);
	}
}
