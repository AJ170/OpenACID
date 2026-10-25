namespace UbiCore.Messenger
{
	public class TwistEvent : InputEvent
	{
		public float DeltaRotation { get; private set; }

		public float TotalRotation { get; private set; }

		public InputPhase Phase { get; private set; }

		public TwistEvent(float _deltaRotation, float _totalRotation, InputPhase _phase, int _fingerCount)
			: base(_fingerCount)
		{
			DeltaRotation = _deltaRotation;
			TotalRotation = _totalRotation;
			Phase = _phase;
		}
	}
}
