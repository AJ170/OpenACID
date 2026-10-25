using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class DisguiseRule : SkillRuleBase<AssassinCharacter>
	{
		private static readonly BBList<Character> InPreparePhase = new BBList<Character>(1);

		public DisguiseRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			ActivateCategoryCheckDuringActivity(NotInPreparePhasePredicate);
			ActivateMaximumTriggeredSkillsCheckDuringActivity();
			AddMandatoryTrigger(new AssassinSpeedSkillTrigger(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, EMovementSpeed.Walk)
			{
				TriggerCondition = false,
				ActivityCondition = true,
				Predicate = NotInPreparePhasePredicate
			});
			AddMandatoryTrigger(new AssassinExposedTrigger(this, SkillFrequency, SkillCategories)
			{
				Sign = ETriggerSign.Negative,
				TriggerCondition = false,
				ActivityCondition = true,
				Predicate = NotInPreparePhasePredicate
			});
			AddTrigger(new AssassinAnonymousTrigger(this, SkillFrequency, SkillCategories & ~ETriggerCategoryFlags.InCombat)
			{
				TriggerCondition = true
			});
			AddTrigger(new HasSkillSkillTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, ETriggerCategoryFlags.InCombat, ETargetSource.Player, "Carnival_01".ToEnum<SkillTypes>()));
			AddTrigger(new HasKillTokensSkillTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, ETriggerCategoryFlags.InCombat, ETargetSource.Player, 1));
			AddTrigger(new CharacterMainStateTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, EStateType.CombatCharacterMain_Roaming)
			{
				TriggerCondition = false,
				ActivityCondition = true,
				Predicate = NotInPreparePhasePredicate
			});
			AddTrigger(new CharacterMainStateTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories & ~ETriggerCategoryFlags.InCombat, ETargetSource.RuleOwner, EStateType.CombatCharacterMain_Roaming));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ActionSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.Player, OnCombatDisguise)
				{
					Categories = ETriggerCategoryFlags.InCombat
				},
				new ExitCombatSkillEffect<AssassinCharacter>(this, ETargetSource.RuleOwner, ExitCombatMode.Disguise)
				{
					Categories = ETriggerCategoryFlags.InCombat
				},
				new NotorietyChangeSkillEffect<AssassinCharacter>(this, 0.5f, ETargetSource.RuleOwner)
				{
					Categories = ETriggerCategoryFlags.InCombat
				},
				new ActionSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.Player, OnFinishDisguise)
			};
			AddEffect(new BuffSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, new BooleanOrBuffGenerator(EBuffType.Disguised, StatValueCollection, StatTypes.EStatType.Rule_SkillDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid)
			{
				Value = true
			}));
			AddEffect(new CountdownTimerSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, StatTypes.EStatType.Rule_SkillDuration));
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnPrepareDisguise));
			AddEffect(new DelaySkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, AllReadyExitCombatAndDisguisedCondition, false, effects));
		}

		private static bool NotInPreparePhasePredicate(ITrigger _trigger, IRulesCarrier _rulesCarrier, IBuffedStatsCollectionHandle _stats)
		{
			Character item = _rulesCarrier.Cast<Character>();
			return !InPreparePhase.Contains(item);
		}

		private static bool AllReadyExitCombatAndDisguisedCondition(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, ISkillPayload _payload)
		{
			if (!_entity.GetIsDisguised() && !_entity.GetIsBeingChased())
			{
				return false;
			}
			if (!CombatController.IsActive)
			{
				return true;
			}
			CombatController instance = CombatController.Instance;
			int num = instance.ParticipantCount();
			for (int i = 0; i < num; i++)
			{
				if (!instance.GetParticipant(i).ReadyForExitCombat())
				{
					return false;
				}
			}
			return num > 0;
		}

		private static IEffectInstance OnPrepareDisguise(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			if (InPreparePhase.Count > 0)
			{
				InPreparePhase.RemoveAll((Character _c) => _c == _entity);
			}
			InPreparePhase.Add(_entity);
			CombatController instance = CombatController.Instance;
			if (CombatController.IsActive && instance.PlayerExitCombatMode == ExitCombatMode.Invalid)
			{
				instance.PlayerExitCombatMode = ExitCombatMode.EndOfCombatImminent;
			}
			return null;
		}

		private static IEffectInstance OnCombatDisguise(IEffect _effect, PlayerCharacter _player, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, ISkillPayload _payload)
		{
			float num = _player.PlayerStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth) * _stats.Get_float(StatTypes.EStatType.Rule_RetrieveHealthRelative);
			int num2 = _player.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
			int num3 = _player.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenRequiredForKill);
			float num4 = ((num3 <= 1) ? 0f : Mathf.Max(0f, (float)(num2 - 1) / (float)(num3 - 1)));
			num *= num4;
			CombatHelper.ExitKillTokenMode(true);
			if (num > 0f)
			{
				CombatHelper.ApplyGenericHeal(null, _player, EApplyDamageFlags.None, num);
			}
			int num5 = _stats.Get_int(StatTypes.EStatType.Rule_ConfusedDuration);
			if (num5 > 0)
			{
				CombatController instance = CombatController.Instance;
				int num6 = instance.ParticipantCount();
				for (int i = 0; i < num6; i++)
				{
					Character character = instance.GetParticipant(i).Character;
					if (character != _player)
					{
						character.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Confused, num5, 1f));
					}
				}
			}
			return null;
		}

		private static IEffectInstance OnFinishDisguise(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, ISkillPayload _payload)
		{
			InPreparePhase.Remove(_entity);
			return null;
		}
	}
}
