using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class BreathTakerRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public BreathTakerRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddAdditiveSuperBuff_int(StatTypes.EStatType.Rule_SilencedDuration);
			AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_SkillDamageFactor, "Silent_Dagger_01".ToEnum<SkillTypes>());
		}
	}
}
