using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;

namespace Assets.Scripts.UI.Mail
{
	public static class MailHelpers
	{
		private static PopUpConfiguration s_missingFriendsConfiguration;

		private static PopUpConfiguration s_mailSentConfiguration;

		public static PopUpConfiguration MissinFriendsConfiguration
		{
			get
			{
				if (s_missingFriendsConfiguration == null)
				{
					PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
					{
						new PopUpButtonConfiguration
						{
							ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
							Label = LocalizationManager.Instance.Get(1780),
							OnButtonPressed = delegate
							{
								Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
							}
						}
					};
					PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
					popUpConfiguration.Data = new ErrorContent.ErrorData
					{
						Description = LocalizationManager.Instance.Get(1)
					};
					popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
					popUpConfiguration.ShowCloseButton = false;
					popUpConfiguration.Heading = LocalizationManager.Instance.Get(1);
					popUpConfiguration.Buttons = buttons;
					s_missingFriendsConfiguration = popUpConfiguration;
				}
				return s_missingFriendsConfiguration;
			}
		}

		public static PopUpConfiguration MailSentConfiguration
		{
			get
			{
				if (s_mailSentConfiguration == null)
				{
					PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
					{
						new PopUpButtonConfiguration
						{
							ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
							Label = LocalizationManager.Instance.Get(1780),
							OnButtonPressed = delegate
							{
								Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
							}
						}
					};
					PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
					popUpConfiguration.Data = new ErrorContent.ErrorData
					{
						Description = LocalizationManager.Instance.Get(1)
					};
					popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
					popUpConfiguration.ShowCloseButton = false;
					popUpConfiguration.Heading = LocalizationManager.Instance.Get(1);
					popUpConfiguration.Buttons = buttons;
					s_mailSentConfiguration = popUpConfiguration;
				}
				return s_mailSentConfiguration;
			}
		}
	}
}
