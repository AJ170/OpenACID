using UnityEngine;

namespace UbiCore.Messenger
{
	public class TraverseLinkStartedEvent : Message
	{
		public Vector3 TargetPosition { get; private set; }

		public TraverseLinkStartedEvent(Vector3 _targetPosition)
		{
			TargetPosition = _targetPosition;
		}
	}
}
