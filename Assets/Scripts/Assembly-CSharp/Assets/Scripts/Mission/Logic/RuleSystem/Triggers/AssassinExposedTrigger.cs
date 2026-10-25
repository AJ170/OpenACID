using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AssassinExposedTrigger : CategoryTriggerBase<AssassinCharacter>, ITrigger, IActivityConditionTrigger
	{
		public bool ActivityCondition { get; set; }

		public AssassinExposedTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories)
			: base(ETriggerType.PlayerExposed, _owner, _frequency, _categories)
		{
			ActivityCondition = false;
		}

		protected override bool IsActive(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return _entity.GetIsExposed();
		}
	}
}
