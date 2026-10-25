using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectSubCategory")]
	public enum EObjectSubCategory
	{
		[ProtoEnum(Name = "Chest", Value = 1)]
		Chest = 1,
		[ProtoEnum(Name = "PigeonCoup", Value = 2)]
		PigeonCoup = 2,
		[ProtoEnum(Name = "Elevated", Value = 4)]
		Elevated = 4
	}
}
