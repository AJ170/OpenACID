using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

public class GamepadNavigation : UIScreenChild
{
	private static class OptionStrings
	{
		public const string GamepadNotification = "GamepadPlugInNotification";
	}

	private const string c_GamepadTexture = "UI/Texture/aUI_img_gamepad";

	[SerializeField]
	private UIButton m_backButton;

	[SerializeField]
	private UIExtendedButton m_switchLayoutButton;

	[SerializeField]
	private UIExtendedLabel m_headerLabel;

	[SerializeField]
	private UITexture m_gamepadTexture;

	[SerializeField]
	private UIExtendedLabel m_labelDigitalPad;

	[SerializeField]
	private UIExtendedLabel m_labelLeftAnalogStick;

	[SerializeField]
	private UIExtendedLabel m_labelRightAnalogStick;

	[SerializeField]
	private UIExtendedLabel m_labelLowerActionButtons;

	[SerializeField]
	private UIExtendedLabel m_labelUpperActionButtons;

	[SerializeField]
	private UIExtendedLabel m_labelLeftTrigger;

	[SerializeField]
	private UIExtendedLabel m_labelRightTrigger;

	[SerializeField]
	private UIExtendedLabel m_labelCenterButton;

	[SerializeField]
	private UISlider m_notificationSlider;

	[SerializeField]
	private GenericToggleHighlighter m_notificationSliderHighlighter;

	private bool m_isAnimusLayout = true;

	private ResourceRequest m_resource;

	private bool m_showScreenOnConnect = true;

	private Timer.EPauseState m_previousPauseState;

	private bool m_isInitialized;

	protected override void OnScreenInit()
	{
		ReadSettings();
		EventDelegate.Add(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Add(m_switchLayoutButton.onClick, SwitchLayout);
		AcierInputHandler.Instance.Gamepad.OnControllerConnected += GamepadControllerConnected;
	}

	private void GamepadControllerConnected()
	{
		if (m_showScreenOnConnect)
		{
			OptionsScreenHelper.Close();
			OptionsScreenHelper.ActivateMenu(m_screen.ScreenId);
		}
	}

	public void OnEnable()
	{
		StartCoroutine(LoadImage());
		m_isAnimusLayout = Main.Instance.SceneLoader.InAnimus;
		Initialize();
		if (!Main.Instance.SceneLoader.InAnimus)
		{
			m_previousPauseState = UbiCore.Utils.Singleton<Timer>.Instance.PauseState;
			UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseOptions);
		}
	}

	public void OnDisable()
	{
		UnloadImage();
	}

	protected override void OnScreenDestroy()
	{
		EventDelegate.Remove(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Remove(m_switchLayoutButton.onClick, SwitchLayout);
		AcierInputHandler.Instance.Gamepad.OnControllerConnected -= GamepadControllerConnected;
	}

	private void SwitchLayout()
	{
		m_isAnimusLayout = !m_isAnimusLayout;
		Initialize();
	}

	public override void OnBackKey()
	{
		GoBackToPreviousScreen();
	}

	public void OnToggle()
	{
		if (m_isInitialized)
		{
			m_showScreenOnConnect = m_notificationSlider.value > 0.5f;
		}
	}

	private void GoBackToPreviousScreen()
	{
		SaveSettings();
		Singleton<UIScreenManager>.Instance.PopScreen();
		if (EntitlementExtensions.HasCompletedIntroMission())
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(m_screen.m_previousScreen);
			OptionsMenuNavigation componentInChildren = screen.GetComponentInChildren<OptionsMenuNavigation>();
			if (componentInChildren != null)
			{
				componentInChildren.PreviousPauseState = m_previousPauseState;
			}
			Singleton<UIScreenManager>.Instance.PushScreen(screen, null);
		}
		else if (m_previousPauseState == Timer.EPauseState.Running)
		{
			UbiCore.Utils.Singleton<Timer>.Instance.Unpause();
		}
	}

	private void Initialize()
	{
		m_switchLayoutButton.SetLabel((!m_isAnimusLayout) ? 7791 : 7785);
		m_headerLabel.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7785 : 7791);
		m_labelLeftAnalogStick.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7781 : 7789);
		m_labelRightAnalogStick.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7782 : 7790);
		m_labelLowerActionButtons.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7780 : 7788);
		m_labelUpperActionButtons.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7779 : 7787);
		m_labelLeftTrigger.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7783 : 7786);
		m_labelRightTrigger.text = LocalizationManager.Instance.Get((!m_isAnimusLayout) ? 7784 : 7786);
		m_labelCenterButton.text = LocalizationManager.Instance.Get(7778);
		if (!m_isAnimusLayout)
		{
			m_labelDigitalPad.text = LocalizationManager.Instance.Get(7777);
		}
		m_labelDigitalPad.gameObject.SetActive(!m_isAnimusLayout);
		m_isInitialized = true;
	}

	private void ReadSettings()
	{
		if (PlayerPrefs.HasKey("GamepadPlugInNotification"))
		{
			m_showScreenOnConnect = PlayerPrefsExtension.GetBool("GamepadPlugInNotification");
		}
		m_notificationSlider.value = ((!m_showScreenOnConnect) ? 0f : 1f);
		m_notificationSliderHighlighter.OnValueChange();
	}

	private void SaveSettings()
	{
		PlayerPrefsExtension.SetBool("GamepadPlugInNotification", m_showScreenOnConnect);
	}

	private IEnumerator LoadImage()
	{
		m_resource = Resources.LoadAsync<Texture2D>("UI/Texture/aUI_img_gamepad");
		while (!m_resource.isDone)
		{
			yield return 0;
		}
		m_gamepadTexture.uvRect = new Rect(0f, 0f, 1f, 1f);
		m_gamepadTexture.mainTexture = m_resource.asset as Texture2D;
	}

	private void UnloadImage()
	{
		m_gamepadTexture.mainTexture = null;
		Resources.UnloadAsset(m_resource.asset);
	}
}
