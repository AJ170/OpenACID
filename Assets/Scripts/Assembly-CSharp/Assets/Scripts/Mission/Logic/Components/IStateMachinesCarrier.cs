namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		StateMachines StateMachines { get; }
	}
}
