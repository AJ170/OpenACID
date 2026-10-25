using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HirelingMaxHealthBonusRule : MaxHealthRuleBase
	{
		public HirelingMaxHealthBonusRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			m_Generator.ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor1_float;
		}

		protected override CustomBuffGenerator<float> AddMaxHealthEffect()
		{
			return AddAdditiveBuff_float(EBuffType.Adrenaline, StatTypes.EStatType.Rule_MaxHealthBonus);
		}
	}
}
