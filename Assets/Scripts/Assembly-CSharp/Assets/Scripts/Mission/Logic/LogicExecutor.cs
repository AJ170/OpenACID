using System.Collections.Generic;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic
{
	public class LogicExecutor
	{
		private readonly BBSortedList<float, ILogicExecutorFrameJob> m_FrameJobs = new BBSortedList<float, ILogicExecutorFrameJob>();

		private readonly BBSortedList<int, ILogicExecutorTurnJob> m_TurnJobs = new BBSortedList<int, ILogicExecutorTurnJob>();

		private readonly BBList<ILogicExecutorJob> m_PendingJobs = new BBList<ILogicExecutorJob>();

		public void AddJob(ILogicExecutorJob _job)
		{
			m_PendingJobs.Add(_job);
		}

		public void UpdateFrame()
		{
			Profiling.Start("LogicMain.LogicExecutor.UpdateFrame()", float.MaxValue);
			for (int i = 0; i < m_PendingJobs.Count; i++)
			{
				ILogicExecutorFrameJob logicExecutorFrameJob = m_PendingJobs[i] as ILogicExecutorFrameJob;
				if (logicExecutorFrameJob != null)
				{
					m_FrameJobs.Add(logicExecutorFrameJob.ExecutionGameTime, logicExecutorFrameJob);
				}
				ILogicExecutorTurnJob logicExecutorTurnJob = m_PendingJobs[i] as ILogicExecutorTurnJob;
				if (logicExecutorTurnJob != null)
				{
					m_TurnJobs.Add(logicExecutorTurnJob.ExecutionGameTurn, logicExecutorTurnJob);
				}
			}
			m_PendingJobs.ClearIfNotEmpty();
			while (m_FrameJobs.Count > 0)
			{
				IEnumerator<KeyValuePair<float, ILogicExecutorFrameJob>> enumerator = m_FrameJobs.GetEnumerator();
				enumerator.MoveNext();
				if (enumerator.Current.Value.ExecutionGameTime > LogicMain.CurrentTime)
				{
					break;
				}
				enumerator.Current.Value.Execute();
				m_FrameJobs.RemoveAt(0);
			}
			while (m_TurnJobs.Count > 0)
			{
				IEnumerator<KeyValuePair<int, ILogicExecutorTurnJob>> enumerator2 = m_TurnJobs.GetEnumerator();
				enumerator2.MoveNext();
				if (enumerator2.Current.Value.ExecutionGameTurn > LogicMain.CurrentTurn)
				{
					break;
				}
				enumerator2.Current.Value.Execute();
				m_TurnJobs.RemoveAt(0);
			}
			Profiling.Stop();
		}
	}
}
