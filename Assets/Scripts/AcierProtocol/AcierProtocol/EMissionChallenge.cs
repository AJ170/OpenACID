using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionChallenge")]
	public enum EMissionChallenge
	{
		[ProtoEnum(Name = "EasyChallenge", Value = 1)]
		EasyChallenge = 1,
		[ProtoEnum(Name = "MediumChallenge", Value = 2)]
		MediumChallenge = 2,
		[ProtoEnum(Name = "HardChallenge", Value = 3)]
		HardChallenge = 3
	}
}
