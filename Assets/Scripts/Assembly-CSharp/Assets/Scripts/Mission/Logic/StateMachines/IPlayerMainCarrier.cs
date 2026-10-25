using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IPlayerMainCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
	{
		PlayerMain PlayerMain { get; }

		PlayerStalkingZone PlayerStalkingZone { get; }
	}
}
