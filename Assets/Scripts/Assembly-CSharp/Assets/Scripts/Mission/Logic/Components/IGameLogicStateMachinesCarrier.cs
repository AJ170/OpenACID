namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IGameLogicStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		GameLogicStateMachines GameLogicStateMachines { get; }
	}
}
