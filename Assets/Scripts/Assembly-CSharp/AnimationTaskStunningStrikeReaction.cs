using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskStunningStrikeReaction : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Character Target { get; set; }

		public Parameters()
			: base(AnimationTaskType.StunningStrikeReaction)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.StunningStrikeReaction;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private readonly Character m_target;

	private bool m_rootMotion;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.StunningStrikeReaction;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskStunningStrikeReaction()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskStunningStrikeReaction(Parameters _parameters)
		: base(_parameters)
	{
		m_target = _parameters.Target;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentStopAction(false);
		AnimationController.GetEntity<Character>().SetKnowinglyInCombat();
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
