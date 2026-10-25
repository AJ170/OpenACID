using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.UI
{
	public class InteractingWithEntityEvent : LogicUiEvent
	{
		public EntityId EntityId { get; private set; }

		public InteractingWithEntityEvent(EntityId _entityId)
		{
			EntityId = _entityId;
		}
	}
}
