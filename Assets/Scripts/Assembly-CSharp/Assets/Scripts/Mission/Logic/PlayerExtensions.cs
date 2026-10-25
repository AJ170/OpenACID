using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public static class PlayerExtensions
	{
		public static bool PrepareCombat(this PlayerCharacter _entity, bool _simulate = false)
		{
			if (_entity.PlayerCharacterStateMachines.CharacterVitality.CurrentStateType != EStateType.Character_Alive)
			{
				return false;
			}
			if (_entity.AssassinInteraction.AbortInteractions(_simulate) && _simulate)
			{
				return true;
			}
			return _entity.PlayerCharacterStateMachines.PlayerMain.ScheduledStateType == EStateType.Invalid && (_entity.GetIsRoaming() || _entity.GetIsBeingChased() || _entity.GetIsInCombat());
		}

		public static void UpdateVision(this PlayerCharacter _entity)
		{
			_entity.Vision.Activate();
		}

		public static bool CheckBeingChased(this PlayerCharacter _entity)
		{
			PlayerStats playerStats = _entity.PlayerStats;
			return playerStats.Get_int(StatTypes.EStatType.Player_ChasingEnemies) > 0 && !playerStats.Get_bool(StatTypes.EStatType.Player_Disguise);
		}

		public static bool GetIsApproachingTappedPosition(this PlayerCharacter _entity, bool _recompute = false)
		{
			return _entity.GetIsApproachingPosition(_recompute) && _entity.PlayerEventHandler.ApproachPositionWasTapped;
		}

		public static bool GetIsApproachingPosition(this PlayerCharacter _entity, bool _recompute = false)
		{
			return (!_recompute) ? _entity.PlayerEventHandler.IsApproachingPosition : _entity.PlayerEventHandler.CheckIsApproachingPosition();
		}

		public static Vector3 GetTargetPosition(this PlayerCharacter _entity)
		{
			return _entity.PlayerEventHandler.TargetPosition;
		}

		public static bool GetIsAttackable(this PlayerCharacter _entity)
		{
			return _entity.PrepareCombat(true);
		}

		public static bool GetIsExposed(this PlayerCharacter _entity)
		{
			return _entity.PlayerCharacterStateMachines.PlayerAlertLevel.CurrentStateType == EStateType.CharacterAlertLevel_Exposed;
		}

		public static bool GetIsAnonymous(this PlayerCharacter _entity)
		{
			return _entity.PlayerCharacterStateMachines.PlayerAlertLevel.CurrentStateType == EStateType.CharacterAlertLevel_Anonymous;
		}

		public static bool GetIsUsingEagleEye(this PlayerCharacter _entity)
		{
			return _entity.PlayerCharacterStateMachines.PlayerVision.CurrentStateType == EStateType.PlayerVision_EagleEye;
		}

		public static void Die(this PlayerCharacter _entity)
		{
			_entity.KillCharacter(EKillType.Scripted);
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetHit(Vector3.zero, AttackMeleeType.DownLeft, true);
		}

		public static bool Revive(this PlayerCharacter _entity)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(_entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Dead))
			{
				return false;
			}
			AudioManager.Instance.EventManager.OnPlayerRevive();
			_entity.RefillHealth();
			_entity.PlayerCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			_entity.PlayerCharacterStateMachines.PlayerAlertLevel.ForceNotoriety(0f);
			if (!_entity.GetIsRoaming())
			{
				_entity.EnterRoaming();
			}
			_entity.Skills.Respawn();
			_entity.AssassinCharacterStateMachines.CharacterCondition.ExternalChange();
			_entity.Buffs.AddBuff(new BooleanOrBuff(StatTypes.EStatType.Player_WasJustRevived, EBuffDurationType.Finite, 100, 0, true));
			_entity.Buffs.AddBuff(new BooleanOrBuff(StatTypes.EStatType.Player_Invulnerable, EBuffDurationType.Finite, 50, 0, true));
			_entity.EntityStats.Set_bool(StatTypes.EStatType.Player_ProtectedFromDeath, true);
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.Revive();
			_entity.Effects.OnEffectTrigger("RevivePlayer");
			return true;
		}

		public static void FinalizeRevive(this PlayerCharacter _entity)
		{
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdleWithCrossfade(0.01f);
			_entity.UnityComponentCache.AnimationController.ApplyRootMotion = false;
			_entity.UnityComponentCache.Animator.SetCurrentGeneralState(GeneralState.Default);
			_entity.UnityComponentCache.Animator.SetCurrentReactionType(ReactionType.Invalid);
			_entity.UnityComponentCache.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			_entity.UnityComponentCache.Animator.SetCurrentLinkType(eLinkType.Invalid);
			_entity.UnityComponentCache.Animator.SetCurrentLinkLength(0f);
			_entity.UnityComponentCache.Animator.SetCurrentBankLeftRight(0f);
			_entity.UnityComponentCache.Animator.SetCurrentMovementState(MovementState.Idle);
			_entity.UnityComponentCache.Animator.SetCurrentComboAttack(Flag.False);
			_entity.UnityComponentCache.Animator.SetCurrentStopAction(true);
			_entity.UnityComponentCache.Animator.speed = 1f;
			_entity.PlayerCharacterStateMachines.CharacterDrawMeleeWeapon.ReattachDroppedWeapon();
		}
	}
}
