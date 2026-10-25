public class AnimationTaskHayStackEnter : AnimationTaskHideoutEnterBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.HayStackEnter)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskHayStackEnter()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskHayStackEnter(Parameters _parameters)
		: base(_parameters, AnimationTaskType.HayStackEnter)
	{
		OnAddState(new AnimationStateHayStackEnter(_parameters.AnimationController, _parameters.Hideout, _parameters.SelectedLocator));
	}
}
