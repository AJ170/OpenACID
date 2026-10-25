using UnityEngine;

namespace UbiCore.Messenger
{
	public class TraverseLinkFinishedEvent : Message
	{
		public Vector3 TargetPosition { get; private set; }

		public TraverseLinkFinishedEvent(Vector3 _targetPosition)
		{
			TargetPosition = _targetPosition;
		}
	}
}
