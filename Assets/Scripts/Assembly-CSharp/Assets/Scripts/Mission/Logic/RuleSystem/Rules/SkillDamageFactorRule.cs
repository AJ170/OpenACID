using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SkillDamageFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public SkillDamageFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveBuff_float(StatTypes.EStatType.Character_Conflict_SkillDamageBonusFactor, StatTypes.EStatType.Rule_SkillDamageFactor);
		}
	}
}
