using System;
using System.Text;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.RuleSystem.Rules;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.System;
using UbiCore;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.Combat
{
	internal static class CombatHelper
	{
		private static readonly StringBuilder Builder = new StringBuilder(100);

		public static bool LoseAllTokensOnHit = false;

		private static readonly int[] DirectionMapMatrix = new int[16]
		{
			1, 2, 3, 4, 2, 1, 4, 3, 2, 2,
			4, 4, 1, 1, 3, 3
		};

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private static NavMeshPath m_dummyPath__lazy;

		private static NavMeshPath m_dummyPath
		{
			get
			{
				return m_dummyPath__lazy ?? (m_dummyPath__lazy = new NavMeshPath());
			}
		}

		private static readonly Enum<SkillTypes> m_greedSkill = "Greed_01".ToEnum<SkillTypes>();

		private static readonly Enum<SkillTypes> m_intimidationSkill = "Intimidation_01".ToEnum<SkillTypes>();

		public static void ApplyWeaponDamage(CombatParticipant _source, CombatParticipant _target, EApplyDamageFlags _flags)
		{
			ApplyWeaponDamage(_source.Character, _target.Character, _flags);
		}

		public static void ApplyWeaponDamage(Character _source, Character _target, EApplyDamageFlags _flags)
		{
			UpdateExplicitStats(_source, _target, _flags);
			EntityStats entityStats = _source.EntityStats;
			EntityStats entityStats2 = _target.EntityStats;
			float num = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualWeaponDamage);
			if (IsSet(_flags, EApplyDamageFlags.ApplySkills) && entityStats.Get_int(StatTypes.EStatType.Character_Conflict_PiercingStrikeCount) > 0)
			{
				Skills skills = _source.Cast<ISkillsCarrier>().Skills;
				num *= skills.GetBuffedStat_float("Piercing_Strike_01".ToEnum<SkillTypes>(), StatTypes.EStatType.Rule_SkillDamageFactor);
			}
			if (IsSet(_flags, EApplyDamageFlags.ApplyStrengthDmg))
			{
				float overrideValue = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageOverride);
				num += DerivedStatsHelper.StrengthDamage(entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualStrength), overrideValue);
			}
			if (entityStats.Get_bool(StatTypes.EStatType.Character_Conflict_InitAnonymousBonus))
			{
				num *= entityStats.Get_float(StatTypes.EStatType.Character_Conflict_InitAnonymousDamageMultiplier);
			}
			float parryDmgRed = 0f;
			if (IsSet(_flags, EApplyDamageFlags.ApplyDeflect))
			{
				float num2 = entityStats2.Get_float(StatTypes.EStatType.Character_Conflict_ActualWeaponDamage);
				float num3 = entityStats2.Get_float(StatTypes.EStatType.Character_Conflict_ActualWeaponDeflect);
				float overrideValue2 = entityStats2.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageReductionOverride);
				float num4 = DerivedStatsHelper.StrengthDamageReduction(entityStats2.Get_float(StatTypes.EStatType.Character_Conflict_ActualStrength), overrideValue2);
				float num5 = DerivedStatsHelper.FeintDamage(entityStats.Get_float(StatTypes.EStatType.Character_Conflict_Feint));
				parryDmgRed = num2 + num3 + num4 - num5;
				parryDmgRed = Mathf.Max(parryDmgRed, 0f);
			}
			ApplyInternalDamage(_source, _target, _flags, num, parryDmgRed);
		}

		public static void ApplyRangedWeaponDamage(Character _source, Character _target)
		{
			UpdateExplicitStats(_source, _target, EApplyDamageFlags.RangedAttack);
			float inflictedDmg = _source.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualRangedlWeaponDamage);
			ApplyInternalDamage(_source, _target, EApplyDamageFlags.RangedAttack, inflictedDmg);
		}

		public static float ApplySkillDamage(ISkillsCarrier _skillsCarrier, Character _source, Character _target, IBuffedStatsCollectionHandle _skillStatsHandle, EApplyDamageFlags _flags)
		{
			UpdateExplicitStats(_source, _target, _flags);
			EntityStats entityStats = ((_source == null) ? null : _source.EntityStats);
			float num = _skillStatsHandle.Get_float(StatTypes.EStatType.Rule_Damage);
			if ((_flags & EApplyDamageFlags.ApplyHirelingLevelScaling1) == EApplyDamageFlags.ApplyHirelingLevelScaling1)
			{
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor1_float(_skillsCarrier, _skillStatsHandle, num);
			}
			if ((_flags & EApplyDamageFlags.ApplyHirelingLevelScaling2) == EApplyDamageFlags.ApplyHirelingLevelScaling2)
			{
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor2_float(_skillsCarrier, _skillStatsHandle, num);
			}
			if ((_flags & EApplyDamageFlags.ApplyHirelingLevelScaling3) == EApplyDamageFlags.ApplyHirelingLevelScaling3)
			{
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor3_float(_skillsCarrier, _skillStatsHandle, num);
			}
			if ((_flags & EApplyDamageFlags.ApplyHirelingLevelScaling4) == EApplyDamageFlags.ApplyHirelingLevelScaling4)
			{
				num = (float)RulesHelper.LinearHirelingLevelValueProcessor4_float(_skillsCarrier, _skillStatsHandle, num);
			}
			if (IsSet(_flags, EApplyDamageFlags.ApplyGunpowder))
			{
				num *= entityStats.Get_float(StatTypes.EStatType.Character_Conflict_GunpowderDamageBonusFactor);
			}
			float num2 = _skillStatsHandle.Get_float(StatTypes.EStatType.Rule_SkillDamageFactor);
			if (_source != null && num2 > 0f)
			{
				num2 *= entityStats.Get_float(StatTypes.EStatType.Character_Conflict_SkillDamageBonusFactor);
				float num3 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualWeaponDamage);
				float overrideValue = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageOverride);
				float num4 = DerivedStatsHelper.StrengthDamage(entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualStrength), overrideValue);
				num += num2 * (num3 + num4);
			}
			if (IsSet(_flags, EApplyDamageFlags.ApplyAnonymous) && _source.Cast<PlayerCharacter>().GetIsAnonymous())
			{
				num *= _skillStatsHandle.Get_float(StatTypes.EStatType.Rule_AnonymousDamageMultiplier);
			}
			return ApplyInternalDamage(_source, _target, _flags, num);
		}

		public static void ApplyGenericDamage(Character _source, Character _target, EApplyDamageFlags _flags, float _inflictedDmg)
		{
			UpdateExplicitStats(_source, _target, _flags);
			ApplyInternalDamage(_source, _target, _flags, _inflictedDmg);
		}

		private static float ApplyInternalDamage(Character _source, Character _target, EApplyDamageFlags _flags, float _inflictedDmg, float _parryDmgRed = 0f)
		{
			float num = 0f;
			EntityStats entityStats = ((_source == null) ? null : _source.EntityStats);
			EntityStats entityStats2 = _target.EntityStats;
			int num2 = 1;
			if (entityStats != null && IsSet(_flags, EApplyDamageFlags.ApplySkills))
			{
				bool flag = entityStats.Get_int(StatTypes.EStatType.Character_Conflict_PiercingStrikeCount) > 0;
				bool flag2 = entityStats.Get_int(StatTypes.EStatType.Character_Conflict_FastFingersStrikeCount) > 0;
				if (flag || flag2)
				{
					_flags |= EApplyDamageFlags.IsSkillOrigin;
				}
				if (flag2)
				{
					num2 = 2;
				}
			}
			bool flag3 = IsSet(_flags, EApplyDamageFlags.ApplyKillMove) && IsSet(_flags, EApplyDamageFlags.ApplyDeflect);
			float num3 = 0f;
			float num4 = 0f;
			if (IsSet(_flags, EApplyDamageFlags.ApplyArmour))
			{
				num4 = entityStats2.Get_float(StatTypes.EStatType.Character_Conflict_ArmourDamageReduction);
				num4 = Mathf.Max(num4, 0f);
				if (_source != null)
				{
					float num5 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFactor);
					float num6 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFlat);
					num4 = Mathf.Max(num4 * num5 - num6, 0f);
				}
			}
			for (int i = 0; i < num2; i++)
			{
				bool flag4 = _parryDmgRed > 0f;
				bool flag5 = false;
				float num7 = _inflictedDmg;
				if (flag3)
				{
					num7 *= LogicMain.GameDefinition.KillMoveDeflectDamageMultiplier;
				}
				num7 = Mathf.Max(num7 - _parryDmgRed - num4, 0f);
				if (IsSet(_flags, EApplyDamageFlags.ApplyCriticalDmg) && !CombatController.Instance.DebugDisableCriticalDamage)
				{
					flag5 = IsSet(_flags, EApplyDamageFlags.ForceCriticalDamage) || DiceRoll(1f - entityStats.Get_float(StatTypes.EStatType.Character_Conflict_CriticalChance));
					if (flag5)
					{
						num7 *= entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualCriticalMultiplier);
						num7 += entityStats.Get_float(StatTypes.EStatType.Character_Conflict_CriticalDamageBonus);
						if (!IsSet(_flags, EApplyDamageFlags.SimulateOnly))
						{
							ISkillsCarrier skillsCarrier = _source as ISkillsCarrier;
							if (skillsCarrier != null && skillsCarrier.Skills.Has(m_intimidationSkill))
							{
								IntimidationRule intimidationRule = skillsCarrier.Skills.GetBuffedRule(m_intimidationSkill).Cast<IntimidationRule>();
								intimidationRule.StunEffect.Prepare(skillsCarrier);
								intimidationRule.StunEffect.TargetEntityId = _target.Id;
								intimidationRule.StunEffect.Apply(skillsCarrier);
							}
						}
					}
				}
				if (entityStats != null)
				{
					num7 *= entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualDamageMultiplier);
					num7 = Math.Max(num7, entityStats.Get_float(StatTypes.EStatType.Character_Conflict_MinStrikeDamage));
				}
				LogicInterface.ECombatEventModifierFlags eCombatEventModifierFlags = LogicInterface.ECombatEventModifierFlags.None;
				if (flag5)
				{
					eCombatEventModifierFlags |= LogicInterface.ECombatEventModifierFlags.Critical;
				}
				if (flag4)
				{
					eCombatEventModifierFlags |= LogicInterface.ECombatEventModifierFlags.Deflected;
				}
				if (!IsSet(_flags, EApplyDamageFlags.SimulateOnly))
				{
					LogicMain.LogicInterface.Combat_Hit(_source, _target, num7, eCombatEventModifierFlags);
					if (flag5)
					{
						LogicMain.MissionResult.IncreaseCriticalHitCount();
					}
				}
				num3 += num7;
			}
			num = num3;
			if (IsSet(_flags, EApplyDamageFlags.SimulateOnly))
			{
				return num;
			}
			if (_source is PlayerCharacter && !IsSet(_flags, EApplyDamageFlags.IsSkillOrigin))
			{
				LogicMain.MissionResult.RegisterRegularDamageDealt(num3);
			}
			bool flag6 = _target is PlayerCharacter;
			float num8;
			if (!flag6 || !_target.EntityStats.Get_bool(StatTypes.EStatType.Player_Invulnerable))
			{
				num8 = _target.EntityStats.AddFloat(StatTypes.EStatType.Character_Conflict_Health, 0f - num3);
				if (num8 <= 0f && flag6 && (_target.EntityStats.Get_bool(StatTypes.EStatType.Player_ProtectedFromDeath) || _target.EntityStats.Get_bool(StatTypes.EStatType.Player_Invincible)))
				{
					_target.EntityStats.Set_bool(StatTypes.EStatType.Player_ProtectedFromDeath, false);
					_target.EntityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, 1f);
					num8 = 1f;
				}
			}
			else
			{
				num8 = _target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health);
			}
			if (num8 <= 0f)
			{
				if (CombatController.IsActive)
				{
					_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Combat);
				}
				if (IsSet(_flags, EApplyDamageFlags.IsSkillOrigin))
				{
					_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Skill);
				}
				else
				{
					_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Meelee);
				}
				if (IsSet(_flags, EApplyDamageFlags.SkillBomb))
				{
					_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Bomb);
				}
				if (_source == _target || (_source == null && _target is AssassinCharacter))
				{
					_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Suicide);
				}
			}
			if (IsSet(_flags, EApplyDamageFlags.ApplySkills))
			{
				_source.Buffs.RemoveBuffs(EBuffType.PiercingStrike, 1);
				_source.Buffs.RemoveBuffs(EBuffType.FastFingers, 1);
			}
			if (flag3)
			{
				SlowMotionEventManager component = LogicMain.PlayerEntity.GameObject.GetComponent<SlowMotionEventManager>();
				component.AddSlowMotion(CombatController.Instance.SlowMotionParryKillMove);
				if (num8 > 0f)
				{
					LogicMain.LogicInterface.Combat_KillMoveDeflect(_source, _target);
				}
			}
			return num;
		}

		public static void ApplyKillTokenKill(CombatParticipant _source, CombatParticipant _target)
		{
			Character character = _source.Character;
			Character character2 = _target.Character;
			EntityStats entityStats = character2.EntityStats;
			float delta = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health);
			LogicMain.LogicInterface.Combat_Hit(character, character2, delta, LogicInterface.ECombatEventModifierFlags.None);
			entityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, 0f);
			_target.Character.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.Token);
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			SpendTokensForKillMove(character);
			if (!playerEntity.Skills.Has(m_greedSkill))
			{
				return;
			}
			EnemyCharacter enemyCharacter = character2 as EnemyCharacter;
			if (enemyCharacter != null)
			{
				using (IBuffedStatsCollectionHandle stats = playerEntity.Skills.GetBuffedStatsCollectionHandle(m_greedSkill))
				{
					enemyCharacter.TryPickpocket(playerEntity, m_greedSkill, stats);
				}
			}
		}

		public static void ApplyDodge(CombatParticipant _source, CombatParticipant _target, bool _missed)
		{
			LogicMain.LogicInterface.Combat_Dodge(_source.Character, _target.Character, _missed);
		}

		public static float ComputeHealthToHeal(Character _target)
		{
			float num = _target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
			float num2 = _target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health);
			return num - num2;
		}

		public static float ApplyGenericHeal(Character _source, Character _target, EApplyDamageFlags _flags, float _healedDmg)
		{
			_healedDmg = Mathf.Min(ComputeHealthToHeal(_target), _healedDmg);
			_target.EntityStats.GetStatValue(StatTypes.EStatType.Character_Conflict_Health).Value_float += _healedDmg;
			if (_healedDmg > 0f)
			{
				LogicMain.LogicInterface.Combat_Heal(_source, _target, _healedDmg, LogicInterface.ECombatEventModifierFlags.None);
			}
			return _healedDmg;
		}

		public static bool CanKillTokenKill(Character _character)
		{
			if (!CombatController.Instance.EnableKillTokenKill)
			{
				return false;
			}
			if (_character != LogicMain.PlayerEntity)
			{
				return false;
			}
			PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
			return playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount) >= CombatController.Instance.KillTokenCountRequired;
		}

		public static void ResetKillTokenDecayClock(Character _character)
		{
			if (_character == LogicMain.PlayerEntity)
			{
				PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
				playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 1f);
			}
		}

		public static void EnterKillTokenMode()
		{
			PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
			if (!playerStats.Get_bool(StatTypes.EStatType.Player_KillTokenModeActive))
			{
				playerStats.Set_bool(StatTypes.EStatType.Player_KillTokenModeActive, true);
				playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 1f);
				LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.EnterKillTokenMode, 0);
			}
		}

		public static void ExitKillTokenMode(bool _spend = false)
		{
			PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
			if (playerStats.Get_bool(StatTypes.EStatType.Player_KillTokenModeActive))
			{
				int num = playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
				if (num > 0)
				{
					playerStats.Set_int(StatTypes.EStatType.Player_KillTokenCount, 0);
					CombatKillTokenUiEvent.EType type = ((!_spend) ? CombatKillTokenUiEvent.EType.Lost : CombatKillTokenUiEvent.EType.Spend);
					LogicMain.LogicInterface.Combat_KillToken(type, 0);
				}
				playerStats.Set_bool(StatTypes.EStatType.Player_KillTokenModeActive, false);
				playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 0f);
				LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.ExitKillTokenMode, 0);
			}
		}

		public static void EscapeKillTokenChance()
		{
			float chance = LogicMain.PlayerEntity.PlayerStats.Get_float(StatTypes.EStatType.Player_EnemyEscapeKillTokenChance);
			if (DiceRoll(chance))
			{
				IncreaseKillTokenCount(false);
			}
		}

		public static bool IncreaseKillTokenCount(bool _kill)
		{
			int _increase = 0;
			return IncreaseKillTokenCountMultiEnemy(_kill, ref _increase);
		}

		public static bool IncreaseKillTokenCountMultiEnemy(bool _kill, ref int _increase)
		{
			if (!CombatController.Instance.EnableKillTokenKill)
			{
				return false;
			}
			PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
			int num = playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
			num -= _increase;
			bool flag = playerStats.Get_bool(StatTypes.EStatType.Player_KillTokenModeActive);
			int num2 = _increase;
			if (_kill)
			{
				num2 = 1;
			}
			else if (num2 < 1 && flag)
			{
				num2 = 1;
			}
			if (num2 == _increase)
			{
				return false;
			}
			if (!flag)
			{
				EnterKillTokenMode();
			}
			num = Mathf.Clamp(num + num2, 0, CombatController.Instance.KillTokenCountRequired);
			playerStats.Set_int(StatTypes.EStatType.Player_KillTokenCount, num);
			playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 1f);
			LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.Gain, num);
			_increase = num2;
			return true;
		}

		public static void DecreaseKillTokenCount()
		{
			if (CombatController.Instance.EnableKillTokenKill)
			{
				PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
				int num = playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
				if (num > 0)
				{
					num = ((!LoseAllTokensOnHit || num <= 0) ? (num - 1) : 0);
					playerStats.Set_int(StatTypes.EStatType.Player_KillTokenCount, num);
					playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 1f);
					LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.Lost, num);
				}
				else if (playerStats.Get_bool(StatTypes.EStatType.Player_KillTokenModeActive))
				{
					ExitKillTokenMode();
				}
			}
		}

		public static void DecayKillTokenCount(Character _character)
		{
			if (!LogicMain.IsInTurnUpdate || !CombatController.Instance.EnableKillTokenKill)
			{
				return;
			}
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			PlayerStats playerStats = playerEntity.PlayerStats;
			if (_character != playerEntity)
			{
				return;
			}
			int num = playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
			if (!playerStats.Get_bool(StatTypes.EStatType.Player_KillTokenModeActive))
			{
				return;
			}
			CombatController instance = CombatController.Instance;
			bool isInCombat = playerEntity.CombatParticipantProxy.IsInCombat;
			float num2 = ((num > 0) ? ((!isInCombat) ? instance.KillTokenDecayDurationNormal : instance.KillTokenDecayDurationCombat) : ((!isInCombat) ? instance.KillTokenModeDecayDurationNormal : instance.KillTokenModeDecayDurationCombat));
			float num3 = playerStats.Get_float(StatTypes.EStatType.Player_KillTokenDecayClock);
			num3 -= playerEntity.StateMachines.TimeSinceTurnUpdate / num2;
			if (num3 <= 0f)
			{
				if (num > 0)
				{
					int num4 = 1;
					num -= num4;
					num3 += 1f;
					playerStats.Set_int(StatTypes.EStatType.Player_KillTokenCount, num);
					LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.Lost, num);
				}
				else
				{
					ExitKillTokenMode();
					num3 = 0f;
				}
			}
			playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, num3);
		}

		public static void SpendTokensForKillMove(Character _character)
		{
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			if (_character == playerEntity)
			{
				PlayerStats playerStats = playerEntity.PlayerStats;
				int num = playerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
				playerStats.Set_int(StatTypes.EStatType.Player_KillTokenCount, 1);
				playerStats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 1f);
				if (num > 1)
				{
					LogicMain.LogicInterface.Combat_KillToken(CombatKillTokenUiEvent.EType.Spend, num - 1);
				}
			}
		}

		public static Vector3 ReactionDirectionAngle(IPositionProvider _source, Character _target)
		{
			Vector3 from = ReactionDirectionRaw(_source, _target);
			float num = Vector3.Angle(from, Vector3.forward);
			if (num < 45f)
			{
				return new Vector3(0f, 0f, 1f);
			}
			if (num < 90f)
			{
				return new Vector3(Mathf.Sign(from.x), 0f, 0f);
			}
			return new Vector3(0f, 0f, -1f);
		}

		public static Vector3 ReactionDirection(IPositionProvider _source, Character _target)
		{
			Vector3 vector = ReactionDirectionRaw(_source, _target);
			float num = Mathf.Sign(vector.x);
			float num2 = Mathf.Sign(vector.z);
			if (num2 * vector.z > num * vector.x)
			{
				return new Vector3(0f, 0f, num2);
			}
			return new Vector3(num, 0f, 0f);
		}

		private static Vector3 ReactionDirectionRaw(IPositionProvider _source, Character _target)
		{
			Vector3 vector = _source.Position - _target.Position;
			vector = Quaternion.Inverse(_target.Rotation) * vector;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude < 0.001f)
			{
				return new Vector3(0f, 0f, 1f);
			}
			return vector / magnitude;
		}

		public static AttackMeleeType DieAnimationDirection(AttackMeleeType _attackMeleeType, Vector3 _roundedReactionDir)
		{
			if (_attackMeleeType == AttackMeleeType.Invalid)
			{
				return AttackMeleeType.Invalid;
			}
			_attackMeleeType = MapToDirectionalAttackMeleeType(_attackMeleeType);
			int num = 0;
			if (_roundedReactionDir.z > 0f)
			{
				num = 0;
			}
			else if (_roundedReactionDir.z < 0f)
			{
				num = 4;
			}
			else if (_roundedReactionDir.x > 0f)
			{
				num = 8;
			}
			else if (_roundedReactionDir.x < 0f)
			{
				num = 12;
			}
			return (AttackMeleeType)DirectionMapMatrix[(int)(num + _attackMeleeType - 1)];
		}

		private static AttackMeleeType MapToDirectionalAttackMeleeType(AttackMeleeType _attackMeleeType)
		{
			switch (_attackMeleeType)
			{
			case AttackMeleeType.Invalid:
			case AttackMeleeType.UpLeft:
			case AttackMeleeType.UpRight:
			case AttackMeleeType.DownLeft:
			case AttackMeleeType.DownRight:
				return _attackMeleeType;
			case AttackMeleeType.StunningStrike:
				return (LogicMain.Random.RandF() > 0.5f) ? AttackMeleeType.UpLeft : AttackMeleeType.DownLeft;
			case AttackMeleeType.ShreddingArmor:
				return AttackMeleeType.UpLeft;
			case AttackMeleeType.WaveOfFury:
				return (!(LogicMain.Random.RandF() > 0.5f)) ? AttackMeleeType.DownRight : AttackMeleeType.UpRight;
			default:
				return (AttackMeleeType)(1 + LogicMain.Random.Rand(0, 3));
			}
		}

		public static bool DecideToMiss(CombatParticipant _source, CombatParticipant _target)
		{
			Character character = _source.Character;
			Character character2 = _target.Character;
			UpdateExplicitStatsSource(character, character2, EApplyDamageFlags.None);
			float chance = character.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_AttackChance);
			return !DiceRoll(chance);
		}

		public static bool DecideToDodge(CombatParticipant _source, CombatParticipant _target, float _dodgeFactor, bool _applyFeint)
		{
			Character character = _source.Character;
			Character character2 = _target.Character;
			return DecideToDodge(character, character2, _dodgeFactor, _applyFeint);
		}

		public static bool DecideToDodge(Character _sourceCharacter, Character _targetCharacter, float _dodgeFactor = 1f, bool _applyFeint = true)
		{
			UpdateExplicitStats(_sourceCharacter, _targetCharacter, EApplyDamageFlags.None);
			float num = 1f - _targetCharacter.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_DodgeChance);
			num *= _dodgeFactor;
			float num2 = 0f;
			if (_applyFeint)
			{
				float feint = _sourceCharacter.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Feint);
				num2 = DerivedStatsHelper.FeintDodgeChance(feint);
			}
			return DiceRoll(num - num2);
		}

		public static bool DecideToFlee(CombatParticipant _source, CombatParticipant _target, CombatParticipant _victim, float _fearFactor)
		{
			UpdateExplicitStats(null, _target.Character, EApplyDamageFlags.None);
			CombatController instance = CombatController.Instance;
			float num = _target.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualCourage);
			float num2 = num;
			if (_victim != null)
			{
				num2 = _victim.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseCourage);
			}
			float num3 = Mathf.Clamp01(1f - num) * Mathf.Clamp(num2 + 1f, 1f, 2f);
			EnemyCharacter enemyCharacter = _target.Character as EnemyCharacter;
			if (enemyCharacter != null)
			{
				_fearFactor *= enemyCharacter.EnemyNpcStats.Get_float(StatTypes.EStatType.Enemy_FearsomeFactor);
			}
			PlayerCharacter playerCharacter = _source.Character as PlayerCharacter;
			if (playerCharacter != null)
			{
				_fearFactor *= playerCharacter.PlayerStats.Get_float(StatTypes.EStatType.Player_ActualFearsomeFactor);
			}
			int num4 = LogicMain.GameStats.Get_int(StatTypes.EStatType.Game_RecentlyKilledEnemyCount);
			float num5 = Math.Max(1f, (float)(num4 + 1) / (float)instance.ParticipantCount());
			num5 *= num3 * _fearFactor * instance.GlobalFleeChanceMultiplier;
			return DiceRoll(num5);
		}

		public static bool DecideToFlee(CombatParticipant _target, float _fearFactor)
		{
			CombatController instance = CombatController.Instance;
			return DecideToFlee(instance.PlayerParticipant, _target, null, _fearFactor);
		}

		public static void StartFleeing(Character _character, FleeingConditionState.Reason _fleeingReason)
		{
			int num = 0;
			switch (_fleeingReason)
			{
			case FleeingConditionState.Reason.Fleeing:
				num = Mathf.RoundToInt(LogicMain.GameDefinition.EnemyFleeFromCombatDuration / 0.1f);
				break;
			case FleeingConditionState.Reason.WalkFromCombat:
				num = Mathf.RoundToInt(LogicMain.GameDefinition.EnemyWalkFromCombatDuration / 0.1f);
				break;
			}
			if (num > 0)
			{
				_character.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Fleeing, num, 1f));
				CharacterCondition characterCondition = _character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				characterCondition.GetPayload<FleeingConditionState.Payload>(EStateType.CharacterCondition_Fleeing).Reason = _fleeingReason;
			}
		}

		public static bool TargetNeedsWeaponForSkill(Enum<SkillTypes> _skill)
		{
			if (_skill == Enum<SkillTypes>.Invalid)
			{
				return false;
			}
			return _skill.GetDefinition().BaseSkillType.Name == "Stunning_Strike_01";
		}

		public static void UpdateExplicitStats(Character _source, Character _target, EApplyDamageFlags _flags)
		{
			UpdateExplicitStatsSource(_source, _target, _flags);
			UpdateExplicitStatsTarget(_source, _target, _flags);
		}

		public static void UpdateExplicitStatsSource(Character _source, Character _target, EApplyDamageFlags _flags)
		{
			if (_source != null)
			{
				EntityStats entityStats = _source.EntityStats;
				entityStats.Set_int(StatTypes.EStatType.Character_Conflict_CurrentDamageFlags, (int)_flags);
				entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentSourceCharacter, EntityId.InvalidId);
				entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentTargetCharacter, (_target == null) ? EntityId.InvalidId : _target.Id);
				entityStats.UpdateComputedStatsExplicit();
				if (_target != null)
				{
					entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentTargetCharacter, EntityId.InvalidId);
				}
				entityStats.Set_int(StatTypes.EStatType.Character_Conflict_CurrentDamageFlags, 0);
			}
		}

		public static void UpdateExplicitStatsTarget(Character _source, Character _target, EApplyDamageFlags _flags)
		{
			if (_target != null)
			{
				EntityStats entityStats = _target.EntityStats;
				entityStats.Set_int(StatTypes.EStatType.Character_Conflict_CurrentDamageFlags, (int)_flags);
				entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentSourceCharacter, (_source == null) ? EntityId.InvalidId : _source.Id);
				entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentTargetCharacter, EntityId.InvalidId);
				entityStats.UpdateComputedStatsExplicit();
				if (_source != null)
				{
					entityStats.Set_EntityId(StatTypes.EStatType.Character_Conflict_CurrentSourceCharacter, EntityId.InvalidId);
				}
				entityStats.Set_int(StatTypes.EStatType.Character_Conflict_CurrentDamageFlags, 0);
			}
		}

		public static int RandomWithoutRepetition(int _minInclusive, int _maxInclusive, int _lastValue)
		{
			if (_minInclusive == _maxInclusive)
			{
				return _minInclusive;
			}
			if (_lastValue >= _minInclusive && _lastValue < _maxInclusive)
			{
				int num = LogicMain.Random.Rand(_minInclusive, _maxInclusive - 1);
				return (num < _lastValue) ? num : (num + 1);
			}
			_maxInclusive = ((_lastValue != _maxInclusive) ? _maxInclusive : (_maxInclusive - 1));
			return LogicMain.Random.Rand(_minInclusive, _maxInclusive);
		}

		public static bool DiceRoll(float _chance)
		{
			if (_chance < MathHelper.Epsilon)
			{
				return false;
			}
			return _chance >= LogicMain.Random.RandF();
		}

		public static bool IsSet(EApplyDamageFlags _flags, EApplyDamageFlags _checkFor)
		{
			return (_flags & _checkFor) == _checkFor;
		}

		public static bool IsCombatReachable(Vector3 _enemyPos, Vector3 _playerPos)
		{
			return NavMesh.CalculatePath(_enemyPos, _playerPos, NavMeshLayers.WalkableLayerMaskPreset, m_dummyPath) && m_dummyPath.status == NavMeshPathStatus.PathComplete;
		}

		private static void DebugCharacterType(Character _character)
		{
			string value = ((!_character.IsAttached) ? _character.Name : _character.GameObject.name);
			Builder.Append(value);
			INpcDefinitionCarrier npcDefinitionCarrier = _character as INpcDefinitionCarrier;
			if (npcDefinitionCarrier != null)
			{
				string name = npcDefinitionCarrier.NpcDefinition.Definition.Name;
				Builder.Append(" (");
				Builder.Append(name.Substring(name.IndexOf('_') + 1));
				Builder.Append(')');
			}
		}
	}
}
