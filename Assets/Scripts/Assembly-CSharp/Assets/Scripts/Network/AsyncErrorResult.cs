using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts.Network
{
	public class AsyncErrorResult : IAsyncResult
	{
		protected List<ScheduledCallback> m_scheduledCallbacks;

		protected readonly ManualResetEvent mAsyncWaitHandle;

		protected readonly object mState;

		protected bool mSynchronous;

		public Exception Exception { get; protected set; }

		public object AsyncState
		{
			get
			{
				return mState;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return mAsyncWaitHandle;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return mSynchronous;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return mAsyncWaitHandle.WaitOne(0);
			}
		}

		public AsyncErrorResult()
			: this(null)
		{
		}

		public AsyncErrorResult(object _state)
		{
			mAsyncWaitHandle = new ManualResetEvent(false);
			mState = _state;
			mSynchronous = false;
		}

		public void AddScheduledCallback(ScheduledCallback _scheduledCallback)
		{
			if (_scheduledCallback != null)
			{
				if (m_scheduledCallbacks == null)
				{
					m_scheduledCallbacks = new List<ScheduledCallback>(1);
				}
				m_scheduledCallbacks.Add(_scheduledCallback);
			}
		}

		public void Complete(Exception _exception, bool _synchronous)
		{
			Exception = _exception;
			mSynchronous = _synchronous;
			mAsyncWaitHandle.Set();
			if (m_scheduledCallbacks == null || m_scheduledCallbacks.Count <= 0)
			{
				return;
			}
			foreach (ScheduledCallback scheduledCallback in m_scheduledCallbacks)
			{
				if (scheduledCallback.Scheduler != null && scheduledCallback.Callback != null)
				{
					scheduledCallback.Scheduler.Schedule(scheduledCallback.Callback, this);
				}
			}
			m_scheduledCallbacks = null;
		}
	}
}
