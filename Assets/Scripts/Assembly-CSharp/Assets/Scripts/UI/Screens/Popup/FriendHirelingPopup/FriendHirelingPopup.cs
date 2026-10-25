using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.Popup.FriendHirelingPopup
{
	public class FriendHirelingPopup
	{
		private static FriendErrorHandler ErrorHandlerAddFriend;

		public static HirelingAssassin Hireling { get; set; }

		public static bool HirelingIsFriend { get; set; }

		public static bool ShouldOpen()
		{
			return Hireling != null && !HirelingIsFriend && Hireling.hirelingType == HirelingAssassin.EHirelingType.Random && Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FriendingEntitlementGUID);
		}

		public static void Show()
		{
			if (!ShouldOpen())
			{
				Proceed();
				return;
			}
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(4426),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						RequestFriendHireling();
					},
					Data = true
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(4623);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				Proceed();
			};
			popUpConfiguration.Data = new FriendHirelingContent.FriendHirelingData
			{
				Hireling = Hireling
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void Proceed()
		{
			ResetMissionData();
			if (ErrorHandlerAddFriend != null)
			{
				ErrorHandlerAddFriend.AddedFriendHash = null;
			}
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.PopupQueue.Proceed();
		}

		private static void RequestFriendHireling()
		{
			if (ErrorHandlerAddFriend == null)
			{
				ErrorHandlerAddFriend = new FriendErrorHandler(ConvertAlreadyFriendHashToUsername, Show, Proceed);
			}
			ErrorHandlerAddFriend.AddedFriendHash = Hireling.userIdHash;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.SendFriendRequest(Hireling.userIdHash, EModuleOrigin.Origin_HirelingPopup, _result);
			}, ErrorHandlerAddFriend, OnResponseFriendHireling);
		}

		private static void OnResponseFriendHireling(IAsyncResult _response)
		{
			if (!FriendUtil.IsError(_response))
			{
				FriendUtil.NotificationGeneric(4483, new StringMapper { { "player_name", Hireling.userName } });
				Proceed();
			}
		}

		private static string ConvertAlreadyFriendHashToUsername(string _userIdHash)
		{
			return Hireling.userName;
		}

		public static void RecordMissionData()
		{
			if (!Main.Instance.BuildDefinition.AvatarFixed && Main.Instance.SceneLoader.CurrentMissionDescription.ObjectiveType != EObjectiveType.Tutorial_EObjectiveType)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				if (Singleton<StateManager>.Instance.ProfileManager.MissionToken.hirelingCanBeFriend)
				{
					UIAssasinId lastSelectedHirelingId = profileManager.LastSelectedHirelingId;
					Hireling = profileManager.GetHireling(lastSelectedHirelingId);
					HirelingIsFriend = profileManager.MissionToken.hirelingIsFriend;
					return;
				}
			}
			ResetMissionData();
		}

		public static void ResetMissionData()
		{
			Hireling = null;
			HirelingIsFriend = false;
		}
	}
}
