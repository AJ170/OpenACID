using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SilentDaggerRule : ApproachSkillRuleBase
	{
		public SilentDaggerRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMandatoryTrigger(new CharacterMainStateTrigger<PlayerCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, new int[4] { 53, 52, 65, 54 }));
			AddMandatoryTrigger(new LivingEnemySkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload)
			{
				ActivityCondition = true
			});
			AddTrigger(new CombatIdleDelayedSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner));
			AddTrigger(new CombatActiveSkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new GeneralDamageSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, EApplyDamageFlags.SkillNormal | EApplyDamageFlags.ApplyAnonymous, true),
				new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnDaggerHit),
				new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeSilencingBuffGenerator(StatValueCollection))
				{
					IsAbortable = false,
					Predicate = RulesHelper.CategoryMatchingLivingTargetPredicate
				},
				new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeConfusedBuffGenerator(StatValueCollection, 10))
				{
					IsAbortable = false,
					Predicate = RulesHelper.CategoryMatchingLivingTargetPredicate
				},
				new AwarenessUpdateLastKnownPosition<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload),
				new SwitchStateMachineSkillEffect<PlayerCharacter, IEntityIdSkillPayload, IPayload>(this, ETargetSource.Payload, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed, true, null)
				{
					Categories = ETriggerCategoryFlags.Roaming,
					Predicate = RulesHelper.CategoryMatchingLivingTargetPredicate
				}
			};
			SkillEffectsWrapper hitEffectsWrapper = new SkillEffectsWrapper(effects);
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ProjectileData.EProjectileType.Dagger, hitEffectsWrapper)
			};
			AddEffect(new StopApproachingEffect<PlayerCharacter>(this));
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ThrowDagger));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects2, true, AnimatorEventType.ProjectileStart, AnimationTaskManager.ELayer.Base, AnimationStateMachineHashes.StateMachine_SilentDagger));
		}

		private IEffectInstance OnDaggerHit(IEffect _effect, PlayerCharacter playerCharacter, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			GameEntity gameEntity = _entityIdSkillPayload.Id.GameEntity;
			if (gameEntity.IsAttached)
			{
				gameEntity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetHit(playerCharacter, AttackMeleeType.Dagger);
			}
			EffectsHelper.AudioOnKnifeHit(_targetEntityId);
			return null;
		}

		private IEffectInstance ThrowDagger(IEffect _effect, PlayerCharacter playerCharacter, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			playerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskDagger(_entityIdSkillPayload.Id);
			return null;
		}
	}
}
