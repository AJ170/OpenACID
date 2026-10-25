using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class AssassinationStealthRule : AssassinationNoParkourRuleBase
	{
		public AssassinationStealthRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters, EStateType.PlayerMain_AssassinationStealth)
		{
			AddTrigger(new TargetPredicateSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, ETriggerCategoryFlags.HidingInPatrol, ETargetSource.Payload, CourtesansPredicate));
			AddEffectFront(new ActionSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, OnAssassinationFromPatrol));
			AddEffect(new AddCrowdHotspotSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, 0.5f, 10f, 5f, false, 0f, 0f, ReactionType.Invalid));
		}

		private static bool CourtesansPredicate(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetId)
		{
			PlayerMain playerMain = _entity.PlayerMainCarrier.PlayerMain;
			PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = playerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
			PatrolData.IPatrolDefinition definition = payload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>().PatrolDefinition.Definition;
			if (definition.Type != EPatrolType.CourtesanHidingSpot)
			{
				return true;
			}
			Character character = _targetId.TryGetEntity<Character>();
			if (character == null)
			{
				return false;
			}
			ICharacterConditionCarrier characterConditionCarrier = character.CharacterStateMachines as ICharacterConditionCarrier;
			return characterConditionCarrier == null || characterConditionCarrier.CharacterCondition.CurrentStateType == EStateType.CharacterCondition_LuredBySweetMusic;
		}

		private IEffectInstance OnAssassinationFromPatrol(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, ISkillPayload _payload)
		{
			PlayerMain playerMain = _entity.PlayerMainCarrier.PlayerMain;
			if (playerMain.CurrentStateType != EStateType.PlayerMain_HidingInPatrol)
			{
				return null;
			}
			PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = playerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = payload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>().PatrolComponent;
			if (patrolComponent.EntityAtHidingSpot != _targetentityid || !patrolComponent.IsOnSidetrack)
			{
				return null;
			}
			int timeout = Mathf.RoundToInt(20f);
			patrolComponent.StartSidetrack(patrolComponent.Position, false, true, timeout);
			return null;
		}
	}
}
