public class AnimationTaskRooftopGardenIdle : AnimationTaskHideoutIdleBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.RooftopGardenIdle)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskRooftopGardenIdle()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRooftopGardenIdle(Parameters _parameters)
		: base(_parameters, AnimationTaskType.RooftopGardenIdle)
	{
	}
}
