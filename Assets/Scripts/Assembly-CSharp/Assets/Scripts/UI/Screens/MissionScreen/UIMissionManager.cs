using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.MissionDetailsScreen;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionManager : UIScreenChild
	{
		[SerializeField]
		private UIMissionListCampaigns m_missionListCampaigns;

		[SerializeField]
		private UIMissionListCampaignMissions m_missionListCampaignMissions;

		[SerializeField]
		private UIMissionListContractMissions m_missionListContractMissions;

		[SerializeField]
		private MissionDetailsData m_missionDetailsData;

		[SerializeField]
		private EarthPinManager m_earthPinManager;

		private UIMissionSubMenuBase m_currentMissionSubMenuBase;

		protected override void OnScreenInit()
		{
			m_earthPinManager.OnInit();
			m_missionListCampaigns.OnInit(m_earthPinManager);
			m_missionListCampaignMissions.OnInit(m_earthPinManager);
			m_missionListContractMissions.OnInit(m_earthPinManager);
			m_missionDetailsData.OnInit(m_earthPinManager);
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.FixLocalStorageSeenMissionProgression();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivate);
			}
			if (m_currentMissionSubMenuBase != null)
			{
				m_currentMissionSubMenuBase.OnDeactivate();
			}
			m_currentMissionSubMenuBase = null;
		}

		private void UpdateMissionScreen()
		{
			MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
			bool canSwitchRank = EntitlementExtensions.IsHeroicRankGranted(1);
			UIMissionSubMenuBase currentMissionSubMenuBase = null;
			switch (missionSelectionManager.SubMenuType)
			{
			case MissionSelectionManager.EMissionMenu.Campaigns:
				currentMissionSubMenuBase = m_missionListCampaigns;
				canSwitchRank = false;
				break;
			case MissionSelectionManager.EMissionMenu.CampaignMissions:
				currentMissionSubMenuBase = m_missionListCampaignMissions;
				canSwitchRank = false;
				break;
			case MissionSelectionManager.EMissionMenu.ContractMissions:
				currentMissionSubMenuBase = m_missionListContractMissions;
				break;
			case MissionSelectionManager.EMissionMenu.CampaignDetails:
			case MissionSelectionManager.EMissionMenu.ContractDetails:
				currentMissionSubMenuBase = m_missionDetailsData;
				canSwitchRank = false;
				break;
			}
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanSwitchRank = canSwitchRank;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Close();
			if (m_currentMissionSubMenuBase == null || m_currentMissionSubMenuBase.ListType != missionSelectionManager.SubMenuType)
			{
				if (m_currentMissionSubMenuBase != null)
				{
					m_currentMissionSubMenuBase.OnDeactivate();
				}
				m_currentMissionSubMenuBase = currentMissionSubMenuBase;
				if (m_currentMissionSubMenuBase != null)
				{
					m_currentMissionSubMenuBase.OnActivate();
				}
			}
			if (m_currentMissionSubMenuBase != null)
			{
				m_screen.ShowBackButton = m_currentMissionSubMenuBase.ShowBackButton;
			}
		}

		private void OnScreenActivate()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.ShowSwitchAssassins = false;
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.BackButton.OnBeforeTapBack, OnBeforeTapBack);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged += new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnRankChanged += new SimpleDelegate.CallbackFunction(OnHeroicRankChanged);
			UpdateMissionScreen();
		}

		private void OnScreenDeactivate()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null && (bool)Singleton<UIScreenManager>.Instance.SceneData.BackButton)
			{
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.SceneData.BackButton.OnBeforeTapBack, OnBeforeTapBack);
			}
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged -= new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnRankChanged -= new SimpleDelegate.CallbackFunction(OnHeroicRankChanged);
			if (!base.IsDestroyingScreen && BehaviourSingleton<EnvironmentBridgeMain>.IsInitialized)
			{
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.CancelEarthTransitionAndZoomOut();
			}
			if (m_currentMissionSubMenuBase != null)
			{
				m_currentMissionSubMenuBase.OnDeactivate();
			}
			m_currentMissionSubMenuBase = null;
		}

		public override void OnBackKey()
		{
			OnBeforeTapBack();
		}

		private void OnBeforeTapBack()
		{
			bool flag = false;
			if (m_currentMissionSubMenuBase != null)
			{
				flag = m_currentMissionSubMenuBase.OnTapBack();
			}
			Singleton<UIScreenManager>.Instance.SceneData.BackButton.StopBackTapEvent = flag;
			if (!flag)
			{
				GenericAndroidBackKey.ShowExitConfirmation();
			}
		}

		private void OnMissionStateChanged(params object[] _params)
		{
			UpdateMissionScreen();
		}

		private void OnHeroicRankChanged(params object[] _params)
		{
			if (m_currentMissionSubMenuBase != null)
			{
				m_currentMissionSubMenuBase.Refresh();
			}
		}
	}
}
