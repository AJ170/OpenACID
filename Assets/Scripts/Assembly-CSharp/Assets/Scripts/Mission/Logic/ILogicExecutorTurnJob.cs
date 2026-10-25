namespace Assets.Scripts.Mission.Logic
{
	public interface ILogicExecutorTurnJob : ILogicExecutorJob
	{
		int ExecutionGameTurn { get; }
	}
}
