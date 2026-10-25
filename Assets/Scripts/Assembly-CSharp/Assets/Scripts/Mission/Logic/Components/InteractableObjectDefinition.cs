using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractableObjectDefinition : Definition
	{
		public InteractableObjectData.IInteractableObjectDefinition Definition
		{
			get
			{
				return (InteractableObjectData.IInteractableObjectDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		public InteractableObjectDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
