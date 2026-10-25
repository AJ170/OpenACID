using AcierProtocol;

public interface IPushNotifications
{
	void InitializeInstance();

	void SetPushObjectName(string gameObjectName);

	void RegisterToPushNotifications();

	void DeregisterFromPushNotifications();

	string GetPushToken();

	void ScheduleLocalNotificationWithInterval(long interval, string title, string alertBody, string alertKey, EPushNotificationGroup _type);

	void CancelLocalNotificationWithKey(string alertKey);

	string GetHoustonURL();
}
