using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class FleeFromCombatRule : SkillRuleBase<PlayerCharacter>
	{
		public FleeFromCombatRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new PlayerFleeAllowedSkillTrigger(this, SkillFrequency, SkillCategories));
			AddEffect(new FleeEffect(this, ETargetSource.Player));
		}
	}
}
