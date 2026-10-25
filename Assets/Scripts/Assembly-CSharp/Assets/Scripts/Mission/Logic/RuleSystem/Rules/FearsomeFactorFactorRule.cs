using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class FearsomeFactorFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public FearsomeFactorFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultiplicativeBuff_float(EBuffType.ThreadBroadcasting, StatTypes.EStatType.Rule_FearsomeFactorFactor);
		}
	}
}
