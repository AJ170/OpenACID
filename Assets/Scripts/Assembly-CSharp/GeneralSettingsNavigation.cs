using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.GeneralSettingsScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

public class GeneralSettingsNavigation : UIScreenChild
{
	[SerializeField]
	private UIButton m_backButton;

	[SerializeField]
	private UIButtonProxy m_resetButton;

	[SerializeField]
	private UITable m_vBox;

	[SerializeField]
	private UIRect m_alignTarget;

	[SerializeField]
	private GameObject m_globalSwitchPrefab;

	[SerializeField]
	private GameObject m_switchPrefab;

	[SerializeField]
	private UIExtendedLabel m_optionsHeader;

	private PushNotificationSetting[] m_pushNotificationSettings;

	private GlobalPushNotificationSection m_globalSection;

	private float m_globalValue = 1f;

	private bool isOnScreenActivate;

	private bool isGlobalPrefabValueChanged;

	public static bool IsChanged;

	protected override void OnScreenInit()
	{
		ReadSettings();
		EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
		EventDelegate.Add(m_screen.onExit, OnScreenExit);
		EventDelegate.Add(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Add(m_resetButton.onClick, ResetDefault);
		GlobalPushNotificationSection.OnGlobalSwitchDelegate += OnGlobalSliderUpdate;
		PushNotificationSwitchSection.OnSingleSwitchDelegate += OnSingleSwitchUpdate;
	}

	protected override void OnScreenDestroy()
	{
		if (m_screen != null)
		{
			EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Remove(m_screen.onExit, OnScreenExit);
		}
		EventDelegate.Remove(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Remove(m_resetButton.onClick, ResetDefault);
		GlobalPushNotificationSection.OnGlobalSwitchDelegate -= OnGlobalSliderUpdate;
		PushNotificationSwitchSection.OnSingleSwitchDelegate -= OnSingleSwitchUpdate;
	}

	private void OnScreenExit()
	{
		m_vBox.transform.ClearChildren();
	}

	private void OnScreenActivate()
	{
		isOnScreenActivate = true;
		m_optionsHeader.text = LocalizationManager.Instance.Get(2128);
		GameObject gameObject = NGUITools.AddChild(m_vBox.gameObject, m_globalSwitchPrefab);
		UIWidget component = gameObject.GetComponent<UIWidget>();
		UIUtil.UpdateLeftAndRightAnchorsForTarget(component, m_alignTarget);
		m_globalSection = gameObject.GetComponent<GlobalPushNotificationSection>();
		if (m_globalSection.m_switchSlider.value != m_globalValue)
		{
			m_globalSection.m_switchSlider.value = m_globalValue;
			isGlobalPrefabValueChanged = true;
		}
		Array values = Enum.GetValues(typeof(EPushNotificationGroup));
		foreach (object item in values)
		{
			EPushNotificationGroup ePushNotificationGroup = (EPushNotificationGroup)(int)item;
			if (ePushNotificationGroup != EPushNotificationGroup.SharedMissionGroup && ePushNotificationGroup != EPushNotificationGroup.PremiumAccountGroup && ePushNotificationGroup != EPushNotificationGroup.FriendGroup && ePushNotificationGroup != EPushNotificationGroup.SkillTrainingGroup && ePushNotificationGroup != EPushNotificationGroup.BoostGroup)
			{
				GameObject gameObject2 = NGUITools.AddChild(m_vBox.gameObject, m_switchPrefab);
				UIWidget component2 = gameObject2.GetComponent<UIWidget>();
				UIUtil.UpdateLeftAndRightAnchorsForTarget(component2, m_alignTarget);
				PushNotificationSwitchSection component3 = gameObject2.GetComponent<PushNotificationSwitchSection>();
				component3.PushNotificationGroup = ePushNotificationGroup;
			}
		}
		m_vBox.repositionNow = true;
		UpdateSliders();
	}

	private void ResetDefault()
	{
		OnGlobalSliderValueChange(true);
		UpdateAllSettings(true);
	}

	private void OnGlobalSliderUpdate()
	{
		if (isGlobalPrefabValueChanged)
		{
			isGlobalPrefabValueChanged = false;
			return;
		}
		if (isOnScreenActivate)
		{
			isOnScreenActivate = false;
			return;
		}
		IsChanged = true;
		bool isEnabled = m_globalSection.m_switchSlider.value == 1f;
		OnGlobalSliderValueChange(isEnabled);
		UpdateAllSettings(isEnabled);
	}

	private void OnGlobalSliderValueChange(bool isEnabled)
	{
		m_globalValue = ((!isEnabled) ? 0f : 1f);
		PlayerPrefs.SetFloat("GlobalNotificationSectionValue", m_globalValue);
		m_globalSection.SetSliderValue(m_globalValue);
	}

	private void OnSingleSwitchUpdate()
	{
		if (!isOnScreenActivate)
		{
			IsChanged = true;
		}
	}

	private void UpdateAllSettings(bool isEnabled)
	{
		PushNotificationSetting[] pushNotificationSettings = m_pushNotificationSettings;
		foreach (PushNotificationSetting pushNotificationSetting in pushNotificationSettings)
		{
			if (pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.SharedMissionGroup || pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.PremiumAccountGroup || pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.FriendGroup)
			{
				pushNotificationSetting.value = false;
			}
			else
			{
				pushNotificationSetting.value = isEnabled;
			}
		}
		UpdateSliders();
	}

	private void UpdateSliders()
	{
		List<Transform> childList = m_vBox.GetChildList();
		foreach (Transform item in childList)
		{
			PushNotificationSwitchSection component = item.GetComponent<PushNotificationSwitchSection>();
			if (!(component != null))
			{
				continue;
			}
			PushNotificationSetting[] pushNotificationSettings = m_pushNotificationSettings;
			foreach (PushNotificationSetting pushNotificationSetting in pushNotificationSettings)
			{
				if (component.PushNotificationGroup == pushNotificationSetting.pushNotificationGroup)
				{
					component.SetSliderValue((!pushNotificationSetting.value) ? 0f : 1f);
				}
			}
		}
	}

	public override void OnBackKey()
	{
		if (IsChanged)
		{
			SaveSettings();
		}
	}

	private void GoBackToPreviousScreen()
	{
		if (IsChanged)
		{
			SaveSettings();
		}
	}

	private void ReadSettings()
	{
		m_globalValue = PlayerPrefs.GetFloat("GlobalNotificationSectionValue", 1f);
		m_pushNotificationSettings = Singleton<StateManager>.Instance.ProfileManager.Profile.userSettings.pushNotificationSettings;
	}

	private void SaveSettings()
	{
		m_globalValue = m_globalSection.m_switchSlider.value;
		PlayerPrefs.SetFloat("GlobalNotificationSectionValue", m_globalValue);
		List<Transform> childList = m_vBox.GetChildList();
		foreach (Transform item in childList)
		{
			PushNotificationSwitchSection component = item.GetComponent<PushNotificationSwitchSection>();
			if (!(component != null))
			{
				continue;
			}
			PushNotificationSetting[] pushNotificationSettings = m_pushNotificationSettings;
			foreach (PushNotificationSetting pushNotificationSetting in pushNotificationSettings)
			{
				if (pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.SharedMissionGroup || pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.PremiumAccountGroup || pushNotificationSetting.pushNotificationGroup == EPushNotificationGroup.FriendGroup)
				{
					pushNotificationSetting.value = false;
				}
				else if (pushNotificationSetting.pushNotificationGroup == component.PushNotificationGroup)
				{
					pushNotificationSetting.value = component.GetSliderValue() == 1f;
				}
			}
		}
		Singleton<StateManager>.Instance.ProfileManager.Profile.userSettings.pushNotificationSettings = m_pushNotificationSettings;
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.ModifyUserSettings(Singleton<StateManager>.Instance.ProfileManager.Profile.userSettings, _asyncResult);
		}, null, delegate(IAsyncResult _response)
		{
			if (_response.IsError())
			{
			}
			IsChanged = false;
			Singleton<UIScreenManager>.Instance.PopScreen();
			Singleton<UIScreenManager>.Instance.PushScreen(Singleton<UIScreenManager>.Instance.GetScreen(m_screen.m_previousScreen), null);
		});
	}
}
