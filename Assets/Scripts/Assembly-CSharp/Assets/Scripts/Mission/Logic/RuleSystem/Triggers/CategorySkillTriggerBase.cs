using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class CategorySkillTriggerBase<T, TPayload> : SkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		protected CategorySkillTriggerBase(ETriggerType _type, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(_type, _owner, _targetSource)
		{
			base.Frequency = _frequency;
			Categories = _categoryFlags;
		}
	}
}
