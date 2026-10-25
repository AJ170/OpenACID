using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractionCharacterStateMachines : CharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterInteractionCarrier, ICharacterVitalityCarrier, IInteractionCharacterMainCarrier
	{
		public CharacterInteraction CharacterInteraction { get; private set; }

		public InteractionCharacterMain InteractionCharacterMain { get; private set; }

		public IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; private set; }

		public InteractionCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			CharacterInteraction = AddStateMachine(new CharacterInteraction((InteractionCharacter)_entity));
			InteractionCharacterMain = AddStateMachine(new InteractionCharacterMain((InteractionCharacter)_entity));
			CharacterVitality = AddStateMachine(new CharacterVitality((InteractionCharacter)_entity));
		}
	}
}
