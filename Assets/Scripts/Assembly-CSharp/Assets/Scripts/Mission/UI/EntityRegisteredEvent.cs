using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.UI
{
	public class EntityRegisteredEvent : LogicUiEvent
	{
		public EntityId EntityId { get; private set; }

		public bool Registered { get; private set; }

		public EntityRegisteredEvent(EntityId _entityId, bool _registered)
		{
			EntityId = _entityId;
			Registered = _registered;
		}
	}
}
