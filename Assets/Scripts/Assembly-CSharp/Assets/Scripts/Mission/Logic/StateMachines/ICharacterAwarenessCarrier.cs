using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICharacterAwarenessCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		CharacterAwareness CharacterAwareness { get; }
	}
}
