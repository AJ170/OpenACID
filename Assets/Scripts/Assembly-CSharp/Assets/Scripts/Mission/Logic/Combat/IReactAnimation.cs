using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public interface IReactAnimation : ISkillAnimation
	{
		ReactionType Reaction { get; }

		ReactionType ReactionDie { get; }

		AttackMeleeType AttackMeleeDie { get; }

		AnimatorEventType ReactionEvent { get; }
	}
}
