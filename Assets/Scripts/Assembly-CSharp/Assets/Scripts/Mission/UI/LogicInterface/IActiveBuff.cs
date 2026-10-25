namespace Assets.Scripts.Mission.UI.LogicInterface
{
	public interface IActiveBuff
	{
		int Id { get; }

		int StartTime { get; }

		int EndTime { get; }

		int Duration { get; }

		EBuffType Type { get; }
	}
}
