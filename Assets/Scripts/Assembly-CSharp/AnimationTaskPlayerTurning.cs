using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;

public class AnimationTaskPlayerTurning : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public IPositionProvider TargetProvider { get; set; }

		public float DurationSec { get; set; }

		public Parameters()
			: base(AnimationTaskType.PlayerTurning)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.PlayerTurning;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private readonly IPositionProvider m_targetProvider;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.PlayerTurning;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AutoAnimationRotation AutoAnimationRotation { get; private set; }

	public AnimationTaskPlayerTurning()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskPlayerTurning(Parameters _parameters)
		: base(_parameters)
	{
		m_targetProvider = _parameters.TargetProvider;
	}

	public override void OnEnter()
	{
		AutoAnimationRotation = AddAutoUpdateable(new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, m_targetProvider));
		AutoAnimationRotation.Speed = 2f;
		AutoAnimationRotation.Enter();
		base.OnEnter();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		base.IsFinished = !AutoAnimationRotation.IsRunning;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (AutoAnimationRotation != null)
		{
			AutoAnimationRotation.Exit();
		}
	}

	public override bool CanCancel()
	{
		return false;
	}
}
