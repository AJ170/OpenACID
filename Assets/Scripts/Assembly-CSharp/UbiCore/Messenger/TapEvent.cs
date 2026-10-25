using UnityEngine;

namespace UbiCore.Messenger
{
	public class TapEvent : InputEvent
	{
		public Vector2 NormalizedPosition { get; private set; }

		public Vector2 Position { get; private set; }

		public Vector2 ScreenPosition { get; private set; }

		public EFingerId FingerId { get; private set; }

		public TapEvent(Vector2 _position, EFingerId _fingerId, int _fingerCount)
			: base(_fingerCount)
		{
			AcierCameraController instance = AcierCameraController.Instance;
			Vector2 viewportSize = instance.ViewportSize;
			ScreenPosition = _position;
			NormalizedPosition = new Vector2(_position.x / (float)Screen.width, _position.y / (float)Screen.height);
			Position = Vector2.Scale(NormalizedPosition, viewportSize);
			FingerId = _fingerId;
		}
	}
}
