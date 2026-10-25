using System;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

public class ControlsNavigation : UIScreenChild
{
	[SerializeField]
	private UIButton m_backButton;

	[SerializeField]
	private UIButtonProxy m_resetButton;

	[SerializeField]
	private UISlider m_invertXAxis;

	[SerializeField]
	private UISlider m_invertYAxis;

	[SerializeField]
	private UISlider m_toggleLeftHanded;

	[SerializeField]
	private UISlider m_toggleParkourPreview;

	[SerializeField]
	private UISlider m_sensitivitySlider;

	[SerializeField]
	private UIButtonProxy m_sensitivityIncreaseButton;

	[SerializeField]
	private UIButtonProxy m_sensitivityDecreaseButton;

	[SerializeField]
	private UISprite m_background;

	private float m_sensitivity = 0.5f;

	private bool m_XInverted;

	private bool m_YInverted;

	private bool m_leftHanded;

	private bool m_showParkourPreview = true;

	protected override void OnScreenInit()
	{
		Initialize();
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
		EventDelegate.Add(m_sensitivitySlider.onChange, UpdateSensitivity);
		EventDelegate.Add(m_sensitivityIncreaseButton.onClick, IncreaseSensitivity);
		EventDelegate.Add(m_sensitivityDecreaseButton.onClick, DecreaseSensitivity);
		EventDelegate.Add(m_invertXAxis.onChange, UpdateXAxis);
		EventDelegate.Add(m_invertYAxis.onChange, UpdateYAxis);
		EventDelegate.Add(m_toggleLeftHanded.onChange, UpdateLeftHanded);
		EventDelegate.Add(m_toggleParkourPreview.onChange, UpdateParkourPreview);
		UISlider sensitivitySlider = m_sensitivitySlider;
		sensitivitySlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(sensitivitySlider.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider invertXAxis = m_invertXAxis;
		invertXAxis.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(invertXAxis.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider invertYAxis = m_invertYAxis;
		invertYAxis.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(invertYAxis.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider toggleLeftHanded = m_toggleLeftHanded;
		toggleLeftHanded.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(toggleLeftHanded.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider toggleParkourPreview = m_toggleParkourPreview;
		toggleParkourPreview.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(toggleParkourPreview.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
	}

	protected override void OnScreenDestroy()
	{
		EventDelegate.Remove(m_backButton.onClick, GoBackToPreviousScreen);
		EventDelegate.Remove(m_resetButton.onClick, ResetDefault);
		EventDelegate.Remove(m_sensitivitySlider.onChange, UpdateSensitivity);
		EventDelegate.Remove(m_sensitivityIncreaseButton.onClick, IncreaseSensitivity);
		EventDelegate.Remove(m_sensitivityDecreaseButton.onClick, DecreaseSensitivity);
		EventDelegate.Remove(m_invertXAxis.onChange, UpdateXAxis);
		EventDelegate.Remove(m_invertYAxis.onChange, UpdateYAxis);
		EventDelegate.Remove(m_toggleLeftHanded.onChange, UpdateLeftHanded);
		EventDelegate.Remove(m_toggleParkourPreview.onChange, UpdateParkourPreview);
		UISlider sensitivitySlider = m_sensitivitySlider;
		sensitivitySlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(sensitivitySlider.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider invertXAxis = m_invertXAxis;
		invertXAxis.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(invertXAxis.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider invertYAxis = m_invertYAxis;
		invertYAxis.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(invertYAxis.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider toggleLeftHanded = m_toggleLeftHanded;
		toggleLeftHanded.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(toggleLeftHanded.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
		UISlider toggleParkourPreview = m_toggleParkourPreview;
		toggleParkourPreview.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(toggleParkourPreview.onDragFinished, new UIProgressBar.OnDragFinished(OnSliderDragFinished));
	}

	private void ResetDefault()
	{
		m_XInverted = false;
		m_YInverted = false;
		m_sensitivity = 0.5f;
		m_leftHanded = false;
		m_showParkourPreview = true;
		SaveSettings();
		Initialize();
	}

	private void OnSliderDragFinished()
	{
	}

	public override void OnBackKey()
	{
		GoBackToPreviousScreen();
	}

	private void GoBackToPreviousScreen()
	{
		SaveSettings();
		Singleton<UIScreenManager>.Instance.PopScreen();
		Singleton<UIScreenManager>.Instance.PushScreen(Singleton<UIScreenManager>.Instance.GetScreen(m_screen.m_previousScreen), null);
	}

	private void Initialize()
	{
		ReadSettings();
		m_invertXAxis.value = (m_XInverted ? 1 : 0);
		m_invertYAxis.value = (m_YInverted ? 1 : 0);
		m_toggleLeftHanded.value = (m_leftHanded ? 1 : 0);
		m_sensitivitySlider.value = m_sensitivity;
		m_toggleParkourPreview.value = ((!m_showParkourPreview) ? 1 : 0);
		UpdateSensitivity();
		UpdateParkourPreview();
		UpdateLeftHanded();
		UpdateXAxis();
		UpdateYAxis();
		SaveSettings();
	}

	private void UpdateParkourPreview()
	{
		m_showParkourPreview = m_toggleParkourPreview.value < 0.5f;
		AcierInputHandler.Instance.ShowParcourFeedback = m_showParkourPreview;
	}

	private void UpdateSensitivity()
	{
		m_sensitivity = Mathf.Clamp01(m_sensitivitySlider.value);
		AcierInputHandler.Instance.InputSensitivity = m_sensitivity;
	}

	private void IncreaseSensitivity()
	{
		m_sensitivity = Mathf.Clamp01(m_sensitivitySlider.value + ((m_sensitivitySlider.numberOfSteps <= 1) ? 0.1f : (1f / (float)m_sensitivitySlider.numberOfSteps)));
		AcierInputHandler.Instance.InputSensitivity = m_sensitivity;
		m_sensitivitySlider.value = m_sensitivity;
	}

	private void DecreaseSensitivity()
	{
		m_sensitivity = Mathf.Clamp01(m_sensitivitySlider.value - ((m_sensitivitySlider.numberOfSteps <= 1) ? 0.1f : (1f / (float)m_sensitivitySlider.numberOfSteps)));
		AcierInputHandler.Instance.InputSensitivity = m_sensitivity;
		m_sensitivitySlider.value = m_sensitivity;
	}

	private void UpdateXAxis()
	{
		m_XInverted = m_invertXAxis.value > 0.5f;
		CameraEvents.FlipXAxis = m_XInverted;
	}

	private void UpdateYAxis()
	{
		m_YInverted = m_invertYAxis.value > 0.5f;
		CameraEvents.FlipYAxis = m_YInverted;
	}

	private void UpdateLeftHanded()
	{
		m_leftHanded = m_toggleLeftHanded.value > 0.5f;
		UiMain.UIAlignmentRightHanded = m_leftHanded;
	}

	private void ReadSettings()
	{
		if (PlayerPrefs.HasKey("Sensitivity"))
		{
			m_sensitivity = Mathf.Clamp01(PlayerPrefs.GetFloat("Sensitivity"));
		}
		if (PlayerPrefs.HasKey("XInverted"))
		{
			m_XInverted = PlayerPrefsExtension.GetBool("XInverted");
		}
		if (PlayerPrefs.HasKey("YInverted"))
		{
			m_YInverted = PlayerPrefsExtension.GetBool("YInverted");
		}
		if (PlayerPrefs.HasKey("LeftHanded"))
		{
			m_leftHanded = PlayerPrefsExtension.GetBool("LeftHanded");
		}
		if (PlayerPrefs.HasKey("ShowParkourPreview"))
		{
			m_showParkourPreview = PlayerPrefsExtension.GetBool("ShowParkourPreview");
		}
	}

	private void SaveSettings()
	{
		PlayerPrefs.SetFloat("Sensitivity", m_sensitivity);
		PlayerPrefsExtension.SetBool("XInverted", m_XInverted);
		PlayerPrefsExtension.SetBool("YInverted", m_YInverted);
		PlayerPrefsExtension.SetBool("LeftHanded", m_leftHanded);
		PlayerPrefsExtension.SetBool("ShowParkourPreview", m_showParkourPreview);
	}
}
