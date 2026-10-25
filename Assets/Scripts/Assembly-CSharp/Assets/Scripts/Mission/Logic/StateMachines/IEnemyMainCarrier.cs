using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IEnemyMainCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		EnemyMain EnemyMain { get; }
	}
}
