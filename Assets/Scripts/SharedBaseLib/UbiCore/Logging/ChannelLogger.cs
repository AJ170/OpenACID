using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace UbiCore.Logging
{
	public static class ChannelLogger
	{
		public delegate void LogEventHandler(LogEventArgs _event);

		public delegate void LogExceptionHandler(Exception _ex);

		public delegate void LogHandler(string _message);

		public enum EChannel
		{
			Default = 0,
			Cache = 1,
			UnityTrace = 2,
			AssetBundleHelper = 3,
			Rendering = 4,
			AssetBundleManager = 5,
			Mission = 6,
			AudioManager = 7,
			Combat = 8,
			StateMachineControlling = 9,
			Input = 10,
			TappedMesh = 11,
			UIMain = 12,
			Localization = 13,
			ScreenManager = 14,
			AnimCtrlGen = 15,
			RuntimeAssetBundleCreator = 16,
			Network = 17
		}

		public enum ELogLevel
		{
			Info = 0,
			Warning = 1,
			Error = 2
		}

		public enum EVerbosity
		{
			Disable = 0,
			Debug = 100,
			Verbose = 200,
			Info = 300,
			Warning = 400,
			Error = 500,
			Critical = 600
		}

		public sealed class Channel
		{
			public string Name { get; private set; }

			public EVerbosity Verbosity { get; set; }

			public Channel(string _name, EVerbosity _verbosity)
			{
				Name = _name;
				Verbosity = _verbosity;
			}
		}

		public class LogEventArgs : EventArgs
		{
			public ELogLevel Level { get; private set; }

			public EChannel Channel { get; private set; }

			public EVerbosity Verbosity { get; private set; }

			public string Message { get; private set; }

			public LogEventArgs(ELogLevel _level, EChannel _channel, EVerbosity _verbosity, string _msg)
			{
				Level = _level;
				Channel = _channel;
				Verbosity = _verbosity;
				Message = _msg;
			}
		}

		private static readonly BBDictionary<int, Channel> s_channels;

		public static LogHandler CustomLogHandler { get; set; }

		public static LogHandler CustomLogWarningHandler { get; set; }

		public static LogHandler CustomLogErrorHandler { get; set; }

		public static LogExceptionHandler CustomLogExceptionHandler { get; set; }

		public static event LogEventHandler EventHandler;

		static ChannelLogger()
		{
			s_channels = new BBDictionary<int, Channel>
			{
				{
					0,
					new Channel("default", EVerbosity.Info)
				},
				{
					1,
					new Channel("Cache", EVerbosity.Info)
				},
				{
					2,
					new Channel("UnityTrace", EVerbosity.Info)
				},
				{
					3,
					new Channel("AssetBundleHelper", EVerbosity.Info)
				},
				{
					4,
					new Channel("Rendering", EVerbosity.Info)
				},
				{
					5,
					new Channel("AssetBundleManager", EVerbosity.Info)
				},
				{
					6,
					new Channel("Mission", EVerbosity.Info)
				},
				{
					7,
					new Channel("AudioManager", EVerbosity.Info)
				},
				{
					8,
					new Channel("Combat", EVerbosity.Info)
				},
				{
					9,
					new Channel("StateMachineControlling", EVerbosity.Info)
				},
				{
					10,
					new Channel("Input", EVerbosity.Info)
				},
				{
					11,
					new Channel("TappedMesh", EVerbosity.Info)
				},
				{
					12,
					new Channel("UIMain", EVerbosity.Info)
				},
				{
					13,
					new Channel("Localization", EVerbosity.Info)
				},
				{
					14,
					new Channel("ScreenManager", EVerbosity.Info)
				},
				{
					15,
					new Channel("AnimCtrlGen", EVerbosity.Info)
				},
				{
					16,
					new Channel("RuntimeAssetBundleCreator", EVerbosity.Info)
				},
				{
					17,
					new Channel("Network", EVerbosity.Info)
				}
			};
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void EnableChannel(EChannel _channel, EVerbosity _level)
		{
			EnableChannelInternal(_channel, _level);
		}

		public static Channel[] Channels()
		{
			return s_channels.Select((KeyValuePair<int, Channel> _kvp) => _kvp.Value).ToArray();
		}

		private static void EnableChannelInternal(EChannel _channel, EVerbosity _level)
		{
			if (s_channels.ContainsKey((int)_channel))
			{
				s_channels[(int)_channel].Verbosity = _level;
			}
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(object _obj)
		{
			LogMessage(ELogLevel.Info, EChannel.Default, EVerbosity.Info, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EVerbosity _verbosity, object _obj)
		{
			LogMessage(ELogLevel.Info, EChannel.Default, _verbosity, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EVerbosity _verbosity, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Info, EChannel.Default, _verbosity, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EChannel _channel, GameObject _go, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Info, EChannel.Default, EVerbosity.Info, _go, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EChannel _channel, object _obj)
		{
			LogMessage(ELogLevel.Info, _channel, EVerbosity.Info, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EChannel _channel, EVerbosity _verbosity, object _obj)
		{
			LogMessage(ELogLevel.Info, _channel, _verbosity, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EChannel _channel, EVerbosity _verbosity, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Info, _channel, _verbosity, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Info, EChannel.Default, EVerbosity.Info, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Log(EChannel _channel, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Info, _channel, EVerbosity.Info, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogWarning(object _obj)
		{
			LogMessage(ELogLevel.Warning, EChannel.Default, EVerbosity.Warning, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogWarning(EChannel _channel, object _obj)
		{
			LogMessage(ELogLevel.Warning, _channel, EVerbosity.Warning, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogWarning(string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Warning, EChannel.Default, EVerbosity.Warning, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogWarning(EChannel _channel, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Warning, _channel, EVerbosity.Warning, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogWarning(EChannel _channel, GameObject _go, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Warning, EChannel.Default, EVerbosity.Warning, _go, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(object _obj)
		{
			LogMessage(ELogLevel.Error, EChannel.Default, EVerbosity.Error, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(EChannel _channel, object _obj)
		{
			LogMessage(ELogLevel.Error, _channel, EVerbosity.Error, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(EVerbosity _verbosity, object _obj)
		{
			LogMessage(ELogLevel.Error, EChannel.Default, _verbosity, _obj.ToString());
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Error, EChannel.Default, EVerbosity.Warning, (_args.Length != 0) ? string.Format(_msg, _args) : _msg);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogException(Exception _e)
		{
			while (_e != null)
			{
				OutputLogException(_e);
				_e = _e.InnerException;
			}
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(EChannel _channel, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Error, EChannel.Default, EVerbosity.Error, string.Format(_msg, _args));
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void LogError(EChannel _channel, GameObject _go, string _msg, params object[] _args)
		{
			LogMessage(ELogLevel.Error, EChannel.Default, EVerbosity.Error, _go, string.Format(_msg, _args));
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Assert(bool _condition)
		{
			AssertInternal(_condition, EChannel.Default, "Assert", null, null, null);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Assert(bool _condition, string _msg)
		{
			AssertInternal(_condition, EChannel.Default, _msg, null, null, null);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Assert(bool _condition, EChannel _channel, string _msg)
		{
			AssertInternal(_condition, _channel, _msg, null, null, null);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Assert(bool _condition, string _msg, params object[] _args)
		{
			AssertInternal(_condition, EChannel.Default, _msg, _args);
		}

		[Conditional("CHANNEL_LOGGER")]
		public static void Assert(bool _condition, EChannel _channel, string _msg, params object[] _args)
		{
			AssertInternal(_condition, _channel, _msg, _args);
		}

		public static void AssertInternal(bool _condition, EChannel _channel, object _obj, object[] _args)
		{
			if (!_condition)
			{
				string text = ((_obj is string) ? ((string)_obj) : _obj.ToString());
				text = text + "\n" + GenerateCallstack();
				LogMessage(ELogLevel.Error, _channel, EVerbosity.Error, string.Format(text, _args));
			}
		}

		public static void AssertInternal(bool _condition, EChannel _channel, object _obj, object _arg0, object _arg1, object _arg2)
		{
			if (!_condition)
			{
				string text = ((_obj is string) ? ((string)_obj) : _obj.ToString());
				text = text + "\n" + GenerateCallstack();
				LogMessage(ELogLevel.Error, _channel, EVerbosity.Error, string.Format(text, new object[3] { _arg0, _arg1, _arg2 }));
			}
		}

		public static string GenerateCallstack()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StackTrace stackTrace = new StackTrace(true);
			stringBuilder.Append("StackTrace:\n");
			int i;
			for (i = 0; i < stackTrace.FrameCount && !(stackTrace.GetFrame(i).GetFileName() != stackTrace.GetFrame(0).GetFileName()); i++)
			{
			}
			for (int j = i; j < stackTrace.FrameCount; j++)
			{
				StackFrame frame = stackTrace.GetFrame(j);
				stringBuilder.AppendFormat("[{0}:{1}] {2}\n", Path.GetFileName(frame.GetFileName()), frame.GetFileLineNumber(), frame.GetMethod());
			}
			return stringBuilder.ToString();
		}

		private static string FormatMessage(string _channel, string _msg, EVerbosity _verbosity)
		{
			return string.Format("[{0}]-[{1}:{3}] : {2}", DateTime.Now, _channel, _msg, _verbosity);
		}

		private static void LogMessage(ELogLevel _level, EChannel _channel, EVerbosity _verbosity, GameObject _go, string _msg)
		{
			Channel value;
			if (!s_channels.TryGetValue((int)_channel, out value))
			{
				OutputLogError(string.Concat("Could not find LogChannel \"", _channel, "\". Please register your channel first."));
				value = s_channels[0];
			}
			if (_channel == EChannel.Default || (value.Verbosity != 0 && _verbosity >= value.Verbosity))
			{
				switch (_level)
				{
				case ELogLevel.Info:
					OutputLog(FormatMessage(_channel.ToString(), _msg, _verbosity), _go);
					break;
				case ELogLevel.Warning:
					OutputLogWarning(FormatMessage(_channel.ToString(), _msg, _verbosity), _go);
					break;
				case ELogLevel.Error:
					OutputLogError(FormatMessage(_channel.ToString(), _msg, _verbosity), _go);
					break;
				default:
					throw new Exception("Bad log level " + _level);
				}
			}
			OnNewLogMessage(_level, _channel, _verbosity, _msg);
		}

		private static void LogMessage(ELogLevel _level, EChannel _channel, EVerbosity _verbosity, string _msg)
		{
			LogMessage(_level, _channel, _verbosity, null, _msg);
		}

		public static void OnNewLogMessage(ELogLevel _level, EChannel _channel, EVerbosity _verbosity, string _msg)
		{
			if (ChannelLogger.EventHandler != null)
			{
				LogEventArgs logEventArgs = new LogEventArgs(_level, _channel, _verbosity, _msg);
				Delegate[] invocationList = ChannelLogger.EventHandler.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					invocationList[i].DynamicInvoke(logEventArgs);
				}
			}
		}

		private static void OutputLog(string _str, GameObject _go)
		{
			if (CustomLogHandler != null)
			{
				UnityEngine.Debug.Log("Output Log CustomLogHandler ToString: " + CustomLogHandler.ToString());
				UnityEngine.Debug.Log("Output Log CustomLogHandler Type: " + CustomLogHandler.Target.GetType());
				CustomLogHandler(_str);
			}
			else
			{
				UnityEngine.Debug.Log("Output Log: " + _str);
				UnityEngine.Debug.Log(_str, _go);
			}
		}

		private static void OutputLog(string _str)
		{
			OutputLog(_str, null);
		}

		private static void OutputLogWarning(string _str, GameObject _go)
		{
			if (CustomLogWarningHandler != null)
			{
				CustomLogWarningHandler(_str);
				return;
			}
			UnityEngine.Debug.Log("Output Log: " + _str);
			UnityEngine.Debug.LogWarning(_str, _go);
		}

		private static void OutputLogWarning(string _str)
		{
			OutputLogWarning(_str, null);
		}

		private static void OutputLogError(string _str, GameObject _go)
		{
			if (CustomLogErrorHandler != null)
			{
				CustomLogErrorHandler(_str);
			}
			else
			{
				UnityEngine.Debug.LogError(_str, _go);
			}
		}

		private static void OutputLogError(string _str)
		{
			OutputLogError(_str, null);
		}

		private static void OutputLogException(Exception _ex)
		{
			if (CustomLogExceptionHandler != null)
			{
				CustomLogExceptionHandler(_ex);
			}
			else
			{
				UnityEngine.Debug.LogException(_ex);
			}
		}
	}
}
