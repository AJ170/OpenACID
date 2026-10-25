using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ELanguages")]
	public enum ELanguages
	{
		[ProtoEnum(Name = "BlueByte", Value = 0)]
		BlueByte = 0,
		[ProtoEnum(Name = "en_US", Value = 1)]
		en_US = 1,
		[ProtoEnum(Name = "en_GB", Value = 2)]
		en_GB = 2,
		[ProtoEnum(Name = "de_DE", Value = 3)]
		de_DE = 3,
		[ProtoEnum(Name = "es_ES", Value = 4)]
		es_ES = 4,
		[ProtoEnum(Name = "it_IT", Value = 5)]
		it_IT = 5,
		[ProtoEnum(Name = "ja_JP", Value = 6)]
		ja_JP = 6,
		[ProtoEnum(Name = "ko_KR", Value = 7)]
		ko_KR = 7,
		[ProtoEnum(Name = "pt_BR", Value = 8)]
		pt_BR = 8,
		[ProtoEnum(Name = "ru_RU", Value = 9)]
		ru_RU = 9,
		[ProtoEnum(Name = "zh_CHS", Value = 10)]
		zh_CHS = 10,
		[ProtoEnum(Name = "zh_CHT", Value = 11)]
		zh_CHT = 11,
		[ProtoEnum(Name = "fr_FR", Value = 12)]
		fr_FR = 12,
		[ProtoEnum(Name = "pl_PL", Value = 13)]
		pl_PL = 13
	}
}
