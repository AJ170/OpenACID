using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AssassinAnonymousTrigger : CategoryTriggerBase<AssassinCharacter>, ITrigger, IActivityConditionTrigger
	{
		public bool ActivityCondition { get; set; }

		public AssassinAnonymousTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories)
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
			return _entity.GetIsAnonymous();
		}

		public override ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			return ESkillAvailabilityFlags.NotAnonymous;
		}
	}
}
