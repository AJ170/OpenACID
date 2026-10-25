using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class ArmorDamageReductionBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public ArmorDamageReductionBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveBuff_float(StatTypes.EStatType.Character_Conflict_ArmourDamageReduction, StatTypes.EStatType.Rule_ArmourDamageReductionBonus);
		}
	}
}
