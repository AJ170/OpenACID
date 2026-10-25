using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Selection
{
	public interface ISelectable : IDefinitionCarrier, IEntityStatsCarrier, IEntity, IPositionProvider, IPositionEntity
	{
	}
}
