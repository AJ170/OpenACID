using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CatLikeRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public CatLikeRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveBuff_float(StatTypes.EStatType.Player_FallingDamageReductionFactor, StatTypes.EStatType.Rule_FallingDamageReductionFactor);
		}
	}
}
