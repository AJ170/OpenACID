using UnityEngine;

namespace UbiCore.Messenger
{
	public class DragEvent : InputEvent
	{
		public InputPhase Phase { get; private set; }

		public Vector2 Delta { get; private set; }

		public Vector2 Distance
		{
			get
			{
				return Position - StartPosition;
			}
		}

		public Vector2 Position { get; private set; }

		public Vector2 StartPosition { get; private set; }

		public Vector2 StickInput { get; private set; }

		public EFingerId FingerId { get; set; }

		public DragEvent(InputPhase _phase, Vector2 _delta, Vector2 _position, Vector2 _start, Vector2 _stickInput, EFingerId _fingerId, int _fingerCount)
			: base(_fingerCount)
		{
			Phase = _phase;
			Delta = _delta;
			Position = _position;
			StartPosition = _start;
			FingerId = _fingerId;
			StickInput = _stickInput;
		}
	}
}
