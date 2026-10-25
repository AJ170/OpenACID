using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;

public class AnimationTaskHayStackKillTarget : AnimationTaskHideoutKillTargetBase
{
	private readonly Haystack m_haystack;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters(AnimationTaskType.HayStackKillTarget)
	};

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskHayStackKillTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskHayStackKillTarget(Parameters _parameters)
		: base(_parameters, AnimationTaskType.HayStackKillTarget)
	{
		m_haystack = UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current;
	}

	public override bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.HaystackBounce)
		{
			m_haystack.TriggerJumpInAnimation();
		}
		return base.OnAnimatorEvent(_event);
	}
}
