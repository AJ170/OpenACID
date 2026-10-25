namespace UbiCore.Messenger
{
	public class PinchEvent : InputEvent
	{
		public InputPhase Phase { get; private set; }

		public float Delta { get; private set; }

		public float Distance { get; private set; }

		public PinchEvent(InputPhase _phase, float _delta, float _distance, int _fingerCount)
			: base(_fingerCount)
		{
			Phase = _phase;
			Delta = _delta;
			Distance = _distance;
		}
	}
}
