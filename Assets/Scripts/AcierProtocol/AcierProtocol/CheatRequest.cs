using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CheatRequest")]
	public class CheatRequest : IExtensible
	{
		[ProtoContract(Name = "CheatType")]
		public enum CheatType
		{
			[ProtoEnum(Name = "LEVEL_UP", Value = 1)]
			LEVEL_UP = 1,
			[ProtoEnum(Name = "ADD_SKILLPOINTS", Value = 2)]
			ADD_SKILLPOINTS = 2,
			[ProtoEnum(Name = "ADD_ITEM", Value = 3)]
			ADD_ITEM = 3,
			[ProtoEnum(Name = "CLEAR_ALL_ITEMS", Value = 5)]
			CLEAR_ALL_ITEMS = 5,
			[ProtoEnum(Name = "ADD_XP", Value = 7)]
			ADD_XP = 7,
			[ProtoEnum(Name = "ADD_RESOURCES", Value = 10)]
			ADD_RESOURCES = 10,
			[ProtoEnum(Name = "INC_COUNTER", Value = 11)]
			INC_COUNTER = 11,
			[ProtoEnum(Name = "GET_MISSION", Value = 12)]
			GET_MISSION = 12,
			[ProtoEnum(Name = "RELOAD_STATICS", Value = 13)]
			RELOAD_STATICS = 13,
			[ProtoEnum(Name = "INSTANTIATE_LOOT_TEMPLATE", Value = 14)]
			INSTANTIATE_LOOT_TEMPLATE = 14,
			[ProtoEnum(Name = "ADD_FRIEND", Value = 15)]
			ADD_FRIEND = 15,
			[ProtoEnum(Name = "REPOPULATE_MISSIONS", Value = 16)]
			REPOPULATE_MISSIONS = 16,
			[ProtoEnum(Name = "SEND_SYSTEM_MAILS", Value = 17)]
			SEND_SYSTEM_MAILS = 17,
			[ProtoEnum(Name = "SEND_MISSION_MAILS", Value = 18)]
			SEND_MISSION_MAILS = 18,
			[ProtoEnum(Name = "SEND_FRIEND_MAILS", Value = 19)]
			SEND_FRIEND_MAILS = 19,
			[ProtoEnum(Name = "COPY_USER_PROFILE", Value = 20)]
			COPY_USER_PROFILE = 20,
			[ProtoEnum(Name = "RESET_SECOND_CLASS", Value = 21)]
			RESET_SECOND_CLASS = 21,
			[ProtoEnum(Name = "REACH_ACHIEVEMENT", Value = 22)]
			REACH_ACHIEVEMENT = 22,
			[ProtoEnum(Name = "SET_COUNTERS_FOR_ACHIEVEMENT", Value = 23)]
			SET_COUNTERS_FOR_ACHIEVEMENT = 23,
			[ProtoEnum(Name = "UNCLAIM_ACHIEVEMENT", Value = 24)]
			UNCLAIM_ACHIEVEMENT = 24,
			[ProtoEnum(Name = "UNREAD_ACHIEVEMENT", Value = 25)]
			UNREAD_ACHIEVEMENT = 25,
			[ProtoEnum(Name = "UNREACH_ACHIEVEMENT", Value = 26)]
			UNREACH_ACHIEVEMENT = 26,
			[ProtoEnum(Name = "REACH_OR_UNREACH_ALL_ACHIEVEMENTS", Value = 27)]
			REACH_OR_UNREACH_ALL_ACHIEVEMENTS = 27,
			[ProtoEnum(Name = "RESET_ACHIEVEMENT", Value = 28)]
			RESET_ACHIEVEMENT = 28,
			[ProtoEnum(Name = "RESET_DAILY_REWARD", Value = 29)]
			RESET_DAILY_REWARD = 29,
			[ProtoEnum(Name = "GET_BACK_MISSED_DAY", Value = 30)]
			GET_BACK_MISSED_DAY = 30,
			[ProtoEnum(Name = "SET_DAILY_REWARD", Value = 31)]
			SET_DAILY_REWARD = 31,
			[ProtoEnum(Name = "MOVE_DAILY_REWARD_TIMESTAMPS", Value = 32)]
			MOVE_DAILY_REWARD_TIMESTAMPS = 32,
			[ProtoEnum(Name = "MOVE_DAY_CHANGE", Value = 33)]
			MOVE_DAY_CHANGE = 33,
			[ProtoEnum(Name = "ADD_MODULES", Value = 34)]
			ADD_MODULES = 34,
			[ProtoEnum(Name = "DEACTIVATE_MODULE", Value = 35)]
			DEACTIVATE_MODULE = 35,
			[ProtoEnum(Name = "MODULE_SET_REMAINING_TIME", Value = 40)]
			MODULE_SET_REMAINING_TIME = 40,
			[ProtoEnum(Name = "SEND_TEST_PUSH_NOTIFICATION", Value = 42)]
			SEND_TEST_PUSH_NOTIFICATION = 42,
			[ProtoEnum(Name = "ADD_PREDEFINED_ASSASSIN", Value = 43)]
			ADD_PREDEFINED_ASSASSIN = 43,
			[ProtoEnum(Name = "CLEAR_MISSION_HISTORY", Value = 44)]
			CLEAR_MISSION_HISTORY = 44,
			[ProtoEnum(Name = "FULL_ENERGY", Value = 45)]
			FULL_ENERGY = 45,
			[ProtoEnum(Name = "EXTEND_ENERGY", Value = 46)]
			EXTEND_ENERGY = 46,
			[ProtoEnum(Name = "DEPLETE_ENERGY", Value = 47)]
			DEPLETE_ENERGY = 47,
			[ProtoEnum(Name = "GRANT_ENTITLEMENT", Value = 48)]
			GRANT_ENTITLEMENT = 48,
			[ProtoEnum(Name = "REMOVE_ENTITLEMENT", Value = 49)]
			REMOVE_ENTITLEMENT = 49,
			[ProtoEnum(Name = "GRANT_ALL_ENTITLEMENTS", Value = 50)]
			GRANT_ALL_ENTITLEMENTS = 50,
			[ProtoEnum(Name = "MYSTERY_BOX_SET_TIME", Value = 51)]
			MYSTERY_BOX_SET_TIME = 51,
			[ProtoEnum(Name = "PROGRESSION_QUESTS_PERFORM", Value = 52)]
			PROGRESSION_QUESTS_PERFORM = 52,
			[ProtoEnum(Name = "PROGRESSION_QUESTS_EXPIRE", Value = 53)]
			PROGRESSION_QUESTS_EXPIRE = 53,
			[ProtoEnum(Name = "PERFROMANCE_TEST", Value = 54)]
			PERFROMANCE_TEST = 54,
			[ProtoEnum(Name = "CLEAR_TUTORIAL_CATEGORIES", Value = 55)]
			CLEAR_TUTORIAL_CATEGORIES = 55,
			[ProtoEnum(Name = "ADD_BLUEPRINTS", Value = 56)]
			ADD_BLUEPRINTS = 56,
			[ProtoEnum(Name = "REFRESH_EVENTS", Value = 57)]
			REFRESH_EVENTS = 57,
			[ProtoEnum(Name = "RESET_ELITE_MISSION_TIMER", Value = 58)]
			RESET_ELITE_MISSION_TIMER = 58,
			[ProtoEnum(Name = "RESET_PROGRESSION_EVENTS_PROGRESS", Value = 59)]
			RESET_PROGRESSION_EVENTS_PROGRESS = 59,
			[ProtoEnum(Name = "SEND_HIRELING_MAILS", Value = 60)]
			SEND_HIRELING_MAILS = 60,
			[ProtoEnum(Name = "REPOPULATE_HIRELINGS", Value = 61)]
			REPOPULATE_HIRELINGS = 61,
			[ProtoEnum(Name = "REPOPULATE_LEADER_BOARDS", Value = 62)]
			REPOPULATE_LEADER_BOARDS = 62,
			[ProtoEnum(Name = "SET_EVENT_SCORE", Value = 63)]
			SET_EVENT_SCORE = 63,
			[ProtoEnum(Name = "SET_MAX_ITEM_POWER", Value = 64)]
			SET_MAX_ITEM_POWER = 64
		}

		private CheatType _type = CheatType.LEVEL_UP;

		private int _param1;

		private int _param2;

		private string _param3 = "";

		private string _param4 = "";

		private string _param5 = "";

		private string _assassinId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(CheatType.LEVEL_UP)]
		public CheatType type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "param1", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int param1
		{
			get
			{
				return _param1;
			}
			set
			{
				_param1 = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "param2", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int param2
		{
			get
			{
				return _param2;
			}
			set
			{
				_param2 = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "param3", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string param3
		{
			get
			{
				return _param3;
			}
			set
			{
				_param3 = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "param4", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string param4
		{
			get
			{
				return _param4;
			}
			set
			{
				_param4 = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "param5", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string param5
		{
			get
			{
				return _param5;
			}
			set
			{
				_param5 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string assassinId
		{
			get
			{
				return _assassinId;
			}
			set
			{
				_assassinId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
