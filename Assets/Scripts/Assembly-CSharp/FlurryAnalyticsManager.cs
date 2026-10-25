using System.Collections.Generic;
using Assets.Scripts.Tracking;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class FlurryAnalyticsManager : MonoBehaviour
{
	public enum LogEventIds
	{
		START_SESSION = 0,
		END_SESSION = 1,
		IAP = 2
	}

	private static FlurryAnalyticsBase m_flurryAnalytics;

	public static string ApiKey { get; set; }

	private static string GetEventName(LogEventIds _eventId)
	{
		switch (_eventId)
		{
		case LogEventIds.START_SESSION:
			return "01_START_SESSION";
		case LogEventIds.END_SESSION:
			return "02_END_SESSION";
		case LogEventIds.IAP:
			return "03_IAP";
		default:
			Debug.LogError(string.Concat("Event name for event Id ", _eventId, " is not yet set!"));
			return null;
		}
	}

	public static FlurryAnalyticsManager Initialize(string _apiKey, GameObject _root)
	{
		ApiKey = _apiKey;
		return _root.AddComponentIfNotExist<FlurryAnalyticsManager>();
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	public static void LogEventWithParameters(LogEventIds _eventId, Dictionary<string, string> _parameters)
	{
		if (m_flurryAnalytics != null)
		{
			m_flurryAnalytics.LogEventWithParameters(GetEventName(_eventId), _parameters);
		}
	}

	public static void LogTimedEventBegin(LogEventIds _eventId)
	{
		if (m_flurryAnalytics != null)
		{
			m_flurryAnalytics.LogTimedEvent(GetEventName(_eventId));
		}
	}

	public static void LogTimedEventEnd(LogEventIds _eventId)
	{
		LogTimedEventWithParametersEnd(_eventId, null);
	}

	public static void LogTimedEventWithParametersBegin(LogEventIds _eventId, Dictionary<string, string> _parameters)
	{
		if (m_flurryAnalytics != null)
		{
			m_flurryAnalytics.LogTimedEventWithParameters(GetEventName(_eventId), _parameters);
		}
	}

	public static void LogTimedEventWithParametersEnd(LogEventIds _eventId, Dictionary<string, string> _parameters)
	{
		if (m_flurryAnalytics != null)
		{
			m_flurryAnalytics.EndTimedEventWithParameters(GetEventName(_eventId), _parameters);
		}
	}

	private static void LogStartSession()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("User_Country", DeviceManager.UserCountry);
		LogEventWithParameters(LogEventIds.START_SESSION, dictionary);
	}

	private void OnApplicationPause(bool _pauseStatus)
	{
		if (m_flurryAnalytics != null)
		{
			if (_pauseStatus)
			{
				LogEventWithParameters(LogEventIds.END_SESSION, null);
			}
			else
			{
				LogStartSession();
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (m_flurryAnalytics != null)
		{
			LogEventWithParameters(LogEventIds.END_SESSION, null);
		}
	}

	private void OnDestroy()
	{
		m_flurryAnalytics = null;
	}
}
