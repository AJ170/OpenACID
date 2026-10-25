#define TRACE
using System;
using System.Diagnostics;
using AcierProtocol;
using Assets.Scripts.Network.Error;

namespace Assets.Scripts.Network
{
	internal class AsyncDataResult<T> : AsyncErrorResult, IAsyncResult, IAsyncDataResult<T>
	{
		private T mResult;

		public Response OriginResponse { get; private set; }

		public AsyncDataResult()
			: this((object)null)
		{
		}

		public AsyncDataResult(object _state)
			: base(_state)
		{
		}

		public T Result()
		{
			if (!IsCompleted)
			{
				mAsyncWaitHandle.WaitOne();
			}
			if (base.Exception != null)
			{
				AcierErrorException ex = base.Exception as AcierErrorException;
				if (ex != null)
				{
					Trace.TraceError(ex.Message);
				}
				throw base.Exception;
			}
			return mResult;
		}

		public void Complete(Response _originResponse, T _result, Exception _exception, bool _synchronous)
		{
			OriginResponse = _originResponse;
			mResult = _result;
			Complete(_exception, _synchronous);
		}
	}
}
