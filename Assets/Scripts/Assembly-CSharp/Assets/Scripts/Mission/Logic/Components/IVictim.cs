using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IVictim
	{
		EVictimType VictimType { get; }

		bool AreEnemiesThreatening { get; }

		bool IsVictimAttacked { get; }

		bool IsVictimHit { get; }

		bool ThreateningOrKillRunning { get; }

		bool ThreateningAndKillFinished { get; }

		bool ThreateningFinished { get; }

		bool RegisterArrivedEnemy(EntityId _enemyId);

		void DeregisterArrivedEnemy(EntityId _enemyId);

		bool HasArrived(EntityId _enemyId);

		EnemyCharacter GetClosestEnemyInRange();

		void StartKillVictim(NpcCharacter _source);

		void EndKillVictim();
	}
}
