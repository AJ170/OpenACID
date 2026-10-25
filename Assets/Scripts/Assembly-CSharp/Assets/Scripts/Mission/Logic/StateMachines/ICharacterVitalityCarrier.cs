using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICharacterVitalityCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; }
	}
}
