using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GlobalLeaderBoardResult")]
	public class GlobalLeaderBoardResult : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RankLeaderBoard")]
		public class RankLeaderBoard : IExtensible
		{
			private int _rank;

			private LeaderBoardUser[] _leaderBoardUsers = new LeaderBoardUser[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int rank
			{
				get
				{
					return _rank;
				}
				set
				{
					_rank = value;
				}
			}

			[ProtoMember(2, Name = "leaderBoardUsers", DataFormat = DataFormat.Default)]
			public LeaderBoardUser[] leaderBoardUsers
			{
				get
				{
					return _leaderBoardUsers;
				}
				set
				{
					Array.Resize(ref _leaderBoardUsers, value.Length);
					value.CopyTo(_leaderBoardUsers, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private RankLeaderBoard[] _rankLeaderBoards = new RankLeaderBoard[0];

		private LeaderBoardUser[] _friendsleaderBoards = new LeaderBoardUser[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "rankLeaderBoards", DataFormat = DataFormat.Default)]
		public RankLeaderBoard[] rankLeaderBoards
		{
			get
			{
				return _rankLeaderBoards;
			}
			set
			{
				Array.Resize(ref _rankLeaderBoards, value.Length);
				value.CopyTo(_rankLeaderBoards, 0);
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
