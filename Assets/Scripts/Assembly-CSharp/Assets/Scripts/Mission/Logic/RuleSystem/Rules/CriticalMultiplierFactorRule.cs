using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CriticalMultiplierFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public CriticalMultiplierFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_ActualCriticalMultiplier, StatTypes.EStatType.Rule_CriticalMultiplierFactor);
		}
	}
}
