using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ERuntimeMissionType")]
	public enum ERuntimeMissionType
	{
		[ProtoEnum(Name = "RandomMission", Value = 1)]
		RandomMission = 1,
		[ProtoEnum(Name = "StoryMission", Value = 2)]
		StoryMission = 2,
		[ProtoEnum(Name = "SharedMission", Value = 3)]
		SharedMission = 3
	}
}
