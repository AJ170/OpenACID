using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIKillTokenBar : UIBase
	{
		private UIKillTokenBarBehavior m_barBehaviour;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.KillTokenBar;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_barBehaviour;
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return true;
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

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				InitLogic();
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			if (m_barBehaviour != null && UbiCore.Utils.Singleton<UiMain>.Instance != null && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatKillTokenUiEvent>(OnCombatKillTokenUiEvent);
			}
			m_barBehaviour = null;
		}

		private void OnCombatKillTokenUiEvent(CombatKillTokenUiEvent _event)
		{
			m_barBehaviour.OnCombatKillTokenUiEvent(_event);
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				SetVisible(false);
				base.IsInitialized = false;
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				SetVisible(false);
				if (m_barBehaviour != null)
				{
					m_barBehaviour.Exclude();
				}
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			base.OnUIAlignmentChanged(_isRightHanded);
			if (m_barBehaviour != null)
			{
				m_barBehaviour.OnUIAlignmentChanged(_isRightHanded);
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			if (m_barBehaviour == null || UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.InProgress)
			{
				return;
			}
			if (m_barBehaviour.UpdateProgress())
			{
				if (!m_barBehaviour.IsVisible)
				{
					SetVisible(true);
				}
			}
			else if (m_barBehaviour.IsVisible)
			{
				SetVisible(false);
			}
		}

		protected void Init()
		{
			if (!(m_barBehaviour != null))
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_barBehaviour = widgetByType as UIKillTokenBarBehavior;
					m_barBehaviour.Reset();
				}
			}
		}

		protected void InitLogic()
		{
			if (m_barBehaviour != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatKillTokenUiEvent>(OnCombatKillTokenUiEvent);
				base.IsInitialized = true;
				m_barBehaviour.Show();
			}
		}
	}
}
