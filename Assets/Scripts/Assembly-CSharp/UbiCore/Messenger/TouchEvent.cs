using UnityEngine;

namespace UbiCore.Messenger
{
	public class TouchEvent : TapEvent
	{
		public TouchEvent(Vector2 _position, EFingerId _fingerId, int _fingerCount)
			: base(_position, _fingerId, _fingerCount)
		{
		}
	}
}
