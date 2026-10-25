using System;

namespace Assets.Scripts.Mission.Logic
{
	public class SimpleLogicExecutorFrameJob : ILogicExecutorJob, ILogicExecutorFrameJob
	{
		public float ExecutionGameTime { get; private set; }

		public Action Action { get; private set; }

		public SimpleLogicExecutorFrameJob(float _time, Action _action)
		{
			ExecutionGameTime = _time;
			Action = _action;
		}

		public void Execute()
		{
			Action();
		}
	}
}
