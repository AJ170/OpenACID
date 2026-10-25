using System;
using AcierProtocol;

namespace Assets.Scripts.Network.Error
{
	public class Error
	{
		public enum DataStoreRetryResult
		{
			None = 0,
			Reconnect = 1,
			UpdateProfile = 2
		}

		private readonly ErrorState mErrorState;

		public static Error DEFAULT = new Error(null);

		private static bool g_initialized = false;

		public ErrorState ErrorState
		{
			get
			{
				return mErrorState;
			}
		}

		public ErrorCode ErrorCode
		{
			get
			{
				return mErrorState.ErrorCode;
			}
		}

		public ErrorCategory ErrorCategory
		{
			get
			{
				return mErrorState.ErrorGroup;
			}
		}

		public bool ShouldNotfiyUser { get; private set; }

		public int UserMessage { get; private set; }

		public DataStoreRetryResult DataStoreResult { get; private set; }

		public bool PotentialConnectionIssue { get; private set; }

		public Error(ErrorState _errorState, int _userMessage = -1)
		{
			mErrorState = _errorState ?? new ErrorState();
			UserMessage = _userMessage;
			if (!g_initialized)
			{
				foreach (int value in Enum.GetValues(typeof(ErrorCode)))
				{
					HandleErrorCode((ErrorCode)value);
				}
				g_initialized = true;
			}
			HandleErrorCode(mErrorState.ErrorCode);
		}

		public Error(ErrorCode _errorCode, ErrorCategory _errorCategory, int _userMessage = -1)
			: this(new ErrorState
			{
				ErrorCode = _errorCode,
				ErrorGroup = _errorCategory
			}, _userMessage)
		{
		}

