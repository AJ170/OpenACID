using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class TracelessRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public TracelessRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			CustomBuffGenerator<float> customBuffGenerator = AddAdditiveBuff_float(StatTypes.EStatType.Character_Conspicuous_NotorietyIncreaseDelay, StatTypes.EStatType.Rule_NotorietyIncreaseDelay);
			customBuffGenerator.IsActivePredicate = IsActivePredicate;
		}

		private bool IsActivePredicate(IEntity _entity, IBuff _buff)
		{
			return !_entity.EntityStats.Get_bool(StatTypes.EStatType.Player_IsInRestrictedArea);
		}
	}
}
