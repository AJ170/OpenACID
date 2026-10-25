using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class BloodyShardsRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public BloodyShardsRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			Enum<SkillTypes> skillType = "Throw_Sand_01".ToEnum<SkillTypes>();
			AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_BleedAmountRelative, skillType);
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_BleedDuration, skillType);
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_BleedInterval, skillType);
		}
	}
}
