using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class AwareInteractionCharacterStateMachines : InteractionCharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterAwarenessCarrier
	{
		public CharacterAwareness CharacterAwareness { get; private set; }

		public AwareInteractionCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			CharacterAwareness = AddStateMachine(new CharacterAwareness((AwareInteractionCharacter)_entity));
		}
	}
}
