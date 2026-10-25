using System;
using System.Diagnostics;
using System.Threading;

namespace BB.Mobile.Net
{
	public abstract class AsyncResult<C> : IAsyncResult
	{
		private EnumSentinel<AsyncResultState> mSentinel = new EnumSentinel<AsyncResultState>(AsyncResultState.Processing);

		private readonly ManualResetEvent mAsyncWaitHandle = new ManualResetEvent(false);

		private readonly C mContext;

		private readonly IAsyncCallbackScheduler mScheduler;

		private readonly AsyncCallback mCallback;

		private readonly object mState;

		private readonly Stopwatch mDuration = Stopwatch.StartNew();

		private Exception mException;

		private bool mSynchronous;

		public C Context
		{
			get
			{
				return mContext;
			}
		}

		public Exception Exception
		{
			get
			{
				return mException;
			}
		}

		public TimeSpan Duration
		{
			get
			{
				if (!IsCompleted)
				{
					return TimeSpan.Zero;
				}
				return mDuration.Elapsed;
			}
		}

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
				return mSentinel.State == AsyncResultState.Completed;
			}
		}

		public AsyncResult(C context)
			: this(context, (AsyncCallback)null, (object)null, (IAsyncCallbackScheduler)null)
		{
		}

		public AsyncResult(C context, AsyncCallback callback, object state, IAsyncCallbackScheduler scheduler)
		{
			if (callback != null && scheduler == null)
			{
				throw new ArgumentNullException();
			}
			mContext = context;
			mScheduler = scheduler;
			mCallback = callback;
			mState = state;
		}

		protected void Complete(Exception exception, bool synchronous)
		{
			mSentinel.Transition(AsyncResultState.Completing, AsyncResultState.Processing);
			mException = exception;
			mSynchronous = synchronous;
			mDuration.Stop();
			mSentinel.Transition(AsyncResultState.Completed);
			mAsyncWaitHandle.Set();
			if (mScheduler != null)
			{
				mScheduler.Schedule(mCallback, this);
			}
		}

		protected void Complete(bool synchronous)
		{
			Complete(null, synchronous);
		}

		public bool WaitForCompletion()
		{
			if (!IsCompleted)
			{
				mAsyncWaitHandle.WaitOne();
			}
			return mException == null;
		}

		public void End()
		{
			if (!WaitForCompletion())
			{
				throw mException;
			}
		}
	}
}
