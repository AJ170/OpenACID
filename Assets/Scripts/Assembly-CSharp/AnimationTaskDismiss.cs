public class AnimationTaskDismiss : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public int[] NpcIds { get; set; }

		public Parameters()
			: base(AnimationTaskType.Dismiss)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Dismiss;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Dismiss;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskDismiss()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskDismiss(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStateDismiss(_parameters.AnimationController, _parameters.NpcIds));
	}
}
