using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EShopProductQuality")]
	public enum EShopProductQuality
	{
		[ProtoEnum(Name = "MundaneProduct", Value = 1)]
		MundaneProduct = 1,
		[ProtoEnum(Name = "CommonProduct", Value = 2)]
		CommonProduct = 2,
		[ProtoEnum(Name = "UncommonProduct", Value = 3)]
		UncommonProduct = 3,
		[ProtoEnum(Name = "RareProduct", Value = 4)]
		RareProduct = 4,
		[ProtoEnum(Name = "PerfectProduct", Value = 5)]
		PerfectProduct = 5
	}
}
