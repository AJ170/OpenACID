using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PatrolStateMachines : StateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IPatrolSeduceCarrier
	{
		public PatrolSeduce PatrolSeduce { get; private set; }

		public PatrolStateMachines(IEntity _entity)
			: base(_entity)
		{
			PatrolSeduce = AddStateMachine(new PatrolSeduce((Assets.Scripts.Mission.Logic.Entities.Patrol)_entity));
		}
	}
}
