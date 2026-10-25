using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class GlitchRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public GlitchRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddOverwritingBuff_int(StatTypes.EStatType.Player_GlitchHidingSpots, StatTypes.EStatType.Rule_GlitchHidingSpots);
		}
	}
}
