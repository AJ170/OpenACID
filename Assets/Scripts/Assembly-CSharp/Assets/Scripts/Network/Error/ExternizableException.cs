using System;

namespace Assets.Scripts.Network.Error
{
	internal class ExternizableException : Exception
	{
		public ExternizableException(string message)
			: base(message)
		{
		}
	}
}
