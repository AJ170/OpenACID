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
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class TripwireBombRule : SkillRuleBase<PlayerCharacter>
	{
		private static EntityId LastActiveProjectile = EntityId.InvalidId;

		public TripwireBombRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new AssassinAnonymousTrigger(this, SkillFrequency, SkillCategories)
			{
				TriggerCondition = true
			});
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ActionSkillEffect<PlayerCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombDeployed)
			};
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new ActionSkillEffect<PlayerCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombExplode)
			};
			SkillEffectsWrapper hitEffectsWrapper = new SkillEffectsWrapper(effects, null);
			SkillEffectsWrapper explodeEffectsWrapper = new SkillEffectsWrapper(effects2, null);
			BBList<IEffect> effects3 = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, ProjectileData.EProjectileType.TripwireBomb, hitEffectsWrapper, explodeEffectsWrapper),
				new ExposingEffect(this)
			};
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnBombThrow));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects3, false, AnimatorEventType.ProjectileStart));
		}

		private static IEffectInstance OnBombThrow(IEffect _effect, PlayerCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			_assassin.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskDeployBomb();
			_assassin.PlayerCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.DeployBomb);
			return null;
		}

		private IEffectInstance OnBombDeployed(IEffect _effect, PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, IEntityIdProjectileSkillPayload _payload)
		{
			Projectile projectile = LastActiveProjectile.TryGetEntity<Projectile>();
			if (projectile != null)
			{
				projectile.ProjectileStateMachines.ProjectileFlight.Expire();
			}
			LastActiveProjectile = _payload.Projectile.Id;
			return null;
		}

		private static IEffectInstance OnBombExplode(IEffect _effect, PlayerCharacter _assassin, IBuffedStatsCollectionHandle _ruleStats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload)
		{
			bool flag = false;
			bool kill = false;
			Vector3 position = _payload.Projectile.Position;
			SimplePositionProvider simplePositionProvider = new SimplePositionProvider(position);
			string guid = _ruleStats.Get_String(StatTypes.EStatType.Rule_FxGuid);
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(guid);
			EffectHelper.SpawnEffectInWorld(gameID, position);
			int duration = _ruleStats.Get_int(StatTypes.EStatType.Rule_BleedDuration);
			int interval = _ruleStats.Get_int(StatTypes.EStatType.Rule_BleedInterval);
			float num = _ruleStats.Get_float(StatTypes.EStatType.Rule_BleedAmountRelative);
			float num2 = _ruleStats.Get_float(StatTypes.EStatType.Rule_EffectRadius);
			IBuffGenerator generator = BuffHelpers.MakeFrightenedBuffGenerator(_ruleStats.StatValueCollection);
			float num3 = num2 * num2;
			float num4 = num2 * 2f;
			IEnumerable<Character> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<Character>(position, num4, CharacterHelper.FilterBombHitCharacters);
			int confusedDuration = _ruleStats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration);
			foreach (Character item in enumerable)
			{
				EnemyCharacter enemyCharacter = item as EnemyCharacter;
				if (enemyCharacter != null)
				{
					if (!enemyCharacter.GetDeadOrAboutToDie())
					{
						if (enemyCharacter.Position.DistanceSquared(position) < num3)
						{
							flag = true;
							CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _ruleStats, EApplyDamageFlags.SkillBomb);
							float num5 = CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _ruleStats, EApplyDamageFlags.ApplyGunpowder | EApplyDamageFlags.IsSkillOrigin | EApplyDamageFlags.SimulateOnly);
							EffectsHelper.CharacterBleeding(enemyCharacter, duration, interval, num * num5);
							BuffHelpers.ApplyBuffGenerator(generator, _assassin, enemyCharacter);
							EffectsHelper.CharacterBombHit(simplePositionProvider, enemyCharacter);
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
					if (EffectsHelper.CanAssassinBeHitByBomb(simplePositionProvider, num3, item as AssassinCharacter))
					{
						if (item is PlayerCharacter)
						{
							CombatHelper.ApplySkillDamage(_assassin, _assassin, item, _ruleStats, EApplyDamageFlags.SkillBomb);
							float num6 = CombatHelper.ApplySkillDamage(_assassin, _assassin, item, _ruleStats, EApplyDamageFlags.ApplyGunpowder | EApplyDamageFlags.IsSkillOrigin | EApplyDamageFlags.SimulateOnly);
							EffectsHelper.CharacterBleeding(item, duration, interval, num * num6);
						}
						EffectsHelper.CharacterBombHit(simplePositionProvider, item);
					}
				}
				else if (item is NpcCharacter)
				{
					EffectsHelper.CharacterBombHitSpecialNpc(simplePositionProvider, num3, item);
				}
			}
			EffectsHelper.LetCrowdFleeFromBomb(position, num4);
			if (flag)
			{
				int value = int.MinValue;
				if (CombatHelper.IncreaseKillTokenCount(kill))
				{
					value = LogicMain.CurrentTurn;
				}
				LogicMain.PlayerEntity.PlayerStats.Set_int(StatTypes.EStatType.Player_LastTurnKillTokenFromBleeding, value);
			}
			return null;
		}
	}
}
