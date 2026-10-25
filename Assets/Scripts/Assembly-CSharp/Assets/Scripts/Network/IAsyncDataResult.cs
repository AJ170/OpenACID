using System;
using AcierProtocol;

namespace Assets.Scripts.Network
{
	public interface IAsyncDataResult<T> : IAsyncResult
	{
		Response OriginResponse { get; }

		T Result();

		void Complete(Response _originResponse, T _result, Exception _exception, bool _synchronous);
	}
}
