using System;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Extensions;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIPause : UIBase
	{
		private UIPauseBehaviour m_pauseBehaviour;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Pause;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_pauseBehaviour;
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

		public override void OnMissionEnded()
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
			if (screen != null)
			{
				OptionsMenuNavigation componentInChildren = screen.GetComponentInChildren<OptionsMenuNavigation>();
				if (componentInChildren != null)
				{
					componentInChildren.PreviousPauseState = Timer.EPauseState.Running;
				}
			}
			OptionsScreenHelper.Close();
			base.OnMissionEnded();
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			base.IsInitialized = false;
			if (m_pauseBehaviour != null)
			{
				UIPauseBehaviour pauseBehaviour = m_pauseBehaviour;
				pauseBehaviour.OnTapPause = (UIPauseBehaviour.TapPause)Delegate.Remove(pauseBehaviour.OnTapPause, new UIPauseBehaviour.TapPause(OnTapPause));
			}
			m_pauseBehaviour = null;
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive && m_pauseBehaviour != null)
			{
				m_pauseBehaviour.Exclude();
			}
		}

		public void Init()
		{
			if (m_pauseBehaviour != null)
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				m_pauseBehaviour = widgetByType as UIPauseBehaviour;
				if (m_pauseBehaviour == null)
				{
					return;
				}
				UIPauseBehaviour pauseBehaviour = m_pauseBehaviour;
				pauseBehaviour.OnTapPause = (UIPauseBehaviour.TapPause)Delegate.Combine(pauseBehaviour.OnTapPause, new UIPauseBehaviour.TapPause(OnTapPause));
				UpdateButtonState();
			}
			base.IsInitialized = true;
			SetVisible(false);
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (PlayerHelper.IsDead)
			{
				OptionsScreenHelper.DeactivateMenu();
				return;
			}
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (gamepad.PauseDown)
			{
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
				if (screen.Active || UIGamepadHandler.CanHitCollider(m_pauseBehaviour.gameObject))
				{
					OnTapPause();
				}
			}
		}

		private void UpdateButtonState()
		{
			if (EntitlementExtensions.HasCompletedIntroMission())
			{
				m_pauseBehaviour.SetToOptionButton();
			}
			else
			{
				m_pauseBehaviour.SetToSoundButton(!SoundMenuNavigation.IsMute);
			}
		}

		public void OnTapPause()
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Success || UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Failed)
			{
				return;
			}
			if (EntitlementExtensions.HasCompletedIntroMission())
			{
				UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
				if (!screen.Active)
				{
					OptionsScreenHelper.ActivateMenu();
				}
				else
				{
					OptionsScreenHelper.DeactivateMenu();
				}
			}
			else
			{
				bool isMute = SoundMenuNavigation.IsMute;
				SoundMenuNavigation.IsMute = !isMute;
			}
			UpdateButtonState();
		}
	}
}
