using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Error;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class NetworkExtensions
	{
		private static readonly List<int> s_restartErrors = new List<int> { 4009, 4063 };

		private static readonly List<int> s_restartGroups = new List<int> { 4007 };

		public static bool ShouldRemoveMail(this IAsyncResult _asyncResult)
		{
			AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
			if (asyncErrorResult == null)
			{
				return false;
			}
			return asyncErrorResult.ShouldRemoveMail();
		}

		public static bool ShouldRemoveMail(this AsyncErrorResult _asyncResult)
		{
			if (_asyncResult.Exception != null)
			{
				AcierErrorException ex = _asyncResult.Exception as AcierErrorException;
				if (ex != null)
				{
					return ex.ShouldRemoveMail();
				}
			}
			return false;
		}

		public static bool ShouldRemoveMail(this AcierErrorException _ex)
		{
			ErrorCode errorCode = _ex.Error.ErrorCode;
			if (errorCode == ErrorCode.MAIL_HEADER_NOT_FOUND || errorCode == ErrorCode.MAIL_BODY_NOT_FOUND || errorCode == ErrorCode.MAIL_SENDER_ALREADY_CANCELED_REQUEST)
			{
				return true;
			}
			return false;
		}

		public static bool IsError(this IAsyncResult _asyncResult)
		{
			AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
			return asyncErrorResult != null && asyncErrorResult.Exception != null;
		}

		public static bool RestartRequired(this AcierErrorException _error)
		{
			if (_error == null)
			{
				return false;
			}
			return s_restartErrors.Contains((int)_error.Error.ErrorCode) && s_restartGroups.Contains((int)_error.Error.ErrorCategory);
		}

		public static AcierErrorException GetException(this IAsyncResult _asyncResult)
		{
			AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
			return (asyncErrorResult == null) ? null : (asyncErrorResult.Exception as AcierErrorException);
		}
	}
}
