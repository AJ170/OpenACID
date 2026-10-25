using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionDifficulty")]
	public enum EMissionDifficulty
	{
		[ProtoEnum(Name = "Easy_EMissionDifficulty", Value = 1)]
		Easy_EMissionDifficulty = 1,
		[ProtoEnum(Name = "Medium_EMissionDifficulty", Value = 2)]
		Medium_EMissionDifficulty = 2,
		[ProtoEnum(Name = "Hard_EMissionDifficulty", Value = 4)]
		Hard_EMissionDifficulty = 4
	}
}
