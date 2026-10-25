using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SkillRelativeCooldownReductionRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public SkillRelativeCooldownReductionRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			Assassin.EClassTypeId eClassTypeId = (Assassin.EClassTypeId)StatValueCollection.Get_int(StatTypes.EStatType.Rule_AssassinClassType, (ISuperBuffsStatsCarrier)null);
			if (eClassTypeId != Assassin.EClassTypeId.None)
			{
				AddMandatoryTrigger(new AvatarClassTrigger(this, eClassTypeId));
			}
			AddAdditiveSuperBuff_float(StatTypes.EStatType.Rule_SkillCooldownRelativeDurationReduction);
		}
	}
}
