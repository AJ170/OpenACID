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
	public class FanOfKnivesRule : ApproachSkillRuleBase
	{
		private int m_killTokenIncrease;

		public FanOfKnivesRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
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
				new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeStunningBuffGenerator(StatValueCollection))
				{
					IsAbortable = false
				},
				new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnKnifeHit),
				new AwarenessUpdateLastKnownPosition<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload),
				new SwitchStateMachineSkillEffect<PlayerCharacter, IEntityIdSkillPayload, IPayload>(this, ETargetSource.Payload, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed, true, null)
				{
					Predicate = RulesHelper.CategoryMatchingLivingTargetPredicate
				}
			};
			SkillEffectsWrapper hitEffectsWrapper = new SkillEffectsWrapper(effects, RulesHelper.CategoryMatchingLivingTargetPredicate);
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new SpawnProjectilesSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ProjectileData.EProjectileType.FanOfKnivesKnive, hitEffectsWrapper, CollectTargetCharacters),
				new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnKnifeStart)
				{
					Categories = (ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.HidingInStalkingZone)
				}
			};
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnThrow));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects2, true, AnimatorEventType.ProjectileStart));
		}

		private static IEffectInstance OnKnifeStart(IEffect _effect, PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, IEntityIdSkillPayload _payload)
		{
			PlayerProfile playerProfile = _entity.PlayerCharacterStateMachines.PlayerProfile;
			if (playerProfile.CurrentStateType != EStateType.PlayerProfile_High)
			{
				playerProfile.ChangeStateForced(EStateType.PlayerProfile_High);
			}
			return null;
		}

		private IEffectInstance OnKnifeHit(IEffect _effect, PlayerCharacter _playerCharacter, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			EnemyCharacter entity = _targetEntityId.GetEntity<EnemyCharacter>();
			if (entity.GetHasZeroHealth())
			{
				return null;
			}
			CombatHelper.ApplySkillDamage(_playerCharacter, _playerCharacter, entity, _stats, EApplyDamageFlags.SkillNormal);
			bool hasZeroHealth = entity.GetHasZeroHealth();
			CombatHelper.IncreaseKillTokenCountMultiEnemy(hasZeroHealth, ref m_killTokenIncrease);
			if (!hasZeroHealth)
			{
				int num = _stats.Get_int(StatTypes.EStatType.Rule_BlindedDuration);
				if (num > 0)
				{
					int blindStartDelay = _stats.Get_int(StatTypes.EStatType.Rule_StunDuration);
					EffectsHelper.CharacterBlinded(entity, num, blindStartDelay);
				}
			}
			entity.EnemyCharacterStateMachines.CharacterCondition.ExternalChangeNoTask();
			if (entity.IsAttached)
			{
				entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetHit(_playerCharacter, AttackMeleeType.Invalid);
			}
			EffectsHelper.AudioOnKnifeHit(_targetEntityId);
			return null;
		}

		private IEffectInstance OnThrow(IEffect _effect, PlayerCharacter _playerCharacter, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			m_killTokenIncrease = 0;
			_playerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskFanOfKnives(_entityIdSkillPayload.Id);
			return null;
		}

		private BBList<EnemyCharacter> CollectTargetCharacters(PlayerCharacter _entity, EntityId _targetId)
		{
			using (IBuffedStatsCollectionHandle stats = StatValueCollection.Apply(_entity))
			{
				return EffectsHelper.CollectBestTargetCharacters(_entity, _targetId, stats);
			}
		}
	}
}
