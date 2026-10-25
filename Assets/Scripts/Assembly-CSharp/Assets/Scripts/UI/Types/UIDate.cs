using System;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Types
{
	public class UIDate : IFormattable
	{
		public enum EDisplayType
		{
			Date = 0,
			Time = 1,
			DateTime = 2
		}

		public enum ETimeFormat
		{
			Normal = 0,
			AmPm = 1
		}

		private static readonly StringId s_today = 4586;

		private static readonly StringId s_yesterday = 4589;

		private static readonly StringId s_tomorrow = 4590;

		private static readonly StringId s_date = 4591;

		private static readonly StringId s_am = 4592;

		private static readonly StringId s_pm = 4593;

		private static readonly StringId s_time = 4588;

		private static readonly StringId s_dateTime = 4587;

		private readonly DateTime m_date;

		private readonly EDisplayType m_displayType;

		private readonly ETimeFormat m_timeFormat;

		public UIDate(DateTime _time, EDisplayType _displayType, ETimeFormat _format = ETimeFormat.Normal)
		{
			m_date = _time;
			m_displayType = _displayType;
			m_timeFormat = _format;
		}

		public override int GetHashCode()
		{
			return (m_date.GetHashCode() + m_displayType.GetHashCode() + m_timeFormat.GetHashCode()).GetHashCode();
		}

		public override string ToString()
		{
			return ToString(string.Empty, null);
		}

		public string ToString(string _format)
		{
			return ToString(_format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			switch (m_displayType)
			{
			case EDisplayType.Date:
				return FormatDate();
			case EDisplayType.Time:
				return FormatTime();
			case EDisplayType.DateTime:
				return FormatDateTime();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private string FormatDate()
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(m_date);
			if (Convert.ToInt64(timeSpan.TotalDays) == 0L)
			{
				return LocalizationManager.Instance.Get(s_today);
			}
			if (Convert.ToInt64(timeSpan.TotalDays) == 1)
			{
				return LocalizationManager.Instance.Get(s_yesterday);
			}
			if (Convert.ToInt64(timeSpan.TotalDays) == -1)
			{
				return LocalizationManager.Instance.Get(s_tomorrow);
			}
			return LocalizationManager.Instance.Get(s_date, new StringMapper
			{
				{
					"days",
					string.Format("{0:00}", m_date.Day)
				},
				{
					"months",
					string.Format("{0:00}", m_date.Month)
				},
				{ "years", m_date.Year }
			});
		}

		private string FormatTime()
		{
			if (m_timeFormat == ETimeFormat.Normal)
			{
				return LocalizationManager.Instance.Get(s_time, new StringMapper
				{
					{ "hours", m_date.Hour },
					{ "minutes", m_date.Minute },
					{ "seconds", m_date.Second }
				});
			}
			int num = m_date.Hour - 12;
			if (num >= 0)
			{
				return LocalizationManager.Instance.Get(s_pm, new StringMapper
				{
					{
						"hours",
						m_date.Hour - 12
					},
					{ "minutes", m_date.Minute },
					{ "seconds", m_date.Second }
				});
			}
			return LocalizationManager.Instance.Get(s_am, new StringMapper
			{
				{ "hours", m_date.Hour },
				{ "minutes", m_date.Minute },
				{ "seconds", m_date.Second }
			});
		}

		private string FormatDateTime()
		{
			return LocalizationManager.Instance.Get(s_dateTime, new StringMapper
			{
				{
					"date",
					FormatDate()
				},
				{
					"time",
					FormatTime()
				}
			});
		}
	}
}
