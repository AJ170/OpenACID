using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EShopCategory")]
	public enum EShopCategory
	{
		[ProtoEnum(Name = "Shopcat_HcPackage", Value = 1)]
		Shopcat_HcPackage = 1,
		[ProtoEnum(Name = "Shopcat_Premium", Value = 2)]
		Shopcat_Premium = 2,
		[ProtoEnum(Name = "Shopcat_Modules", Value = 3)]
		Shopcat_Modules = 3,
		[ProtoEnum(Name = "Shopcat_Decryption", Value = 6)]
		Shopcat_Decryption = 6,
		[ProtoEnum(Name = "Shopcat_Outfits", Value = 7)]
		Shopcat_Outfits = 7,
		[ProtoEnum(Name = "Shopcat_DailyRewards", Value = 8)]
		Shopcat_DailyRewards = 8,
		[ProtoEnum(Name = "Shopcat_Items", Value = 9)]
		Shopcat_Items = 9,
		[ProtoEnum(Name = "Shopcat_Heroic", Value = 10)]
		Shopcat_Heroic = 10,
		[ProtoEnum(Name = "Shopcat_Forge", Value = 11)]
		Shopcat_Forge = 11,
		[ProtoEnum(Name = "Shopcat_Highlights", Value = 12)]
		Shopcat_Highlights = 12,
		[ProtoEnum(Name = "Shopcat_MysteryBoxes", Value = 13)]
		Shopcat_MysteryBoxes = 13,
		[ProtoEnum(Name = "Shopcat_StarterPacks", Value = 14)]
		Shopcat_StarterPacks = 14
	}
}
