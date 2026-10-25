using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class GameLogicStateMachines : StateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IGameMainCarrier
	{
		public GameMain GameMain { get; private set; }

		public GameLogicStateMachines(IEntity _entity)
			: base(_entity)
		{
			GameMain = AddStateMachine(new GameMain((GameLogic)_entity));
		}
	}
}
