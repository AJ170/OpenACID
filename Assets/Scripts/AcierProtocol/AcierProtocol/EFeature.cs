using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFeature")]
	public enum EFeature
	{
		[ProtoEnum(Name = "Feature_LocationNormalAccess", Value = 1)]
		Feature_LocationNormalAccess = 1,
		[ProtoEnum(Name = "Feature_Decryption", Value = 3)]
		Feature_Decryption = 3,
		[ProtoEnum(Name = "Feature_MysteryBox", Value = 4)]
		Feature_MysteryBox = 4,
		[ProtoEnum(Name = "Feature_ProgressionQuest", Value = 5)]
		Feature_ProgressionQuest = 5,
		[ProtoEnum(Name = "Feature_MysteryBoxCarribean", Value = 6)]
		Feature_MysteryBoxCarribean = 6,
		[ProtoEnum(Name = "Feature_MysteryBoxACM", Value = 7)]
		Feature_MysteryBoxACM = 7
	}
}
