using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class ActAnimation : ISkillAnimation, IActAnimation
	{
		public AttackMeleeType AttackMelee { get; set; }

		public ActionTypeUpperBody ActionUpperBody { get; set; }

		public AnimatorStateMachineStatic.StateMachineHashValue WaitForHash { get; set; }

		public override string ToString()
		{
			return "ActAnimation" + AttackMelee;
		}
	}
}
