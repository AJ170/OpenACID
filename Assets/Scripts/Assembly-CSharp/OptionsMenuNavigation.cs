using System.Collections.Generic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using UbiCore.Utils;
using UnityEngine;

public class OptionsMenuNavigation : UIScreenChild
{
	public delegate void OptionsMenuClosed();

	[SerializeField]
	private UIButton m_backButton;

	[SerializeField]
	private UIButton m_restartMission;

	[SerializeField]
	private UIButton m_backToAnimus;

	[SerializeField]
	private UISprite m_background;

	private List<GenericOptionsTab> m_allTabs;

	private GenericOptionsTab m_currentTab;

	private GenericOptionsTab m_lastTab;

	public static OptionsMenuClosed OverlayMenuClosed;

	[HideInInspector]
	public bool InGame;

	public Timer.EPauseState PreviousPauseState { get; set; }

	public void GetActivated(List<GenericOptionsTab> getTabs, UIGrid _getGrid)
	{
		if (InGame)
		{
			m_background.alpha = 0.8f;
		}
		else
		{
			m_background.alpha = 1f;
		}
		EventDelegate.Add(m_backButton.onClick, Deactivate);
		m_allTabs = getTabs;
		for (int i = 0; i < m_allTabs.Count; i++)
		{
			EventDelegate.Add(m_allTabs[i].m_tabTrigger.onClick, OnClickedTab);
		}
		m_allTabs[0].GetActivated();
		m_lastTab = m_allTabs[0];
		for (int j = 1; j < m_allTabs.Count; j++)
		{
			m_allTabs[j].GetDeactivated();
		}
		m_backToAnimus.gameObject.SetActive(UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsSettingsReturnToAnimusButtonVisible());
		EventDelegate.Add(m_restartMission.onClick, OnRestartMission);
		EventDelegate.Add(m_backToAnimus.onClick, OnAbortMission);
	}

	public override void OnBackKey()
	{
		Deactivate();
	}

	public void Deactivate()
	{
		Singleton<UIScreenManager>.Instance.PopScreen();
		if (UbiCore.Utils.Singleton<Timer>.Instance.PauseState == Timer.EPauseState.PauseOptions)
		{
			UbiCore.Utils.Singleton<Timer>.Instance.Pause(PreviousPauseState);
		}
		PreviousPauseState = Timer.EPauseState.Running;
		if (Main.Instance.SceneLoader.InAnimus && Singleton<UIScreenManager>.Instance.CurrentScreen == null)
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(Singleton<UIScreenManager>.Instance.LastValidScreenId, new UITweenHandle(null, null));
		}
		if (OverlayMenuClosed != null)
		{
			OverlayMenuClosed();
		}
	}

	protected override void OnScreenInit()
	{
	}

	protected override void OnScreenDestroy()
	{
		EventDelegate.Remove(m_backButton.onClick, Deactivate);
		if (m_allTabs != null)
		{
			for (int i = 0; i < m_allTabs.Count; i++)
			{
				EventDelegate.Remove(m_allTabs[i].m_tabTrigger.onClick, OnClickedTab);
			}
		}
		EventDelegate.Remove(m_restartMission.onClick, OnRestartMission);
		EventDelegate.Remove(m_backToAnimus.onClick, OnAbortMission);
	}

	private void OnClickedTab()
	{
		if (m_lastTab != null)
		{
			m_lastTab.GetDeactivated();
		}
		GenericOptionsTab component = UIEventTrigger.current.transform.parent.GetComponent<GenericOptionsTab>();
		m_currentTab = component;
		m_currentTab.GetActivated();
		m_lastTab = m_currentTab;
	}

	private void OnRestartMission()
	{
		PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
		{
			new PopUpButtonConfiguration
			{
				ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
				Label = LocalizationManager.Instance.Get(1780),
				OnButtonPressed = delegate
				{
					UIMissionHelper.RetryMission();
				}
			}
		};
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
		popUpConfiguration.Buttons = buttons;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		};
		popUpConfiguration.Data = new ErrorContent.ErrorData
		{
			Description = LocalizationManager.Instance.Get(4372)
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private void OnAbortMission()
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
					UIMissionHelper.GoToAnimus();
				}
			}
		};
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
		popUpConfiguration.Buttons = buttons;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		};
		popUpConfiguration.Data = new ErrorContent.ErrorData
		{
			Description = LocalizationManager.Instance.Get(4371)
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}
}
