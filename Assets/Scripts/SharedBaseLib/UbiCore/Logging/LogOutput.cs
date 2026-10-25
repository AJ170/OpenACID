using UnityEngine;

namespace UbiCore.Logging
{
	public class LogOutput
	{
		private static LogOutput m_instance;

		public static LogOutput Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new LogOutput();
				}
				return m_instance;
			}
		}

		private LogOutput()
		{
		}

		public void Init()
		{
			Application.logMessageReceived += HandleLog;
		}

		public void Dispose()
		{
		}

		public void AddHandler(ILogOutputHandler _handler, bool _enable = true)
		{
		}

		public void RemoveHandler(ILogOutputHandler _handler)
		{
		}

		public void EnableHandler(ILogOutputHandler _handler, bool _enable)
		{
		}

		private void HandleLog(string _logString, string _stackTrace, LogType _type)
		{
		}
	}
}
