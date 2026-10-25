using System;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class TimeExtensions
	{
		public static TimeSpan ToTimeStamp(this DateTime _time)
		{
			return _time - ServerTimeManager.UnixTimeReference;
		}

		public static long ToJavaTimeStamp(this DateTime _time)
		{
			return (long)_time.ToTimeStamp().TotalMilliseconds;
		}
	}
}
