using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Helpers;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public static class EnemyHelper
	{
		private const float ConfusedDurationFactor = 0.01f;

		public static bool EnterChasing(EnemyCharacter _enemy)
		{
			if (_enemy.EnemyCharacterStateMachines.EnemyMain.IsCurrentOrScheduled(EStateType.CombatCharacterMain_Parcour))
			{
				return false;
			}
			if (_enemy.EnemyCharacterStateMachines.EnemyMain.CurrentStateType == EStateType.EnemyMain_Chasing)
			{
				return true;
			}
			return _enemy.EnemyCharacterStateMachines.EnemyMain.ScheduleStateChange(EStateType.EnemyMain_Chasing);
		}

		public static bool EnterRoaming(EnemyCharacter _enemy)
		{
			if (_enemy.EnemyCharacterStateMachines.EnemyMain.IsCurrentOrScheduled(EStateType.CombatCharacterMain_Parcour))
			{
				if (_enemy.EnemyCharacterStateMachines.EnemyMain.GetDelayedNextState(EStateType.CombatCharacterMain_Parcour) == EStateType.EnemyMain_Chasing)
				{
					_enemy.EnemyCharacterStateMachines.EnemyMain.SetDelayedNextState(EStateType.CombatCharacterMain_Parcour, EStateType.CombatCharacterMain_Roaming);
					_enemy.EnemyCharacterStateMachines.EnemyMain.SetDelayedCancelationState(EStateType.CombatCharacterMain_Parcour, EStateType.CombatCharacterMain_Roaming);
					return true;
				}
				return false;
			}
			if (_enemy.EnemyCharacterStateMachines.EnemyMain.CurrentStateType == EStateType.CombatCharacterMain_Roaming)
			{
				return true;
			}
			return _enemy.EnemyCharacterStateMachines.EnemyMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}

		public static bool EnterCombat(EnemyCharacter _enemy)
		{
			EnemyMain enemyMain = _enemy.EnemyCharacterStateMachines.EnemyMain;
			if (enemyMain.CurrentStateType == EStateType.CombatCharacterMain_Combat)
			{
				return enemyMain.ScheduledStateType == EStateType.Invalid;
			}
			if (enemyMain.CurrentStateType == EStateType.CombatCharacterMain_Parcour)
			{
				return false;
			}
			return enemyMain.ChangeStateForced(EStateType.CombatCharacterMain_Combat);
		}

		public static bool ExitCombat(EnemyCharacter _enemy)
		{
			EnemyCharacterStateMachines enemyCharacterStateMachines = _enemy.EnemyCharacterStateMachines;
			if (enemyCharacterStateMachines.EnemyMain.CurrentStateType != EStateType.CombatCharacterMain_Combat)
			{
				return true;
			}
			ExitCombatMode playerExitCombatMode = CombatController.Instance.PlayerExitCombatMode;
			if (enemyCharacterStateMachines.CharacterAwareness.CurrentStateType != EStateType.CharacterAwareness_Alarmed && !_enemy.GetIsUnconscious() && !_enemy.GetIsInCondition(EStateType.CharacterCondition_Fleeing) && playerExitCombatMode != ExitCombatMode.SmokeBomb && playerExitCombatMode != ExitCombatMode.Disguise && _enemy.GetTargetAsPlayer().AssassinCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Alive)
			{
				enemyCharacterStateMachines.CharacterAwareness.ScheduleStateChangeForced(EStateType.CharacterAwareness_Alarmed);
			}
			return enemyCharacterStateMachines.EnemyMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}

		public static bool StartThreatening(this EnemyCharacter _enemyCharacter, EntityId _victimId)
		{
			EStateType currentStateType = _enemyCharacter.EnemyCharacterStateMachines.CharacterCondition.CurrentStateType;
			bool flag = currentStateType == EStateType.CharacterCondition_ThreateningVictim;
			if (!flag && currentStateType == EStateType.CharacterCondition_Normal)
			{
				flag = true;
				CharacterCondition characterCondition = _enemyCharacter.EnemyCharacterStateMachines.CharacterCondition;
				TargetConditionState.Payload payload = characterCondition.GetPayload<TargetConditionState.Payload>(EStateType.CharacterCondition_ThreateningVictim);
				payload.Target = _victimId;
				_enemyCharacter.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.ThreatVictim, 1f));
				characterCondition.ExternalChange(EStateType.CharacterCondition_ThreateningVictim);
			}
			return flag;
		}

		public static void SetAlarmed(this EnemyCharacter _enemy, CharacterAwareness.AlarmedState.AlarmedPayload _otherPayload)
		{
			EnemyCharacterStateMachines enemyCharacterStateMachines = _enemy.EnemyCharacterStateMachines;
			CharacterAwareness characterAwareness = enemyCharacterStateMachines.CharacterAwareness;
			if (characterAwareness.CurrentStateType != EStateType.CharacterAwareness_Alarmed)
			{
				CharacterAwareness.AlarmedState.AlarmedPayload payload = characterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
				payload.CopyLastKnownValues(_otherPayload);
				characterAwareness.ScheduleStateChangeForced(EStateType.CharacterAwareness_Alarmed);
			}
		}

		public static bool GetIsFrightened(this EnemyCharacter _entity)
		{
			return _entity.Buffs.HasBuffs(EBuffType.Frightened);
		}

		public static bool GetIsSilenced(this EnemyCharacter _entity)
		{
			return _entity.EnemyNpcStats.Get_bool(StatTypes.EStatType.Enemy_Silenced);
		}

		public static float GetAttackDelayFactor(this EnemyCharacter _entity)
		{
			return _entity.EnemyNpcStats.Get_float(StatTypes.EStatType.Enemy_AttackDelayFactor);
		}

		public static bool GetIsChasing(this EnemyCharacter _entity)
		{
			return !_entity.GetIsDead() && _entity.EnemyCharacterStateMachines.EnemyMain.CurrentStateType == EStateType.EnemyMain_Chasing;
		}

		public static EWeaponState GetRangedWeaponState(this Character _entity)
		{
			return _entity.StateMachines.DoIfIs<ICharacterDrawRangedWeaponCarrier, EWeaponState>(OnGetRangedWeaponState, EWeaponState.Invalid);
		}

		public static bool GetIsRangedUnsheathedOrInAnim(this Character _character, EWeaponType _type = EWeaponType.Invalid)
		{
			ICharacterDrawRangedWeaponCarrier characterDrawRangedWeaponCarrier = _character.StateMachines as ICharacterDrawRangedWeaponCarrier;
			if (characterDrawRangedWeaponCarrier == null)
			{
				return false;
			}
			CharacterDrawWeaponBase characterDrawRangedWeapon = characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon;
			return characterDrawRangedWeapon != null && (_type == EWeaponType.Invalid || _type == characterDrawRangedWeapon.WeaponType) && (characterDrawRangedWeapon.CurrentStateType == EStateType.CharacterDrawWeapon_Unsheathed || characterDrawRangedWeapon.CurrentStateType == EStateType.Generic_WaitingForAnim);
		}

		public static bool GetIsInDisguise(this EnemyCharacter _entity)
		{
			return _entity.EnemyNpcStats.Get_bool(StatTypes.EStatType.Enemy_IsDisguised);
		}

		public static void SetDisguised(this EnemyCharacter _entity, bool _disguised)
		{
			_entity.EnemyNpcStats.Set_bool(StatTypes.EStatType.Enemy_IsDisguised, _disguised);
			if (_entity.IsAttached)
			{
				_entity.RefreshDisguise();
			}
		}

		public static void RefreshDisguise(this EnemyCharacter _entity, bool _playTransition = true)
		{
			GameObject gameObject = _entity.GameObject;
			if (gameObject == null)
			{
				return;
			}
			bool flag = _entity.EnemyNpcStats.Get_bool(StatTypes.EStatType.Enemy_IsDisguised);
			AlternativeCharacterMesh alternativeCharacterMesh = gameObject.GetComponent<AlternativeCharacterMesh>();
			bool flag2 = alternativeCharacterMesh != null && alternativeCharacterMesh.UseAlternativeMesh;
			if (flag == flag2)
			{
				return;
			}
			if (alternativeCharacterMesh == null && flag)
			{
				alternativeCharacterMesh = gameObject.AddComponent<AlternativeCharacterMesh>();
				alternativeCharacterMesh.HumanoidGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_maleCrowd;
				alternativeCharacterMesh.m_Appearance = _entity.CharacterAppearance;
			}
			alternativeCharacterMesh.PlayTransition = _playTransition;
			alternativeCharacterMesh.UseAlternativeMesh = flag;
			WeaponHandler component = gameObject.GetComponent<WeaponHandler>();
			if (component != null)
			{
				component.ShowWeapon(!flag);
			}
			GameObject gameObject2 = gameObject.FindChildByName("SelectionCollider");
			if (gameObject2 != null)
			{
				CapsuleCollider component2 = gameObject2.GetComponent<CapsuleCollider>();
				if (component2 != null)
				{
					component2.enabled = !flag;
				}
			}
		}

		public static bool CanBeSeduced(this EnemyCharacter _enemyCharacter)
		{
			return !_enemyCharacter.GetIsDead() && _enemyCharacter.IsAttached && !_enemyCharacter.GetIsInCombat() && !_enemyCharacter.GetIsUnconscious() && !_enemyCharacter.GetIsInParcour();
		}

		public static void SetConfusedByPlayerFleeing(this EnemyCharacter _enemy)
		{
			if (_enemy.Buffs.HasBuffs(EBuffType.Confused))
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			if (LogicMain.PlayerEntity.EntityStats.Get_bool(StatTypes.EStatType.Player_WasJustRevived))
			{
				num = LogicMain.PlayerEntity.Buffs.GetLatestBuffDurationEnd(StatTypes.EStatType.Player_WasJustRevived) - LogicMain.CurrentTurn;
				if (num > 0)
				{
					num2 = num;
				}
			}
			else if (PlayerHelper.GetIsActive("Flee_From_Combat".ToEnum<SkillTypes>()))
			{
				num = PlayerHelper.GetActivityEnd("Flee_From_Combat".ToEnum<SkillTypes>()) - LogicMain.CurrentTurn;
				if (num > 0)
				{
					float num3 = LogicMain.PlayerEntity.PlayerStats.Get_float(StatTypes.EStatType.Player_StealthModifier);
					float num4 = _enemy.EnemyNpcStats.Get_float(StatTypes.EStatType.CharacterDetection_MomentOfChaosTime);
					float num5 = num4 / 0.1f;
					float num6 = num3 * (float)num * 0.01f;
					num2 = (int)(num5 + num6);
				}
			}
			if (num2 > 0)
			{
				_enemy.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Confused, num2, 1f));
				if (num > num2)
				{
					_enemy.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Confused, num, 0f));
				}
			}
		}

		private static EWeaponState OnGetRangedWeaponState(ICharacterDrawRangedWeaponCarrier s)
		{
			return (s.CharacterDrawRangedWeapon == null) ? EWeaponState.Invalid : s.CharacterDrawRangedWeapon.ChosenWeaponState;
		}

		public static void FindEnemiesInRange(Vector3 _center, float _range, Predicate<EnemyCharacter> _predicate, BBList<EnemyCharacter> _result)
		{
			_result.ClearIfNotEmpty();
			IEnumerable<EnemyCharacter> collection = LogicMain.EntityManager.CollectEntitiesInRadius(_center, _range, _predicate);
			_result.AddRange(collection);
		}

		public static bool TryPickpocket(this EnemyCharacter _enemy, AssassinCharacter _assassin, Enum<SkillTypes> _skill, IBuffedStatsCollectionHandle _stats, StatTypes.EStatType _chanceHirelingLevelFactor = StatTypes.EStatType.Invalid, StatTypes.EStatType _factorHirelingLevelFactor = StatTypes.EStatType.Invalid)
		{
			if (_enemy.EnemyNpcStats.Get_bool(StatTypes.EStatType.Character_Conflict_WasPickpocketed))
			{
				return false;
			}
			float num = _stats.Get_float(StatTypes.EStatType.Rule_PickpocketingChance);
			if (_chanceHirelingLevelFactor != StatTypes.EStatType.Invalid)
			{
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor_float(_assassin, _stats, num, _chanceHirelingLevelFactor);
			}
			if (!CombatHelper.DiceRoll(num))
			{
				return false;
			}
			int num2 = _enemy.EnemyNpcStats.Get_int(StatTypes.EStatType.Enemy_PickpocketingCoins);
			if (num2 <= 0)
			{
				return false;
			}
			int num3 = MissionMathHelper.FloatToFixed(_stats.Get_float(StatTypes.EStatType.Rule_PickpocketingFactor));
			if (_factorHirelingLevelFactor != StatTypes.EStatType.Invalid)
			{
				num3 = (int)RulesHelper.LinearHirelingLevelValueProcessor_fixed(_assassin, _stats, num3, _factorHirelingLevelFactor);
			}
			num2 = MissionMathHelper.FixedToIntRounded(num2 * num3);
			num2 = Mathf.Max(num2, 1);
			LogicMain.LogicInterface.Combat_SoftCurrency(_enemy, num2);
			LogicMain.MissionResult.IncreaseSkillUsageOnNpcTargetCounter(_skill, _enemy.NpcDefinition.Definition.Type);
			_enemy.Buffs.AddBuff(new BooleanOrBuff(EBuffType.WasPickpocketed, true));
			return true;
		}
	}
}
