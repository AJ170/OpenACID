using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EventLeaderBoardResult")]
	public class EventLeaderBoardResult : IExtensible
	{
		private LeaderBoardUser[] _eventsleaderBoards = new LeaderBoardUser[0];

		private LeaderBoardUser[] _friendsleaderBoards = new LeaderBoardUser[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "eventsleaderBoards", DataFormat = DataFormat.Default)]
		public LeaderBoardUser[] eventsleaderBoards
		{
			get
			{
				return _eventsleaderBoards;
			}
			set
			{
				Array.Resize(ref _eventsleaderBoards, value.Length);
				value.CopyTo(_eventsleaderBoards, 0);
			}
		}

		[ProtoMember(2, Name = "friendsleaderBoards", DataFormat = DataFormat.Default)]
		public LeaderBoardUser[] friendsleaderBoards
		{
			get
			{
				return _friendsleaderBoards;
			}
			set
			{
				Array.Resize(ref _friendsleaderBoards, value.Length);
				value.CopyTo(_friendsleaderBoards, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
