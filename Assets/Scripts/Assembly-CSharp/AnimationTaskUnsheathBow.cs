using Assets.Scripts.Animation;
using UnityEngine;

internal class AnimationTaskUnsheathBow : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Animator BowAnimator { get; set; }

		public Parameters()
			: base(AnimationTaskType.UnsheathBow)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.UnsheathBow;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.UnsheathBow;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public Animator BowAnimator { get; private set; }

	public AnimationTaskUnsheathBow()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskUnsheathBow(Parameters _parameters)
		: base(_parameters)
	{
		BowAnimator = _parameters.BowAnimator;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		OnEnterSetParameters(AnimationController.Animator);
		OnEnterSetParameters(BowAnimator);
		AnimationController.StateMachine.StateEntered += OnStateEntered;
	}

	public override bool CanCancel()
	{
		return true;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
	}

	private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _stateArgs)
	{
		if (_stateArgs.State.CompareNameHash(AnimationStateHashes.State_UnsheathBow))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}
	}

	private void OnEnterSetParameters(Animator _animator)
	{
		_animator.SetCurrentStopAction(false);
		_animator.SetCurrentGeneralState(GeneralState.CombatRangeBow);
	}
}
