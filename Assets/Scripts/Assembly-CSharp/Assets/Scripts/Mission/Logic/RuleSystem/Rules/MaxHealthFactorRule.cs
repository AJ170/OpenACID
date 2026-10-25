using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class MaxHealthFactorRule : MaxHealthRuleBase
	{
		public MaxHealthFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}

		protected override CustomBuffGenerator<float> AddMaxHealthEffect()
		{
			return AddMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth, StatTypes.EStatType.Rule_MaxHealthFactor);
		}
	}
}
