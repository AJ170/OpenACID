using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CriticalChanceFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public CriticalChanceFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultInverseMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_CriticalChance, StatTypes.EStatType.Rule_ChanceFactor);
		}
	}
}
