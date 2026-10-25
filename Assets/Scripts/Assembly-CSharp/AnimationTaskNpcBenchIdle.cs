using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskNpcBenchIdle : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Bench.Seat Seat { get; set; }

		public Parameters()
			: base(AnimationTaskType.NpcBenchIdle)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.NpcBenchIdle;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Bench.Seat m_seat;

	private bool m_firstUpdate = true;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.NpcBenchIdle;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskNpcBenchIdle()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskNpcBenchIdle(Parameters _parameters)
		: base(_parameters)
	{
		m_seat = _parameters.Seat;
		AnimationController.Agent.enabled = false;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.SetCurrentIdleType(IdleType.Invalid);
		AnimationController.Animator.Play(AnimationStateHashes.States_NpcBenchIdle[LogicMain.Random.Rand(0, AnimationStateHashes.States_NpcBenchIdle.Length - 1)].NameHash);
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_firstUpdate)
		{
			AnimationController.NextPosition = m_seat.Position + m_seat.RandomOffset;
			AnimationController.NextRotation = m_seat.Orientation;
			m_firstUpdate = false;
		}
	}
}
