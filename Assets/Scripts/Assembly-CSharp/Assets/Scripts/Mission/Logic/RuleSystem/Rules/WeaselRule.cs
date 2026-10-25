using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class WeaselRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public WeaselRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_ActualEvasion, StatTypes.EStatType.Rule_EvasionFactor);
		}
	}
}
