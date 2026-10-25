using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EResourceType")]
	public enum EResourceType
	{
		[ProtoEnum(Name = "HardCurrency", Value = 1)]
		HardCurrency = 1,
		[ProtoEnum(Name = "SoftCurrency", Value = 2)]
		SoftCurrency = 2,
		[ProtoEnum(Name = "SocialCurrency", Value = 3)]
		SocialCurrency = 3,
		[ProtoEnum(Name = "AssassinSlot", Value = 18)]
		AssassinSlot = 18,
		[ProtoEnum(Name = "MaxEnergyExtension", Value = 19)]
		MaxEnergyExtension = 19,
		[ProtoEnum(Name = "InventorySlot", Value = 20)]
		InventorySlot = 20
	}
}
