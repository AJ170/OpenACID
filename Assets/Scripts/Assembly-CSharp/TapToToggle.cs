using System;
using UnityEngine;

public class TapToToggle : MonoBehaviour
{
	public UISlider Slider;

	private float m_changedTime;

	public void Awake()
	{
		EventDelegate.Add(Slider.onChange, OnChange);
		UISlider slider = Slider;
		slider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(slider.onDragFinished, new UIProgressBar.OnDragFinished(OnDragFinished));
	}

	public void OnChange()
	{
		m_changedTime = Time.realtimeSinceStartup;
	}

	public void OnDragFinished()
	{
		if (Time.realtimeSinceStartup > m_changedTime + 1f)
		{
			Slider.value = 1f - Slider.value;
		}
		m_changedTime = 0f;
	}
}
