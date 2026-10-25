using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskRooftopGardenExit : AnimationTaskHideoutExitBase, IAnimatorEventListener
{
	public const AnimationTaskType sTaskType = AnimationTaskType.RooftopGardenExit;

	private RooftopGarden m_rooftopGarden;

	private InteractionLocators.Locator m_locator;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.RooftopGardenExit)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.RooftopGardenExit;
		}
	}

	public AnimationTaskRooftopGardenExit()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRooftopGardenExit(Parameters _parameters)
		: base(_parameters)
	{
		m_rooftopGarden = (RooftopGarden)_parameters.Hideout;
		m_locator = _parameters.Locator;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (_event.Type == AnimatorEventType.RooftopClothesExit)
		{
			m_rooftopGarden.RooftopGardenClothes.JumpOut(m_locator);
		}
		return false;
	}

	public override void OnEnter()
	{
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitRooftopGarden, true);
		base.OnEnter();
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitRooftopGarden, false);
	}

	public override void OnDispose()
	{
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
		base.OnDispose();
	}
}
