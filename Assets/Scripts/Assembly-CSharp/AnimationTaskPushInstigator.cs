using Assets.Scripts.Mission.Logic;

public class AnimationTaskPushInstigator : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public EntityId Target { get; set; }

		public Parameters()
			: base(AnimationTaskType.PushInstigator)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.PushInstigator;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.PushInstigator;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskPushInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskPushInstigator(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStatePushInstigator(_parameters.AnimationController, _parameters.Target));
	}
}
