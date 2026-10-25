using UnityEngine;

namespace UbiCore.Logging
{
	public interface ILogOutputHandler
	{
		void HandleLog(string _logString, string _stackTrace, LogType _type);
	}
}
