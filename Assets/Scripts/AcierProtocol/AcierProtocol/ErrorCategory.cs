using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ErrorCategory")]
	public enum ErrorCategory
	{
		[ProtoEnum(Name = "SERVER_INTERNAL", Value = 4000)]
		SERVER_INTERNAL = 4000,
		[ProtoEnum(Name = "SERVER_INVALID_PARAM", Value = 4001)]
		SERVER_INVALID_PARAM = 4001,
		[ProtoEnum(Name = "SERVER_MISSION", Value = 4002)]
		SERVER_MISSION = 4002,
		[ProtoEnum(Name = "SERVER_INVENTORY", Value = 4003)]
		SERVER_INVENTORY = 4003,
		[ProtoEnum(Name = "SERVER_MAIL", Value = 4004)]
		SERVER_MAIL = 4004,
		[ProtoEnum(Name = "SERVER_ASSASSIN", Value = 4005)]
		SERVER_ASSASSIN = 4005,
		[ProtoEnum(Name = "SERVER_USERPROFILE", Value = 4006)]
		SERVER_USERPROFILE = 4006,
		[ProtoEnum(Name = "SERVER_SESSION", Value = 4007)]
		SERVER_SESSION = 4007,
		[ProtoEnum(Name = "SERVER_INGAME_SHOP", Value = 4008)]
		SERVER_INGAME_SHOP = 4008,
		[ProtoEnum(Name = "SERVER_FRIENDLIST", Value = 4009)]
		SERVER_FRIENDLIST = 4009,
		[ProtoEnum(Name = "SERVER_ACHIEVEMENT", Value = 4010)]
		SERVER_ACHIEVEMENT = 4010,
		[ProtoEnum(Name = "SERVER_DAILY_REWARD", Value = 4011)]
		SERVER_DAILY_REWARD = 4011,
		[ProtoEnum(Name = "SERVER_MYSTERY_BOX", Value = 4013)]
		SERVER_MYSTERY_BOX = 4013,
		[ProtoEnum(Name = "SERVER_FORGE", Value = 4014)]
		SERVER_FORGE = 4014,
		[ProtoEnum(Name = "SERVER_ENTITLEMENT", Value = 4015)]
		SERVER_ENTITLEMENT = 4015,
		[ProtoEnum(Name = "SERVER_PROGRESSION_QUEST", Value = 4016)]
		SERVER_PROGRESSION_QUEST = 4016,
		[ProtoEnum(Name = "SERVER_ENERGY_BAR", Value = 4017)]
		SERVER_ENERGY_BAR = 4017,
		[ProtoEnum(Name = "SERVER_EVENTS_CHALLENGE", Value = 4018)]
		SERVER_EVENTS_CHALLENGE = 4018,
		[ProtoEnum(Name = "SERVER_LEADER_BOARDS", Value = 4019)]
		SERVER_LEADER_BOARDS = 4019,
		[ProtoEnum(Name = "UnknownCategory", Value = 0)]
		UnknownCategory = 0,
		[ProtoEnum(Name = "GeneralCategory", Value = 1)]
		GeneralCategory = 1,
		[ProtoEnum(Name = "Availability", Value = 2)]
		Availability = 2,
		[ProtoEnum(Name = "Reachability", Value = 3)]
		Reachability = 3,
		[ProtoEnum(Name = "UbiServices", Value = 30)]
		UbiServices = 30,
		[ProtoEnum(Name = "WebStatusException", Value = 40)]
		WebStatusException = 40,
		[ProtoEnum(Name = "CLIENT_HC_SHOP", Value = 5000)]
		CLIENT_HC_SHOP = 5000,
		[ProtoEnum(Name = "SERVER_HC_SHOP", Value = 5001)]
		SERVER_HC_SHOP = 5001,
		[ProtoEnum(Name = "AcierStartupErrors", Value = 6000)]
		AcierStartupErrors = 6000,
		[ProtoEnum(Name = "UPLAY", Value = 7000)]
		UPLAY = 7000,
		[ProtoEnum(Name = "FACEBOOK", Value = 8000)]
		FACEBOOK = 8000,
		[ProtoEnum(Name = "ADCOLONY", Value = 9000)]
		ADCOLONY = 9000,
		[ProtoEnum(Name = "LICENSE_CHECK", Value = 9100)]
		LICENSE_CHECK = 9100
	}
}
