using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Extensions;
using Mission;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIMissionIntroduction : UIBase
	{
		private UIMissionIntroductionBehaviour m_missionIntroductionBehaviour;

		private AnimationEventListener m_listener;

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MissionIntroduction;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_missionIntroductionBehaviour;
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
			if (m_missionIntroductionBehaviour != null)
			{
				UIMissionIntroductionBehaviour missionIntroductionBehaviour = m_missionIntroductionBehaviour;
				missionIntroductionBehaviour.OnTapScreen = (UIMissionIntroductionBehaviour.TapScreen)Delegate.Remove(missionIntroductionBehaviour.OnTapScreen, new UIMissionIntroductionBehaviour.TapScreen(OnTapScreen));
			}
			m_missionIntroductionBehaviour = null;
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			base.IsActive = UbiCore.Utils.Singleton<UiMain>.Instance.MissionType != 0 && UbiCore.Utils.Singleton<MissionManager>.Instance.DoIntroduction;
			if (!IsActive)
			{
				return;
			}
			InitLogic();
			UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionStateChanged;
			GameObject gameObject = CameraStateIntroduction.FindSceneIntroAnimation();
			if (gameObject != null)
			{
				m_listener = gameObject.GetComponent<AnimationEventListener>();
				if (m_listener != null)
				{
					AnimationEventListener listener = m_listener;
					listener.OnEvent = (AnimationEventListener.OnEventDelegate)Delegate.Combine(listener.OnEvent, new AnimationEventListener.OnEventDelegate(OnAnimationEvent));
				}
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				if (m_listener != null)
				{
					AnimationEventListener listener = m_listener;
					listener.OnEvent = (AnimationEventListener.OnEventDelegate)Delegate.Remove(listener.OnEvent, new AnimationEventListener.OnEventDelegate(OnAnimationEvent));
				}
				UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionStateChanged;
				SetVisible(false);
			}
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			if (IsActive)
			{
				SetVisible(false);
			}
		}

		public void Init()
		{
			if (m_missionIntroductionBehaviour != null)
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				m_missionIntroductionBehaviour = widgetByType as UIMissionIntroductionBehaviour;
				if (m_missionIntroductionBehaviour != null)
				{
					UIMissionIntroductionBehaviour missionIntroductionBehaviour = m_missionIntroductionBehaviour;
					missionIntroductionBehaviour.OnTapScreen = (UIMissionIntroductionBehaviour.TapScreen)Delegate.Combine(missionIntroductionBehaviour.OnTapScreen, new UIMissionIntroductionBehaviour.TapScreen(OnTapScreen));
					m_missionIntroductionBehaviour.Init();
					base.IsInitialized = true;
				}
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			CheckInput();
		}

		public void InitLogic()
		{
			if (base.IsInitialized)
			{
				SetData();
			}
		}

		private void SetData()
		{
			MissionDescExt currentMissionDescription = Main.Instance.SceneLoader.CurrentMissionDescription;
			if (currentMissionDescription.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
			{
				m_missionIntroductionBehaviour.SetData(LocalizationManager.Instance.Get(1776));
			}
			else if (currentMissionDescription.ObjectiveType != EObjectiveType.Story_EObjectiveType)
			{
				RegionDefinition regionDefinition = currentMissionDescription.GetRegionDefinition();
				string data = currentMissionDescription.LocaName(regionDefinition);
				m_missionIntroductionBehaviour.SetData(data);
			}
		}

		public void OnTapScreen()
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.ShowIntroduction)
			{
				return;
			}
			if (AcierCameraController.Instance.CurrentStateType == AcierCameraController.ECameraState.Introduction)
			{
				CameraStateIntroduction cameraStateIntroduction = (CameraStateIntroduction)AcierCameraController.Instance.CurrentState;
				UbiCore.Utils.Singleton<MissionManager>.Instance.Go();
				if (cameraStateIntroduction != null)
				{
					cameraStateIntroduction.OnAbortedIntroduction();
				}
				if (m_missionIntroductionBehaviour != null)
				{
					m_missionIntroductionBehaviour.Exclude();
				}
			}
			else
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.Go();
				if (m_missionIntroductionBehaviour != null)
				{
					m_missionIntroductionBehaviour.Exclude();
				}
			}
		}

		private void CheckInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (gamepad.AcceptDown || gamepad.CloseDown)
			{
				OnTapScreen();
			}
		}

		private void OnMissionStateChanged(EMissionState _state)
		{
			if (IsActive)
			{
				SetVisible(_state == EMissionState.ShowIntroduction);
			}
		}

		private void OnAnimationEvent(string _desc)
		{
			if (m_missionIntroductionBehaviour != null && _desc == "ShowUI")
			{
				CameraStateIntroduction cameraStateIntroduction = AcierCameraController.Instance.CurrentState as CameraStateIntroduction;
				Introduction introduction = cameraStateIntroduction.Introduction;
				if (introduction != null)
				{
					m_missionIntroductionBehaviour.ShowIntroductionText(introduction.UILocaStringID, introduction.UIAlignment, introduction.UIFontOffset, introduction.UIFontColor, introduction.UIFontSize);
				}
			}
		}
	}
}
