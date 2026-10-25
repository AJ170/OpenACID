public class AnimationTaskLureReaction : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.LureReaction)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.LureReaction;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.LureReaction;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskLureReaction()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskLureReaction(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStateLureReaction(_parameters.AnimationController));
	}
}
