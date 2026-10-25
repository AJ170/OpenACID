using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CrippleRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public CrippleRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			Enum<SkillTypes> skillType = "Jack_in_the_Box_01".ToEnum<SkillTypes>();
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_SilencedDuration, skillType);
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_BlindedDuration, skillType);
		}
	}
}
