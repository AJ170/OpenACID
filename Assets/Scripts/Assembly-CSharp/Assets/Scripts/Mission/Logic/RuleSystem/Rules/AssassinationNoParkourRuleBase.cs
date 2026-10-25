using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class AssassinationNoParkourRuleBase : AssassinationRuleBase
	{
		protected AssassinationNoParkourRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters, EStateType _playerMainTargetState)
			: base(_ruleDefinition, _parameters, _playerMainTargetState)
		{
			AddMandatoryTrigger(new EntityInWalkableRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload)
			{
				Predicate = TriggerPredicate
			});
		}

		private bool TriggerPredicate(ITrigger _obj1, IRulesCarrier _obj2, IBuffedStatsCollectionHandle _obj3)
		{
			PlayerCharacter playerCharacter = _obj2 as PlayerCharacter;
			return playerCharacter == null || !playerCharacter.GetIsApproaching() || playerCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget.Reason != base.ApproachSkillDefinition.InteractionReason;
		}
	}
}
