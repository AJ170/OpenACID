using System;
using Assets.Scripts.Mission.Logic;

namespace UbiCore.Messenger
{
	[Serializable]
	public class ParcourCancelEvent : ParcourUpdateEvent
	{
		public enum ECancelType
		{
			Hit = 0,
			Die = 1
		}

		public ECancelType CancelType { get; private set; }

		public EntityId TargetId { get; protected set; }

		public ParcourCancelEvent(EntityId _targetId, ECancelType _eCancelCancelType)
		{
			base.UpdateType = EUpdateType.Cancel;
			CancelType = _eCancelCancelType;
			TargetId = _targetId;
		}
	}
}
