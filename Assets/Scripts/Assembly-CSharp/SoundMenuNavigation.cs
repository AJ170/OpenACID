using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.System.Extensions;
using UnityEngine;

public class SoundMenuNavigation : UIScreenChild
{
	[Serializable]
	public class UISoundSliderMapEntry
	{
		public UISlider Slider;

		public ELayer ControlledChannel;

		public bool IsThemeToggle;

		public GenericToggleHighlighter GenericToggleHighlighter;
	}

	private static class OptionStrings
	{
		public const string MasterVolume = "MasterVolume";

		public const string MusicVolume = "MusicVolume";

		public const string UiVolume = "UIVolume";

		public const string ThemeOn = "ThemeOn";
	}

	[SerializeField]
	private UIExtendedButton m_backButton;

	[SerializeField]
	private UIButtonProxy m_resetButton;

	[SerializeField]
	private List<UISoundSliderMapEntry> m_toggles;

	[SerializeField]
	private UISprite m_background;

	private bool m_listenToEvents;

	public static bool IsMute
	{
		get
		{
			float value = ((!PlayerPrefs.HasKey("MasterVolume")) ? 1f : PlayerPrefs.GetFloat("MasterVolume"));
			return Math.Abs(value) < 0.001f;
		}
		set
		{
			if (AudioManager.IsAvailable)
			{
				AudioManager.Instance.Volume = ((!value) ? 1f : 0f);
				PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.Volume);
			}
		}
	}

	protected override void OnScreenInit()
	{
		if (!Main.Instance.SceneLoader.InAnimus)
		{
			m_background.alpha = 0.8f;
		}
		else
		{
			m_background.alpha = 1f;
		}
		EventDelegate.Add(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Add(m_resetButton.onClick, ResetDefault);
		EventDelegate.Add(m_screen.onActivate, OnActivateListeners);
		EventDelegate.Add(m_screen.onDeActivate, OnDeactivateListeners);
		ReadSettings();
		SaveSettings();
	}

	private void OnDeactivateListeners()
	{
		m_listenToEvents = false;
	}

	public void OnActivateListeners()
	{
		m_listenToEvents = true;
	}

	protected override void OnScreenDestroy()
	{
		EventDelegate.Remove(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Remove(m_resetButton.onClick, ResetDefault);
		if (m_screen != null)
		{
			EventDelegate.Remove(m_screen.onActivate, OnActivateListeners);
			EventDelegate.Remove(m_screen.onDeActivate, OnDeactivateListeners);
		}
	}

	private void ResetDefault()
	{
		AudioManager.Instance.Volume = 1f;
		AudioManager.Instance.SetVolume(ELayer.Effect, 1f);
		AudioManager.Instance.SetVolume(ELayer.Music, 1f);
		AudioManager.Instance.SetVolume(ELayer.UI, 1f);
		AudioManager.Instance.SetVolume(ELayer.Environment, 1f);
		AudioManager.Instance.ThemeSongOn = true;
		SaveSettings();
		ReadSettings();
	}

	public void OnToggle(UISlider _toggle)
	{
		if (!m_listenToEvents)
		{
			return;
		}
		UISoundSliderMapEntry uISoundSliderMapEntry = m_toggles.FirstOrDefault((UISoundSliderMapEntry x) => x.Slider == _toggle);
		if (uISoundSliderMapEntry == null || !IsSliderInteractible(uISoundSliderMapEntry))
		{
			return;
		}
		bool flag = (double)_toggle.value >= 0.5;
		float volume = (flag ? 1 : 0);
		if (uISoundSliderMapEntry.ControlledChannel == ELayer.Invalid)
		{
			if (!uISoundSliderMapEntry.IsThemeToggle)
			{
				AudioManager.Instance.Volume = volume;
			}
			else
			{
				AudioManager.Instance.ThemeSongOn = flag;
			}
		}
		else
		{
			AudioManager.Instance.SetVolume(uISoundSliderMapEntry.ControlledChannel, volume);
		}
		ApplySliderConfiguration();
	}

	private void SetSliderPosition(bool _isOn, UISoundSliderMapEntry _slider)
	{
		int num = (_isOn ? 1 : 0);
		_slider.Slider.value = num;
		_slider.GenericToggleHighlighter.OnValueChange();
	}

	public override void OnBackKey()
	{
		GoBackToPreviousScreen();
	}

	private void GoBackToPreviousScreen()
	{
		SaveSettings();
		TrackingManager.AddSoundsTrackPoint(PlayerPrefs.GetFloat("MasterVolume") == 1f, PlayerPrefs.GetFloat("UIVolume") == 1f, PlayerPrefs.GetFloat("MusicVolume") == 1f, PlayerPrefsExtension.GetBool("ThemeOn"));
		Singleton<UIScreenManager>.Instance.PopScreen();
		Singleton<UIScreenManager>.Instance.PushScreen(Singleton<UIScreenManager>.Instance.GetScreen(m_screen.m_previousScreen), null);
	}

	private void ReadSettings()
	{
		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			AudioManager.Instance.SetVolume(ELayer.Music, PlayerPrefs.GetFloat("MusicVolume"));
		}
		if (PlayerPrefs.HasKey("UIVolume"))
		{
			AudioManager.Instance.SetVolume(ELayer.UI, PlayerPrefs.GetFloat("UIVolume"));
		}
		if (PlayerPrefs.HasKey("ThemeOn"))
		{
			AudioManager.Instance.ThemeSongOn = PlayerPrefsExtension.GetBool("ThemeOn");
		}
		if (PlayerPrefs.HasKey("MasterVolume"))
		{
			AudioManager.Instance.Volume = PlayerPrefs.GetFloat("MasterVolume");
		}
		ApplySliderConfiguration();
		AudioManager.Instance.SetVolume(ELayer.Environment, 1f);
	}

	private void ApplySliderConfiguration()
	{
		foreach (UISoundSliderMapEntry toggle in m_toggles)
		{
			bool flag = IsSliderOn(toggle);
			bool flag2 = IsSliderInteractible(toggle);
			SetSliderPosition(flag && flag2, toggle);
			toggle.Slider.GetComponent<Collider>().enabled = flag2;
		}
	}

	private static bool IsSliderInteractible(UISoundSliderMapEntry _volumeInterface)
	{
		if (AudioManager.Instance.Volume < 0.5f)
		{
			return _volumeInterface.ControlledChannel == ELayer.Invalid && !_volumeInterface.IsThemeToggle;
		}
		if (AudioManager.Instance.GetVolume(ELayer.Music) < 0.5f)
		{
			return _volumeInterface.ControlledChannel != 0 || !_volumeInterface.IsThemeToggle;
		}
		return true;
	}

	private static bool IsSliderOn(UISoundSliderMapEntry _slider)
	{
		if (_slider.IsThemeToggle)
		{
			return AudioManager.Instance.ThemeSongOn;
		}
		return ((_slider.ControlledChannel != 0) ? AudioManager.Instance.GetVolume(_slider.ControlledChannel) : AudioManager.Instance.Volume) >= 0.5f;
	}

	private void SaveSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.Volume);
		PlayerPrefs.SetFloat("MusicVolume", AudioManager.Instance.GetVolume(ELayer.Music));
		PlayerPrefs.SetFloat("UIVolume", AudioManager.Instance.GetVolume(ELayer.UI));
		PlayerPrefsExtension.SetBool("ThemeOn", AudioManager.Instance.ThemeSongOn);
	}
}
