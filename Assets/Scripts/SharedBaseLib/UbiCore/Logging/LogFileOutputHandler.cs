using UnityEngine;

namespace UbiCore.Logging
{
	public class LogFileOutputHandler : ILogOutputHandler
	{
		public static bool Active
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void HandleLog(string _logString, string _stackTrace, LogType _type)
		{
		}
	}
}
