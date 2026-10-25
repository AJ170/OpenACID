using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GeneratedLeaderBoardMapping")]
	public class GeneratedLeaderBoardMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "GeneratedLeaderBoardUser")]
		public class GeneratedLeaderBoardUser : IExtensible
		{
			private string _userName = "";

			private int _rank;

			private LeaderBoardUser.GeneratedLeaderBoardAssassin _generatedLeaderBoardAssassin;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string userName
			{
				get
				{
					return _userName;
				}
				set
				{
					_userName = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(3, IsRequired = false, Name = "generatedLeaderBoardAssassin", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LeaderBoardUser.GeneratedLeaderBoardAssassin generatedLeaderBoardAssassin
			{
				get
				{
					return _generatedLeaderBoardAssassin;
				}
				set
				{
					_generatedLeaderBoardAssassin = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "RankGeneratedLeaderBoardUser")]
		public class RankGeneratedLeaderBoardUser : IExtensible
		{
			private int _rank;

			private GeneratedLeaderBoardUser[] _generatedLeaderBoardUsers = new GeneratedLeaderBoardUser[0];

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

			[ProtoMember(2, Name = "generatedLeaderBoardUsers", DataFormat = DataFormat.Default)]
			public GeneratedLeaderBoardUser[] generatedLeaderBoardUsers
			{
				get
				{
					return _generatedLeaderBoardUsers;
				}
				set
				{
					Array.Resize(ref _generatedLeaderBoardUsers, value.Length);
					value.CopyTo(_generatedLeaderBoardUsers, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private RankGeneratedLeaderBoardUser[] _rankGeneratedLeaderBoardUsers = new RankGeneratedLeaderBoardUser[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "rankGeneratedLeaderBoardUsers", DataFormat = DataFormat.Default)]
		public RankGeneratedLeaderBoardUser[] rankGeneratedLeaderBoardUsers
		{
			get
			{
				return _rankGeneratedLeaderBoardUsers;
			}
			set
			{
				Array.Resize(ref _rankGeneratedLeaderBoardUsers, value.Length);
				value.CopyTo(_rankGeneratedLeaderBoardUsers, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
