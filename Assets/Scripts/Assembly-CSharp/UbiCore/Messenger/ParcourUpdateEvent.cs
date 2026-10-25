using System;

namespace UbiCore.Messenger
{
	[Serializable]
	public class ParcourUpdateEvent : Message
	{
		public enum EUpdateType
		{
			Input = 0,
			Cancel = 1
		}

		public EUpdateType UpdateType { get; protected set; }
	}
}
