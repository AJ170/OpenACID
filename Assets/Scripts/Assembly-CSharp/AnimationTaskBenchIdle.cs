public class AnimationTaskBenchIdle : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.BenchIdle)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BenchIdle;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BenchIdle;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskBenchIdle()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBenchIdle(Parameters _parameters)
		: base(_parameters)
	{
	}

	public override bool CanCancel()
	{
		return true;
	}
}
