using System;

namespace BB.Mobile.Net
{
	public sealed class AsyncValueResult<T, C> : AsyncResult<C>, IAsyncValueResult<T>, IAsyncResult
	{
		public delegate bool VerifyContext(C context);

		private T mResult;

		public T Result
		{
			get
			{
				return mResult;
			}
		}

		public static AsyncValueResult<T, C> Verify(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException();
			}
			AsyncValueResult<T, C> obj = asyncResult as AsyncValueResult<T, C>;
			if (obj == null)
			{
				throw new ArgumentException();
			}
			return obj;
		}

		public static AsyncValueResult<T, C> Verify(IAsyncResult asyncResult, VerifyContext verifyContext)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException();
			}
			AsyncValueResult<T, C> asyncValueResult = asyncResult as AsyncValueResult<T, C>;
			if (asyncValueResult == null || (verifyContext != null && !verifyContext(asyncValueResult.Context)))
			{
				throw new ArgumentException();
			}
			return asyncValueResult;
		}

		public AsyncValueResult(C context)
			: this(context, (AsyncCallback)null, (object)null, (IAsyncCallbackScheduler)null)
		{
		}

		public AsyncValueResult(C context, AsyncCallback callback, object state, IAsyncCallbackScheduler scheduler)
			: base(context, callback, state, scheduler)
		{
		}

		public void Complete(T result, bool synchronous)
		{
			mResult = result;
			Complete(synchronous);
		}

		public void CompleteWithError(Exception exception, bool synchronous)
		{
			Complete(exception, synchronous);
		}
	}
}
