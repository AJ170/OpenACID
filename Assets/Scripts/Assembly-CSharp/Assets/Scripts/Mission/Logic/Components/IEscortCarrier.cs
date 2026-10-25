namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEscortCarrier : IDefinitionCarrier, IEntityStatsCarrier, IVictimCarrier, IEntity
	{
		Escort EscortComponent { get; }
	}
}
