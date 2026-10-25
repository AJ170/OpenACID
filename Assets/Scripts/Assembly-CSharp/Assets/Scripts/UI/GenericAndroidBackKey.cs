using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.UserConfirmationPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.UI
{
	public class GenericAndroidBackKey
	{
		public static bool IsExitPopupShown;

		public static void HandleBackKey()
		{
			if (Main.Instance.SceneLoader.CurrentMissionDescription == null)
			{
				return;
			}
			UIScreen currentScreen = Singleton<UIScreenManager>.Instance.CurrentScreen;
			bool flag = !Main.Instance.SceneLoader.InAnimus;
			if (Singleton<UIScreenManager>.Instance.CurrentModalScreen != null)
			{
				HandlePopUp(flag);
			}
			else if (!HandleInfoPopup() && (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetIsATutorialStepActive() || !HandleScreensInTutorial(currentScreen, flag)) && !HandleGenericBackButtonForPopups() && (!flag || !ToggleOptionsMenu()) && currentScreen != null)
			{
				switch (currentScreen.ScreenId)
				{
				case EScreenID.NewsFeedScreen:
					ShowExitConfirmation();
					break;
				case EScreenID.Mission:
					DispactCallToGenericHandeller(EScreenID.Mission);
					break;
				case EScreenID.MissionHireling:
					ChangeScreen(EScreenID.Mission);
					break;
				case EScreenID.AssassinSelection:
				case EScreenID.SocialSelectionScreen:
				case EScreenID.CodexScreen:
					LoadMissionScreen();
					break;
				case EScreenID.AssassinSkillTree:
				case EScreenID.AssassinCreation:
				case EScreenID.AssasinStats:
				case EScreenID.UIDecrytionScreen:
				case EScreenID.InventoryScreen:
					ChangeScreen(EScreenID.AssassinSelection);
					break;
				case EScreenID.AssassinSkillWheel:
					DispactCallToGenericHandeller(EScreenID.AssassinSkillWheel);
					break;
				case EScreenID.ShopScreen:
					DispactCallToGenericHandeller(EScreenID.ShopScreen);
					break;
				case EScreenID.EventProgressionScreen:
					DispactCallToGenericHandeller(currentScreen.ScreenId);
					break;
				case EScreenID.FriendsListScreen:
				case EScreenID.MailScreen:
				case EScreenID.InviteFriendsScreen:
				case EScreenID.LeaderboardScreen:
					DispactCallToGenericHandeller(currentScreen.ScreenId);
					break;
				case EScreenID.AchievementsScreen:
					ChangeScreen(EScreenID.SocialSelectionScreen);
					break;
				case EScreenID.OptionsMenu:
				case EScreenID.SoundMenu:
				case EScreenID.ControlsMenu:
				case EScreenID.GeneralSettingsMenu:
				case EScreenID.SupportScreen:
				case EScreenID.CreditsScreen:
				case EScreenID.GamepadScreen:
					DispactCallToGenericHandeller(currentScreen.ScreenId);
					break;
				case EScreenID.MissionEndGame:
				case EScreenID.AssassinInventory:
				case EScreenID.SystemPopUp_Deprecated:
				case EScreenID.LoadingOverlay:
				case EScreenID.TutorialMenu:
				case EScreenID.NotificationsMenu:
				case EScreenID.AccountMenu:
				case (EScreenID)23:
				case EScreenID.AccountCreation:
				case EScreenID.WelcomeScreen:
				case EScreenID.ItemsPopup_deprecated:
				case EScreenID.UIPopUp:
				case (EScreenID)34:
				case EScreenID.AssassinModule:
				case EScreenID.MysteryBox:
				case EScreenID.TabUnlockScreen:
				case EScreenID.UIPopUpBare:
					break;
				}
			}
		}

		private static void ChangeScreen(EScreenID _screenID)
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(_screenID, new UITweenHandle());
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
		}

		private static void DispactCallToGenericHandeller(EScreenID _screenID)
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(_screenID);
			UIScreenChild uIScreenChild = null;
			uIScreenChild = ((_screenID != EScreenID.SoundMenu) ? screen.GetComponentInChildren<UIScreenChild>() : screen.GetComponentInChildren<SoundMenuNavigation>());
			uIScreenChild.OnBackKey();
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
		}

		public static void LoadMissionScreen()
		{
			MissionSelectionManager.EMissionMenu subMenuType = MissionSelectionManager.EMissionMenu.Campaigns;
			EScreenID screenID = EScreenID.Mission;
			UIScreenHistoryDetails.EUIScreenGroups groupId = UIScreenHistoryDetails.EUIScreenGroups.CampaignMission;
			UIScreenHistoryDetails.ScreenHistory lastScreenByScreenGroup = Singleton<UIScreenManager>.Instance.LastScreenHistory.GetLastScreenByScreenGroup(groupId);
			if (lastScreenByScreenGroup != null)
			{
				subMenuType = lastScreenByScreenGroup.MenuType;
				screenID = lastScreenByScreenGroup.ScreenId;
			}
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = subMenuType;
			ChangeScreen(screenID);
		}

		private static bool HandleGenericBackButtonForPopups()
		{
			int count = GenericAndroidBackKeyHandler.CloseButtonsForGenericBackButtonList.Count;
			if (count > 0)
			{
				GenericAndroidBackKeyHandler.CloseButtonsForGenericBackButtonList[count - 1].Close();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
				return true;
			}
			return false;
		}

		private static bool HandleInfoPopup()
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetIsATutorialStepActive())
			{
				return false;
			}
			int count = UIInfoPopup.InfoPopupInstanceList.Count;
			if (count > 0)
			{
				UIInfoPopup uIInfoPopup = UIInfoPopup.InfoPopupInstanceList[count - 1];
				if (!uIInfoPopup.IsCloseButtonShown)
				{
					return true;
				}
				uIInfoPopup.Close();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
				return true;
			}
			return false;
		}

		private static bool HandleScreensInTutorial(UIScreen _currentScreen, bool _inGame)
		{
			if (_currentScreen == null)
			{
				return true;
			}
			bool flag = false;
			switch (_currentScreen.ScreenId)
			{
			case EScreenID.AssassinCreation:
				ChangeScreen(EScreenID.AssassinSelection);
				return true;
			case EScreenID.OptionsMenu:
			case EScreenID.SoundMenu:
			case EScreenID.ControlsMenu:
			case EScreenID.GeneralSettingsMenu:
			case EScreenID.SupportScreen:
			case EScreenID.CreditsScreen:
			case EScreenID.GamepadScreen:
				return false;
			default:
				IsExitPopupShown = true;
				ShowExitConfirmation();
				return true;
			}
		}

		private static void HandlePopUp(bool _inGame)
		{
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId == EScreenID.LoadingOverlay)
			{
				return;
			}
			UIScreen currentModalScreen = Singleton<UIScreenManager>.Instance.CurrentModalScreen;
			if (HandleAccCreationScreenPopup() || (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetIsATutorialStepActive() && (currentModalScreen as UIPopup).CurrentContentType != UIPopup.EContentType.UserConfirmation))
			{
				return;
			}
			if (_inGame)
			{
				if ((currentModalScreen as UIPopup).CurrentContentType == UIPopup.EContentType.RevivePlayer || (currentModalScreen as UIPopup).CurrentContentType == UIPopup.EContentType.RevivePlayerWithVideo)
				{
					return;
				}
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
				if (!screen.Active)
				{
					UbiCore.Utils.Singleton<Timer>.Instance.Unpause();
				}
			}
			IsExitPopupShown = false;
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
		}

		private static bool HandleAccCreationScreenPopup()
		{
			if (null == Singleton<UIScreenManager>.Instance.CurrentScreen)
			{
				return false;
			}
			if (Singleton<UIScreenManager>.Instance.CurrentScreen.ScreenId == EScreenID.AccountCreation)
			{
				UIScreen currentModalScreen = Singleton<UIScreenManager>.Instance.CurrentModalScreen;
				if ((currentModalScreen as UIPopup).CurrentContentType == UIPopup.EContentType.ErrorMessage)
				{
					UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.AccountCreation);
					AccountCreationScreen componentInChildren = screen.GetComponentInChildren<AccountCreationScreen>();
					componentInChildren.OnBackKey();
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
					return true;
				}
				if ((currentModalScreen as UIPopup).CurrentContentType == UIPopup.EContentType.ThirdPartyAccount)
				{
					if (FacebookManager.OnBackKey != null)
					{
						FacebookManager.OnBackKey();
					}
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
					return true;
				}
			}
			return false;
		}

		private static bool ToggleOptionsMenu()
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.InProgress)
			{
				return false;
			}
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.name == "tutorial_0001")
			{
				if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
				{
					UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseOptions);
					ShowExitConfirmation();
				}
				return true;
			}
			if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseOptions);
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
				Singleton<UIScreenManager>.Instance.PushScreen(screen, null);
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
				return true;
			}
			return false;
		}

		public static void ShowExitConfirmation()
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(7271);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.Data = new UserConfirmationContent.ConfirmationData
			{
				Description = LocalizationManager.Instance.Get(7274)
			};
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(7272),
					OnButtonPressed = delegate
					{
						UIUtil.QuitApplication();
					}
				},
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(3904),
					OnButtonPressed = delegate
					{
						IsExitPopupShown = false;
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
						{
							UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.Running);
						}
					}
				}
			};
			PopUpConfiguration config = popUpConfiguration;
			IsExitPopupShown = true;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
		}
	}
}
