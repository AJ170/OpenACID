using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class UnfairAdvantageRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public UnfairAdvantageRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			CustomBuffGenerator<float> customBuffGenerator = AddMultiplicativeBuff_float(StatTypes.EStatType.Character_Conflict_ActualPrecision, StatTypes.EStatType.Rule_PrecisionFactor);
			customBuffGenerator.IsActivePredicate = IsActivePredicate;
		}

		private bool IsActivePredicate(IEntity _entity, IBuff _buff)
		{
			if (!CombatController.IsActive)
			{
				return false;
			}
			return CombatController.Instance.ParticipantCount() == 2;
		}
	}
}
