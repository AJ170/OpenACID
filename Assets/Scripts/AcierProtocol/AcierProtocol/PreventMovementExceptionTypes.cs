using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "PreventMovementExceptionTypes")]
	public enum PreventMovementExceptionTypes
	{
		[ProtoEnum(Name = "PreventMovementException_Invalid", Value = 0)]
		PreventMovementException_Invalid = 0,
		[ProtoEnum(Name = "PreventMovementException_Courtesans", Value = 1)]
		PreventMovementException_Courtesans = 1,
		[ProtoEnum(Name = "PreventMovementException_Hireling", Value = 2)]
		PreventMovementException_Hireling = 2,
		[ProtoEnum(Name = "PreventMovementException_EagleVision", Value = 3)]
		PreventMovementException_EagleVision = 3,
		[ProtoEnum(Name = "PreventMovementException_AssassinateFromHideout", Value = 4)]
		PreventMovementException_AssassinateFromHideout = 4
	}
}
