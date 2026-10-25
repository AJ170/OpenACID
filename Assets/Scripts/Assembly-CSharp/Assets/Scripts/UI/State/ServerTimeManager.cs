using System;

namespace Assets.Scripts.UI.State
{
	public sealed class ServerTimeManager
	{
		private long m_timeJava;

		private DateTime m_timeUtc;

		private DateTime m_updatedUtc;

		private TimeSpan m_offsetUtc;

		public static readonly DateTime UnixTimeReference = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public long Time
		{
			get
			{
				return m_timeJava;
			}
			set
			{
				m_timeJava = value;
				m_timeUtc = ToUtcTime(m_timeJava);
				m_updatedUtc = DateTime.UtcNow;
				m_offsetUtc = m_updatedUtc.Subtract(m_timeUtc);
			}
		}

		public DateTime Now
		{
			get
			{
				return UtcNow.Add(DateTime.Now - DateTime.UtcNow);
			}
		}

		public DateTime UtcNow
		{
			get
			{
				TimeSpan value = DateTime.UtcNow.Subtract(m_updatedUtc);
				return m_timeUtc.Add(value);
			}
		}

		public TimeSpan Offset
		{
			get
			{
				return m_offsetUtc;
			}
		}

		public static DateTime ToUtcTime(long _javaTimestamp)
		{
			return UnixTimeReference.AddMilliseconds(_javaTimestamp);
		}

		public static DateTime SecondsToUtcTime(long _seconds)
		{
			return UnixTimeReference.AddSeconds(_seconds);
		}

		public static DateTime ToLocalTime(long _seconds)
		{
			return SecondsToUtcTime(_seconds).ToLocalTime();
		}

		public static DateTime ToLocalTimeMilliseconds(long _milliseconds)
		{
			return ToUtcTime(_milliseconds).ToLocalTime();
		}
	}
}
