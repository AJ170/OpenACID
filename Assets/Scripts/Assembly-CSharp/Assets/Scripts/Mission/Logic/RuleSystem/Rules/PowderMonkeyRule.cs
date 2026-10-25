using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class PowderMonkeyRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public PowderMonkeyRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			Enum<SkillTypes> skillType = "Sticky_Bomb_01".ToEnum<SkillTypes>();
			AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_ArmourRatingFactor, skillType);
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_ArmourRatingBonusDuration, skillType);
		}
	}
}
