using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ENpcCategory")]
	public enum ENpcCategory
	{
		[ProtoEnum(Name = "Enemy_ENpcCategory", Value = 0)]
		Enemy_ENpcCategory = 0,
		[ProtoEnum(Name = "Crowd_ENpcCategory", Value = 1)]
		Crowd_ENpcCategory = 1,
		[ProtoEnum(Name = "Mission_ENpcCategory", Value = 2)]
		Mission_ENpcCategory = 2,
		[ProtoEnum(Name = "Monk_ENpcCategory", Value = 3)]
		Monk_ENpcCategory = 3,
		[ProtoEnum(Name = "Courtesan_ENpcCategory", Value = 4)]
		Courtesan_ENpcCategory = 4,
		[ProtoEnum(Name = "Doctor_ENpcCategory", Value = 5)]
		Doctor_ENpcCategory = 5,
		[ProtoEnum(Name = "Invalid_ENpcCategory", Value = 10)]
		Invalid_ENpcCategory = 10
	}
}
