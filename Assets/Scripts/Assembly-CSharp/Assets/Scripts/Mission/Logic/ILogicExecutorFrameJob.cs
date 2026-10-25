namespace Assets.Scripts.Mission.Logic
{
	public interface ILogicExecutorFrameJob : ILogicExecutorJob
	{
		float ExecutionGameTime { get; }
	}
}
