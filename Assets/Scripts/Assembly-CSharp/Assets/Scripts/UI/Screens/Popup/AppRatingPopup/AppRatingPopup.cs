using System;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.AppRatingPopup
{
	public class AppRatingPopup
	{
		private const long InitialMissions = 0L;

		private const long UpdateMissions = 10L;

		private const long RemindMissions = 15L;

		private const long DontRemindMissions = -1000L;

		public static bool LastMissionWasSuccess { get; set; }

		public static void Show()
		{
			if (!Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.AppRatingEntitlementGUID))
			{
				Proceed();
				return;
			}
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			bool flag = false;
			object obj = instance.GPGetValue(LocalStorage.GPKeyAppRatingDisplayed);
			if (obj is long)
			{
				flag = (long)obj > 0;
			}
			if (flag && !LastMissionWasSuccess)
			{
				Proceed();
				return;
			}
			string changeList = Main.Instance.BuildDefinition.ChangeList;
			string text = instance.GPGetValue(LocalStorage.GPKeyAppRatingChangeList) as string;
			if (changeList == text)
			{
				Proceed();
				return;
			}
			long num = 0L;
			obj = instance.GPGetValue(LocalStorage.GPKeyAppRatingMissions);
			if (obj is long)
			{
				num = (long)obj;
			}
			if (num > 1)
			{
				instance.GPSetValue(LocalStorage.GPKeyAppRatingMissions, num - 1);
				Proceed();
				return;
			}
			if (num <= -1000)
			{
				instance.GPSetValue(LocalStorage.GPKeyAppRatingChangeList, changeList);
				Proceed();
				return;
			}
			instance.GPSetValue(LocalStorage.GPKeyAppRatingMissions, 0L);
			instance.GPSetValue(LocalStorage.GPKeyAppRatingDisplayed, 1L);
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[3]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(6119),
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					OnButtonPressed = delegate
					{
						ButtonPressed(NoThanks);
					}
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(6120),
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					OnButtonPressed = delegate
					{
						ButtonPressed(RemindMeLater);
					}
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(6118),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						ButtonPressed(RedirectToAppStore);
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(6116);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				ButtonPressed(RemindMeLater);
			};
			popUpConfiguration.Data = new AppRatingContent.AppRatingData
			{
				Description = LocalizationManager.Instance.Get(6117)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void Proceed()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.PopupQueue.Proceed();
		}

		private static void ButtonPressed(Action _action)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (_action != null)
			{
				_action();
			}
			Proceed();
		}

		private static void RedirectToAppStore()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			string changeList = Main.Instance.BuildDefinition.ChangeList;
			instance.GPSetValue(LocalStorage.GPKeyAppRatingChangeList, changeList);
			instance.GPSetValue(LocalStorage.GPKeyAppRatingMissions, 10L);
			string text = null;
			text = LocalizationManager.Instance.Get(6611);
			if (text != null)
			{
				Application.OpenURL(text);
			}
		}

		private static void RemindMeLater()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			instance.GPSetValue(LocalStorage.GPKeyAppRatingChangeList, null);
			instance.GPSetValue(LocalStorage.GPKeyAppRatingMissions, 15L);
		}

		private static void NoThanks()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			string changeList = Main.Instance.BuildDefinition.ChangeList;
			instance.GPSetValue(LocalStorage.GPKeyAppRatingChangeList, changeList);
			instance.GPSetValue(LocalStorage.GPKeyAppRatingMissions, -1000L);
		}
	}
}
