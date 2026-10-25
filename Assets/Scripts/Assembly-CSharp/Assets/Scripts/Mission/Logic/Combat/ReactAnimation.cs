using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class ReactAnimation : ISkillAnimation, IReactAnimation
	{
		public ReactionType Reaction { get; set; }

		public AttackMeleeType AttackMelee { get; set; }

		public ReactionType ReactionDie { get; set; }

		public AttackMeleeType AttackMeleeDie { get; set; }

		public AnimatorEventType ReactionEvent { get; set; }

		public ActionTypeUpperBody ActionUpperBody { get; set; }

		public AnimatorStateMachineStatic.StateMachineHashValue WaitForHash { get; set; }

		public override string ToString()
		{
			return "ReactAnimation" + Reaction;
		}
	}
}
