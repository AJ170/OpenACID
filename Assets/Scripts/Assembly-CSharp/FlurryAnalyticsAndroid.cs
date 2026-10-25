using System;
using System.Collections.Generic;
using UnityEngine;

public class FlurryAnalyticsAndroid : FlurryAnalyticsBase
{
	private static AndroidJavaObject s_flurryManager = new AndroidJavaObject("com.acid.plugins.tracking.FlurryManager");

	public override void OnInit(string _apiKey)
	{
		s_flurryManager.CallStatic("Init", _apiKey);
	}

	public override void LogEvent(string _eventName)
	{
		if (!string.IsNullOrEmpty(_eventName))
		{
			s_flurryManager.CallStatic("LogEvent", _eventName);
		}
	}

	public override void LogTimedEvent(string _eventName)
	{
		if (!string.IsNullOrEmpty(_eventName))
		{
			s_flurryManager.CallStatic("LogTimedEvent", _eventName, true);
		}
	}

	public override void LogEventWithParameters(string _eventName, Dictionary<string, string> _parameters)
	{
		if (!string.IsNullOrEmpty(_eventName))
		{
			if (_parameters != null && _parameters.Count > 0)
			{
				s_flurryManager.CallStatic("LogEventWithParameters", _eventName, ConvertDictToMapObject(_parameters));
			}
			else
			{
				s_flurryManager.CallStatic("LogEvent", _eventName);
			}
		}
	}

	public override void LogTimedEventWithParameters(string _eventName, Dictionary<string, string> _parameters)
	{
		if (!string.IsNullOrEmpty(_eventName))
		{
			if (_parameters != null && _parameters.Count > 0)
			{
				s_flurryManager.CallStatic("LogTimedEventWithParameters", _eventName, ConvertDictToMapObject(_parameters), true);
			}
			else
			{
				s_flurryManager.CallStatic("LogTimedEvent", _eventName, true);
			}
		}
	}

	public override void EndTimedEventWithParameters(string _eventName, Dictionary<string, string> _parameters)
	{
		if (!string.IsNullOrEmpty(_eventName))
		{
			if (_parameters != null && _parameters.Count > 0)
			{
				s_flurryManager.CallStatic("EndTimedEventWithParameters", _eventName, ConvertDictToMapObject(_parameters));
			}
			else
			{
				s_flurryManager.CallStatic("EndTimedEvent", _eventName);
			}
		}
	}

	public AndroidJavaObject ConvertDictToMapObject(Dictionary<string, string> _parameters)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		object[] array = new object[2];
		foreach (KeyValuePair<string, string> _parameter in _parameters)
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", _parameter.Key))
			{
				using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", _parameter.Value))
				{
					array[0] = androidJavaObject2;
					array[1] = androidJavaObject3;
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
		}
		return androidJavaObject;
	}
}
