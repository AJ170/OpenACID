using System;
using System.Collections.Generic;
using System.Text;
using SharedBaseLib.Core;
using UbiCore.Logging;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug
{
	public class Profiling
	{
		private static readonly StringBuilder s_dumpBuilder = new StringBuilder();

		private static int m_outStandingSampled = 0;

		public static bool isEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private static int FrameCount
		{
			get
			{
				if (!Singleton<Timer>.IsAvailable)
				{
					return 0;
				}
				return Singleton<Timer>.Instance.frameCount;
			}
		}

		public double selfms
		{
			get
			{
				return 0.0;
			}
		}

		public string name
		{
			get
			{
				return "profiler disabled";
			}
		}

		private static bool BeginSample(string _name)
		{
			if (Profiler.enabled)
			{
				m_outStandingSampled++;
				return true;
			}
			return false;
		}

		private static void EndSampleInt()
		{
		}

		private static bool EndSample()
		{
			if (Profiler.enabled)
			{
				EndSampleInt();
				m_outStandingSampled--;
				return true;
			}
			return false;
		}

		public static void Start(string _name, float _threshold = float.MaxValue, params object[] _args)
		{
		}

		public static void Stop(ref bool aboveThreshold)
		{
		}

		public static void Stop()
		{
		}

		public static void StopAndLogBreach(ChannelLogger.EVerbosity _level = ChannelLogger.EVerbosity.Warning)
		{
		}

		public static void Clear()
		{
		}

		public static void LimitSamples(int _count)
		{
		}

		public static void GetSamples(ref BBList<Profiling> resultList)
		{
		}

		public static void DumpLastFrame(ref string _output)
		{
		}

		public static void DumpLastFrame(ChannelLogger.EVerbosity _level = ChannelLogger.EVerbosity.Info)
		{
		}

		public static void DumpAll(ChannelLogger.EVerbosity _level = ChannelLogger.EVerbosity.Info)
		{
		}

		public static void GetTopSelfForFrame(ref List<Profiling> _result, int _frame, int _count)
		{
			_result.Clear();
		}

		private static void Log(string _value, ChannelLogger.EVerbosity _verbosity)
		{
			if (_verbosity <= ChannelLogger.EVerbosity.Info)
			{
				if (_verbosity != ChannelLogger.EVerbosity.Verbose)
				{
					int num = 300;
				}
			}
			else if (_verbosity != ChannelLogger.EVerbosity.Warning && _verbosity != ChannelLogger.EVerbosity.Error)
			{
				int num2 = 600;
			}
		}

		private static string GetMemoryStats()
		{
			return string.Format("GC Memory: {0}", GC.GetTotalMemory(false));
		}

		public static void DumpMemoryStats(ChannelLogger.EVerbosity _level = ChannelLogger.EVerbosity.Info)
		{
			Log(GetMemoryStats(), _level);
		}
	}
}
