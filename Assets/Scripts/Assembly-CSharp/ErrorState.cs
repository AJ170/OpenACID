using System.Collections;
using AcierProtocol;
using Assets.Scripts.Network.Error;
using UnityEngine;

public class ErrorState
{
	public ErrorCode ErrorCode { get; set; }

	public ErrorCategory ErrorGroup { get; set; }

	public IEnumerator DetermineReachabilityError(AcierErrorException acierErrorException = null)
	{
		if (object.ReferenceEquals(acierErrorException, null) || acierErrorException.PotentialConnectionIssue)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				ErrorCode = ErrorCode.NoNetworkConnection;
				ErrorGroup = ErrorCategory.Reachability;
			}
			else if (!object.ReferenceEquals(acierErrorException, null) && acierErrorException.Error.ErrorCode == ErrorCode.WESTimeout)
			{
				ErrorCode = ErrorCode.WESTimeout;
				ErrorGroup = ErrorCategory.Reachability;
			}
			else
			{
				ErrorCode = ErrorCode.NoInternetAccess;
				ErrorGroup = ErrorCategory.Reachability;
			}
		}
		yield break;
	}
}
