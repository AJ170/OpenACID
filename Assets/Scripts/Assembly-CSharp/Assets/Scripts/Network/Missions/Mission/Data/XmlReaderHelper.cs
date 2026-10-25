using System;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public static class XmlReaderHelper
	{
		public static DateTime ParseDateTime(string _timeString)
		{
			if (string.IsNullOrEmpty(_timeString))
			{
				return DateTime.Now;
			}
			string[] array = _timeString.Split(' ');
			if (array.Length != 2)
			{
				return DateTime.Now;
			}
			string text = array[0];
			string text2 = array[1];
			string[] array2 = text.Split('-');
			string[] array3 = text2.Split(':', '-');
			if (array2.Length != 3)
			{
				return DateTime.Now;
			}
			if (array3.Length != 3)
			{
				return DateTime.Now;
			}
			int hour = int.Parse(array3[0]);
			int minute = int.Parse(array3[1]);
			int second = int.Parse(array3[2]);
			int year = int.Parse(array2[0]);
			int month = int.Parse(array2[1]);
			int day = int.Parse(array2[2]);
			return new DateTime(year, month, day, hour, minute, second);
		}

		public static string FormatDateTime(DateTime _time)
		{
			return _time.ToString("yyyy-MM-dd hh:mm:ss");
		}
	}
}
