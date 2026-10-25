using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskStunned : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Character Target { get; set; }

		public bool FromGround { get; set; }

		public Parameters()
			: base(AnimationTaskType.Stunned)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Stunned;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private bool m_rootMotion;

	private readonly Character m_target;

	private readonly bool m_fromGround;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Stunned;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskStunned()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskStunned(Parameters _parameters)
		: base(_parameters)
	{
		m_target = _parameters.Target;
		m_fromGround = _parameters.FromGround;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
	}

	private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_Stunned_Footl) || _event.State.CompareNameHash(AnimationStateHashes.State_Stunned_Footr))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.Animator.SetCurrentStopAction(false);
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Animator.SetCurrentStopAction(m_fromGround);
		AnimationController.Animator.SetCurrentReactionType((!m_fromGround) ? ReactionType.Stunned : ReactionType.StunnedOnGround);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimationHelper.StayOnNavMesh(m_target);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentStopAction(true);
		AnimationController.ApplyRootMotion = m_rootMotion;
	}
}
