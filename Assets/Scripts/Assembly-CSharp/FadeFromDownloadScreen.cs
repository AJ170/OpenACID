using System;
using Assets.Scripts.UI;
using UnityEngine;

public class FadeFromDownloadScreen : MonoBehaviour
{
	[SerializeField]
	private TweenAlpha m_fade;

	private void Start()
	{
		UIDownloadScreenDetails.DownloadScreenClosedEvent = (UIDownloadScreenDetails.DownloadScreenClosed)Delegate.Combine(UIDownloadScreenDetails.DownloadScreenClosedEvent, new UIDownloadScreenDetails.DownloadScreenClosed(StartFade));
	}

	private void OnDestroy()
	{
		UIDownloadScreenDetails.DownloadScreenClosedEvent = (UIDownloadScreenDetails.DownloadScreenClosed)Delegate.Remove(UIDownloadScreenDetails.DownloadScreenClosedEvent, new UIDownloadScreenDetails.DownloadScreenClosed(StartFade));
	}

	private void StartFade(bool fade)
	{
		if (fade)
		{
			m_fade.duration = 1.5f;
		}
		else
		{
			m_fade.duration = 0.01f;
		}
		m_fade.ResetToBeginning();
		m_fade.PlayForward();
	}

	public void TurnOFffSelf()
	{
		base.gameObject.SetActive(false);
	}
}
