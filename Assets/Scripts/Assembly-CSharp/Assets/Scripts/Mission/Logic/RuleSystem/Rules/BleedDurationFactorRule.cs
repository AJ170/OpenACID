using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class BleedDurationFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public BleedDurationFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultiplicativeSuperBuff_float(StatTypes.EStatType.Rule_BleedDuration, StatTypes.EStatType.Rule_DurationFactor);
		}
	}
}
