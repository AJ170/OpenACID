using System;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Types
{
	public sealed class UIDuration : IComparable, IFormattable
	{
		private static readonly StringId s_DaysHoursLong = 5418;

		private static readonly StringId s_DaysMinutesLong = 7681;

		private static readonly StringId s_HoursLong = 2025;

		private static readonly StringId s_MinuteLong = 2026;

		private static readonly StringId s_SecondsLong = 2027;

		private static readonly StringId s_DaysShort = 2024;

		private static readonly StringId s_HoursShort = 4595;

		private static readonly StringId s_MinuteShort = 4594;

		private static readonly StringId s_SecondsShort = 2027;

		private static readonly StringId s_DaysLongName = 5706;

		private long m_value = -1L;

		private readonly bool m_shortFormat;

		private readonly bool m_longName;

		public long Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
			}
		}

		public bool Valid
		{
			get
			{
				return Value >= 0;
			}
		}

		public UIDuration(long _value, bool _shortFormat = false, bool _longName = false)
		{
			m_value = _value;
			m_shortFormat = _shortFormat;
			m_longName = _longName;
		}

		public static UIDuration FromMilliSeconds(long _milliSeconds, bool _shortFormat = false, bool _longName = false)
		{
			return new UIDuration(_milliSeconds, _shortFormat, _longName);
		}

		public static UIDuration FromSeconds(long _seconds, bool _shortFormat = false, bool _longName = false)
		{
			return new UIDuration(_seconds * 1000, _shortFormat, _longName);
		}

		public static UIDuration FromMinutes(long _minutes, bool _shortFormat = false, bool _longName = false)
		{
			return new UIDuration(_minutes * 60000, _shortFormat, _longName);
		}

		public static UIDuration FromHours(long _hours, bool _shortFormat = false, bool _longName = false)
		{
			return new UIDuration(_hours * 3600000, _shortFormat, _longName);
		}

		public override string ToString()
		{
			return ToString(string.Empty, null);
		}

		public string ToString(string _format)
		{
			return ToString(_format, null);
		}

		public string ToString(string _format, IFormatProvider _provider)
		{
			if (string.IsNullOrEmpty(_format))
			{
				_format = "N";
			}
			_format = _format.Trim().ToUpperInvariant();
			if (_provider == null)
			{
				_provider = new BBTextFormatter();
			}
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(Value);
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("Days", LocalizationManager.Instance.GetInteger(timeSpan.Days));
			stringMapper.Add("Hours", LocalizationManager.Instance.GetInteger(timeSpan.Hours));
			stringMapper.Add("Minutes", LocalizationManager.Instance.GetInteger(timeSpan.Minutes));
			stringMapper.Add("Seconds", LocalizationManager.Instance.GetInteger(timeSpan.Seconds));
			stringMapper.Add("MilliSeconds", LocalizationManager.Instance.GetInteger(timeSpan.Milliseconds));
			StringMapper mapper = stringMapper;
			if (timeSpan.Days >= 1)
			{
				StringId key = ((!m_longName) ? s_DaysShort : s_DaysLongName);
				if (!m_shortFormat)
				{
					if (timeSpan.Hours > 0)
					{
						key = s_DaysHoursLong;
					}
					else if (timeSpan.Minutes > 0)
					{
						key = s_DaysMinutesLong;
					}
				}
				return LocalizationManager.Instance.Get(key, mapper);
			}
			if (timeSpan.Hours >= 1)
			{
				return LocalizationManager.Instance.Get((!m_shortFormat && timeSpan.Minutes != 0) ? s_HoursLong : s_HoursShort, mapper);
			}
			if (timeSpan.Minutes >= 1)
			{
				return LocalizationManager.Instance.Get((!m_shortFormat && timeSpan.Seconds != 0) ? s_MinuteLong : s_MinuteShort, mapper);
			}
			return LocalizationManager.Instance.Get((!m_shortFormat) ? s_SecondsLong : s_SecondsShort, mapper);
		}

		public override int GetHashCode()
		{
			return m_value.GetHashCode();
		}

		public override bool Equals(object _obj)
		{
			return Equals(_obj as UIDuration);
		}

		public bool Equals(UIDuration _other)
		{
			if (object.ReferenceEquals(_other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, _other))
			{
				return true;
			}
			return m_value.Equals(_other.m_value);
		}

		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			UIDuration uIDuration = _obj as UIDuration;
			if (uIDuration != null)
			{
				return m_value.CompareTo(uIDuration.m_value);
			}
			throw new ArgumentException("Right hand side must be UIDuration");
		}

		public static implicit operator long(UIDuration _duration)
		{
			return _duration.m_value;
		}

		public static implicit operator UIDuration(double _duration)
		{
			return new UIDuration((long)_duration);
		}

		public static implicit operator UIDuration(int _duration)
		{
			return new UIDuration(_duration);
		}

		public static implicit operator UIDuration(short _duration)
		{
			return new UIDuration(_duration);
		}

		public static bool operator ==(UIDuration _lhs, UIDuration _rhs)
		{
			if (object.ReferenceEquals(_lhs, null))
			{
				if (object.ReferenceEquals(_rhs, null))
				{
					return true;
				}
				return false;
			}
			return _lhs.Equals(_rhs);
		}

		public static bool operator !=(UIDuration _lhs, UIDuration _rhs)
		{
			return !(_lhs == _rhs);
		}
	}
}
