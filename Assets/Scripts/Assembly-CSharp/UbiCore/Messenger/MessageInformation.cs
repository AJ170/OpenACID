using System;

namespace UbiCore.Messenger
{
	public sealed class MessageInformation
	{
		public Type Type { get; private set; }

		public int RegisteredListeners { get; private set; }

		public MessageInformation(Type _type, int _numListeners)
		{
			Type = _type;
			RegisteredListeners = _numListeners;
		}
	}
}
