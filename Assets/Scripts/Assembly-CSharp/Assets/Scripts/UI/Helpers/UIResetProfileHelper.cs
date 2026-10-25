using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UbiPlayground;

namespace Assets.Scripts.UI.Helpers
{
	public class UIResetProfileHelper
	{
		public static bool IsEnabled
		{
			get
			{
				return false;
			}
		}

		public static void OpenResetProfilePopup()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(10308),
					OnButtonPressed = OnVerifyAccountDeletion
				},
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(10309);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10310)
			};
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void OnVerifyAccountDeletion(object _userData)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (!IsProfileLinkedToSocialMedia())
			{
				OfferLinkProfilePopup();
			}
			else
			{
				WarnDeleteProfilePopup();
			}
		}

		private static void OfferLinkProfilePopup()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(10312),
					OnButtonPressed = OnVerifyLinkProfile,
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick
				},
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(3791),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						WarnDeleteProfilePopup();
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(10309);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10311)
			};
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void WarnDeleteProfilePopup()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(10307),
					OnButtonPressed = OnDeleteProfile
				},
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(10313);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10314)
			};
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void OnVerifyLinkProfile(object _userData)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			Singleton<UIScreenManager>.Instance.OverrideLockedScreen = true;
			OptionsScreenHelper.DeactivateMenu();
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.SocialSelectionScreen)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.SocialSelectionScreen, new UITweenHandle(null, null));
			}
		}

		private static void OnDeleteProfile(object _userData)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			UINetworkAdaptor.BlockedNetworkRequest(delegate
			{
				Singleton<StateManager>.Instance.ProfileManager.GetResetProfile(delegate(IAsyncResult _response)
				{
					if (_response.IsError())
					{
						PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
						Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
					}
					else if (Singleton<StateManager>.Instance.ProfileManager.AbandonUserProfile())
					{
						MailCache.Clear();
						RestartAppPopup();
					}
				});
			});
		}

		private static void RestartAppPopup()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						UIUtil.QuitApplication();
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(10441);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(10440)
			};
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static bool IsProfileLinkedToSocialMedia()
		{
			return UbiPlaygroundManager.IsProfileLinkedToUplay || FacebookManager.IsProfileLinkedToFacebook || WeiboManager.IsProfileLinkedToWeibo;
		}
	}
}
