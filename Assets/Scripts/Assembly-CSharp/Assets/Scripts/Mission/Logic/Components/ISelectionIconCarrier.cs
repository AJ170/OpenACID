using Assets.Scripts.Mission.Selection;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ISelectionIconCarrier : IDefinitionCarrier, IEntityStatsCarrier, IHeadIconCarrier, IEntity, IPositionProvider, IPositionEntity, ISelectable
	{
		SelectionIcon SelectionIcon { get; }
	}
}
