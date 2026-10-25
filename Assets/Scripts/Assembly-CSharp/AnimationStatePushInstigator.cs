using Assets.Scripts.Animation;
using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;

internal class AnimationStatePushInstigator : AnimationStatePlaySimple, IAnimatorEventListener
{
	private const string StateName = "Push";

	private EntityId m_target = EntityId.InvalidId;

	private bool m_hadReactionStart;

	public AnimationStatePushInstigator(AnimationController _animationController, EntityId _target)
		: base(_animationController, new ParameterValues
		{
			ReactionType = ReactionType.CloseRangePush
		}, AnimationStateMachineHashes.StateMachine_Push, "Push")
	{
		m_target = _target;
	}

	public override void OnEnter()
	{
		m_hadReactionStart = false;
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.EntityId.GameEntity.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
		AutoAnimationRotationToTarget autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, m_target.GameEntity);
		autoAnimationRotationToTarget.FireAndForget = true;
		autoAnimationRotationToTarget.Speed = 2f;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		if (_isCanceled && !m_hadReactionStart)
		{
			Character character = m_target.TryGetEntity<Character>();
			if (character != null)
			{
				AnimationTaskManager taskManager = character.UnityComponentCache.AnimationController.TaskManager;
				if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.PushTarget)
				{
					taskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base, true);
				}
			}
		}
		AnimationController.ApplyRootMotion = false;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		GameEntity gameEntity = AnimationController.EntityId.TryGetEntity<GameEntity>();
		if (gameEntity.IsAttached)
		{
			gameEntity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (_event.Type == AnimatorEventType.ReactionStart)
		{
			m_hadReactionStart = true;
		}
		return false;
	}
}
