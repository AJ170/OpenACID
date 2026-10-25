using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ERegion")]
	public enum ERegion
	{
		[ProtoEnum(Name = "Animus", Value = 0)]
		Animus = 0,
		[ProtoEnum(Name = "Monteriggioni", Value = 1)]
		Monteriggioni = 1,
		[ProtoEnum(Name = "Firenze_Palazzo", Value = 2)]
		Firenze_Palazzo = 2,
		[ProtoEnum(Name = "Firenze_SantaCroce", Value = 3)]
		Firenze_SantaCroce = 3,
		[ProtoEnum(Name = "Roma_Coloseum", Value = 4)]
		Roma_Coloseum = 4,
		[ProtoEnum(Name = "Roma_SanAngelo", Value = 5)]
		Roma_SanAngelo = 5,
		[ProtoEnum(Name = "Venezia", Value = 6)]
		Venezia = 6,
		[ProtoEnum(Name = "Forli", Value = 9)]
		Forli = 9,
		[ProtoEnum(Name = "Havana", Value = 10)]
		Havana = 10,
		[ProtoEnum(Name = "Nassau", Value = 11)]
		Nassau = 11,
		[ProtoEnum(Name = "Tortuga", Value = 12)]
		Tortuga = 12,
		[ProtoEnum(Name = "ACMS1", Value = 13)]
		ACMS1 = 13,
		[ProtoEnum(Name = "ACMG1", Value = 14)]
		ACMG1 = 14,
		[ProtoEnum(Name = "Montreal", Value = 7)]
		Montreal = 7,
		[ProtoEnum(Name = "Boston", Value = 8)]
		Boston = 8
	}
}
