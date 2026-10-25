namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IHighlightCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		Highlight Highlight { get; }

		bool HighlightedSelection { get; }

		bool HighlightUsesRimColor { get; }
	}
}
