using System.Collections.Generic;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class ThrowSandRule : ApproachSkillRuleBase
	{
		public ThrowSandRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMandatoryTrigger(new LivingEnemySkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnHitSand)
			};
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnThrowSand));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, true, AnimatorEventType.Hit));
		}

		private static IEffectInstance OnThrowSand(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			IPositionEntity entity = _targetEntityId.GetEntity<IPositionEntity>();
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskThrowSand(entity);
			if (!_entity.GetIsInCombat())
			{
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.ThrowSand);
			}
			return null;
		}

		private static IEffectInstance OnHitSand(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			int blindDuration = _stats.Get_int(StatTypes.EStatType.Rule_BlindedDuration);
			int num = _stats.Get_int(StatTypes.EStatType.Rule_BleedDuration);
			int interval = _stats.Get_int(StatTypes.EStatType.Rule_BleedInterval);
			float num2 = _stats.Get_float(StatTypes.EStatType.Rule_BleedAmountRelative);
			BBList<EnemyCharacter> bBList = EffectsHelper.CollectBestTargetCharacters(_entity, _targetEntityId, _stats);
			for (int i = 0; i < bBList.Count; i++)
			{
				EnemyCharacter target = bBList[i];
				EffectsHelper.CharacterBlinded(target, blindDuration);
				EffectsHelper.CharacterBleedingRelative(target, num, interval, num2);
				EffectsHelper.CharacterSanded(target, 14);
			}
			if (num2 > 0f && num > 0)
			{
				LogicMain.PlayerEntity.PlayerStats.Set_int(StatTypes.EStatType.Player_LastTurnKillTokenFromBleeding, int.MinValue);
			}
			return null;
		}
	}
}
