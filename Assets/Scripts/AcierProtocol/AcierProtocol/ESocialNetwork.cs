using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ESocialNetwork")]
	public enum ESocialNetwork
	{
		[ProtoEnum(Name = "Facebook", Value = 1)]
		Facebook = 1,
		[ProtoEnum(Name = "Weibo", Value = 2)]
		Weibo = 2,
		[ProtoEnum(Name = "UPlay", Value = 3)]
		UPlay = 3
	}
}
