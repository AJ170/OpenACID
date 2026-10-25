using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractableObjectStateMachines : StateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IObjectInteractionCarrier
	{
		public ObjectInteraction ObjectInteraction { get; private set; }

		public InteractableObjectStateMachines(IEntity _entity)
			: base(_entity)
		{
			ObjectInteraction = AddStateMachine(new ObjectInteraction((InteractableObject)_entity));
		}
	}
}
