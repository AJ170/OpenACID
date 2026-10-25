using Assets.Scripts.Animation;

public class AnimationTaskThrowSandReaction : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.ThrowSandReaction)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.ThrowSandReaction;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new AnimationTaskSmokeBombReaction.Parameters()
	};

	private bool m_rootMotion;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.ThrowSandReaction;
		}
	}

	public static AnimationTaskSmokeBombReaction.Parameters ParametersTemplate
	{
		get
		{
			return (AnimationTaskSmokeBombReaction.Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskThrowSandReaction()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskThrowSandReaction(AnimationTaskSmokeBombReaction.Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.StateMachine.TransitionStarted += OnTransitionStarted;
	}

	private void OnTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _event)
	{
		AnimatorStateMachineStatic.StateMachine parentStateMachine = _event.Transition.SourceState.ParentStateMachine;
		AnimatorStateMachineStatic.StateMachine parentStateMachine2 = _event.Transition.TargetState.ParentStateMachine;
		bool flag = parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_ThrowSandReaction);
		bool flag2 = parentStateMachine2.CompareNameHash(AnimationStateMachineHashes.StateMachine_ThrowSandReaction);
		if (!flag && flag2)
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		}
		else if (flag && !flag2)
		{
			base.IsFinished = true;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentReactionType(ReactionType.ThrowSandReaction);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		AnimationController.ApplyRootMotion = m_rootMotion;
		AnimationController.StateMachine.TransitionStarted -= OnTransitionStarted;
	}
}
