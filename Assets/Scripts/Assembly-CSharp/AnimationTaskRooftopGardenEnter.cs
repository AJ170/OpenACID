using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskRooftopGardenEnter : AnimationTaskHideoutEnterBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.RooftopGardenEnter)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskRooftopGardenEnter()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRooftopGardenEnter(Parameters _parameters)
		: base(_parameters, AnimationTaskType.RooftopGardenEnter)
	{
		OnAddState(new AnimationStateRooftopGardenEnter(_parameters.AnimationController, (RooftopGarden)_parameters.Hideout, _parameters.SelectedLocator));
	}
}
