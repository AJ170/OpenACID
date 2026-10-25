using System;

namespace Assets.Scripts.Mission.Logic
{
	public class SimpleLogicExecutorTurnJob : ILogicExecutorJob, ILogicExecutorTurnJob
	{
		public int ExecutionGameTurn { get; private set; }

		public Action Action { get; private set; }

		public SimpleLogicExecutorTurnJob(int _turn, Action _action)
		{
			ExecutionGameTurn = _turn;
			Action = _action;
		}

		public void Execute()
		{
			Action();
		}
	}
}
