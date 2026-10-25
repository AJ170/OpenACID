namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ISpacialEntityCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		SpacialEntity SpacialEntity { get; }
	}
}
