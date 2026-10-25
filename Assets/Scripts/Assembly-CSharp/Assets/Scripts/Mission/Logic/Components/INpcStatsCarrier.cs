namespace Assets.Scripts.Mission.Logic.Components
{
	public interface INpcStatsCarrier : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		NpcStats NpcStats { get; }
	}
}
