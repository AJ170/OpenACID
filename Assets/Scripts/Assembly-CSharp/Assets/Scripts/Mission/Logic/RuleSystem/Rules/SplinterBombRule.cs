using System.Collections.Generic;
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
using SharedBaseLib.System.Extensions;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SplinterBombRule : ApproachSkillRuleBase
	{
		private BuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload> m_BleedingBuffSkillEffect;

		private static float s_LastBleedingBaseDamage;

		public SplinterBombRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddTrigger(new CombatIdleDelayedSkillTrigger<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner));
			AddTrigger(new CombatActiveSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload));
			AddMandatoryTrigger(new LivingEnemySkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload));
			m_BleedingBuffSkillEffect = new BuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeBleedingBuffGenerator(StatValueCollection, BleedingValueProcessor, RulesHelper.LinearHirelingLevelTurnCountProcessor2, StatTypes.EStatType.Rule_BleedAmountRelative), BleedingActivityPredicate);
			IEffect[] effects = new IEffect[1]
			{
				new MultipleTargetsSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(ETargetSource.Payload, new BuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeDazedBuffGenerator(StatValueCollection, RulesHelper.LinearHirelingLevelTurnCountProcessor3))
				{
					IsAbortable = false
				})
			};
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new TargetCollectorSkillEffectsWrapper<AssassinCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, TargetCollector, effects, null),
				new SpawnFxSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, Vector3.zero),
				new ActionSkillEffect<AssassinCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombHit),
				new FearsomeSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload)
			};
			SkillEffectsWrapper hitEffectsWrapper = new SkillEffectsWrapper(effects2);
			BBList<IEffect> effects3 = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ProjectileData.EProjectileType.SplinterBomb, hitEffectsWrapper),
				new ExposingEffect(this)
			};
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnThrowBomb));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<AssassinCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects3, true, AnimatorEventType.ProjectileStart));
		}

		private object BleedingValueProcessor(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return RulesHelper.LinearHirelingLevelValueProcessor2_float(_creator, _stats, (float)_value * s_LastBleedingBaseDamage);
		}

		private bool BleedingActivityPredicate(IBuffsCarrier _buffsCarrier)
		{
			if (LogicMain.IsInTurnUpdate)
			{
				LogicMain.MissionResult.IncreaseCausedSplinterBombBleedingTurnCount();
			}
			return true;
		}

		private static IEffectInstance OnThrowBomb(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			_assassin.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskSplinterBomb(_entityIdSkillPayload.Id.GameEntity);
			if (!_assassin.GetIsInCombat())
			{
				_assassin.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.SplinterBomb, AnimationTaskManager.ELayer.UpperBody);
			}
			return null;
		}

		private IEffectInstance OnBombHit(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload)
		{
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(_effect);
			bool flag = false;
			bool kill = false;
			Vector3 position = _payload.Projectile.Position;
			SimplePositionProvider simplePositionProvider = new SimplePositionProvider(position);
			float num = _stats.Get_float(StatTypes.EStatType.Rule_EffectRadius);
			num = (float)RulesHelper.LinearHirelingLevelValueProcessor4_float(_assassin, _stats, num);
			float num2 = num * num;
			float num3 = num * 2f;
			IEnumerable<Character> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<Character>(position, num3, CharacterHelper.FilterBombHitCharacters);
			int confusedDuration = _stats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration);
			foreach (Character item in enumerable)
			{
				EnemyCharacter enemyCharacter = item as EnemyCharacter;
				if (enemyCharacter != null)
				{
					if (!enemyCharacter.GetDeadOrAboutToDie())
					{
						if (enemyCharacter.Position.DistanceSquared(position) < num2)
						{
							flag = true;
							CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _stats, EApplyDamageFlags.SkillBomb | EApplyDamageFlags.ApplyHirelingLevelScaling1);
							s_LastBleedingBaseDamage = CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _stats, EApplyDamageFlags.ApplyGunpowder | EApplyDamageFlags.ApplyHirelingLevelScaling1 | EApplyDamageFlags.IsSkillOrigin | EApplyDamageFlags.SimulateOnly);
							m_BleedingBuffSkillEffect.Payload = new EntityIdSkillPayload(enemyCharacter.Id);
							m_BleedingBuffSkillEffect.Prepare(_assassin);
							wrapperEffectInstance.WrappedEffectInstances.Add(m_BleedingBuffSkillEffect.Apply(_assassin, _stats));
							EffectsHelper.CharacterBombHit(simplePositionProvider, item);
						}
						if (flag && enemyCharacter.GetHasZeroHealth())
						{
							kill = true;
							continue;
						}
						EffectsHelper.CharacterAlarmed(simplePositionProvider, enemyCharacter);
						EffectsHelper.CharacterConfused(enemyCharacter, confusedDuration);
					}
				}
				else if (item is AssassinCharacter)
				{
					if (EffectsHelper.CanAssassinBeHitByBomb(simplePositionProvider, num2, item as AssassinCharacter))
					{
						EffectsHelper.CharacterHit(simplePositionProvider, item);
					}
				}
				else if (item is NpcCharacter)
				{
					EffectsHelper.CharacterBombHitSpecialNpc(simplePositionProvider, num2, item);
				}
			}
			EffectsHelper.LetCrowdFleeFromBomb(position, num3);
			if (flag)
			{
				int value = int.MinValue;
				if (CombatHelper.IncreaseKillTokenCount(kill))
				{
					value = LogicMain.CurrentTurn;
				}
				LogicMain.PlayerEntity.PlayerStats.Set_int(StatTypes.EStatType.Player_LastTurnKillTokenFromBleeding, value);
			}
			if (_assassin is HirelingCharacter)
			{
				LogicMain.MissionResult.IncreaseSuccessfullHirelingAttack();
			}
			return wrapperEffectInstance;
		}

		private bool TargetAlivePredicate(EnemyCharacter _enemyCharacter)
		{
			return _enemyCharacter != null && !_enemyCharacter.GetIsDead();
		}

		private void TargetCollector(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload, BBList<EntityId> _targetEntityIds)
		{
			using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = StatValueCollection.Apply(_entity))
			{
				Vector3 position = _payload.Projectile.Position;
				float num = buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_EffectRadius);
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor4_float(_entity, _stats, num);
				BBList<EnemyCharacter> bBList = new BBList<EnemyCharacter>();
				EnemyHelper.FindEnemiesInRange(position, num, TargetAlivePredicate, bBList);
				_targetEntityIds.AddRange(bBList.Select((EnemyCharacter t) => t.Id));
			}
		}
	}
}
