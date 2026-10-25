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
	public class JackInTheBoxRule : SkillRuleBase<PlayerCharacter>
	{
		public JackInTheBoxRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new AssassinAnonymousTrigger(this, SkillFrequency, SkillCategories)
			{
				TriggerCondition = true
			});
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ActionSkillEffect<PlayerCharacter, IEntityIdProjectileSkillPayload>(this, ETargetSource.Payload, OnBombExplode),
				new FearsomeSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner)
			};
			SkillEffectsWrapper explodeEffectsWrapper = new SkillEffectsWrapper(effects, null);
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, ProjectileData.EProjectileType.JackInTheBox, null, explodeEffectsWrapper),
				new ExposingEffect(this)
			};
			BBList<IEffect> effects3 = WrapEffect(new JackBoxAreaSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner));
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnBombThrow));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects2, false, AnimatorEventType.ProjectileStart));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects3, false, AnimatorEventType.Hit));
		}

		private static BBList<IEffect> WrapEffect(IEffect _effect)
		{
			BBList<IEffect> bBList = new BBList<IEffect>();
			bBList.Add(_effect);
			return bBList;
		}

		private static IEffectInstance OnBombThrow(IEffect _effect, PlayerCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			_assassin.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskDeployBomb();
			_assassin.PlayerCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.DeployBomb);
			return null;
		}

		private static IEffectInstance OnBombExplode(IEffect _effect, PlayerCharacter _assassin, IBuffedStatsCollectionHandle _ruleStats, EntityId _targetEntityId, IEntityIdProjectileSkillPayload _payload)
		{
			Vector3 position = _payload.Projectile.Position;
			SimplePositionProvider simplePositionProvider = new SimplePositionProvider(position);
			bool flag = false;
			bool kill = false;
			string guid = _ruleStats.Get_String(StatTypes.EStatType.Rule_FxGuid);
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(guid);
			EffectHelper.SpawnEffectInWorld(gameID, position);
			int silenceDuration = _ruleStats.Get_int(StatTypes.EStatType.Rule_SilencedDuration);
			int blindDuration = _ruleStats.Get_int(StatTypes.EStatType.Rule_BlindedDuration);
			float num = _ruleStats.Get_float(StatTypes.EStatType.Rule_EffectRadius);
			float num2 = num * num;
			float num3 = num * 2f;
			int confusedDuration = _ruleStats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration);
			IEnumerable<Character> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<Character>(position, num3, CharacterHelper.FilterBombHitCharacters);
			foreach (Character item in enumerable)
			{
				EnemyCharacter enemyCharacter = item as EnemyCharacter;
				if (enemyCharacter != null)
				{
					if (!enemyCharacter.GetDeadOrAboutToDie())
					{
						if (item.Position.DistanceSquared(position) <= num2)
						{
							flag = true;
							CombatHelper.ApplySkillDamage(_assassin, _assassin, enemyCharacter, _ruleStats, EApplyDamageFlags.SkillBomb);
							EffectsHelper.CharacterBombHit(simplePositionProvider, enemyCharacter);
						}
						if (flag && enemyCharacter.GetHasZeroHealth())
						{
							kill = true;
							continue;
						}
						EffectsHelper.CharacterAlarmed(simplePositionProvider, enemyCharacter);
						EffectsHelper.CharacterConfused(enemyCharacter, confusedDuration);
						EffectsHelper.CharacterSilence(enemyCharacter, silenceDuration);
						EffectsHelper.CharacterBlinded(enemyCharacter, blindDuration);
					}
				}
				else if (item is AssassinCharacter)
				{
					if (EffectsHelper.CanAssassinBeHitByBomb(simplePositionProvider, num2, item as AssassinCharacter))
					{
						if (item is PlayerCharacter)
						{
							CombatHelper.ApplySkillDamage(_assassin, _assassin, item, _ruleStats, EApplyDamageFlags.SkillBomb);
						}
						EffectsHelper.CharacterBombHit(simplePositionProvider, item);
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
				CombatHelper.IncreaseKillTokenCount(kill);
			}
			return null;
		}
	}
}
