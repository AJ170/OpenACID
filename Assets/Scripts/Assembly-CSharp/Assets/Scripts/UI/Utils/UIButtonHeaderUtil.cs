using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;

namespace Assets.Scripts.UI.Utils
{
	public static class UIButtonHeaderUtil
	{
		public static UIButtonHeader.EHeaderType GetButtonHeaderTypeByScreen(EScreenID _eScreenID)
		{
			UIButtonHeader.EHeaderType result = UIButtonHeader.EHeaderType.Invalid;
			switch (_eScreenID)
			{
			case EScreenID.MissionLoadout:
			case EScreenID.MissionHireling:
			case EScreenID.MissionEndGame:
			case EScreenID.Mission:
				result = ((!Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign) ? UIButtonHeader.EHeaderType.MissionsContracts : UIButtonHeader.EHeaderType.MissionsCampaign);
				break;
			case EScreenID.AssassinSelection:
			case EScreenID.AssassinSkillTree:
			case EScreenID.AssassinCreation:
			case EScreenID.AssasinStats:
			case EScreenID.AssassinInventory:
			case EScreenID.InventoryScreen:
			case EScreenID.MysteryBox:
				result = UIButtonHeader.EHeaderType.Assassin;
				break;
			case EScreenID.SocialSelectionScreen:
			case EScreenID.FriendsListScreen:
			case EScreenID.LeaderboardScreen:
				result = UIButtonHeader.EHeaderType.Social;
				break;
			case EScreenID.ShopScreen:
				result = UIButtonHeader.EHeaderType.Shop;
				break;
			case EScreenID.CodexScreen:
				result = UIButtonHeader.EHeaderType.Codex;
				break;
			case EScreenID.EventProgressionScreen:
				result = UIButtonHeader.EHeaderType.Events;
				break;
			}
			return result;
		}

		public static bool IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType _headerType)
		{
			switch (_headerType)
			{
			case UIButtonHeader.EHeaderType.MissionsCampaign:
				return true;
			case UIButtonHeader.EHeaderType.MissionsContracts:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ContractsTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Assassin:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.AssassinTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Social:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialTabEntitlementGUID) || Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabMailGUID);
			case UIButtonHeader.EHeaderType.Shop:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ShopTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Codex:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.CodexTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Events:
				return true;
			default:
				return true;
			}
		}

		public static bool WasHeaderButtonUnlockedBeforeMission(UIButtonHeader.EHeaderType _headerType)
		{
			LastProfileData lastProfileDataBeforeMission = Singleton<StateManager>.Instance.ProfileManager.LastProfileDataBeforeMission;
			switch (_headerType)
			{
			case UIButtonHeader.EHeaderType.MissionsCampaign:
				return true;
			case UIButtonHeader.EHeaderType.MissionsContracts:
				return lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.ContractsTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Assassin:
				return lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.AssassinTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Social:
				return lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.SocialTabEntitlementGUID) || lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.SocialCategoryTabMailGUID);
			case UIButtonHeader.EHeaderType.Shop:
				return lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.ShopTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Codex:
				return lastProfileDataBeforeMission.HasEntitlement(StaticEntitlementIds.CodexTabEntitlementGUID);
			case UIButtonHeader.EHeaderType.Events:
				return true;
			default:
				return true;
			}
		}
	}
}
