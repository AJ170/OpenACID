using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMood")]
	public enum EMood
	{
		[ProtoEnum(Name = "Daytime", Value = 0)]
		Daytime = 0,
		[ProtoEnum(Name = "Nighttime", Value = 1)]
		Nighttime = 1,
		[ProtoEnum(Name = "Sunny", Value = 2)]
		Sunny = 2,
		[ProtoEnum(Name = "Stormy", Value = 3)]
		Stormy = 3,
		[ProtoEnum(Name = "Foggy", Value = 4)]
		Foggy = 4,
		[ProtoEnum(Name = "Afternoon", Value = 5)]
		Afternoon = 5,
		[ProtoEnum(Name = "Overcast", Value = 6)]
		Overcast = 6,
		[ProtoEnum(Name = "Dusk", Value = 7)]
		Dusk = 7,
		[ProtoEnum(Name = "Siege", Value = 8)]
		Siege = 8
	}
}
