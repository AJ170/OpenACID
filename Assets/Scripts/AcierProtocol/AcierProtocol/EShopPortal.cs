using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EShopPortal")]
	public enum EShopPortal
	{
		[ProtoEnum(Name = "All", Value = 1)]
		All = 1,
		[ProtoEnum(Name = "iOS", Value = 2)]
		iOS = 2,
		[ProtoEnum(Name = "Google", Value = 3)]
		Google = 3,
		[ProtoEnum(Name = "Amazon", Value = 4)]
		Amazon = 4,
		[ProtoEnum(Name = "Samsung", Value = 5)]
		Samsung = 5,
		[ProtoEnum(Name = "SKT", Value = 6)]
		SKT = 6
	}
}
