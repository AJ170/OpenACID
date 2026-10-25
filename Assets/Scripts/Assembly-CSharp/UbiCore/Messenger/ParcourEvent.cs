using System;

namespace UbiCore.Messenger
{
	[Serializable]
	public class ParcourEvent : Message
	{
		public enum EParcourType
		{
			UseLink = 0,
			JumpAnywhere = 1,
			ClimbRebound = 2,
			JumpHorizontalToWall = 3
		}

		public EParcourType Type { get; protected set; }
	}
}
