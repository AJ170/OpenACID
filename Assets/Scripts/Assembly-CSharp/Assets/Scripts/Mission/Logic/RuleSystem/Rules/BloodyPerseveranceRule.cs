using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class BloodyPerseveranceRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public BloodyPerseveranceRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_BleedDuration);
		}
	}
}
