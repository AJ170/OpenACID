using System.Collections.Generic;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIObjectiveTracker : UIBase
	{
		private UIObjectiveTrackerBehaviour m_objectiveTrackerBehaviour;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.ObjectiveTracker;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_objectiveTrackerBehaviour;
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		private bool IsMissionRunning
		{
			get
			{
				return UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.InProgress || UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Failed || UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Success;
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
			m_objectiveTrackerBehaviour = null;
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionStateChanged;
			}
		}

		public void SetPaused(bool _isPaused)
		{
			if (m_objectiveTrackerBehaviour != null)
			{
				m_objectiveTrackerBehaviour.SetPaused(_isPaused);
			}
		}

		private void OnMissionStateChanged(EMissionState _state)
		{
			switch (_state)
			{
			case EMissionState.InProgress:
				OnMissionTrackerChanged();
				UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.OnMissionTrackerChanged += OnMissionTrackerChanged;
				break;
			case EMissionState.ShowIntroduction:
				m_objectiveTrackerBehaviour.Exclude();
				break;
			}
		}

		public override void OnMissionUnload()
		{
			base.OnMissionUnload();
			if (!(m_objectiveTrackerBehaviour == null))
			{
				m_objectiveTrackerBehaviour.Exclude();
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionStateChanged;
				UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.OnMissionTrackerChanged -= OnMissionTrackerChanged;
				base.IsInitialized = false;
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (!(m_objectiveTrackerBehaviour == null))
			{
				m_objectiveTrackerBehaviour.Exclude();
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (IsMissionRunning && !base.HasMissionEnded)
			{
				CheckInput();
				m_objectiveTrackerBehaviour.UpdateTracker(false);
			}
		}

		private void CheckInput()
		{
			if (!(Singleton<UIScreenManager>.Instance.CurrentModalScreen != null) && !(Singleton<UIScreenManager>.Instance.CurrentScreen != null) && !OptionsScreenHelper.IsOptionsScreen() && m_objectiveTrackerBehaviour.gameObject.activeInHierarchy)
			{
				GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
				if (gamepad.ShowObjectivesUp)
				{
					m_objectiveTrackerBehaviour.OnTapIcon();
				}
			}
		}

		private void Init()
		{
			if (m_objectiveTrackerBehaviour != null)
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				m_objectiveTrackerBehaviour = widgetByType as UIObjectiveTrackerBehaviour;
				if (m_objectiveTrackerBehaviour != null)
				{
					m_objectiveTrackerBehaviour.Init();
					m_objectiveTrackerBehaviour.Exclude();
					base.IsInitialized = true;
				}
			}
		}

		private void OnMissionTrackerChanged()
		{
			bool flag = false;
			List<MissionTracker.ObjectiveData> collectedObjectiveData = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.CollectedObjectiveData;
			if (collectedObjectiveData.Count > 0)
			{
				MissionTracker.ObjectiveData objectiveData = collectedObjectiveData[collectedObjectiveData.Count - 1];
				flag = !objectiveData.ObjectivesBaseData[0].ObjectiveData.HideDescription;
				if (flag)
				{
					m_objectiveTrackerBehaviour.ChangePhase(objectiveData.PrimaryObjectiveID, objectiveData.ComputeModifierHash());
				}
			}
			SetVisible(flag);
			if (flag)
			{
				m_objectiveTrackerBehaviour.UpdateTracker(false);
			}
		}
	}
}
