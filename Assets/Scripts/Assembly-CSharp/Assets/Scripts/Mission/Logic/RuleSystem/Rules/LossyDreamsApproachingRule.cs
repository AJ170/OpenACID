using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class LossyDreamsApproachingRule : BlackjackRuleBase
	{
		public LossyDreamsApproachingRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}
	}
}