		private void HandleErrorCode(ErrorCode _errorCode)
		{
			ErrorCode errorCode = _errorCode;
			switch (errorCode)
			{
			default:
				switch (errorCode)
				{
				case ErrorCode.UnknownCode:
				case ErrorCode.URIMisformated:
				case ErrorCode.UnableToRequest:
				case ErrorCode.WESTimeout:
				case ErrorCode.ACClientOutdated:
				case ErrorCode.MISSION_CANNOT_REFRESH_MISSION_LIST:
				case ErrorCode.MISSION_REVIVE_FEATURE_DISABLED:
				case ErrorCode.MISSION_ENTITLEMENTS_NEEDED:
				case ErrorCode.MYSTERY_BOX_CHAIN_NOT_FOUND:
				case ErrorCode.MYSTERY_BOX_CHAIN_EXPIRED:
				case ErrorCode.MYSTERY_BOX_NOT_FOUND:
				case ErrorCode.ITEM_ONLY_ITEMS_CAN_BE_SOLD:
				case ErrorCode.FORGE_ONLY_ONE_BLUEPRINT_ALLOWED:
				case ErrorCode.FORGE_TOO_MUCH_SLOTS:
				case ErrorCode.FORGE_QUALITIES_DO_NOT_MATCH:
				case ErrorCode.FORGE_ONLY_ITEMS_AND_BLUEPRINT_ALLOWED:
				case ErrorCode.FORGE_NO_OR_INVALID_DATA_FOUND_FOR_REROLL:
				case ErrorCode.FORGE_NEED_AT_LEAST_ONE_ITEM:
				case ErrorCode.MAXIMUM_ENERGY_ALLOWANCE:
				case ErrorCode.UNCLAIMABLE_QUESTS:
				case ErrorCode.NO_QUEST_IN_PROGRESS:
				case ErrorCode.NO_ENOUGH_STRATEGIES_TO_REROLL:
				case ErrorCode.INSUFFICIENT_FUNDS:
				case ErrorCode.SHOP_ITEM_NOT_FOUND:
				case ErrorCode.INVALID_SHOP_ITEM_ID:
				case ErrorCode.SLOTS_AMOUNT_MISMATCH:
				case ErrorCode.LOCATION_ALREADY_ACCESSIBLE:
				case ErrorCode.LOCATION_NOT_PURCHASEABLE:
				case ErrorCode.INVENTORY_SLOTS_MAXIMUM_REACHED:
				case ErrorCode.SHOP_ENTITLEMENTS_NEEDED:
				case ErrorCode.FRIEND_NOT_FOUND:
				case ErrorCode.REACHED_MAX_FRIEND_COUNT:
				case ErrorCode.OTHER_REACHED_MAX_FRIEND_COUNT:
				case ErrorCode.ENTITLEMENT_NEEDED:
				case ErrorCode.HC_SHOP_ITUNES_CONNECTION_FAILED:
				case ErrorCode.HC_SHOP_NOT_SUPPORTED_ON_THIS_PLATFORM:
				case ErrorCode.HC_SHOP_IAP_DISABLED:
				case ErrorCode.HC_SHOP_CLIENT_INVALID:
				case ErrorCode.HC_SHOP_PAYMENT_CANCELLED:
				case ErrorCode.HC_SHOP_USER_CANCELLED:
				case ErrorCode.HC_SHOP_SERVICE_UNAVAILABLE:
				case ErrorCode.HC_SHOP_ITEM_NOT_OWNED:
				case ErrorCode.HC_SHOP_BINDING_FAILED:
				case ErrorCode.HC_SHOP_BILLING_UNAVAILABLE:
				case ErrorCode.HC_SHOP_PRODUCTS_REQUEST_FAILED:
				case ErrorCode.HC_SHOP_PURCHASE_FAILED:
				case ErrorCode.HC_SHOP_PURCHASE_CANCELLED:
				case ErrorCode.HC_SHOP_CONSUME_FAILED:
				case ErrorCode.HC_SHOP_RECOVER_FAILED:
				case ErrorCode.HC_SHOP_ITEM_ALREADY_OWNED:
				case ErrorCode.DiscoveryServiceUnreachable:
				case ErrorCode.PlayerProfileBroken:
				case ErrorCode.StartupDataFailed:
				case ErrorCode.BundleDownloadFailed:
				case ErrorCode.BundleDownloadCRCFailed:
				case ErrorCode.NoNetworkConnection:
				case ErrorCode.NoInternetAccess:
				case ErrorCode.NotEnoughDiskspace:
				case ErrorCode.NotEnoughCacheSpace:
				case ErrorCode.NetworkProtocolVersionMismatch:
				case ErrorCode.RegionNotAvailable:
				case ErrorCode.UPLAY_START_FAILED:
				case ErrorCode.UPLAY_LOGIN_FAILED:
				case ErrorCode.UPLAY_USER_ALREADY_EXIST:
				case ErrorCode.UPLAY_USER_NOT_FOUND:
				case ErrorCode.FACEBOOK_START_FAILED:
				case ErrorCode.FACEBOOK_LOGIN_FAILED:
				case ErrorCode.FACEBOOK_USER_ALREADY_EXIST:
				case ErrorCode.FACEBOOK_USER_NOT_FOUND:
				case ErrorCode.FACEBOOK_CANNOT_OPEN_URL:
				case ErrorCode.ADCOLONY_ZONE_UNAVAILABLE:
				case ErrorCode.ADCOLONY_VIDEO_NOT_READY:
				case ErrorCode.ADCOLONY_VIDEO_LIMIT_REACHED:
				case ErrorCode.ADCOLONY_V4VC_TRANSACTION_FAILED:
				case ErrorCode.ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED:
				case ErrorCode.LICENSE_CHECK_FAILED_CANNOT_CONNECT:
				case ErrorCode.LICENSE_CHECK_FAILED_GO_TO_STORE:
				case ErrorCode.LICENSE_CHECK_FAILED_ERROR_INVALID_PACKAGE_NAME:
				case ErrorCode.LICENSE_CHECK_FAILED_ERROR_NOT_MATCHING_UID:
				case ErrorCode.LICENSE_CHECK_FAILED_ERROR_NOT_MARKET_MANAGED:
					break;
				case ErrorCode.WESCacheEntryNotFound:
				case ErrorCode.WESConnectFailure:
				case ErrorCode.WESConnectionClosed:
				case ErrorCode.WESKeepAliveFailure:
				case ErrorCode.WESMessageLengthLimitExceeded:
				case ErrorCode.WESNameResolutionFailure:
				case ErrorCode.WESPending:
				case ErrorCode.WESPipelineFailure:
				case ErrorCode.WESProtocolError:
				case ErrorCode.WESProxyNameResolutionFailure:
				case ErrorCode.WESReceiveFailure:
				case ErrorCode.WESRequestCanceled:
				case ErrorCode.WESRequestProhibitedByCachePolicy:
				case ErrorCode.WESRequestProhibitedByProxy:
				case ErrorCode.WESSecureChannelFailure:
				case ErrorCode.WESSendFailure:
				case ErrorCode.WESServerProtocolViolation:
				case ErrorCode.WESTrustFailure:
				case ErrorCode.WESUnknownError:
				case ErrorCode.WESSignatureVerificationFailure:
				case ErrorCode.NO_SHOP_DATA_AVAILABLE:
				case ErrorCode.NO_ITEM_BUNDLES_AVAILABLE:
				case ErrorCode.HC_SHOP_INVALID_PARAMATERS:
				case ErrorCode.HC_SHOP_ERROR:
				case ErrorCode.HC_SHOP_PAYMENT_INVALID:
				case ErrorCode.HC_SHOP_PAYMENT_NOT_ALLOWED:
				case ErrorCode.HC_SHOP_PRODUCT_UNAVAILABLE:
				case ErrorCode.HC_SHOP_RECEIPT_ERROR:
				case ErrorCode.HC_SHOP_ACTIVITY_NOT_FOUND:
				case ErrorCode.HC_SHOP_PRODUCTS_REQUEST_EMPTY:
				case ErrorCode.SesssionTimeout:
				case ErrorCode.SesssionNotOpen:
					goto IL_04f5;
				default:
					goto end_IL_0009;
				}
				goto case ErrorCode.INTERNAL_ERROR;
			case ErrorCode.INTERNAL_ERROR:
			case ErrorCode.INVALID_PARAM:
			case ErrorCode.USER_NOT_FOUND:
			case ErrorCode.REQUEST_INVALID:
			case ErrorCode.REQUEST_EMPTY:
			case ErrorCode.DATA_LOCK_FAILED:
			case ErrorCode.PROTOBUF_VERSION_NOT_MAPPED:
			case ErrorCode.MAINTENANCE_MODE:
			case ErrorCode.STATICS_VERSION_OUTDATED:
			case ErrorCode.MISSION_TOKEN_INVALID:
			case ErrorCode.MISSION_TOKEN_EXPIRED:
			case ErrorCode.MISSION_STATS_INVALID:
			case ErrorCode.MISSION_NOT_FOUND:
			case ErrorCode.FAILED_TO_GENERATE_AVATAR_DEFINITION:
			case ErrorCode.MISSION_LOCATION_LOCKED:
			case ErrorCode.MISSION_NO_BUDGET_MAPPING_FOUND:
			case ErrorCode.MISSION_BLACKLISTED:
			case ErrorCode.MISSION_HIRELING_NEEDED:
			case ErrorCode.LOCATION_HAS_NO_STORY_MISSIONS:
			case ErrorCode.ITEM_ALREADY_EQUIPPED:
			case ErrorCode.ITEM_NOT_OWNER:
			case ErrorCode.ITEM_SLOT_NOT_EQUIPPABLE:
			case ErrorCode.ITEM_NOT_FOUND:
			case ErrorCode.NOT_ENOUGH_RESOURCES:
			case ErrorCode.INVALID_RESOURCE:
			case ErrorCode.MULTIPLE_HACK_RESOURCE:
			case ErrorCode.ITEM_LEVEL_TOO_HIGH:
			case ErrorCode.ITEM_FULL_INVENTORY:
			case ErrorCode.ITEM_CONSUMABLE_NOT_UNEQUIPPABLE:
			case ErrorCode.TIER_LOCKED:
			case ErrorCode.ASSASSIN_NOT_FOUND:
			case ErrorCode.NOT_ENOUGH_SKILLPOINTS:
			case ErrorCode.ASSASSIN_ALREADY_IN_TRAINING:
			case ErrorCode.ASSASSIN_EXT_CLASS_ALREADY_SET:
			case ErrorCode.ASSASSIN_EXT_CLASS_NOT_SET:
			case ErrorCode.NOT_ALL_TIERS_UNLOCKED:
			case ErrorCode.SKILLWHEEL_INVALID:
			case ErrorCode.SKILL_MAX_LEVEL_REACHED:
			case ErrorCode.AVATAR_CLASS_NOT_FOUND:
			case ErrorCode.INVALID_AVATAR_NAME:
			case ErrorCode.INVALID_HEAD_GUID:
			case ErrorCode.INVALID_SKILL_CLASS:
			case ErrorCode.MAX_ASSASSIN_SLOTS_REACHED:
			case ErrorCode.SKILL_MIN_LEVEL_REACHED:
			case ErrorCode.ASSASSIN_COUNT_MISMATCH:
			case ErrorCode.CLASS_LOCKED:
			case ErrorCode.CLASS_NOT_ACCESSIBLE:
			case ErrorCode.CLASS_ENTITLEMENTS_NEEDED:
			case ErrorCode.ASSASSIN_NOT_IN_TRAINING:
			case ErrorCode.USER_ALREADY_EXISTS:
			case ErrorCode.ACCOUNT_CREATION_FAILED:
			case ErrorCode.WRONG_USERID_PASSWORD_COMBINATION:
			case ErrorCode.INVALID_PROFILENAME:
			case ErrorCode.INVALID_PROFILENAME_TOO_LONG:
			case ErrorCode.AUTHENTICATION_FAILED:
			case ErrorCode.ACCOUNT_DOES_NOT_EXIST:
			case ErrorCode.USERPROFILE_DOES_NOT_EXIST:
			case ErrorCode.SESSIONS_CREATION_FAILED:
			case ErrorCode.SESSION_OTHER_SESSION_EXISTS:
			case ErrorCode.ASSASSIN_EXT_CLASS_NUMBER_NOT_MAPPED:
			case ErrorCode.MAIL_HEADER_NOT_FOUND:
			case ErrorCode.MAIL_BODY_NOT_FOUND:
			case ErrorCode.MAIL_ALREADY_FRIENDS:
			case ErrorCode.MAIL_SUBJECT_CHARACTER_LIMIT_EXCEEDED:
			case ErrorCode.MAIL_BODY_CHARACTER_LIMIT_EXCEEDED:
			case ErrorCode.MAIL_CANNOT_REJECT_OWN_GIFT:
			case ErrorCode.MAIL_RECEIVER_NOT_FRIEND:
			case ErrorCode.MAIL_SENDER_ALREADY_CANCELED_REQUEST:
			case ErrorCode.MAIL_HAS_NO_ATTACHMENT:
			case ErrorCode.ACHIEVEMENT_NOT_REACHED:
			case ErrorCode.ACHIEVEMENT_RANK_NOT_REACHED:
			case ErrorCode.ACHIEVEMENT_ALREADY_CLAIMED:
			case ErrorCode.ACHIEVEMENT_NOT_EXISTING:
			case ErrorCode.DAILY_REWARD_NO_REWARD_CALCULATED:
			case ErrorCode.DAILY_REWARD_ALREADY_HANDLED_TODAY:
			case ErrorCode.DAILY_REWARD_CANNOT_CONTINUE_ON_LAST_DAY:
			case ErrorCode.DAILY_REWARD_CANNOT_HANDLE_INTERRUPTED_CHAIN:
			case ErrorCode.DAILY_REWARD_CANNOT_BUY_BACK:
			case ErrorCode.DAILY_REWARD_MISSED_DAYS_COUNT_CHANGED:
				ShouldNotfiyUser = true;
				break;
			case ErrorCode.SESSION_INVALID:
			case ErrorCode.SESSION_INVALID_REQUEST_TOKEN:
				goto IL_04f5;
				IL_04f5:
				ShouldNotfiyUser = false;
				break;
				end_IL_0009:
				break;
			}
			switch (_errorCode)
			{
			case ErrorCode.UnknownCode:
			case ErrorCode.INTERNAL_ERROR:
			case ErrorCode.INVALID_PARAM:
			case ErrorCode.USER_NOT_FOUND:
			case ErrorCode.REQUEST_INVALID:
			case ErrorCode.MISSION_TOKEN_INVALID:
			case ErrorCode.MISSION_TOKEN_EXPIRED:
			case ErrorCode.MISSION_STATS_INVALID:
			case ErrorCode.MISSION_NOT_FOUND:
			case ErrorCode.FAILED_TO_GENERATE_AVATAR_DEFINITION:
			case ErrorCode.MISSION_LOCATION_LOCKED:
			case ErrorCode.MISSION_NO_BUDGET_MAPPING_FOUND:
			case ErrorCode.MISSION_BLACKLISTED:
			case ErrorCode.ITEM_ALREADY_EQUIPPED:
			case ErrorCode.ITEM_NOT_OWNER:
			case ErrorCode.ITEM_SLOT_NOT_EQUIPPABLE:
			case ErrorCode.ITEM_NOT_FOUND:
			case ErrorCode.NOT_ENOUGH_RESOURCES:
			case ErrorCode.INVALID_RESOURCE:
			case ErrorCode.MULTIPLE_HACK_RESOURCE:
			case ErrorCode.ITEM_FULL_INVENTORY:
			case ErrorCode.TIER_LOCKED:
			case ErrorCode.ASSASSIN_NOT_FOUND:
			case ErrorCode.NOT_ENOUGH_SKILLPOINTS:
			case ErrorCode.ASSASSIN_ALREADY_IN_TRAINING:
			case ErrorCode.ASSASSIN_EXT_CLASS_ALREADY_SET:
			case ErrorCode.ASSASSIN_EXT_CLASS_NOT_SET:
			case ErrorCode.NOT_ALL_TIERS_UNLOCKED:
			case ErrorCode.SKILLWHEEL_INVALID:
			case ErrorCode.SKILL_MAX_LEVEL_REACHED:
			case ErrorCode.AVATAR_CLASS_NOT_FOUND:
			case ErrorCode.INVALID_AVATAR_NAME:
			case ErrorCode.INVALID_HEAD_GUID:
			case ErrorCode.INVALID_SKILL_CLASS:
			case ErrorCode.MAX_ASSASSIN_SLOTS_REACHED:
			case ErrorCode.SKILL_MIN_LEVEL_REACHED:
			case ErrorCode.CLASS_LOCKED:
			case ErrorCode.CLASS_NOT_ACCESSIBLE:
			case ErrorCode.CLASS_ENTITLEMENTS_NEEDED:
			case ErrorCode.ASSASSIN_NOT_IN_TRAINING:
			case ErrorCode.INVALID_PROFILENAME:
			case ErrorCode.ASSASSIN_EXT_CLASS_NUMBER_NOT_MAPPED:
			case ErrorCode.MAIL_HAS_NO_ATTACHMENT:
			case ErrorCode.ACHIEVEMENT_NOT_REACHED:
			case ErrorCode.ACHIEVEMENT_RANK_NOT_REACHED:
			case ErrorCode.ACHIEVEMENT_ALREADY_CLAIMED:
			case ErrorCode.ACHIEVEMENT_NOT_EXISTING:
			case ErrorCode.DAILY_REWARD_NO_REWARD_CALCULATED:
			case ErrorCode.DAILY_REWARD_ALREADY_HANDLED_TODAY:
			case ErrorCode.DAILY_REWARD_CANNOT_CONTINUE_ON_LAST_DAY:
			case ErrorCode.DAILY_REWARD_CANNOT_HANDLE_INTERRUPTED_CHAIN:
			case ErrorCode.DAILY_REWARD_CANNOT_BUY_BACK:
			case ErrorCode.DAILY_REWARD_MISSED_DAYS_COUNT_CHANGED:
			case ErrorCode.MISSION_CANNOT_REFRESH_MISSION_LIST:
			case ErrorCode.MISSION_ENTITLEMENTS_NEEDED:
			case ErrorCode.FORGE_ONLY_ONE_BLUEPRINT_ALLOWED:
			case ErrorCode.FORGE_QUALITIES_DO_NOT_MATCH:
			case ErrorCode.FORGE_ONLY_ITEMS_AND_BLUEPRINT_ALLOWED:
			case ErrorCode.FORGE_NEED_AT_LEAST_ONE_ITEM:
			case ErrorCode.MAXIMUM_ENERGY_ALLOWANCE:
			case ErrorCode.INSUFFICIENT_FUNDS:
			case ErrorCode.SHOP_ITEM_NOT_FOUND:
			case ErrorCode.INVALID_SHOP_ITEM_ID:
			case ErrorCode.SLOTS_AMOUNT_MISMATCH:
			case ErrorCode.LOCATION_ALREADY_ACCESSIBLE:
			case ErrorCode.LOCATION_NOT_PURCHASEABLE:
			case ErrorCode.INVENTORY_SLOTS_MAXIMUM_REACHED:
			case ErrorCode.SHOP_ENTITLEMENTS_NEEDED:
			case ErrorCode.ENTITLEMENT_NEEDED:
				DataStoreResult = DataStoreRetryResult.UpdateProfile;
				break;
			case ErrorCode.WESCacheEntryNotFound:
			case ErrorCode.WESConnectFailure:
			case ErrorCode.WESConnectionClosed:
			case ErrorCode.WESKeepAliveFailure:
			case ErrorCode.WESMessageLengthLimitExceeded:
			case ErrorCode.WESNameResolutionFailure:
			case ErrorCode.WESPending:
			case ErrorCode.WESPipelineFailure:
			case ErrorCode.WESProtocolError:
			case ErrorCode.WESProxyNameResolutionFailure:
			case ErrorCode.WESReceiveFailure:
			case ErrorCode.WESRequestCanceled:
			case ErrorCode.WESRequestProhibitedByCachePolicy:
			case ErrorCode.WESRequestProhibitedByProxy:
			case ErrorCode.WESSecureChannelFailure:
			case ErrorCode.WESSendFailure:
			case ErrorCode.WESServerProtocolViolation:
			case ErrorCode.WESTimeout:
			case ErrorCode.WESTrustFailure:
			case ErrorCode.WESUnknownError:
			case ErrorCode.REQUEST_EMPTY:
			case ErrorCode.AUTHENTICATION_FAILED:
			case ErrorCode.SESSION_INVALID:
			case ErrorCode.SESSIONS_CREATION_FAILED:
			case ErrorCode.SesssionTimeout:
			case ErrorCode.SesssionNotOpen:
			case ErrorCode.DiscoveryServiceUnreachable:
			case ErrorCode.NoNetworkConnection:
			case ErrorCode.NoInternetAccess:
				DataStoreResult = DataStoreRetryResult.Reconnect;
				break;
			case ErrorCode.URIMisformated:
			case ErrorCode.UnableToRequest:
			case ErrorCode.WESSignatureVerificationFailure:
			case ErrorCode.ACClientOutdated:
			case ErrorCode.DATA_LOCK_FAILED:
			case ErrorCode.PROTOBUF_VERSION_NOT_MAPPED:
			case ErrorCode.MAINTENANCE_MODE:
			case ErrorCode.STATICS_VERSION_OUTDATED:
			case ErrorCode.MISSION_HIRELING_NEEDED:
			case ErrorCode.LOCATION_HAS_NO_STORY_MISSIONS:
			case ErrorCode.ITEM_LEVEL_TOO_HIGH:
			case ErrorCode.ITEM_CONSUMABLE_NOT_UNEQUIPPABLE:
			case ErrorCode.ASSASSIN_COUNT_MISMATCH:
			case ErrorCode.USER_ALREADY_EXISTS:
			case ErrorCode.ACCOUNT_CREATION_FAILED:
			case ErrorCode.WRONG_USERID_PASSWORD_COMBINATION:
			case ErrorCode.INVALID_PROFILENAME_TOO_LONG:
			case ErrorCode.ACCOUNT_DOES_NOT_EXIST:
			case ErrorCode.USERPROFILE_DOES_NOT_EXIST:
			case ErrorCode.SESSION_INVALID_REQUEST_TOKEN:
			case ErrorCode.SESSION_OTHER_SESSION_EXISTS:
			case ErrorCode.MAIL_HEADER_NOT_FOUND:
			case ErrorCode.MAIL_BODY_NOT_FOUND:
			case ErrorCode.MAIL_ALREADY_FRIENDS:
			case ErrorCode.MAIL_SUBJECT_CHARACTER_LIMIT_EXCEEDED:
			case ErrorCode.MAIL_BODY_CHARACTER_LIMIT_EXCEEDED:
			case ErrorCode.MAIL_CANNOT_REJECT_OWN_GIFT:
			case ErrorCode.MAIL_RECEIVER_NOT_FRIEND:
			case ErrorCode.MAIL_SENDER_ALREADY_CANCELED_REQUEST:
			case ErrorCode.MISSION_REVIVE_FEATURE_DISABLED:
			case ErrorCode.MYSTERY_BOX_CHAIN_NOT_FOUND:
			case ErrorCode.MYSTERY_BOX_CHAIN_EXPIRED:
			case ErrorCode.MYSTERY_BOX_NOT_FOUND:
			case ErrorCode.ITEM_ONLY_ITEMS_CAN_BE_SOLD:
			case ErrorCode.FORGE_TOO_MUCH_SLOTS:
			case ErrorCode.FORGE_NO_OR_INVALID_DATA_FOUND_FOR_REROLL:
			case ErrorCode.UNCLAIMABLE_QUESTS:
			case ErrorCode.NO_QUEST_IN_PROGRESS:
			case ErrorCode.NO_ENOUGH_STRATEGIES_TO_REROLL:
			case ErrorCode.NO_SHOP_DATA_AVAILABLE:
			case ErrorCode.NO_ITEM_BUNDLES_AVAILABLE:
			case ErrorCode.FRIEND_NOT_FOUND:
			case ErrorCode.REACHED_MAX_FRIEND_COUNT:
			case ErrorCode.OTHER_REACHED_MAX_FRIEND_COUNT:
			case ErrorCode.HC_SHOP_INVALID_PARAMATERS:
			case ErrorCode.HC_SHOP_ERROR:
			case ErrorCode.HC_SHOP_ITUNES_CONNECTION_FAILED:
			case ErrorCode.HC_SHOP_NOT_SUPPORTED_ON_THIS_PLATFORM:
			case ErrorCode.HC_SHOP_IAP_DISABLED:
			case ErrorCode.HC_SHOP_CLIENT_INVALID:
			case ErrorCode.HC_SHOP_PAYMENT_CANCELLED:
			case ErrorCode.HC_SHOP_PAYMENT_INVALID:
			case ErrorCode.HC_SHOP_PAYMENT_NOT_ALLOWED:
			case ErrorCode.HC_SHOP_PRODUCT_UNAVAILABLE:
			case ErrorCode.HC_SHOP_RECEIPT_ERROR:
			case ErrorCode.HC_SHOP_USER_CANCELLED:
			case ErrorCode.HC_SHOP_SERVICE_UNAVAILABLE:
			case ErrorCode.HC_SHOP_ITEM_NOT_OWNED:
			case ErrorCode.HC_SHOP_ACTIVITY_NOT_FOUND:
			case ErrorCode.HC_SHOP_BINDING_FAILED:
			case ErrorCode.HC_SHOP_BILLING_UNAVAILABLE:
			case ErrorCode.HC_SHOP_PRODUCTS_REQUEST_FAILED:
			case ErrorCode.HC_SHOP_PRODUCTS_REQUEST_EMPTY:
			case ErrorCode.HC_SHOP_PURCHASE_FAILED:
			case ErrorCode.HC_SHOP_PURCHASE_CANCELLED:
			case ErrorCode.HC_SHOP_CONSUME_FAILED:
			case ErrorCode.HC_SHOP_RECOVER_FAILED:
			case ErrorCode.HC_SHOP_ITEM_ALREADY_OWNED:
			case ErrorCode.PlayerProfileBroken:
			case ErrorCode.StartupDataFailed:
			case ErrorCode.BundleDownloadFailed:
			case ErrorCode.BundleDownloadCRCFailed:
			case ErrorCode.NotEnoughDiskspace:
			case ErrorCode.NotEnoughCacheSpace:
			case ErrorCode.NetworkProtocolVersionMismatch:
			case ErrorCode.RegionNotAvailable:
			case ErrorCode.UPLAY_START_FAILED:
			case ErrorCode.UPLAY_LOGIN_FAILED:
			case ErrorCode.UPLAY_USER_ALREADY_EXIST:
			case ErrorCode.UPLAY_USER_NOT_FOUND:
			case ErrorCode.FACEBOOK_START_FAILED:
			case ErrorCode.FACEBOOK_LOGIN_FAILED:
			case ErrorCode.FACEBOOK_USER_ALREADY_EXIST:
			case ErrorCode.FACEBOOK_USER_NOT_FOUND:
			case ErrorCode.FACEBOOK_CANNOT_OPEN_URL:
			case ErrorCode.ADCOLONY_ZONE_UNAVAILABLE:
			case ErrorCode.ADCOLONY_VIDEO_NOT_READY:
			case ErrorCode.ADCOLONY_VIDEO_LIMIT_REACHED:
			case ErrorCode.ADCOLONY_V4VC_TRANSACTION_FAILED:
			case ErrorCode.ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED:
			case ErrorCode.LICENSE_CHECK_FAILED_CANNOT_CONNECT:
			case ErrorCode.LICENSE_CHECK_FAILED_GO_TO_STORE:
			case ErrorCode.LICENSE_CHECK_FAILED_ERROR_INVALID_PACKAGE_NAME:
			case ErrorCode.LICENSE_CHECK_FAILED_ERROR_NOT_MATCHING_UID:
			case ErrorCode.LICENSE_CHECK_FAILED_ERROR_NOT_MARKET_MANAGED:
				DataStoreResult = DataStoreRetryResult.None;
				break;
			}
			switch (_errorCode)
			{
			case ErrorCode.WESConnectFailure:
			case ErrorCode.WESConnectionClosed:
			case ErrorCode.WESKeepAliveFailure:
			case ErrorCode.WESNameResolutionFailure:
			case ErrorCode.WESPipelineFailure:
			case ErrorCode.WESProxyNameResolutionFailure:
			case ErrorCode.WESReceiveFailure:
			case ErrorCode.WESRequestProhibitedByProxy:
			case ErrorCode.WESSecureChannelFailure:
			case ErrorCode.WESSendFailure:
			case ErrorCode.WESTimeout:
			case ErrorCode.WESUnknownError:
			case ErrorCode.NoNetworkConnection:
			case ErrorCode.NoInternetAccess:
				PotentialConnectionIssue = true;
				break;
			default:
				PotentialConnectionIssue = false;
				break;
			}
		}

		public static ErrorCode ErrorCodeForHttpStatus(int _httpStatusCode)
		{
			return ErrorCode.UnknownCode;
		}

		public static ErrorCategory ErrorCatergoryForErrorCode(ErrorCode _errorCode)
		{
			return ErrorCategory.UnknownCategory;
		}
	}
}
