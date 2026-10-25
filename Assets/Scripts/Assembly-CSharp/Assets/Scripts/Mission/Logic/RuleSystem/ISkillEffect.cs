using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ISkillEffect : ISkillPayloadCarrier, ITargetCarrier, IEffect, ITargetEffect
	{
		IEffectInstance Apply(ISkillsCarrier _entity);
	}
}
