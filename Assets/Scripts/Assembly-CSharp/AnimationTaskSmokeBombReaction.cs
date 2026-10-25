using Assets.Scripts.Animation;

public class AnimationTaskSmokeBombReaction : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.SmokeBombReaction)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.SmokeBombReaction;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private bool m_rootMotion;

	private bool m_resetParameter;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.SmokeBombReaction;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskSmokeBombReaction()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskSmokeBombReaction(Parameters _parameters)
		: base(_parameters)
	{
		m_resetParameter = true;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (AnimationStateHashes.IsStateContainedInArray(_event.State, AnimationStateHashes.States_SmokeBombed))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
			m_resetParameter = false;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentStopAction(false);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.SmokeBombReaction);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentStopAction(true);
		AnimationController.ApplyRootMotion = m_rootMotion;
		if (m_resetParameter)
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
	}
}
