using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EShopProductLabel")]
	public enum EShopProductLabel
	{
		[ProtoEnum(Name = "NoLabel", Value = 0)]
		NoLabel = 0,
		[ProtoEnum(Name = "MostPopular", Value = 1)]
		MostPopular = 1,
		[ProtoEnum(Name = "BestValue", Value = 2)]
		BestValue = 2
	}
}
