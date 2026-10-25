public class AnimationTaskRooftopGardenKillTarget : AnimationTaskHideoutKillTargetBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.RooftopGardenKillTarget)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskRooftopGardenKillTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRooftopGardenKillTarget(Parameters _parameters)
		: base(_parameters, AnimationTaskType.RooftopGardenKillTarget)
	{
	}
}
