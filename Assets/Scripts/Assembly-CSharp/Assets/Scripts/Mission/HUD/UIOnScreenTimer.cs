using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIOnScreenTimer : UIBase
	{
		private const float TimerSoundStartAtPercentageLeft = 0.2f;

		private UIOnScreenTimerBehaviour m_onScreenTimerBehaviour;

		private bool m_hasText;

		private int m_lastTimeDisplayed = -1;

		private int m_startTime;

		private IAudioLoadHandle m_audioTimerTick;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenTimer;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_onScreenTimerBehaviour;
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			if (base.IsActiveOnSceneLoaded)
			{
				Init();
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			base.IsInitialized = false;
			m_onScreenTimerBehaviour = null;
			if (m_audioTimerTick != null)
			{
				m_audioTimerTick.Unload();
				m_audioTimerTick = null;
			}
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			if (IsActive)
			{
				Reset();
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.InProgress)
			{
				SetVisible(false);
				return;
			}
			base.UpdateFrame(_deltaTime);
			int num = -1;
			MissionTracker.ObjectiveData lastPhaseObjectiveData = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.GetLastPhaseObjectiveData();
			if (lastPhaseObjectiveData != null)
			{
				List<IObjectiveBase> visibleObjectives = lastPhaseObjectiveData.GetVisibleObjectives();
				for (int i = 0; i < visibleObjectives.Count; i++)
				{
					IObjectiveTimer objectiveTimer = visibleObjectives[i] as IObjectiveTimer;
					if (objectiveTimer != null && objectiveTimer.HasTimer)
					{
						num = objectiveTimer.GetTime();
						break;
					}
				}
			}
			if (m_lastTimeDisplayed != num)
			{
				OnTimerChanged(num);
				m_lastTimeDisplayed = num;
			}
			SetVisible(m_hasText);
		}

		public void SetTimerText(int _timeInSecond)
		{
			if (!base.HasMissionEnded)
			{
				m_hasText = _timeInSecond > 0;
				m_onScreenTimerBehaviour.SetDisplayTime(_timeInSecond);
			}
		}

		private void Reset()
		{
			if ((bool)m_onScreenTimerBehaviour)
			{
				m_onScreenTimerBehaviour.ClearTimer();
			}
			m_hasText = false;
			m_lastTimeDisplayed = -1;
			m_startTime = 0;
		}

		private void Init()
		{
			if (m_onScreenTimerBehaviour != null)
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				m_onScreenTimerBehaviour = widgetByType as UIOnScreenTimerBehaviour;
				if (m_onScreenTimerBehaviour == null)
				{
					return;
				}
				Reset();
				SetVisible(false);
			}
			m_audioTimerTick = AudioManager.Instance.CreateLoadHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_timer_1s_ping);
			m_audioTimerTick.Load();
			base.IsInitialized = true;
		}

		private void OnTimerChanged(int _timeInSecond)
		{
			if (m_lastTimeDisplayed == -1)
			{
				OnTimerStarted(_timeInSecond);
			}
			HandleTimerSound(_timeInSecond);
			SetTimerText(_timeInSecond);
		}

		private void OnTimerStarted(int _timeInSecond)
		{
			m_startTime = _timeInSecond;
		}

		private void HandleTimerSound(int _timeInSecond)
		{
			if ((float)_timeInSecond / (float)m_startTime <= 0.2f)
			{
				AudioManager.Instance.Play(m_audioTimerTick.AudioControllerId);
			}
		}
	}
}
