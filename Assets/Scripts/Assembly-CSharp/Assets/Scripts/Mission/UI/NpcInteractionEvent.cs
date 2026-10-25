using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.UI
{
	public class NpcInteractionEvent : LogicUiEvent
	{
		public enum ESubType
		{
			Invalid = 0,
			OfferOpen = 1,
			OfferAccepted = 2,
			OfferRejected = 3
		}

		public ESubType SubType { get; private set; }

		public EntityId NpcId { get; private set; }

		public NpcInteractionEvent(ESubType _subType, EntityId _npcId)
		{
			SubType = _subType;
			NpcId = _npcId;
		}
	}
}
