using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class SpawnPoint : Entity
	{
		private readonly SpawnPointAi m_spawnPointAi;

		public override EntityStats EntityStats
		{
			get
			{
				return null;
			}
		}

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.SpawnPoint;
			}
		}

		public override Definition Definition
		{
			get
			{
				return null;
			}
		}

		public SpawnPointAi SpawnPointAI
		{
			get
			{
				return m_spawnPointAi;
			}
		}

		public SpawnPoint()
		{
			m_spawnPointAi = AddComponent<SpawnPointAi>();
		}
	}
}
