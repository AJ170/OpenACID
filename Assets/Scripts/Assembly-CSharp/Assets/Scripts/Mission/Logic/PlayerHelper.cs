using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.System;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public static class PlayerHelper
	{
		public static PlayerCharacter Entity
		{
			get
			{
				return LogicMain.PlayerEntity;
			}
		}

		public static AnimationController AnimationController
		{
			get
			{
				return Entity.UnityComponentCache.AnimationController;
			}
		}

		public static AnimationTaskManager AnimationTaskManager
		{
			get
			{
				return Entity.UnityComponentCache.AnimationController.TaskManager;
			}
		}

		public static AssassinInteraction AssassinInteraction
		{
			get
			{
				return Entity.AssassinInteraction;
			}
		}

		public static bool IsApproaching
		{
			get
			{
				return Entity.GetIsApproaching();
			}
		}

		public static bool IsApproachingTappedPosition
		{
			get
			{
				return Entity.GetIsApproachingTappedPosition();
			}
		}

		public static bool IsApproachingPosition
		{
			get
			{
				return Entity.GetIsApproachingPosition();
			}
		}

		public static Vector3 TargetPosition
		{
			get
			{
				return Entity.GetTargetPosition();
			}
		}

		public static bool IsInCombat
		{
			get
			{
				return Entity.GetIsInCombat();
			}
		}

		public static bool IsBeingChased
		{
			get
			{
				return Entity.GetIsBeingChased();
			}
		}

		public static bool IsInParcour
		{
			get
			{
				return Entity.GetIsInParcour();
			}
		}

		public static bool IsUnseen
		{
			get
			{
				return Entity.GetIsUnseen();
			}
		}

		public static bool IsExposed
		{
			get
			{
				return Entity.GetIsExposed();
			}
		}

		public static bool IsStalking
		{
			get
			{
				return Entity.GetIsStalking();
			}
		}

		public static bool IsInRooftopGarden
		{
			get
			{
				return Entity.GetIsInRooftopGarden();
			}
		}

		public static bool IsInHaystack
		{
			get
			{
				return Entity.GetIsInHaystack();
			}
		}

		public static bool IsOnBench
		{
			get
			{
				return Entity.GetIsOnBench();
			}
		}

		public static bool IsHidingInPatrol
		{
			get
			{
				return Entity.GetIsHidingInPatrol();
			}
		}

		public static Assets.Scripts.Mission.Logic.Entities.Patrol HidingPatrol
		{
			get
			{
				return Entity.GetHidingPatrol();
			}
		}

		public static bool IsHidingInCourtesans
		{
			get
			{
				return Entity.GetIsHidingInCourtesans();
			}
		}

		public static bool IsHiding
		{
			get
			{
				return Entity.GetIsHiding();
			}
		}

		public static bool IsHidingInObject
		{
			get
			{
				return Entity.GetIsHidingInObject();
			}
		}

		public static bool IsDead
		{
			get
			{
				return Entity.GetIsDead();
			}
		}

		public static bool IsImmortal
		{
			get
			{
				return Entity.GetIsImmortal();
			}
		}

		public static bool IsRoaming
		{
			get
			{
				return Entity.GetIsRoaming();
			}
		}

		public static bool IsWaitingForAnim
		{
			get
			{
				return Entity.GetIsWaitingForAnim();
			}
		}

		public static bool IsInDisguise
		{
			get
			{
				return Entity.GetIsInDisguise();
			}
		}

		public static bool IsPerformingLeapAssassination
		{
			get
			{
				return Entity.GetIsPerformingLeapAssasination();
			}
		}

		public static bool IsPerformingStealthAssassination
		{
			get
			{
				return Entity.GetIsPerformingStealthAssasination();
			}
		}

		public static bool IsPerformingAirAssassination
		{
			get
			{
				return Entity.GetIsPerformingAirAssasination();
			}
		}

		public static bool IsPerformingAssassination
		{
			get
			{
				return Entity.GetIsPerformingAssassination();
			}
		}

		public static bool IsPerformingStrikingAssassination
		{
			get
			{
				return Entity.GetIsPerformingStrikingAssassination();
			}
		}

		public static bool IsApproachingTarget
		{
			get
			{
				return Entity.GetIsApproachingTarget();
			}
		}

		public static bool IsUsingEagleEye
		{
			get
			{
				return Entity.GetIsUsingEagleEye();
			}
		}

		public static EntityId CurrentInteractionObject
		{
			get
			{
				return Entity.GetCurrentInteractionObject();
			}
		}

		public static EntityId CurrentInteractionNpc
		{
			get
			{
				return Entity.GetCurrentInteractionNpc();
			}
		}

		public static PlayerStats Stats
		{
			get
			{
				return Entity.PlayerStats;
			}
		}

		public static PlayerMain PlayerMain
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.PlayerMain;
			}
		}

		public static IStateMachine<ICharacterVitalityCarrierEntity> PlayerVitality
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.CharacterVitality;
			}
		}

		public static PlayerAlertLevel PlayerAlertLevel
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.PlayerAlertLevel;
			}
		}

		public static CharacterCondition CharacterCondition
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.CharacterCondition;
			}
		}

		public static PlayerVision PlayerVision
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.PlayerVision;
			}
		}

		public static PlayerProfile PlayerProfile
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.PlayerProfile;
			}
		}

		public static AssassinApproachingTarget AssassinApproachingTarget
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.AssassinApproachingTarget;
			}
		}

		public static CharacterDrawWeaponBase CharacterDrawWeapon
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.CharacterDrawMeleeWeapon;
			}
		}

		public static PlayerDisguise PlayerDisguise
		{
			get
			{
				return Entity.PlayerCharacterStateMachines.PlayerDisguise;
			}
		}

		public static EWeaponState WeaponState
		{
			get
			{
				return Entity.GetMeleeWeaponState();
			}
		}

		public static bool IsAttackable
		{
			get
			{
				return Entity.GetIsAttackable();
			}
		}

		public static bool IsMovementAllowed
		{
			get
			{
				return Entity.GetIsMovementAllowed();
			}
		}

		public static EMovementSpeed Speed
		{
			get
			{
				return Entity.GetSpeed();
			}
		}

		public static Enum<SkillTypes>[] SkillSlots
		{
			get
			{
				return Entity.Skills.SkillSlots;
			}
		}

		public static Enum<SkillTypes>[] SkillSlotsHirelingPassive
		{
			get
			{
				return Entity.Skills.SkillSlotsHirelingPassive;
			}
		}

		public static Enum<SkillTypes>[] SkillWheel
		{
			get
			{
				return Entity.Skills.SkillWheel;
			}
		}

		public static Enum<SkillTypes> SelectedSkill
		{
			get
			{
				return Entity.Skills.SelectedSkill;
			}
		}

		public static void Die()
		{
			Entity.Die();
		}

		public static bool Revive()
		{
			return Entity.Revive();
		}

		public static bool PrepareCombat(bool _simulate = false)
		{
			return Entity.PrepareCombat(_simulate);
		}

		public static bool EnterParcour(ParcourEvent _event)
		{
			return Entity.EnterParcour(_event);
		}

		public static bool ExitBench(Vector3 _destination, BenchManager.ELeaveReason _reason)
		{
			return Entity.ExitBench(_destination, _reason);
		}

		public static bool ExitHaystack(Vector3 _destination)
		{
			return Entity.ExitHaystack(_destination);
		}

		public static bool StopMoving()
		{
			return Entity.StopMoving();
		}

		public static void EnterRoaming()
		{
			Entity.EnterRoaming();
		}

		public static bool EnterBeingPushed(EntityId _instigator)
		{
			return Entity.EnterBeingPushed(_instigator);
		}

		public static bool EnterInitCombat(EnemyCharacter _enemy, bool _enemyIsInstigator)
		{
			if (!PrepareCombat())
			{
				return false;
			}
			PlayerCharacter entity = Entity;
			if (entity.GetDeadOrAboutToDie() || _enemy.GetDeadOrAboutToDie())
			{
				return false;
			}
			if (!_enemy.IsAttached)
			{
				return false;
			}
			if (!CombatHelper.IsCombatReachable(_enemy.Position, entity.Position))
			{
				return false;
			}
			if (_enemy.GetIsInParcour() || CharacterHelper.GetIsInParcour(entity) || _enemy.GetIsSchedulingParcour() || entity.GetIsSchedulingParcour())
			{
				return false;
			}
			if (entity.GetIsInCombat())
			{
				if (CombatController.Instance.PlayerExitCombatMode == ExitCombatMode.Invalid)
				{
					return CombatController.Instance.AddParticipant(_enemy, EnterCombatMode.Normal);
				}
				return false;
			}
			if (IsPerformingAssassination || _enemy.GetIsInCombat())
			{
				return false;
			}
			if (_enemyIsInstigator && _enemy.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range) >= 3)
			{
				Vector3 headPosition = _enemy.GetHeadPosition();
				Vector3 headPosition2 = entity.GetHeadPosition();
				Vector3 direction = headPosition2 - headPosition;
				if (Physics.Raycast(headPosition, direction, direction.magnitude, Layers.StaticCollidersLayersMaskPreset))
				{
					return false;
				}
			}
			if (_enemyIsInstigator && entity.PlayerCharacterStateMachines.AssassinApproachingTarget.CurrentStateType == EStateType.AssassinApproachingTarget_Approaching)
			{
				_enemyIsInstigator = false;
			}
			Enum<SkillTypes> queuedCombatSkillType = entity.PlayerCharacterStateMachines.AssassinApproachingTarget.QueuedCombatSkillType;
			bool flag = false;
			EnterCombatMode playerEnterCombatMode;
			EnterCombatMode enterCombatMode;
			if (queuedCombatSkillType != Enum<SkillTypes>.Invalid)
			{
				playerEnterCombatMode = EnterCombatMode.Skill;
				enterCombatMode = EnterCombatMode.Normal;
				flag = CombatHelper.TargetNeedsWeaponForSkill(queuedCombatSkillType);
			}
			else if (_enemyIsInstigator)
			{
				playerEnterCombatMode = EnterCombatMode.Normal;
				enterCombatMode = EnterCombatMode.Attack;
				if (_enemy.EntityStats.Get_float(StatTypes.EStatType.Enemy_AttackDelayFactor) > 1f)
				{
					enterCombatMode = EnterCombatMode.Normal;
				}
				flag = true;
			}
			else
			{
				playerEnterCombatMode = EnterCombatMode.Attack;
				enterCombatMode = EnterCombatMode.Normal;
			}
			if (enterCombatMode == EnterCombatMode.Normal && !flag)
			{
				EDetectionRange eDetectionRange = (EDetectionRange)_enemy.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range);
				if (eDetectionRange > EDetectionRange.Normal)
				{
					enterCombatMode = EnterCombatMode.Unaware;
				}
				else if (_enemy.EnemyCharacterStateMachines.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Normal)
				{
					enterCombatMode = EnterCombatMode.Unaware;
				}
			}
			entity.Cast<IPlayerEventHandlerCarrier>().PlayerEventHandler.StopApproaching();
			CombatController.Instance.PlayerEnterCombatMode = playerEnterCombatMode;
			PlayerMain.ChangeStateForced(EStateType.CombatCharacterMain_Combat);
			CombatController.Instance.AddParticipant(_enemy, enterCombatMode);
			CombatParticipant combatParticipant = entity.Cast<IParticipantCarrier>().CombatParticipantProxy.CombatParticipant;
			CombatParticipant combatParticipant2 = _enemy.Cast<IParticipantCarrier>().CombatParticipantProxy.CombatParticipant;
			CombatParticipant currentTarget = (combatParticipant2.NextTarget = combatParticipant);
			combatParticipant2.CurrentTarget = currentTarget;
			currentTarget = (combatParticipant.NextTarget = combatParticipant2);
			combatParticipant.CurrentTarget = currentTarget;
			Entity.RegisterInteraction(EInteractionReason.Combat);
			if (queuedCombatSkillType != Enum<SkillTypes>.Invalid)
			{
				InitWithSkillBehaviour initWithSkillBehaviour = combatParticipant.ActiveBehaviour as InitWithSkillBehaviour;
				initWithSkillBehaviour.QueuedSkillType = queuedCombatSkillType;
			}
			return true;
		}

		public static void UpdateVision()
		{
			Entity.UpdateVision();
		}

		public static void SetSpeed(EMovementSpeed _speed)
		{
			Entity.SetSpeed(_speed);
		}

		public static bool CanBeSelected(Enum<SkillTypes> _type)
		{
			return Entity.Skills.CanBeSelected(_type);
		}

		public static bool Select(Enum<SkillTypes> _type)
		{
			return Entity.Skills.Select(_type);
		}

		public static bool Trigger(Enum<SkillTypes> _type, ISkillPayload _payload)
		{
			return Entity.Skills.Trigger(_type, _payload);
		}

		public static bool CanBeAborted(Enum<SkillTypes> _skillType)
		{
			return Entity.Skills.CanBeAborted(_skillType);
		}

		public static bool Abort(Enum<SkillTypes> _skillType)
		{
			return Entity.Skills.Abort(_skillType);
		}

		public static bool GetIsActive(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetIsActive(_type);
		}

		public static int GetActivityStart(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetActivityStart(_type);
		}

		public static int GetActivityEnd(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetActivityEnd(_type);
		}

		public static bool GetIsCoolingDown(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetIsCoolingDown(_type);
		}

		public static bool CanBeTriggered(Enum<SkillTypes> _type, ISkillPayload _payload, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return Entity.Skills.CanBeTriggered(_type, _payload, out _availabilityFlags);
		}

		public static int GetCoolDownStart(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetCoolDownStart(_type);
		}

		public static int GetCoolDownEnd(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetCoolDownEnd(_type);
		}

		public static ESkillState GetState(Enum<SkillTypes> _type)
		{
			return Entity.Skills.GetState(_type);
		}
	}
}
