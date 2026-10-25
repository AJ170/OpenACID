using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class StealthModifierBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public StealthModifierBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveBuff_float(StatTypes.EStatType.Player_StealthModifier, StatTypes.EStatType.Rule_StealthModifierBonus);
		}
	}
}
