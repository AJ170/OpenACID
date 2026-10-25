using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class DodgeChanceBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public DodgeChanceBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddInverseMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_DodgeChance, StatTypes.EStatType.Rule_DodgeChanceBonus);
		}
	}
}
