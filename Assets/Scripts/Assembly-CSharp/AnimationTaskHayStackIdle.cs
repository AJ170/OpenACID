public class AnimationTaskHayStackIdle : AnimationTaskHideoutIdleBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.HayStackIdle)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskHayStackIdle()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskHayStackIdle(Parameters _parameters)
		: base(_parameters, AnimationTaskType.HayStackIdle)
	{
	}
}
