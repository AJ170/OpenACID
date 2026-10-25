public class AnimationTaskHayStackKillInstigator : AnimationTaskHideoutKillInstigatorBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.HayStackKillInstigator)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskHayStackKillInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskHayStackKillInstigator(Parameters _parameters)
		: base(_parameters, AnimationTaskType.HayStackKillInstigator)
	{
	}
}
