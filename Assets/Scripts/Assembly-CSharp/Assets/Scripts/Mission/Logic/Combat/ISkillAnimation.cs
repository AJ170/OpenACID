using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public interface ISkillAnimation
	{
		AttackMeleeType AttackMelee { get; }

		ActionTypeUpperBody ActionUpperBody { get; }

		AnimatorStateMachineStatic.StateMachineHashValue WaitForHash { get; }
	}
}
