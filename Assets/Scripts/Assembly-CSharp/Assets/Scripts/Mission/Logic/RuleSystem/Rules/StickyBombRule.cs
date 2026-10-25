using System.Collections.Generic;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class StickyBombRule : ApproachSkillRuleBase
	{
		private const int IconPriority = 3;

		private const float ExplosionRadiusFactor = 2f;

		private const float PanicCrowdFleeRadiusFactor = 2f;

		private const float PanicConfuseEnemiesRadiusFactor = 2f;

		private const float PanicConfusedDurationFactor = 1f;

		public StickyBombRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new LivingEnemySkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload)
			{
				ActivityCondition = true
			});
			SkillEffectsWrapper hitEffectsWrapper = WrapSkillEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombHit));
			SkillEffectsWrapper explodeEffectsWrapper = new SkillEffectsWrapper(new BBList<IEffect>
			{
				new BuffSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, BuffHelpers.MakeArmoredBuffGenerator(StatValueCollection))
				{
					IsAbortable = false,
					Predicate = PowderMonkeyPredicate
				},
				new ActionSkillEffect<AssassinCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombExplode)
			});
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ProjectileData.EProjectileType.StickyBomb, hitEffectsWrapper, explodeEffectsWrapper)
			};
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnBombThrow));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<AssassinCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, false, AnimatorEventType.ProjectileStart));
		}

		private bool PowderMonkeyPredicate(IEffect _effect, IRulesCarrier _rulesCarrier)
		{
			return _rulesCarrier is PlayerCharacter;
		}

		private static SkillEffectsWrapper WrapSkillEffect(IEffect _effect)
		{
			BBList<IEffect> bBList = new BBList<IEffect>();
			bBList.Add(_effect);
			BBList<IEffect> effects = bBList;
			return new SkillEffectsWrapper(effects);
		}

		private static IEffectInstance OnBombThrow(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskSplinterBomb(_payload.Id.GameEntity);
			if (!_entity.GetIsInCombat())
			{
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.SplinterBomb, AnimationTaskManager.ELayer.UpperBody);
			}
			return null;
		}

		private static IEffectInstance OnBombHit(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _ruleStats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload)
		{
			Character character = _targetEntityId.TryGetEntity<Character>();
			if (character == null)
			{
				return null;
			}
			EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)_payload.Projectile.EntityStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
			bool flag = false;
			IParticipantCarrier participantCarrier = character as IParticipantCarrier;
			if (participantCarrier != null)
			{
				CombatParticipant combatParticipant = participantCarrier.CombatParticipantProxy.CombatParticipant;
				if (combatParticipant != null && combatParticipant.IsInCombat)
				{
					flag = combatParticipant.KnowinglyInCombat;
				}
			}
			if (eProjectileFlightType == EProjectileFlightType.StickyBombFront && !flag)
			{
				character.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.StickyBombPanic, 1f));
				CharacterCondition characterCondition = character.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				StickyBombPanicConditionState.Payload payload = characterCondition.GetPayload<StickyBombPanicConditionState.Payload>(EStateType.CharacterCondition_StickyBombPanic);
				payload.TargetPosition = character.Position;
				characterCondition.ExternalChange(EStateType.CharacterCondition_StickyBombPanic);
				Vector3 position = _payload.Projectile.Position;
				SimplePositionProvider bombPos = new SimplePositionProvider(position);
				float effectRadius = GetEffectRadius(_assassin, _ruleStats);
				EffectsHelper.LetCrowdFleeFromBomb(position, effectRadius * 2f);
				IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(position, effectRadius * 2f);
				int confusedDurationTurns = (int)((float)_ruleStats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration) * 1f);
				foreach (EnemyCharacter item in enumerable)
				{
					if (item != character && !item.GetDeadOrAboutToDie())
					{
						AlarmAndConfuseEnemy(bombPos, item, confusedDurationTurns);
					}
				}
			}
			character.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.StickyBomb, 0f));
			character.HeadIcon.IconAttachment.SetIcon(IconMaterial.ConditionStickyBomb, 3);
			return null;
		}

		private static IEffectInstance OnBombExplode(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _ruleStats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload)
		{
			Character character = _targetEntityId.TryGetEntity<Character>();
			Vector3 position = _payload.Projectile.Position;
			SimplePositionProvider simplePositionProvider = new SimplePositionProvider(position);
			bool flag = false;
			bool flag2 = false;
			string guid = _ruleStats.Get_String(StatTypes.EStatType.Rule_FxGuid);
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(guid);
			EffectHelper.SpawnEffectInWorld(gameID, position);
			int turnCount = _ruleStats.Get_int(StatTypes.EStatType.Rule_KnockdownDuration);
			turnCount = RulesHelper.LinearHirelingLevelTurnCountProcessor2(_assassin, _ruleStats, turnCount);
			float effectRadius = GetEffectRadius(_assassin, _ruleStats);
			float num = effectRadius * effectRadius;
			float num2 = effectRadius * 2f;
			IEnumerable<Character> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<Character>(position, num2, CharacterHelper.FilterBombHitCharacters);
			int num3 = _ruleStats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration);
			foreach (Character item in enumerable)
			{
				EnemyCharacter enemyCharacter = item as EnemyCharacter;
				if (enemyCharacter != null)
				{
					if (enemyCharacter != character && !enemyCharacter.GetDeadOrAboutToDie())
					{
						if (enemyCharacter.Position.DistanceSquared(position) <= num)
						{
							flag = true;
							CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _ruleStats, EApplyDamageFlags.SkillBomb | EApplyDamageFlags.ApplyHirelingLevelScaling1);
							EffectsHelper.CharacterBombHit(simplePositionProvider, enemyCharacter);
						}
						if (flag && enemyCharacter.GetHasZeroHealth())
						{
							flag2 = true;
						}
						else
						{
							AlarmAndConfuseEnemy(simplePositionProvider, enemyCharacter, num3);
						}
					}
				}
				else if (item is AssassinCharacter)
				{
					if (EffectsHelper.CanAssassinBeHitByBomb(simplePositionProvider, num, item as AssassinCharacter))
					{
						if (item is PlayerCharacter)
						{
							CombatHelper.ApplySkillDamage(_assassin, _assassin, item, _ruleStats, EApplyDamageFlags.SkillBomb | EApplyDamageFlags.ApplyHirelingLevelScaling1);
						}
						EffectsHelper.CharacterBombHit(simplePositionProvider, item);
					}
				}
				else if (item is NpcCharacter)
				{
					EffectsHelper.CharacterBombHitSpecialNpc(simplePositionProvider, num, item);
				}
			}
			EffectsHelper.LetCrowdFleeFromBomb(position, num2);
			if (character != null)
			{
				EntityIcon iconAttachment = character.HeadIcon.IconAttachment;
				EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)_payload.Projectile.EntityStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
				if (eProjectileFlightType == EProjectileFlightType.StickyBombFront)
				{
					character.Buffs.RemoveBuffs(EBuffType.StickyBombPanic, 1);
				}
				character.Buffs.RemoveBuffs(EBuffType.StickyBomb, 1);
				if (!character.Buffs.HasBuffs(EBuffType.StickyBomb) && iconAttachment.GetIcon(3) == IconMaterial.ConditionStickyBomb)
				{
					iconAttachment.SetIcon(IconMaterial.Invalid, 3);
				}
				if (!character.GetDeadOrAboutToDie())
				{
					CombatHelper.ApplySkillDamage(_assassin, _assassin, character, _ruleStats, EApplyDamageFlags.SkillBomb | EApplyDamageFlags.ApplyHirelingLevelScaling1);
					EffectsHelper.CharacterBombHit(simplePositionProvider, character, turnCount);
					EffectsHelper.CharacterAlarmed(simplePositionProvider, character);
					EffectsHelper.CharacterConfused(character, num3);
					flag = true;
					flag2 |= character.GetHasZeroHealth();
				}
			}
			if (flag)
			{
				CombatHelper.IncreaseKillTokenCount(flag2);
			}
			if (_assassin is HirelingCharacter)
			{
				LogicMain.MissionResult.IncreaseSuccessfullHirelingAttack();
			}
			return null;
		}

		private static float GetEffectRadius(AssassinCharacter _assassin, IBuffedStatsCollectionHandle _ruleStats)
		{
			float num = _ruleStats.Get_float(StatTypes.EStatType.Rule_EffectRadius);
			return (float)RulesHelper.LinearHirelingLevelValueProcessor3_float(_assassin, _ruleStats, num);
		}

		private static void AlarmAndConfuseEnemy(SimplePositionProvider _bombPos, EnemyCharacter _enemyCharacter, int _confusedDurationTurns)
		{
			EffectsHelper.CharacterAlarmed(_bombPos, _enemyCharacter);
			EffectsHelper.CharacterConfused(_enemyCharacter, _confusedDurationTurns);
		}
	}
}
