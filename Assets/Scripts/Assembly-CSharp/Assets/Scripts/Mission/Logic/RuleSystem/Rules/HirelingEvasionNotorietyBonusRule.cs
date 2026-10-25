using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HirelingEvasionNotorietyBonusRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public HirelingEvasionNotorietyBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			CustomBuffGenerator<float> customBuffGenerator = AddAdditiveBuff_float(EBuffType.SoftFooted, StatTypes.EStatType.Rule_EvasionBonus);
			customBuffGenerator.ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor1_float;
			customBuffGenerator = AddAdditiveBuff_float(StatTypes.EStatType.Character_Conspicuous_NotorietyIncreaseDelay, StatTypes.EStatType.Rule_NotorietyIncreaseDelay);
			customBuffGenerator.ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor2_float;
		}
	}
}
