using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HastyRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public HastyRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_SkillCooldownRelativeDurationReduction);
		}
	}
}
