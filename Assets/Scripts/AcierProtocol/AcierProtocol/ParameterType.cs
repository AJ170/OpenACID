using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ParameterType")]
	public enum ParameterType
	{
		[ProtoEnum(Name = "Integer_ParameterType", Value = 1)]
		Integer_ParameterType = 1,
		[ProtoEnum(Name = "Float_ParameterType", Value = 2)]
		Float_ParameterType = 2,
		[ProtoEnum(Name = "Double_ParameterType", Value = 3)]
		Double_ParameterType = 3,
		[ProtoEnum(Name = "String_ParameterType", Value = 4)]
		String_ParameterType = 4,
		[ProtoEnum(Name = "EAlertLevel_ParameterType", Value = 5)]
		EAlertLevel_ParameterType = 5,
		[ProtoEnum(Name = "ECompare_ParameterType", Value = 6)]
		ECompare_ParameterType = 6,
		[ProtoEnum(Name = "ELimitType_ParameterType", Value = 7)]
		ELimitType_ParameterType = 7,
		[ProtoEnum(Name = "EObjectSubCategory_ParameterType", Value = 8)]
		EObjectSubCategory_ParameterType = 8,
		[ProtoEnum(Name = "Boolean_ParameterType", Value = 9)]
		Boolean_ParameterType = 9,
		[ProtoEnum(Name = "ETemplateTargetKind_ParameterType", Value = 10)]
		ETemplateTargetKind_ParameterType = 10,
		[ProtoEnum(Name = "ENpcInteractionType_ParameterType", Value = 11)]
		ENpcInteractionType_ParameterType = 11,
		[ProtoEnum(Name = "EMissionAreaType_ParameterType", Value = 12)]
		EMissionAreaType_ParameterType = 12
	}
}
