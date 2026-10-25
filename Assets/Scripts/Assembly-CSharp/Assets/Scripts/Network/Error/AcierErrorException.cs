using System;

namespace Assets.Scripts.Network.Error
{
	public class AcierErrorException : Exception
	{
		public Error Error { get; private set; }

		public override string Message
		{
			get
			{
				string text = "\r\n\r\nNETWORK-ERROR\r\n";
				string text2 = text;
				text = text2 + "\tCategory = " + Error.ErrorCategory.ToString() + " (" + (int)Error.ErrorCategory + ")\r\n";
				text2 = text;
				text = text2 + "\tCode = " + Error.ErrorCode.ToString() + " (" + (int)Error.ErrorCode + ")\r\n";
				if (Error.ShouldNotfiyUser)
				{
					text2 = text;
					return text2 + "\tUser-Notification = YES (with message: " + Error.UserMessage + ")\r\n";
				}
				return text + "\tUser-Notification = NONE (automatic handling)\r\n";
			}
		}

		public bool PotentialConnectionIssue
		{
			get
			{
				if (Error.PotentialConnectionIssue)
				{
					return true;
				}
				AcierErrorException ex = InnerException as AcierErrorException;
				if (object.ReferenceEquals(ex, null))
				{
					return false;
				}
				return ex.PotentialConnectionIssue;
			}
		}

		public AcierErrorException(Error _error, Exception _innerException = null)
			: base(null, _innerException)
		{
			Error = _error ?? Error.DEFAULT;
		}
	}
}
