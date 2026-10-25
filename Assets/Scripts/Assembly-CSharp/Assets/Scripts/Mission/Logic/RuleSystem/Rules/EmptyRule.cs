using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class EmptyRule : RuleBase<PlayerCharacter>
	{
		public EmptyRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
		}
	}
}
