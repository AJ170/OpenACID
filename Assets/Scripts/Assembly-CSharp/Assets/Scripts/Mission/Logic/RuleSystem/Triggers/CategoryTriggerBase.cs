using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class CategoryTriggerBase<T> : TriggerBase<T> where T : IRulesCarrier
	{
		protected CategoryTriggerBase(ETriggerType _type, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(_type, _owner)
		{
			base.Frequency = _frequency;
			Categories = _categoryFlags;
		}
	}
}
