using System;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Native;
using UnityEngine;

public class PushNotificationsAndroid : IPushNotifications
{
	private static AndroidJavaClass ms_classGcmBridge = new AndroidJavaClass("com.acid.plugins.gcm.GcmBridge");

	private static AndroidJavaClass ms_classUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

	private static AndroidJavaObject m_currentActivity = null;

	private static AndroidJavaObject m_gcmBridgeInstance = null;

	private static string NATIVE_MESSAGE_CALLBACK = "OnPushNativeMessage";

	private static string ANDROID_GAME_OBJECT_NAME = string.Empty;

	public static AndroidJavaObject GetCurrentActivity()
	{
		if (m_currentActivity == null)
		{
			m_currentActivity = ms_classUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		}
		return m_currentActivity;
	}

	public static AndroidJavaObject GetGcmBridgeInstance()
	{
		if (m_gcmBridgeInstance == null)
		{
			m_gcmBridgeInstance = ms_classGcmBridge.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
		return m_gcmBridgeInstance;
	}

	public void InitializeInstance()
	{
		string text = "626369821745";
		if (!NativeTools.IsShipping())
		{
			text = "753037180532";
		}
		Array values = Enum.GetValues(typeof(EPushNotificationGroup));
		string[] array = new string[values.Length];
		int num = 0;
		foreach (int item in values)
		{
			array[num] = ((EPushNotificationGroup)item).ToString();
			num++;
		}
		GetGcmBridgeInstance().Call("SetVariables", ANDROID_GAME_OBJECT_NAME, NATIVE_MESSAGE_CALLBACK, text, GetCurrentActivity(), array);
	}

	public void SetPushObjectName(string _gameObjectName)
	{
		ANDROID_GAME_OBJECT_NAME = _gameObjectName;
	}

	public void RegisterToPushNotifications()
	{
		GetCurrentActivity().Call("runOnUiThread", (AndroidJavaRunnable)delegate
		{
			GetGcmBridgeInstance().Call("RegisterToGCM");
		});
	}

	public void DeregisterFromPushNotifications()
	{
		GetGcmBridgeInstance().Call("UnregisterFromNotifications");
	}

	public string GetPushToken()
	{
		if (PushNotificationsProvider.isRegisteredToClientPNS)
		{
			return GetGcmBridgeInstance().Call<string>("GetRegistrationId", new object[1] { GetCurrentActivity() });
		}
		return string.Empty;
	}

	public void ScheduleLocalNotificationWithInterval(long _interval, string _title, string _alertBody, string _alertKey, EPushNotificationGroup _type)
	{
		GetGcmBridgeInstance().Call("ScheduleLocalNotification", _type.ToString(), _alertKey, _title, _alertBody, _interval.ToString());
	}

	public void CancelLocalNotificationWithKey(string _alertKey)
	{
		GetGcmBridgeInstance().Call("CancelLocalNotification", _alertKey);
	}

	public string GetHoustonURL()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string value = "https://gamecfg-mob.ubi.com/profile/?action=register";
		if (!NativeTools.IsShipping())
		{
			value = "https://dev-gamecfg-mob.ubi.com/profile/?action=register";
		}
		stringBuilder.Append(value);
		string value2 = "391";
		stringBuilder.Append("&productid=");
		stringBuilder.Append(value2);
		string deviceUniqueId = NativeTools.DeviceUniqueId;
		stringBuilder.Append("&deviceuid=");
		stringBuilder.Append(deviceUniqueId);
		string pushToken = GetPushToken();
		stringBuilder.Append("&gcm_regid=");
		stringBuilder.Append(pushToken);
		string languageForHouston = PushNotificationsProvider.GetLanguageForHouston();
		stringBuilder.Append("&lang=");
		stringBuilder.Append(languageForHouston);
		string value3 = DateTime.UtcNow.ToString("zzz").Remove(DateTime.UtcNow.ToString("zzz").IndexOf(':'));
		stringBuilder.Append("&timezone=Etc/GMT");
		stringBuilder.Append(value3);
		if (stringBuilder != null)
		{
			return stringBuilder.ToString();
		}
		return string.Empty;
	}
}
