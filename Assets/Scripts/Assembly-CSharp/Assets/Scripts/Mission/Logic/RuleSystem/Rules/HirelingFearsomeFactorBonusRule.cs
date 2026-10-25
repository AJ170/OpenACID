using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HirelingFearsomeFactorBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public HirelingFearsomeFactorBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveBuff_float(EBuffType.Nightmare, StatTypes.EStatType.Invalid);
			CustomBuffGenerator<float> customBuffGenerator = AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_FearsomeFactor);
			customBuffGenerator.ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor1_float;
		}
	}
}
