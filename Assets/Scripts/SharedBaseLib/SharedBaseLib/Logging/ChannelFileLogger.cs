using System;
using System.Diagnostics;
using UbiCore.Logging;
using UnityEngine;

namespace SharedBaseLib.Logging
{
	public class ChannelFileLogger : ILogOutputHandler, IDisposable
	{
		public ChannelFileLogger(ChannelLogger.EChannel _channel, ChannelLogger.EVerbosity _verbosity = ChannelLogger.EVerbosity.Warning)
		{
		}

		public void Dispose()
		{
			LogOutput.Instance.RemoveHandler(this);
		}

		public void HandleLog(string _logString, string _stackTrace, LogType _type)
		{
			switch (_type)
			{
			}
		}

		[Conditional("UNITY_DEBUG")]
		private void WriteToLogFile(string _message)
		{
		}
	}
}
