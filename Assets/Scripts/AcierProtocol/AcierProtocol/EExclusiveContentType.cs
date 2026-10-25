using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EExclusiveContentType")]
	public enum EExclusiveContentType
	{
		[ProtoEnum(Name = "ExclusiveContent_AppleWinterOutfit", Value = 0)]
		ExclusiveContent_AppleWinterOutfit = 0
	}
}
