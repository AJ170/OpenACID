using System;
using AcierProtocol;
using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class EventPropertiesExtensions
	{
		public static string GetStringValue(this Event.Property _property)
		{
			return (!string.IsNullOrEmpty(_property.value)) ? _property.value : string.Empty;
		}

		public static GameID GetGameIdValue(this Event.Property _property)
		{
			GameDB gameDB = Main.Instance.GameDB;
			string text = ((!string.IsNullOrEmpty(_property.value)) ? _property.value : string.Empty);
			if (!string.IsNullOrEmpty(text) && Guid.Empty.ToString() != text)
			{
				return gameDB.FindGameIdFromGuid(text);
			}
			return GameID.Invalid;
		}

		public static T ParseEnum<T>(this Event.Property _property)
		{
			if (!string.IsNullOrEmpty(_property.value))
			{
				return (T)Enum.Parse(typeof(T), _property.value, true);
			}
			return default(T);
		}

		public static bool GetBoolValue(this Event.Property _property)
		{
			bool result = false;
			if (bool.TryParse(_property.value, out result))
			{
				return result;
			}
			return false;
		}

		public static int GetIntValue(this Event.Property _property)
		{
			int result = 0;
			if (int.TryParse(_property.value, out result))
			{
				return result;
			}
			return -1;
		}

		public static float GetFloatValue(this Event.Property _property)
		{
			float result = 0f;
			if (float.TryParse(_property.value, out result))
			{
				return result;
			}
			return -1f;
		}
	}
}
