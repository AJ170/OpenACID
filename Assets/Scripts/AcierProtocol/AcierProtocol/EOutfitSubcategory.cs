using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EOutfitSubcategory")]
	public enum EOutfitSubcategory
	{
		[ProtoEnum(Name = "OutfitSubcategory_NoviceOutfit", Value = 1)]
		OutfitSubcategory_NoviceOutfit = 1,
		[ProtoEnum(Name = "OutfitSubcategory_ClassOutfit", Value = 2)]
		OutfitSubcategory_ClassOutfit = 2,
		[ProtoEnum(Name = "OutfitSubcategory_PrestigeOutfit", Value = 3)]
		OutfitSubcategory_PrestigeOutfit = 3
	}
}
