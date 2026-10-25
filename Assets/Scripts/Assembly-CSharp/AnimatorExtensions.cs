using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Combat;
using UbiCore.Utils.Extensions;
using UnityEngine;

public static class AnimatorExtensions
{
	public static ActionType GetCurrentActionType(this Animator _animator)
	{
		return (ActionType)_animator.GetInteger(AnimatorParameters.ActionType);
	}

	public static ReactionType GetCurrentReactionType(this Animator _animator)
	{
		return (ReactionType)_animator.GetInteger(AnimatorParameters.ReactionType);
	}

	public static float GetCurrentBenchApproachAngle(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.BenchApproachAngleHash);
	}

	public static void SetCurrentBenchApproachAngle(this Animator _animator, float _angle)
	{
		_animator.SetFloat(AnimatorParameters.BenchApproachAngleHash, _angle);
	}

	public static void SetCurrentActionTypeUpperBody(this Animator _animator, ActionTypeUpperBody _actionType)
	{
		_animator.SetInteger(AnimatorParameters.ActionTypeUpperBody, (int)_actionType);
	}

	public static void SetCurrentNoConditionActive(this Animator _animator, bool _noConditionActive)
	{
		_animator.SetBool(AnimatorParameters.NoConditionActive, _noConditionActive);
	}

	public static void SetCurrentReactionDirectionX(this Animator _animator, float _dir)
	{
		_animator.SetFloat(AnimatorParameters.ReactionDirectionX, _dir);
	}

	public static void SetCurrentReactionDirectionZ(this Animator _animator, float _dir)
	{
		_animator.SetFloat(AnimatorParameters.ReactionDirectionZ, _dir);
	}

	public static void SetCurrentJumpDistanceY(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.JumpDistanceY, _distance);
	}

	public static void SetCurrentJumpDistanceZ(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.JumpDistanceZ, _distance);
	}

	public static AnimationController.Direction GetTargetDirection(this Animator _animator)
	{
		return (AnimationController.Direction)_animator.GetInteger(AnimatorParameters.TargetDirectionHash);
	}

	public static void SetTargetDirection(this Animator _animator, AnimationController.Direction _targetDirection)
	{
		_animator.SetInteger(AnimatorParameters.TargetDirectionHash, (int)_targetDirection);
	}

	public static float GetTargetDirectionX(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.TargetDirectionXHash);
	}

	public static void SetTargetDirectionX(this Animator _animator, float _angle)
	{
		_animator.SetFloat(AnimatorParameters.TargetDirectionXHash, _angle);
	}

	public static float GetTargetDirectionY(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.TargetDirectionYHash);
	}

	public static void SetTargetDirectionY(this Animator _animator, float _angle)
	{
		_animator.SetFloat(AnimatorParameters.TargetDirectionYHash, _angle);
	}

	public static bool GetClimbRightHand(this Animator _animator)
	{
		return _animator.GetBool(AnimatorParameters.ClimbRightHandHash);
	}

	public static void SetClimbRightHand(this Animator _animator, bool _climbRightHand)
	{
		_animator.SetBool(AnimatorParameters.ClimbRightHandHash, _climbRightHand);
	}

	public static int GetClimbHandPosition(this Animator _animator)
	{
		return _animator.GetInteger(AnimatorParameters.ClimbHandPositionHash);
	}

	public static void SetClimbHandPosition(this Animator _animator, int _climbHandPosition)
	{
		_animator.SetInteger(AnimatorParameters.ClimbHandPositionHash, _climbHandPosition);
	}

	public static bool GetCurrentNoConditionActive(this Animator _animator)
	{
		return _animator.GetBool(AnimatorParameters.NoConditionActive);
	}

	public static AttackMeleeType GetCurrentAttackMeleeType(this Animator _animator)
	{
		return (AttackMeleeType)_animator.GetInteger(AnimatorParameters.AttackMeleeType);
	}

	public static void SetCurrentAttackMeleeType(this Animator _animator, AttackMeleeType _type)
	{
		_animator.SetInteger(AnimatorParameters.AttackMeleeType, (int)_type);
	}

	public static void SetCurrentComboAttack(this Animator _animator, Flag _flag)
	{
		_animator.SetInteger(AnimatorParameters.ComboAttack, (int)_flag);
	}

	public static void SetCurrentCombatStance(this Animator _animator, float _value)
	{
		_animator.SetFloat(AnimatorParameters.CombatStance, _value);
	}

	public static AttackRangeType GetCurrentAttackRangeType(this Animator _animator)
	{
		return (AttackRangeType)_animator.GetInteger(AnimatorParameters.AttackRangeType);
	}

	public static void SetCurrentAttackRangeType(this Animator _animator, AttackRangeType _type)
	{
		_animator.SetInteger(AnimatorParameters.AttackRangeType, (int)_type);
	}

	public static MovementState GetCurrentMovementState(this Animator _animator)
	{
		return (MovementState)_animator.GetInteger(AnimatorParameters.MovementState);
	}

	public static void SetCurrentMovementState(this Animator _animator, MovementState _state)
	{
		_animator.SetInteger(AnimatorParameters.MovementState, (int)_state);
	}

	public static void SetCurrentStrafeDirectionX(this Animator _animator, float _value)
	{
		_animator.SetFloat(AnimatorParameters.StrafeDirectionX, _value);
	}

	public static void SetCurrentStrafeDirectionZ(this Animator _animator, float _value)
	{
		_animator.SetFloat(AnimatorParameters.StrafeDirectionZ, _value);
	}

	public static IdleType GetCurrentIdleType(this Animator _animator)
	{
		return (IdleType)_animator.GetInteger(AnimatorParameters.IdleType);
	}

	public static void SetCurrentIdleType(this Animator _animator, IdleType _type)
	{
		_animator.SetInteger(AnimatorParameters.IdleType, (int)_type);
	}

	public static void SetCurrentLinkType(this Animator _animator, eLinkType _type)
	{
		_animator.SetInteger(AnimatorParameters.CurrentLinkTypeHash, (int)_type);
	}

	public static GeneralState GetCurrentGeneralState(this Animator _animator)
	{
		return (GeneralState)_animator.GetInteger(AnimatorParameters.GeneralState);
	}

	public static void SetCurrentGeneralState(this Animator _animator, GeneralState _state)
	{
		_animator.SetInteger(AnimatorParameters.GeneralState, (int)_state);
	}

	public static void SetCurrentReactionType(this Animator _animator, ReactionType _reactionType)
	{
		_animator.SetInteger(AnimatorParameters.ReactionType, (int)_reactionType);
	}

	public static void SetCurrentActionType(this Animator _animator, ActionType _actionType)
	{
		_animator.SetInteger(AnimatorParameters.ActionType, (int)_actionType);
	}

	public static ActionTypeUpperBody GetCurrentActionTypeUpperBody(this Animator _animator)
	{
		return (ActionTypeUpperBody)_animator.GetInteger(AnimatorParameters.ActionTypeUpperBody);
	}

	public static void SetCurrentClimbingStance(this Animator _animator, int _value)
	{
		_animator.SetInteger(AnimatorParameters.ClimbingStanceHash, _value);
	}

	public static int GetCurrentClimbingStance(this Animator _animator)
	{
		return _animator.GetInteger(AnimatorParameters.ClimbingStanceHash);
	}

	public static void SetCurrentNormalizedOverhang(this Animator _animator, float _value)
	{
		_animator.SetFloat(AnimatorParameters.NormalizedOverhangHash, _value);
	}

	public static float GetCurrentNormalizedOverhang(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.NormalizedOverhangHash);
	}

	public static void SetCurrentClimbDirection(this Animator _animator, eDirection _direction)
	{
		_animator.SetInteger(AnimatorParameters.ClimbDirectionHash, (int)_direction);
	}

	public static void SetCurrentTurnDirection(this Animator _animator, TurnDirection _direction)
	{
		_animator.SetInteger(AnimatorParameters.TurnDirectionHash, (int)_direction);
	}

	public static NpcTurnDirection GetCurrentNpcTurnDirection(this Animator _animator)
	{
		return (NpcTurnDirection)Mathf.RoundToInt(_animator.GetFloat(AnimatorParameters.CombatTurnDirection));
	}

	public static void SetCurrentNpcTurnDirection(this Animator _animator, NpcTurnDirection _direction)
	{
		_animator.SetFloat(AnimatorParameters.CombatTurnDirection, (float)_direction);
	}

	public static void SetCurrentEdgeDistance(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.EdgeDistanceHash, _distance);
	}

	public static void SetCurrentEdgeDistanceZ(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.EdgeDistanceZHash, _distance);
	}

	public static float GetCurrentEdgeDistanceX(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.EdgeDistanceXHash);
	}

	public static void SetCurrentEdgeDistanceX(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.EdgeDistanceXHash, _distance);
	}

	public static bool GetCurrentRoofEdge(this Animator _animator)
	{
		return _animator.GetBool(AnimatorParameters.RoofEdgeHash);
	}

	public static void SetCurrentRoofEdge(this Animator _animator, bool _edge)
	{
		_animator.SetBool(AnimatorParameters.RoofEdgeHash, _edge);
	}

	public static float GetCurrentSpeed(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.SpeedHash);
	}

	public static void SetCurrentSpeed(this Animator _animator, float _speed)
	{
		_animator.SetFloat(AnimatorParameters.SpeedHash, _speed);
	}

	public static void SetCurrentStopAction(this Animator _animator, bool _stopped)
	{
		_animator.SetBool(AnimatorParameters.StopAction, _stopped);
	}

	public static float GetCurrentBankLeftRight(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.BankLeftRightHash);
	}

	public static void SetCurrentBankLeftRight(this Animator _animator, float _bank)
	{
		_animator.SetFloat(AnimatorParameters.BankLeftRightHash, _bank);
	}

	public static float GetCurrentLinkLength(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.CurrentLinkLengthHash);
	}

	public static void SetCurrentLinkLength(this Animator _animator, float _length)
	{
		_animator.SetFloat(AnimatorParameters.CurrentLinkLengthHash, _length);
	}

	public static void SetDistanceToTarget(this Animator _animator, float _distance)
	{
		_animator.SetFloat(AnimatorParameters.DistanceToTarget, _distance);
	}

	public static void SetCurrentBalanceWalk(this Animator _animator, float _balance)
	{
		_animator.SetFloat(AnimatorParameters.Balance_walkHash, _balance);
	}

	public static void SetCurrentNpcId(this Animator _animator, int _npcId)
	{
		_animator.SetInteger(AnimatorParameters.NpcId, _npcId);
	}

	public static int GetCurrentNpcId(this Animator _animator)
	{
		return _animator.GetInteger(AnimatorParameters.NpcId);
	}

	public static void SetCurrentCrouchWalk(this Animator _animator, float _crouch)
	{
		_animator.SetFloat(AnimatorParameters.CrouchWalk, _crouch);
	}

	public static float GetCurrentCrouchWalk(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.CrouchWalk);
	}

	public static bool IsCurrentRightFootFront(this Animator _animator)
	{
		return _animator.GetFloat(AnimatorParameters.FootRfront) > 0.5f;
	}

	public static void SetRooftopGardenClothJumpIn(this Animator _animator)
	{
		_animator.SetBool(AnimatorParameters.RooftopGardenClothJumpIn, true);
	}

	public static void SetRooftopGardenClothJumpOut(this Animator _animator)
	{
		_animator.SetBool(AnimatorParameters.RooftopGardenClothJumpOut, true);
	}

	public static void SetRooftopGardenClothAssassinate(this Animator _animator)
	{
		_animator.SetBool(AnimatorParameters.RooftopGardenClothAssassinate, true);
	}

	public static void SetHaystackJumpIn(this Animator _animator)
	{
		_animator.SetBool(AnimatorParameters.HaystackJumpIn, true);
	}

	public static void SetLoadCrossbow(this Animator _animator, bool _load)
	{
		_animator.SetBool(AnimatorParameters.LoadCrossbow, _load);
	}

	public static void SetShootCrossbow(this Animator _animator, bool _shoot)
	{
		_animator.SetBool(AnimatorParameters.ShootCrossbow, _shoot);
	}
}
