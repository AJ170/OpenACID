using System;
using BB.Mobile.Net;

namespace Assets.Scripts.Network
{
	public class ScheduledCallback
	{
		public AsyncCallback Callback { get; private set; }

		public IAsyncCallbackScheduler Scheduler { get; private set; }

		public ScheduledCallback(AsyncCallback _callback, IAsyncCallbackScheduler _scheduler)
		{
			Callback = _callback;
			Scheduler = _scheduler;
		}
	}
}
