using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectCategory")]
	public enum EObjectCategory
	{
		[ProtoEnum(Name = "HidingSpot_EObjectCategory", Value = 0)]
		HidingSpot_EObjectCategory = 0,
		[ProtoEnum(Name = "Codex_EObjectCategory", Value = 1)]
		Codex_EObjectCategory = 1,
		[ProtoEnum(Name = "Mission_EObjectCategory", Value = 2)]
		Mission_EObjectCategory = 2,
		[ProtoEnum(Name = "Portal_EObjectCategory", Value = 3)]
		Portal_EObjectCategory = 3,
		[ProtoEnum(Name = "Count_EObjectCategory", Value = 4)]
		Count_EObjectCategory = 4,
		[ProtoEnum(Name = "Invalid_EObjectCategory", Value = 10)]
		Invalid_EObjectCategory = 10
	}
}
