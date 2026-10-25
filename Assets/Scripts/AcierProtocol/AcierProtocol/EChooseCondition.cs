using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EChooseCondition")]
	public enum EChooseCondition
	{
		[ProtoEnum(Name = "Population", Value = 0)]
		Population = 0,
		[ProtoEnum(Name = "KillType", Value = 1)]
		KillType = 1,
		[ProtoEnum(Name = "DeliverType", Value = 2)]
		DeliverType = 2,
		[ProtoEnum(Name = "KillEnemyType_EChooseCondition", Value = 3)]
		KillEnemyType_EChooseCondition = 3,
		[ProtoEnum(Name = "CivilianType_EChooseCondition", Value = 4)]
		CivilianType_EChooseCondition = 4,
		[ProtoEnum(Name = "CourtesanType_EChooseCondition", Value = 5)]
		CourtesanType_EChooseCondition = 5,
		[ProtoEnum(Name = "DoctorType_EChooseCondition", Value = 6)]
		DoctorType_EChooseCondition = 6,
		[ProtoEnum(Name = "CaptivesType_EChooseCondition", Value = 7)]
		CaptivesType_EChooseCondition = 7,
		[ProtoEnum(Name = "ThievesType_EChooseCondition", Value = 8)]
		ThievesType_EChooseCondition = 8,
		[ProtoEnum(Name = "EscortType_EChooseCondition", Value = 9)]
		EscortType_EChooseCondition = 9,
		[ProtoEnum(Name = "TailType_EChooseCondition", Value = 10)]
		TailType_EChooseCondition = 10,
		[ProtoEnum(Name = "Parkour_EChooseCondition", Value = 11)]
		Parkour_EChooseCondition = 11,
		[ProtoEnum(Name = "Reinforcements_EChooseCondition", Value = 12)]
		Reinforcements_EChooseCondition = 12
	}
}
