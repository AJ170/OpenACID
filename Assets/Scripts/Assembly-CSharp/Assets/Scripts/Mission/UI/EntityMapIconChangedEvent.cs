using AcierProtocol;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.UI
{
	public class EntityMapIconChangedEvent : LogicUiEvent
	{
		public EMapIconType MapIconType { get; private set; }

		public IEntity Entity { get; private set; }

		public EntityMapIconChangedEvent(IEntity _entity, EMapIconType _mapIconType)
		{
			MapIconType = _mapIconType;
			Entity = _entity;
		}
	}
}
