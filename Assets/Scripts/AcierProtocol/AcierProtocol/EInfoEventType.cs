using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EInfoEventType")]
	public enum EInfoEventType
	{
		[ProtoEnum(Name = "RestrictedAreaEnter", Value = 0)]
		RestrictedAreaEnter = 0,
		[ProtoEnum(Name = "RestrictedAreaLeave", Value = 1)]
		RestrictedAreaLeave = 1,
		[ProtoEnum(Name = "AnonymousLevel", Value = 2)]
		AnonymousLevel = 2,
		[ProtoEnum(Name = "AlertedLevel", Value = 3)]
		AlertedLevel = 3,
		[ProtoEnum(Name = "ExposedLevel", Value = 4)]
		ExposedLevel = 4,
		[ProtoEnum(Name = "WrongChest", Value = 5)]
		WrongChest = 5,
		[ProtoEnum(Name = "CorrectChest", Value = 6)]
		CorrectChest = 6,
		[ProtoEnum(Name = "DoctorUsed", Value = 7)]
		DoctorUsed = 7,
		[ProtoEnum(Name = "EagleVisionSuccess", Value = 8)]
		EagleVisionSuccess = 8,
		[ProtoEnum(Name = "TextAnnotation", Value = 9)]
		TextAnnotation = 9,
		[ProtoEnum(Name = "CharacterAlarmed", Value = 10)]
		CharacterAlarmed = 10,
		[ProtoEnum(Name = "CharacterRejects", Value = 11)]
		CharacterRejects = 11,
		[ProtoEnum(Name = "TailingTargetGetsAway", Value = 12)]
		TailingTargetGetsAway = 12,
		[ProtoEnum(Name = "TailingKeepLineOfSight", Value = 13)]
		TailingKeepLineOfSight = 13,
		[ProtoEnum(Name = "Invalid_EInfoEventType", Value = 20)]
		Invalid_EInfoEventType = 20
	}
}
