using System.Collections.Generic;

public abstract class FlurryAnalyticsBase
{
	public abstract void OnInit(string _apiKey);

	public abstract void LogEvent(string _eventName);

	public abstract void LogEventWithParameters(string _eventName, Dictionary<string, string> _parameters);

	public abstract void LogTimedEvent(string _eventName);

	public abstract void LogTimedEventWithParameters(string _eventName, Dictionary<string, string> _parameters);

	public abstract void EndTimedEventWithParameters(string _eventName, Dictionary<string, string> _parameters);
}
