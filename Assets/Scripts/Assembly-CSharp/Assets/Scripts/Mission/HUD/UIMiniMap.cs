using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using UbiCore.Messenger;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIMiniMap : UIBase
	{
		private UIMiniMapBehaviour m_uiMiniMapBehaviour;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MiniMap;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_uiMiniMapBehaviour;
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			m_uiMiniMapBehaviour.UpdateFrame(_deltaTime);
		}

		public override bool Tap(TapEvent _event)
		{
			return false;
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
			m_uiMiniMapBehaviour = null;
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			SetVisible(false);
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				InitLogic();
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				if (m_uiMiniMapBehaviour != null)
				{
					m_uiMiniMapBehaviour.OnMissionUnload();
					m_uiMiniMapBehaviour = null;
				}
			}
		}

		public override void OnMissionUnload()
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet -= OnAreasChanged;
			base.OnMissionUnload();
		}

		private void Init()
		{
			if (!(m_uiMiniMapBehaviour != null))
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_uiMiniMapBehaviour = widgetByType as UIMiniMapBehaviour;
				}
				base.IsInitialized = true;
			}
		}

		private void InitLogic()
		{
			if (base.IsInitialized && m_uiMiniMapBehaviour != null)
			{
				m_uiMiniMapBehaviour.Init();
				m_uiMiniMapBehaviour.OnMissionLoaded();
				UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet += OnAreasChanged;
			}
		}

		private void OnAreasChanged()
		{
			if (m_uiMiniMapBehaviour != null)
			{
				m_uiMiniMapBehaviour.OnAreasChanged();
			}
		}
	}
}
