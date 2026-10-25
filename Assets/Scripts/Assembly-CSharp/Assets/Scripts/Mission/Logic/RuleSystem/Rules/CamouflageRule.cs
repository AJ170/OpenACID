using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CamouflageRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public CamouflageRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}
	}
}
