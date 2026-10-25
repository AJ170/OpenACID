using UnityEngine;

namespace UbiCore.Messenger
{
	public class SelectEvent : DragEvent
	{
		public SelectEvent(InputPhase _phase, Vector2 _delta, Vector2 _position, Vector2 _start, Vector2 _stickInput, EFingerId _fingerId, int _fingerCount)
			: base(_phase, _delta, _position, _start, _stickInput, _fingerId, _fingerCount)
		{
		}
	}
}
