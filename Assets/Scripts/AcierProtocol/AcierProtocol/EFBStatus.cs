using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFBStatus")]
	public enum EFBStatus
	{
		[ProtoEnum(Name = "Registered", Value = 0)]
		Registered = 0,
		[ProtoEnum(Name = "NotRegistered", Value = 1)]
		NotRegistered = 1
	}
}
