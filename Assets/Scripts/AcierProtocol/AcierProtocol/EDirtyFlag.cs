using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EDirtyFlag")]
	public enum EDirtyFlag
	{
		[ProtoEnum(Name = "DirtyMails", Value = 1)]
		DirtyMails = 1,
		[ProtoEnum(Name = "DirtyFriends", Value = 2)]
		DirtyFriends = 2,
		[ProtoEnum(Name = "DirtyCounters", Value = 4)]
		DirtyCounters = 4,
		[ProtoEnum(Name = "DirtyMailsCache", Value = 8)]
		DirtyMailsCache = 8
	}
}
