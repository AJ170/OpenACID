using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFriendType")]
	public enum EFriendType
	{
		[ProtoEnum(Name = "FriendTypeBidirectional", Value = 0)]
		FriendTypeBidirectional = 0,
		[ProtoEnum(Name = "FriendTypeUnidirectional", Value = 1)]
		FriendTypeUnidirectional = 1
	}
}
