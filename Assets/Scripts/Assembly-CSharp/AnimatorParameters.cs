using UbiCore.Utils.Extensions;
using UnityEngine;

public static class AnimatorParameters
{
	public static readonly int ActionType = Animator.StringToHash("ActionType");

	public static readonly int ActionTypeUpperBody = Animator.StringToHash("UpperbodyActionType");

	public static readonly int Balance_walkHash = Animator.StringToHash("Balance_walk");

	public static readonly int BankLeftRightHash = Animator.StringToHash("BankLeftRight");

	public static readonly int BenchApproachAngleHash = Animator.StringToHash("BenchApproachAngle");

	public static readonly int ClimbingStanceHash = Animator.StringToHash("ClimbingStance");

	public static readonly int NormalizedOverhangHash = Animator.StringToHash("NormalizedOverhang");

	public static readonly int ClimbDirectionHash = Animator.StringToHash("ClimbDirection");

	public static readonly int ClimbRightHandHash = Animator.StringToHash("ClimbRightHand");

	public static readonly int CurrentLinkLengthHash = Animator.StringToHash("CurrentLinkLength");

	public static readonly int CurrentLinkTypeHash = Animator.StringToHash("CurrentLinkType");

	public static readonly int DistanceToTarget = Animator.StringToHash("DistanceToTarget");

	public static readonly int EdgeDistanceHash = Animator.StringToHash("EdgeDistance");

	public static readonly int EdgeDistanceZHash = Animator.StringToHash("EdgeDistanceZ");

	public static readonly int EdgeDistanceXHash = Animator.StringToHash("EdgeDistanceX");

	public static readonly int GeneralState = Animator.StringToHash("GeneralState");

	public static readonly int MovementState = Animator.StringToHash("MovementState");

	public static readonly int IdleType = Animator.StringToHash("IdleType");

	public static readonly int RandomHash = Animator.StringToHash("Random");

	public static readonly int RoofEdgeHash = Animator.StringToHash("RoofEdge");

	public static readonly int RoofEdgeJumpDownHash = Animator.StringToHash("RoofEdgeJumpDown");

	public static readonly int ClimbHandPositionHash = Animator.StringToHash("ClimbHandPosition");

	public static readonly int CancelClimbHash = Animator.StringToHash("CancelClimb");

	public static readonly int SpeedHash = Animator.StringToHash("Speed");

	public static readonly int TargetDirectionHash = Animator.StringToHash("TargetDirection");

	public static readonly int TargetDirectionXHash = Animator.StringToHash("TargetDirectionX");

	public static readonly int TargetDirectionYHash = Animator.StringToHash("TargetDirectionY");

	public static readonly int TurnDirectionHash = Animator.StringToHash(typeof(TurnDirection).Name);

	public static readonly int StopAction = Animator.StringToHash("StopAction");

	public static readonly int NoConditionActive = Animator.StringToHash("NoConditionActive");

	public static readonly int FootRfront = Animator.StringToHash("Foot_R_front");

	public static readonly int IKFootL = Animator.StringToHash("IK_footl");

	public static readonly int IKFootR = Animator.StringToHash("IK_footr");

	public static readonly int MatchTargetStart = Animator.StringToHash("MatchStart");

	public static readonly int MatchTargetEnd = Animator.StringToHash("MatchEnd");

	public static readonly int JumpDistanceY = Animator.StringToHash("JumpDistanceY");

	public static readonly int JumpDistanceZ = Animator.StringToHash("JumpDistanceZ");

	public static readonly int NpcId = Animator.StringToHash("NPC_ID");

	public static readonly int RooftopGardenClothJumpIn = Animator.StringToHash("JumpIn");

	public static readonly int RooftopGardenClothJumpOut = Animator.StringToHash("JumpOut");

	public static readonly int RooftopGardenClothAssassinate = Animator.StringToHash("Assassinate");

	public static readonly int CrouchWalk = Animator.StringToHash("Crouch_walk");

	public static readonly int HaystackJumpIn = Animator.StringToHash("jump_in");

	public static readonly int LoadCrossbow = Animator.StringToHash("LoadCrossbow");

	public static readonly int ShootCrossbow = Animator.StringToHash("ShootCrossbow");

	public static readonly int CombatTurnDirection = Animator.StringToHash("CombatTurnDirection");

	public static readonly int StrafeDirectionX = Animator.StringToHash("StrafeDirectionX");

	public static readonly int StrafeDirectionZ = Animator.StringToHash("StrafeDirectionZ");

	public static readonly int ReactionType = Animator.StringToHash("ReactionType");

	public static readonly int ReactionDirectionX = Animator.StringToHash("ReactionDirectionX");

	public static readonly int ReactionDirectionZ = Animator.StringToHash("ReactionDirectionZ");

	public static readonly int AttackMeleeType = Animator.StringToHash("AttackMeleeType");

	public static readonly int ComboAttack = Animator.StringToHash("ComboAttack");

	public static readonly int CombatStance = Animator.StringToHash("CombatStance");

	public static readonly int AttackRangeType = Animator.StringToHash("AttackRangeType");
}
