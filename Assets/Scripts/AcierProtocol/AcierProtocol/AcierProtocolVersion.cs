using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "AcierProtocolVersion")]
	public enum AcierProtocolVersion
	{
		[ProtoEnum(Name = "ACIER_PROTOCOL_VERSION_CURRENT", Value = 59)]
		ACIER_PROTOCOL_VERSION_CURRENT = 59
	}
}
