using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface ISuperBuffsStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		ISuperBuffStats SuperBuffStats { get; }
	}
}
