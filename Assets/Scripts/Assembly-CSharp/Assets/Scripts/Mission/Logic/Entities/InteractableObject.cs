using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Selection;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class InteractableObject : Object, IDefinitionCarrier, IInteractableObjectDefinitionCarrier, IInteractableObjectStateMachinesCarrier, IEntityStatsCarrier, IHeadIconCarrier, IInteractableObjectStatsCarrier, IInteractionLocatorsCarrier, IStateMachinesCarrier, IEntity, IPositionProvider, IPositionEntity, ISelectable
	{
		public override Definition Definition
		{
			get
			{
				return InteractableObjectDefinition;
			}
		}

		public override EntityStats EntityStats
		{
			get
			{
				return InteractableObjectStats;
			}
		}

		public HeadIcon HeadIcon { get; private set; }

		public InteractableObjectDefinition InteractableObjectDefinition { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.StateMachines StateMachines
		{
			get
			{
				return InteractableObjectStateMachines;
			}
		}

		public InteractableObjectStateMachines InteractableObjectStateMachines { get; private set; }

		public InteractableObjectStats InteractableObjectStats { get; private set; }

		public InteractionLocators InteractionLocators { get; private set; }

		protected InteractableObject()
		{
			InteractableObjectDefinition = AddComponent<InteractableObjectDefinition>();
			InteractableObjectStats = AddComponent<InteractableObjectStats>();
			InteractionLocators = AddComponent<InteractionLocators>();
			InteractableObjectStateMachines = AddComponent<InteractableObjectStateMachines>();
			HeadIcon = AddComponent<HeadIcon>();
		}

		public virtual bool CanInteract()
		{
			return InteractableObjectStateMachines.ObjectInteraction.CurrentStateType == EStateType.Object_Enabled;
		}
	}
}
