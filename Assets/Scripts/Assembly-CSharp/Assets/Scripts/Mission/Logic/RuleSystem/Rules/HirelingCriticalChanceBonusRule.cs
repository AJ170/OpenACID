using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HirelingCriticalChanceBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public HirelingCriticalChanceBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			CustomBuffGenerator<float> customBuffGenerator = AddInverseMultiplicativeBuff_float(EBuffType.Inspiration, StatTypes.EStatType.Rule_CriticalChanceBonus);
			customBuffGenerator.ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor1_float;
		}
	}
}
