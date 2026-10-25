namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICrowdCharacterStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		CrowdCharacterStats CrowdCharacterStats { get; }
	}
}
