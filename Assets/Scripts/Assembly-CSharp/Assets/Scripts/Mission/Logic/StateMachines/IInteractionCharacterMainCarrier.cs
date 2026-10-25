using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IInteractionCharacterMainCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		InteractionCharacterMain InteractionCharacterMain { get; }
	}
}
