using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Animation
{
	public static class ParcourAnimationExtensions
	{
		public static bool GetHandlesParkourInputEvents(this IParkourAnimation _animation)
		{
			EntityTypes.EType type = _animation.AnimationController.EntityId.Type;
			return type == EntityTypes.EType.PlayerCharacter;
		}

		public static bool GetHandlesParkourCancelEvents(this IParkourAnimation _animation)
		{
			EntityTypes.EType type = _animation.AnimationController.EntityId.Type;
			return type == EntityTypes.EType.PlayerCharacter || type == EntityTypes.EType.EnemyCharacter;
		}
	}
}
