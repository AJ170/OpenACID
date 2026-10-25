using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;

public class AnimationTaskRooftopGardenKillInstigator : AnimationTaskHideoutKillInstigatorBase
{
	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.RooftopGardenKillInstigator)
	};

	private RooftopGarden m_rooftopGarden;

	private InteractionLocators.Locator m_locator;

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskRooftopGardenKillInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRooftopGardenKillInstigator(Parameters _parameters)
		: base(_parameters, AnimationTaskType.RooftopGardenKillInstigator)
	{
		m_locator = _parameters.Locator;
		m_rooftopGarden = UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current;
	}

	public override bool OnAnimatorEvent(AnimatorEvent _event)
	{
		base.OnAnimatorEvent(_event);
		if (_event.Type == AnimatorEventType.RooftopClothesAssassination)
		{
			m_rooftopGarden.RooftopGardenClothes.Assassinate(m_locator);
		}
		return false;
	}
}
