using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Selection;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class InteractableMissionObject : InteractableObject, IDefinitionCarrier, IEntityStatsCarrier, IEntity, IPositionProvider, IPositionEntity, ISelectable
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.InteractibleMissionObject;
			}
		}
	}
}
