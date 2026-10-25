using Assets.Scripts.Mission.Logic;

public class AnimationTaskPushTarget : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public EntityId Instigator { get; set; }

		public Parameters()
			: base(AnimationTaskType.PushTarget)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.PushTarget;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.PushTarget;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskPushTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskPushTarget(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStatePushTarget(_parameters.AnimationController, _parameters.Instigator));
	}

	public override bool CanCancel()
	{
		return true;
	}
}
