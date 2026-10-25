using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ErrorCode")]
	public enum ErrorCode
	{
		[ProtoEnum(Name = "INTERNAL_ERROR", Value = 4000)]
		INTERNAL_ERROR = 4000,
		[ProtoEnum(Name = "INVALID_PARAM", Value = 4001)]
		INVALID_PARAM = 4001,
		[ProtoEnum(Name = "USER_NOT_FOUND", Value = 4002)]
		USER_NOT_FOUND = 4002,
		[ProtoEnum(Name = "REQUEST_INVALID", Value = 4004)]
		REQUEST_INVALID = 4004,
		[ProtoEnum(Name = "REQUEST_EMPTY", Value = 4005)]
		REQUEST_EMPTY = 4005,
		[ProtoEnum(Name = "DATA_LOCK_FAILED", Value = 4006)]
		DATA_LOCK_FAILED = 4006,
		[ProtoEnum(Name = "PROTOBUF_VERSION_NOT_MAPPED", Value = 4007)]
		PROTOBUF_VERSION_NOT_MAPPED = 4007,
		[ProtoEnum(Name = "MAINTENANCE_MODE", Value = 4008)]
		MAINTENANCE_MODE = 4008,
		[ProtoEnum(Name = "STATICS_VERSION_OUTDATED", Value = 4009)]
		STATICS_VERSION_OUTDATED = 4009,
		[ProtoEnum(Name = "MISSION_TOKEN_INVALID", Value = 4010)]
		MISSION_TOKEN_INVALID = 4010,
		[ProtoEnum(Name = "MISSION_TOKEN_EXPIRED", Value = 4011)]
		MISSION_TOKEN_EXPIRED = 4011,
		[ProtoEnum(Name = "MISSION_STATS_INVALID", Value = 4012)]
		MISSION_STATS_INVALID = 4012,
		[ProtoEnum(Name = "MISSION_NOT_FOUND", Value = 4013)]
		MISSION_NOT_FOUND = 4013,
		[ProtoEnum(Name = "FAILED_TO_GENERATE_AVATAR_DEFINITION", Value = 4014)]
		FAILED_TO_GENERATE_AVATAR_DEFINITION = 4014,
		[ProtoEnum(Name = "MISSION_LOCATION_LOCKED", Value = 4015)]
		MISSION_LOCATION_LOCKED = 4015,
		[ProtoEnum(Name = "MISSION_NO_BUDGET_MAPPING_FOUND", Value = 4016)]
		MISSION_NO_BUDGET_MAPPING_FOUND = 4016,
		[ProtoEnum(Name = "MISSION_BLACKLISTED", Value = 4017)]
		MISSION_BLACKLISTED = 4017,
		[ProtoEnum(Name = "MISSION_HIRELING_NEEDED", Value = 4018)]
		MISSION_HIRELING_NEEDED = 4018,
		[ProtoEnum(Name = "LOCATION_HAS_NO_STORY_MISSIONS", Value = 4019)]
		LOCATION_HAS_NO_STORY_MISSIONS = 4019,
		[ProtoEnum(Name = "ITEM_ALREADY_EQUIPPED", Value = 4020)]
		ITEM_ALREADY_EQUIPPED = 4020,
		[ProtoEnum(Name = "ITEM_NOT_OWNER", Value = 4021)]
		ITEM_NOT_OWNER = 4021,
		[ProtoEnum(Name = "ITEM_SLOT_NOT_EQUIPPABLE", Value = 4022)]
		ITEM_SLOT_NOT_EQUIPPABLE = 4022,
		[ProtoEnum(Name = "ITEM_NOT_FOUND", Value = 4023)]
		ITEM_NOT_FOUND = 4023,
		[ProtoEnum(Name = "NOT_ENOUGH_RESOURCES", Value = 4024)]
		NOT_ENOUGH_RESOURCES = 4024,
		[ProtoEnum(Name = "INVALID_RESOURCE", Value = 4025)]
		INVALID_RESOURCE = 4025,
		[ProtoEnum(Name = "MULTIPLE_HACK_RESOURCE", Value = 4026)]
		MULTIPLE_HACK_RESOURCE = 4026,
		[ProtoEnum(Name = "ITEM_LEVEL_TOO_HIGH", Value = 4027)]
		ITEM_LEVEL_TOO_HIGH = 4027,
		[ProtoEnum(Name = "ITEM_FULL_INVENTORY", Value = 4028)]
		ITEM_FULL_INVENTORY = 4028,
		[ProtoEnum(Name = "ITEM_CONSUMABLE_NOT_UNEQUIPPABLE", Value = 4029)]
		ITEM_CONSUMABLE_NOT_UNEQUIPPABLE = 4029,
		[ProtoEnum(Name = "TIER_LOCKED", Value = 4030)]
		TIER_LOCKED = 4030,
		[ProtoEnum(Name = "ASSASSIN_NOT_FOUND", Value = 4031)]
		ASSASSIN_NOT_FOUND = 4031,
		[ProtoEnum(Name = "NOT_ENOUGH_SKILLPOINTS", Value = 4032)]
		NOT_ENOUGH_SKILLPOINTS = 4032,
		[ProtoEnum(Name = "ASSASSIN_ALREADY_IN_TRAINING", Value = 4033)]
		ASSASSIN_ALREADY_IN_TRAINING = 4033,
		[ProtoEnum(Name = "ASSASSIN_EXT_CLASS_ALREADY_SET", Value = 4034)]
		ASSASSIN_EXT_CLASS_ALREADY_SET = 4034,
		[ProtoEnum(Name = "ASSASSIN_EXT_CLASS_NOT_SET", Value = 4035)]
		ASSASSIN_EXT_CLASS_NOT_SET = 4035,
		[ProtoEnum(Name = "NOT_ALL_TIERS_UNLOCKED", Value = 4036)]
		NOT_ALL_TIERS_UNLOCKED = 4036,
		[ProtoEnum(Name = "SKILLWHEEL_INVALID", Value = 4037)]
		SKILLWHEEL_INVALID = 4037,
		[ProtoEnum(Name = "SKILL_MAX_LEVEL_REACHED", Value = 4038)]
		SKILL_MAX_LEVEL_REACHED = 4038,
		[ProtoEnum(Name = "AVATAR_CLASS_NOT_FOUND", Value = 4039)]
		AVATAR_CLASS_NOT_FOUND = 4039,
		[ProtoEnum(Name = "INVALID_AVATAR_NAME", Value = 4040)]
		INVALID_AVATAR_NAME = 4040,
		[ProtoEnum(Name = "INVALID_HEAD_GUID", Value = 4041)]
		INVALID_HEAD_GUID = 4041,
		[ProtoEnum(Name = "INVALID_SKILL_CLASS", Value = 4042)]
		INVALID_SKILL_CLASS = 4042,
		[ProtoEnum(Name = "MAX_ASSASSIN_SLOTS_REACHED", Value = 4043)]
		MAX_ASSASSIN_SLOTS_REACHED = 4043,
		[ProtoEnum(Name = "SKILL_MIN_LEVEL_REACHED", Value = 4044)]
		SKILL_MIN_LEVEL_REACHED = 4044,
		[ProtoEnum(Name = "ASSASSIN_COUNT_MISMATCH", Value = 4045)]
		ASSASSIN_COUNT_MISMATCH = 4045,
		[ProtoEnum(Name = "CLASS_LOCKED", Value = 4046)]
		CLASS_LOCKED = 4046,
		[ProtoEnum(Name = "CLASS_NOT_ACCESSIBLE", Value = 4047)]
		CLASS_NOT_ACCESSIBLE = 4047,
		[ProtoEnum(Name = "CLASS_ENTITLEMENTS_NEEDED", Value = 4048)]
		CLASS_ENTITLEMENTS_NEEDED = 4048,
		[ProtoEnum(Name = "ASSASSIN_NOT_IN_TRAINING", Value = 4049)]
		ASSASSIN_NOT_IN_TRAINING = 4049,
		[ProtoEnum(Name = "ASSASSIN_EXT_CLASS_NUMBER_NOT_MAPPED", Value = 4064)]
		ASSASSIN_EXT_CLASS_NUMBER_NOT_MAPPED = 4064,
		[ProtoEnum(Name = "USER_ALREADY_EXISTS", Value = 4050)]
		USER_ALREADY_EXISTS = 4050,
		[ProtoEnum(Name = "ACCOUNT_CREATION_FAILED", Value = 4051)]
		ACCOUNT_CREATION_FAILED = 4051,
		[ProtoEnum(Name = "WRONG_USERID_PASSWORD_COMBINATION", Value = 4052)]
		WRONG_USERID_PASSWORD_COMBINATION = 4052,
		[ProtoEnum(Name = "INVALID_PROFILENAME", Value = 4053)]
		INVALID_PROFILENAME = 4053,
		[ProtoEnum(Name = "INVALID_PROFILENAME_TOO_LONG", Value = 4054)]
		INVALID_PROFILENAME_TOO_LONG = 4054,
		[ProtoEnum(Name = "AUTHENTICATION_FAILED", Value = 4055)]
		AUTHENTICATION_FAILED = 4055,
		[ProtoEnum(Name = "ACCOUNT_DOES_NOT_EXIST", Value = 4056)]
		ACCOUNT_DOES_NOT_EXIST = 4056,
		[ProtoEnum(Name = "USERPROFILE_DOES_NOT_EXIST", Value = 4057)]
		USERPROFILE_DOES_NOT_EXIST = 4057,
		[ProtoEnum(Name = "SESSION_INVALID", Value = 4060)]
		SESSION_INVALID = 4060,
		[ProtoEnum(Name = "SESSION_INVALID_REQUEST_TOKEN", Value = 4061)]
		SESSION_INVALID_REQUEST_TOKEN = 4061,
		[ProtoEnum(Name = "SESSIONS_CREATION_FAILED", Value = 4062)]
		SESSIONS_CREATION_FAILED = 4062,
		[ProtoEnum(Name = "SESSION_OTHER_SESSION_EXISTS", Value = 4063)]
		SESSION_OTHER_SESSION_EXISTS = 4063,
		[ProtoEnum(Name = "MAIL_HEADER_NOT_FOUND", Value = 4100)]
		MAIL_HEADER_NOT_FOUND = 4100,
		[ProtoEnum(Name = "MAIL_BODY_NOT_FOUND", Value = 4101)]
		MAIL_BODY_NOT_FOUND = 4101,
		[ProtoEnum(Name = "MAIL_ALREADY_FRIENDS", Value = 4102)]
		MAIL_ALREADY_FRIENDS = 4102,
		[ProtoEnum(Name = "MAIL_SUBJECT_CHARACTER_LIMIT_EXCEEDED", Value = 4103)]
		MAIL_SUBJECT_CHARACTER_LIMIT_EXCEEDED = 4103,
		[ProtoEnum(Name = "MAIL_BODY_CHARACTER_LIMIT_EXCEEDED", Value = 4104)]
		MAIL_BODY_CHARACTER_LIMIT_EXCEEDED = 4104,
		[ProtoEnum(Name = "MAIL_CANNOT_REJECT_OWN_GIFT", Value = 4105)]
		MAIL_CANNOT_REJECT_OWN_GIFT = 4105,
		[ProtoEnum(Name = "MAIL_RECEIVER_NOT_FRIEND", Value = 4106)]
		MAIL_RECEIVER_NOT_FRIEND = 4106,
		[ProtoEnum(Name = "MAIL_SENDER_ALREADY_CANCELED_REQUEST", Value = 4107)]
		MAIL_SENDER_ALREADY_CANCELED_REQUEST = 4107,
		[ProtoEnum(Name = "MAIL_HAS_NO_ATTACHMENT", Value = 4108)]
		MAIL_HAS_NO_ATTACHMENT = 4108,
		[ProtoEnum(Name = "ACHIEVEMENT_NOT_REACHED", Value = 4120)]
		ACHIEVEMENT_NOT_REACHED = 4120,
		[ProtoEnum(Name = "ACHIEVEMENT_RANK_NOT_REACHED", Value = 4121)]
		ACHIEVEMENT_RANK_NOT_REACHED = 4121,
		[ProtoEnum(Name = "ACHIEVEMENT_ALREADY_CLAIMED", Value = 4122)]
		ACHIEVEMENT_ALREADY_CLAIMED = 4122,
		[ProtoEnum(Name = "ACHIEVEMENT_NOT_EXISTING", Value = 4123)]
		ACHIEVEMENT_NOT_EXISTING = 4123,
		[ProtoEnum(Name = "DAILY_REWARD_NO_REWARD_CALCULATED", Value = 4140)]
		DAILY_REWARD_NO_REWARD_CALCULATED = 4140,
		[ProtoEnum(Name = "DAILY_REWARD_ALREADY_HANDLED_TODAY", Value = 4141)]
		DAILY_REWARD_ALREADY_HANDLED_TODAY = 4141,
		[ProtoEnum(Name = "DAILY_REWARD_CANNOT_CONTINUE_ON_LAST_DAY", Value = 4142)]
		DAILY_REWARD_CANNOT_CONTINUE_ON_LAST_DAY = 4142,
		[ProtoEnum(Name = "DAILY_REWARD_CANNOT_HANDLE_INTERRUPTED_CHAIN", Value = 4143)]
		DAILY_REWARD_CANNOT_HANDLE_INTERRUPTED_CHAIN = 4143,
		[ProtoEnum(Name = "DAILY_REWARD_CANNOT_BUY_BACK", Value = 4144)]
		DAILY_REWARD_CANNOT_BUY_BACK = 4144,
		[ProtoEnum(Name = "DAILY_REWARD_MISSED_DAYS_COUNT_CHANGED", Value = 4145)]
		DAILY_REWARD_MISSED_DAYS_COUNT_CHANGED = 4145,
		[ProtoEnum(Name = "MISSION_CANNOT_REFRESH_MISSION_LIST", Value = 4180)]
		MISSION_CANNOT_REFRESH_MISSION_LIST = 4180,
		[ProtoEnum(Name = "MISSION_REVIVE_FEATURE_DISABLED", Value = 4181)]
		MISSION_REVIVE_FEATURE_DISABLED = 4181,
		[ProtoEnum(Name = "MISSION_ENTITLEMENTS_NEEDED", Value = 4182)]
		MISSION_ENTITLEMENTS_NEEDED = 4182,
		[ProtoEnum(Name = "MYSTERY_BOX_CHAIN_NOT_FOUND", Value = 4200)]
		MYSTERY_BOX_CHAIN_NOT_FOUND = 4200,
		[ProtoEnum(Name = "MYSTERY_BOX_CHAIN_EXPIRED", Value = 4201)]
		MYSTERY_BOX_CHAIN_EXPIRED = 4201,
		[ProtoEnum(Name = "MYSTERY_BOX_NOT_FOUND", Value = 4202)]
		MYSTERY_BOX_NOT_FOUND = 4202,
		[ProtoEnum(Name = "ITEM_ONLY_ITEMS_CAN_BE_SOLD", Value = 4220)]
		ITEM_ONLY_ITEMS_CAN_BE_SOLD = 4220,
		[ProtoEnum(Name = "FORGE_ONLY_ONE_BLUEPRINT_ALLOWED", Value = 4240)]
		FORGE_ONLY_ONE_BLUEPRINT_ALLOWED = 4240,
		[ProtoEnum(Name = "FORGE_TOO_MUCH_SLOTS", Value = 4241)]
		FORGE_TOO_MUCH_SLOTS = 4241,
		[ProtoEnum(Name = "FORGE_QUALITIES_DO_NOT_MATCH", Value = 4242)]
		FORGE_QUALITIES_DO_NOT_MATCH = 4242,
		[ProtoEnum(Name = "FORGE_ONLY_ITEMS_AND_BLUEPRINT_ALLOWED", Value = 4243)]
		FORGE_ONLY_ITEMS_AND_BLUEPRINT_ALLOWED = 4243,
		[ProtoEnum(Name = "FORGE_NO_OR_INVALID_DATA_FOUND_FOR_REROLL", Value = 4244)]
		FORGE_NO_OR_INVALID_DATA_FOUND_FOR_REROLL = 4244,
		[ProtoEnum(Name = "FORGE_NEED_AT_LEAST_ONE_ITEM", Value = 4245)]
		FORGE_NEED_AT_LEAST_ONE_ITEM = 4245,
		[ProtoEnum(Name = "MAXIMUM_ENERGY_ALLOWANCE", Value = 4301)]
		MAXIMUM_ENERGY_ALLOWANCE = 4301,
		[ProtoEnum(Name = "INSUFFICIENT_FUNDS", Value = 4500)]
		INSUFFICIENT_FUNDS = 4500,
		[ProtoEnum(Name = "SHOP_ITEM_NOT_FOUND", Value = 4501)]
		SHOP_ITEM_NOT_FOUND = 4501,
		[ProtoEnum(Name = "NO_SHOP_DATA_AVAILABLE", Value = 4502)]
		NO_SHOP_DATA_AVAILABLE = 4502,
		[ProtoEnum(Name = "NO_ITEM_BUNDLES_AVAILABLE", Value = 4503)]
		NO_ITEM_BUNDLES_AVAILABLE = 4503,
		[ProtoEnum(Name = "INVALID_SHOP_ITEM_ID", Value = 4504)]
		INVALID_SHOP_ITEM_ID = 4504,
		[ProtoEnum(Name = "SLOTS_AMOUNT_MISMATCH", Value = 4505)]
		SLOTS_AMOUNT_MISMATCH = 4505,
		[ProtoEnum(Name = "LOCATION_ALREADY_ACCESSIBLE", Value = 4506)]
		LOCATION_ALREADY_ACCESSIBLE = 4506,
		[ProtoEnum(Name = "LOCATION_NOT_PURCHASEABLE", Value = 4507)]
		LOCATION_NOT_PURCHASEABLE = 4507,
		[ProtoEnum(Name = "INVENTORY_SLOTS_MAXIMUM_REACHED", Value = 4508)]
		INVENTORY_SLOTS_MAXIMUM_REACHED = 4508,
		[ProtoEnum(Name = "SHOP_ENTITLEMENTS_NEEDED", Value = 4509)]
		SHOP_ENTITLEMENTS_NEEDED = 4509,
		[ProtoEnum(Name = "FRIEND_NOT_FOUND", Value = 4900)]
		FRIEND_NOT_FOUND = 4900,
		[ProtoEnum(Name = "REACHED_MAX_FRIEND_COUNT", Value = 4901)]
		REACHED_MAX_FRIEND_COUNT = 4901,
		[ProtoEnum(Name = "OTHER_REACHED_MAX_FRIEND_COUNT", Value = 4902)]
		OTHER_REACHED_MAX_FRIEND_COUNT = 4902,
		[ProtoEnum(Name = "ENTITLEMENT_NEEDED", Value = 4950)]
		ENTITLEMENT_NEEDED = 4950,
		[ProtoEnum(Name = "UNCLAIMABLE_QUESTS", Value = 4331)]
		UNCLAIMABLE_QUESTS = 4331,
		[ProtoEnum(Name = "NO_QUEST_IN_PROGRESS", Value = 4332)]
		NO_QUEST_IN_PROGRESS = 4332,
		[ProtoEnum(Name = "NO_ENOUGH_STRATEGIES_TO_REROLL", Value = 4333)]
		NO_ENOUGH_STRATEGIES_TO_REROLL = 4333,
		[ProtoEnum(Name = "HC_SHOP_INVALID_PARAMATERS", Value = 5000)]
		HC_SHOP_INVALID_PARAMATERS = 5000,
		[ProtoEnum(Name = "HC_SHOP_ERROR", Value = 5001)]
		HC_SHOP_ERROR = 5001,
		[ProtoEnum(Name = "HC_SHOP_ITUNES_CONNECTION_FAILED", Value = 5002)]
		HC_SHOP_ITUNES_CONNECTION_FAILED = 5002,
		[ProtoEnum(Name = "HC_SHOP_NOT_SUPPORTED_ON_THIS_PLATFORM", Value = 5003)]
		HC_SHOP_NOT_SUPPORTED_ON_THIS_PLATFORM = 5003,
		[ProtoEnum(Name = "HC_SHOP_IAP_DISABLED", Value = 5004)]
		HC_SHOP_IAP_DISABLED = 5004,
		[ProtoEnum(Name = "HC_SHOP_CLIENT_INVALID", Value = 5008)]
		HC_SHOP_CLIENT_INVALID = 5008,
		[ProtoEnum(Name = "HC_SHOP_PAYMENT_CANCELLED", Value = 5009)]
		HC_SHOP_PAYMENT_CANCELLED = 5009,
		[ProtoEnum(Name = "HC_SHOP_PAYMENT_INVALID", Value = 5010)]
		HC_SHOP_PAYMENT_INVALID = 5010,
		[ProtoEnum(Name = "HC_SHOP_PAYMENT_NOT_ALLOWED", Value = 5011)]
		HC_SHOP_PAYMENT_NOT_ALLOWED = 5011,
		[ProtoEnum(Name = "HC_SHOP_PRODUCT_UNAVAILABLE", Value = 5012)]
		HC_SHOP_PRODUCT_UNAVAILABLE = 5012,
		[ProtoEnum(Name = "HC_SHOP_RECEIPT_ERROR", Value = 5013)]
		HC_SHOP_RECEIPT_ERROR = 5013,
		[ProtoEnum(Name = "HC_SHOP_USER_CANCELLED", Value = 5014)]
		HC_SHOP_USER_CANCELLED = 5014,
		[ProtoEnum(Name = "HC_SHOP_SERVICE_UNAVAILABLE", Value = 5015)]
		HC_SHOP_SERVICE_UNAVAILABLE = 5015,
		[ProtoEnum(Name = "HC_SHOP_ITEM_NOT_OWNED", Value = 5016)]
		HC_SHOP_ITEM_NOT_OWNED = 5016,
		[ProtoEnum(Name = "HC_SHOP_ACTIVITY_NOT_FOUND", Value = 5017)]
		HC_SHOP_ACTIVITY_NOT_FOUND = 5017,
		[ProtoEnum(Name = "HC_SHOP_BINDING_FAILED", Value = 5018)]
		HC_SHOP_BINDING_FAILED = 5018,
		[ProtoEnum(Name = "HC_SHOP_BILLING_UNAVAILABLE", Value = 5019)]
		HC_SHOP_BILLING_UNAVAILABLE = 5019,
		[ProtoEnum(Name = "HC_SHOP_PRODUCTS_REQUEST_FAILED", Value = 5020)]
		HC_SHOP_PRODUCTS_REQUEST_FAILED = 5020,
		[ProtoEnum(Name = "HC_SHOP_PRODUCTS_REQUEST_EMPTY", Value = 5021)]
		HC_SHOP_PRODUCTS_REQUEST_EMPTY = 5021,
		[ProtoEnum(Name = "HC_SHOP_PURCHASE_FAILED", Value = 5022)]
		HC_SHOP_PURCHASE_FAILED = 5022,
		[ProtoEnum(Name = "HC_SHOP_PURCHASE_CANCELLED", Value = 5023)]
		HC_SHOP_PURCHASE_CANCELLED = 5023,
		[ProtoEnum(Name = "HC_SHOP_CONSUME_FAILED", Value = 5024)]
		HC_SHOP_CONSUME_FAILED = 5024,
		[ProtoEnum(Name = "HC_SHOP_RECOVER_FAILED", Value = 5025)]
		HC_SHOP_RECOVER_FAILED = 5025,
		[ProtoEnum(Name = "HC_SHOP_ITEM_ALREADY_OWNED", Value = 5026)]
		HC_SHOP_ITEM_ALREADY_OWNED = 5026,
		[ProtoEnum(Name = "UnknownCode", Value = 0)]
		UnknownCode = 0,
		[ProtoEnum(Name = "URIMisformated", Value = 2)]
		URIMisformated = 2,
		[ProtoEnum(Name = "UnableToRequest", Value = 3)]
		UnableToRequest = 3,
		[ProtoEnum(Name = "WESCacheEntryNotFound", Value = 1000)]
		WESCacheEntryNotFound = 1000,
		[ProtoEnum(Name = "WESConnectFailure", Value = 1001)]
		WESConnectFailure = 1001,
		[ProtoEnum(Name = "WESConnectionClosed", Value = 1002)]
		WESConnectionClosed = 1002,
		[ProtoEnum(Name = "WESKeepAliveFailure", Value = 1003)]
		WESKeepAliveFailure = 1003,
		[ProtoEnum(Name = "WESMessageLengthLimitExceeded", Value = 1004)]
		WESMessageLengthLimitExceeded = 1004,
		[ProtoEnum(Name = "WESNameResolutionFailure", Value = 1005)]
		WESNameResolutionFailure = 1005,
		[ProtoEnum(Name = "WESPending", Value = 1006)]
		WESPending = 1006,
		[ProtoEnum(Name = "WESPipelineFailure", Value = 1007)]
		WESPipelineFailure = 1007,
		[ProtoEnum(Name = "WESProtocolError", Value = 1008)]
		WESProtocolError = 1008,
		[ProtoEnum(Name = "WESProxyNameResolutionFailure", Value = 1009)]
		WESProxyNameResolutionFailure = 1009,
		[ProtoEnum(Name = "WESReceiveFailure", Value = 1010)]
		WESReceiveFailure = 1010,
		[ProtoEnum(Name = "WESRequestCanceled", Value = 1011)]
		WESRequestCanceled = 1011,
		[ProtoEnum(Name = "WESRequestProhibitedByCachePolicy", Value = 1012)]
		WESRequestProhibitedByCachePolicy = 1012,
		[ProtoEnum(Name = "WESRequestProhibitedByProxy", Value = 1013)]
		WESRequestProhibitedByProxy = 1013,
		[ProtoEnum(Name = "WESSecureChannelFailure", Value = 1014)]
		WESSecureChannelFailure = 1014,
		[ProtoEnum(Name = "WESSendFailure", Value = 1015)]
		WESSendFailure = 1015,
		[ProtoEnum(Name = "WESServerProtocolViolation", Value = 1016)]
		WESServerProtocolViolation = 1016,
		[ProtoEnum(Name = "WESTimeout", Value = 1017)]
		WESTimeout = 1017,
		[ProtoEnum(Name = "WESTrustFailure", Value = 1018)]
		WESTrustFailure = 1018,
		[ProtoEnum(Name = "WESUnknownError", Value = 1019)]
		WESUnknownError = 1019,
		[ProtoEnum(Name = "WESSignatureVerificationFailure", Value = 1020)]
		WESSignatureVerificationFailure = 1020,
		[ProtoEnum(Name = "ACClientOutdated", Value = 3000)]
		ACClientOutdated = 3000,
		[ProtoEnum(Name = "SesssionTimeout", Value = 6001)]
		SesssionTimeout = 6001,
		[ProtoEnum(Name = "SesssionNotOpen", Value = 6002)]
		SesssionNotOpen = 6002,
		[ProtoEnum(Name = "DiscoveryServiceUnreachable", Value = 6003)]
		DiscoveryServiceUnreachable = 6003,
		[ProtoEnum(Name = "PlayerProfileBroken", Value = 6004)]
		PlayerProfileBroken = 6004,
		[ProtoEnum(Name = "StartupDataFailed", Value = 6005)]
		StartupDataFailed = 6005,
		[ProtoEnum(Name = "BundleDownloadFailed", Value = 6006)]
		BundleDownloadFailed = 6006,
		[ProtoEnum(Name = "BundleDownloadCRCFailed", Value = 6007)]
		BundleDownloadCRCFailed = 6007,
		[ProtoEnum(Name = "NoNetworkConnection", Value = 6008)]
		NoNetworkConnection = 6008,
		[ProtoEnum(Name = "NoInternetAccess", Value = 6009)]
		NoInternetAccess = 6009,
		[ProtoEnum(Name = "NotEnoughDiskspace", Value = 6010)]
		NotEnoughDiskspace = 6010,
		[ProtoEnum(Name = "NotEnoughCacheSpace", Value = 6011)]
		NotEnoughCacheSpace = 6011,
		[ProtoEnum(Name = "NetworkProtocolVersionMismatch", Value = 6012)]
		NetworkProtocolVersionMismatch = 6012,
		[ProtoEnum(Name = "RegionNotAvailable", Value = 6013)]
		RegionNotAvailable = 6013,
		[ProtoEnum(Name = "UPLAY_START_FAILED", Value = 7001)]
		UPLAY_START_FAILED = 7001,
		[ProtoEnum(Name = "UPLAY_LOGIN_FAILED", Value = 7002)]
		UPLAY_LOGIN_FAILED = 7002,
		[ProtoEnum(Name = "UPLAY_USER_ALREADY_EXIST", Value = 7003)]
		UPLAY_USER_ALREADY_EXIST = 7003,
		[ProtoEnum(Name = "UPLAY_USER_NOT_FOUND", Value = 7004)]
		UPLAY_USER_NOT_FOUND = 7004,
		[ProtoEnum(Name = "FACEBOOK_START_FAILED", Value = 8001)]
		FACEBOOK_START_FAILED = 8001,
		[ProtoEnum(Name = "FACEBOOK_LOGIN_FAILED", Value = 8002)]
		FACEBOOK_LOGIN_FAILED = 8002,
		[ProtoEnum(Name = "FACEBOOK_USER_ALREADY_EXIST", Value = 8003)]
		FACEBOOK_USER_ALREADY_EXIST = 8003,
		[ProtoEnum(Name = "FACEBOOK_USER_NOT_FOUND", Value = 8004)]
		FACEBOOK_USER_NOT_FOUND = 8004,
		[ProtoEnum(Name = "FACEBOOK_CANNOT_OPEN_URL", Value = 8005)]
		FACEBOOK_CANNOT_OPEN_URL = 8005,
		[ProtoEnum(Name = "ADCOLONY_ZONE_UNAVAILABLE", Value = 9001)]
		ADCOLONY_ZONE_UNAVAILABLE = 9001,
		[ProtoEnum(Name = "ADCOLONY_VIDEO_NOT_READY", Value = 9002)]
		ADCOLONY_VIDEO_NOT_READY = 9002,
		[ProtoEnum(Name = "ADCOLONY_VIDEO_LIMIT_REACHED", Value = 9003)]
		ADCOLONY_VIDEO_LIMIT_REACHED = 9003,
		[ProtoEnum(Name = "ADCOLONY_V4VC_TRANSACTION_FAILED", Value = 9004)]
		ADCOLONY_V4VC_TRANSACTION_FAILED = 9004,
		[ProtoEnum(Name = "ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED", Value = 9005)]
		ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED = 9005,
		[ProtoEnum(Name = "LICENSE_CHECK_FAILED_CANNOT_CONNECT", Value = 9101)]
		LICENSE_CHECK_FAILED_CANNOT_CONNECT = 9101,
		[ProtoEnum(Name = "LICENSE_CHECK_FAILED_GO_TO_STORE", Value = 9102)]
		LICENSE_CHECK_FAILED_GO_TO_STORE = 9102,
		[ProtoEnum(Name = "LICENSE_CHECK_FAILED_ERROR_INVALID_PACKAGE_NAME", Value = 9103)]
		LICENSE_CHECK_FAILED_ERROR_INVALID_PACKAGE_NAME = 9103,
		[ProtoEnum(Name = "LICENSE_CHECK_FAILED_ERROR_NOT_MATCHING_UID", Value = 9104)]
		LICENSE_CHECK_FAILED_ERROR_NOT_MATCHING_UID = 9104,
		[ProtoEnum(Name = "LICENSE_CHECK_FAILED_ERROR_NOT_MARKET_MANAGED", Value = 9105)]
		LICENSE_CHECK_FAILED_ERROR_NOT_MARKET_MANAGED = 9105
	}
}
