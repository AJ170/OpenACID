namespace Mission
{
	public interface IObjectiveTimer
	{
		bool HasTimer { get; }

		int GetTime();
	}
}
