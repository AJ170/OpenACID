using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Helpers
{
	public class OptionsScreenHelper
	{
		public static EScreenID[] OptionScreenIds = new EScreenID[10]
		{
			EScreenID.OptionsMenu,
			EScreenID.SoundMenu,
			EScreenID.ControlsMenu,
			EScreenID.TutorialMenu,
			EScreenID.NotificationsMenu,
			EScreenID.AccountMenu,
			EScreenID.GeneralSettingsMenu,
			EScreenID.SupportScreen,
			EScreenID.CreditsScreen,
			EScreenID.GamepadScreen
		};

		public static void ActivateMenu()
		{
			ActivateMenu(EScreenID.OptionsMenu);
		}

		public static void ActivateMenu(EScreenID _screenId)
		{
			if (!IsOptionsScreen() && !UIDownloadScreen.IsLoadedAndFullyVisible)
			{
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(_screenId);
				Singleton<UIScreenManager>.Instance.PushScreen(screen, null);
				OptionsMenuNavigation componentInChildren = screen.GetComponentInChildren<OptionsMenuNavigation>();
				Timer.EPauseState previousPauseState = Timer.EPauseState.Running;
				if (componentInChildren != null && !Main.Instance.SceneLoader.InAnimus)
				{
					previousPauseState = UbiCore.Utils.Singleton<Timer>.Instance.PauseState;
					UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseOptions);
				}
				if (componentInChildren != null)
				{
					componentInChildren.PreviousPauseState = previousPauseState;
				}
			}
		}

		public static void DeactivateMenu()
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
			if (screen != null)
			{
				OptionsMenuNavigation componentInChildren = screen.GetComponentInChildren<OptionsMenuNavigation>();
				if (componentInChildren != null && componentInChildren.gameObject.activeInHierarchy)
				{
					componentInChildren.Deactivate();
				}
			}
		}

		public static void Close()
		{
			if (IsOptionsScreen())
			{
				Singleton<UIScreenManager>.Instance.PopScreen();
			}
		}

		public static bool IsOptionsScreen()
		{
			EScreenID currentScreenId = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			return IsOptionsScreen(currentScreenId);
		}

		public static bool IsOptionsScreen(EScreenID _screen)
		{
			bool result = false;
			for (int i = 0; i < OptionScreenIds.Length; i++)
			{
				if (OptionScreenIds[i] == _screen)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
