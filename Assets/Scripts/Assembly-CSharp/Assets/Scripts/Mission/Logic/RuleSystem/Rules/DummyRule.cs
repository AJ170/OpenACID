using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class DummyRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public DummyRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}
	}
}
