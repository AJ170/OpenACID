using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EGlobalMissionParameter")]
	public enum EGlobalMissionParameter
	{
		[ProtoEnum(Name = "DoctorLimit", Value = 0)]
		DoctorLimit = 0
	}
}
