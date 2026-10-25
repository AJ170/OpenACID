using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Extensions;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class InterruptHandler
{
	private bool m_ToggleSound;

	private bool isTriggered;

	public void OnPause(bool pause)
	{
		if ((pause && isTriggered) || (!pause && !isTriggered))
		{
			return;
		}
		isTriggered = !isTriggered;
		if (Singleton<UIScreenManager>.Instance != null && UbiCore.Utils.Singleton<MissionManager>.Instance != null && UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.InProgress)
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
			if (screen != null)
			{
				ShowPauseMenu(pause);
			}
		}
		ToggleSound(pause);
	}

	private IEnumerator ShowDelayedPauseMenu()
	{
		yield return new WaitForSeconds(0.2f);
		OptionsScreenHelper.ActivateMenu();
	}

	private void ShowPauseMenu(bool isPaused)
	{
		if (EntitlementExtensions.HasCompletedIntroMission())
		{
			if (!isPaused)
			{
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
				if (!screen.Active)
				{
					Main.Instance.StartCoroutine(ShowDelayedPauseMenu());
				}
			}
		}
		else
		{
			HandleIntroMission(isPaused);
		}
	}

	private void HandleIntroMission(bool isPaused)
	{
		if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused() && isPaused)
		{
			UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseOptions);
		}
		else if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused() || UbiCore.Utils.Singleton<Timer>.Instance.PauseState != Timer.EPauseState.PauseDialog)
		{
			UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.Running);
		}
	}

	private void ToggleSound(bool pause)
	{
		if (AudioManager.IsAvailable && (AudioManager.Instance.Volume != 0f || m_ToggleSound))
		{
			m_ToggleSound = !m_ToggleSound;
			AudioManager.Instance.Volume = ((!pause) ? 1f : 0f);
		}
	}
}
