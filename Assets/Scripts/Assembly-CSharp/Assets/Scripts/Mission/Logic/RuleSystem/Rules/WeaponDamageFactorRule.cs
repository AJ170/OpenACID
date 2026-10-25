using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class WeaponDamageFactorRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public WeaponDamageFactorRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMultiplicativeBuff_float(EBuffType.Damage, StatTypes.EStatType.Rule_WeaponDamageFactor);
		}
	}
}
