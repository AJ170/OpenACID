using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AssassinStalkingTrigger : CategoryTriggerBase<AssassinCharacter>, ITrigger, IActivityConditionTrigger
	{
		public bool ActivityCondition { get; set; }

		public AssassinStalkingTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories)
			: base(ETriggerType.PlayerStalking, _owner, _frequency, _categories)
		{
			ActivityCondition = false;
		}

		protected override bool IsActive(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return _entity.GetIsStalking();
		}
	}
}
