using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.CharacterPooling
{
	public class CharacterStats
	{
		public CharacterStatsSection Enemies;

		public CharacterStatsSection Crowd;

		public CharacterStatsSection Other;

		public CharacterStats()
		{
			Enemies = new CharacterStatsSection();
			Crowd = new CharacterStatsSection();
			Other = new CharacterStatsSection();
		}

		public CharacterStatsSection SelectSection(GameEntity _entity)
		{
			if (_entity.Type == EntityTypes.EType.EnemyCharacter)
			{
				return Enemies;
			}
			if (_entity.Type == EntityTypes.EType.CrowdCharacter)
			{
				return Crowd;
			}
			return Other;
		}
	}
}
