namespace UbiCore.Messenger
{
	public abstract class InputEvent : Message
	{
		public enum InputPhase
		{
			None = 0,
			Started = 1,
			Updated = 2,
			Ended = 3
		}

		public int FingerCount { get; private set; }

		public InputEvent(int _fingerCount)
		{
			FingerCount = _fingerCount;
		}
	}
}
