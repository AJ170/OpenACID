using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class MaxHealthBonusRule : MaxHealthRuleBase
	{
		public MaxHealthBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}

		protected override CustomBuffGenerator<float> AddMaxHealthEffect()
		{
			return AddAdditiveBuff_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth, StatTypes.EStatType.Rule_MaxHealthBonus);
		}
	}
}
