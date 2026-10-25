using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskHayStackExit : AnimationTaskHideoutExitBase
{
	public const AnimationTaskType sTaskType = AnimationTaskType.HayStackExit;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.HayStackExit)
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
			return AnimationTaskType.HayStackExit;
		}
	}

	public AnimationTaskHayStackExit()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskHayStackExit(Parameters _parameters)
		: base(_parameters)
	{
	}

	public override void OnEnter()
	{
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitHaystack, true);
		base.OnEnter();
		Haystack haystack = (Haystack)m_hideoutObject;
		haystack.TriggerJumpInAnimation();
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitHaystack, false);
	}
}
