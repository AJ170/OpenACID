using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.AppRatingPopup;
using Assets.Scripts.UI.Screens.Popup.CallToActionPopup;
using Assets.Scripts.UI.Screens.Popup.FriendHirelingPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Progression
{
	public class UIFlowHandler
	{
		private readonly UIEventChallengeProgressionHandler m_eventProgressionHandler = new UIEventChallengeProgressionHandler();

		private readonly UIDailyQuestProgressionHandler m_dailyQuestProgressionHandler = new UIDailyQuestProgressionHandler();

		public UIFlowQueue FlowQueue { get; private set; }

		public UIFlowQueue PopupQueue { get; private set; }

		public bool IsAnimationPlaying { get; set; }

		public static bool CanStartMissionProgressionAnimation { get; private set; }

		private bool CheckCTAPopupsOnUpdate { get; set; }

		public UIFlowHandler()
		{
			FlowQueue = new UIFlowQueue();
			PopupQueue = new UIFlowQueue();
		}

		public bool HasCompleted()
		{
			return FlowQueue.IsDone() && PopupQueue.IsDone();
		}

		public void Update()
		{
			if (!IsAnimationPlaying && CheckCTAPopupsOnUpdate && Singleton<UIScreenManager>.Instance.CurrentModalScreen == null)
			{
				CheckCTAPopupsOnUpdate = false;
				CallToActionPopup.ShowFromProfileUpdate();
			}
		}

		public void PurgeAnimusScreen()
		{
			CheckCTAPopupsOnUpdate = false;
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		public void OnEnterMission()
		{
			FlowQueue.Clear();
			PopupQueue.Clear();
			CanStartMissionProgressionAnimation = false;
			m_eventProgressionHandler.OnEnterMission();
			m_dailyQuestProgressionHandler.OnEnterMission();
			CheckCTAPopupsOnUpdate = false;
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		public void OnEnterAnimusFromStartUp()
		{
			CanStartMissionProgressionAnimation = true;
			m_eventProgressionHandler.OnEnterAnimusFromStartUp();
			m_dailyQuestProgressionHandler.OnEnterAnimusFromStartUp();
		}

		public void OnEnterAnimusAtStartup()
		{
			FlowQueue.Clear();
			CanStartMissionProgressionAnimation = false;
			FlowQueue.Add(OnActivateButtonManager);
			FlowQueue.Add(OnActivteMissionAnimationFlowQueue);
			FlowQueue.Add(OnActivateEventProgression);
			FlowQueue.Proceed();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		public void OnEnterAnimusAfterMission()
		{
			FlowQueue.Clear();
			CanStartMissionProgressionAnimation = false;
			FlowQueue.Add(OnActivateButtonManager);
			FlowQueue.Add(OnActivatePopupFLow);
			FlowQueue.Add(OnActivateEventProgression);
			FlowQueue.Proceed();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		private void OnActivateButtonManager()
		{
			Singleton<UIScreenManager>.Instance.SceneData.FullScreenBlocker.SetActive(UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive);
			UIButtonHeaderManager buttonHeaderManager = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager;
			int waitCounter = buttonHeaderManager.AllBottomBarButtons.Length;
			buttonHeaderManager.ActivateBottomBarButtons(delegate
			{
				waitCounter--;
				if (waitCounter <= 0)
				{
					Singleton<UIScreenManager>.Instance.SceneData.FullScreenBlocker.SetActive(false);
					FlowQueue.Proceed();
				}
			});
		}

		private void OnActivatePopupFLow()
		{
			PopupQueue.Clear();
			PopupQueue.Add(AppRatingPopup.Show);
			if (EntitlementExtensions.IsHirelingUnlocked())
			{
				PopupQueue.Add(FriendHirelingPopup.Show);
			}
			PopupQueue.Add(CallToActionPopup.Show);
			PopupQueue.Add(CallToActionPopup.ShowMessageLines);
			PopupQueue.Add(OnActivateNotificationPopup);
			PopupQueue.Add(OnActivteMissionAnimation);
			PopupQueue.Proceed();
			FlowQueue.Proceed();
		}

		private void OnActivateNotificationPopup()
		{
			UINotificationPopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
			subSystemByType.IsShowingNotificationPaused = false;
			PopupQueue.Proceed();
		}

		private void OnActivteMissionAnimation()
		{
			CanStartMissionProgressionAnimation = true;
			PopupQueue.Proceed();
		}

		private void OnActivteMissionAnimationFlowQueue()
		{
			CanStartMissionProgressionAnimation = true;
			FlowQueue.Proceed();
		}

		private void OnActivateEventProgression()
		{
			m_eventProgressionHandler.OnEnterAnimusAfterMission();
			m_dailyQuestProgressionHandler.OnEnterAnimusAfterMission();
			FlowQueue.Proceed();
		}

		private void OnProfileUpdate(object[] _params)
		{
			if (HasCompleted() && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
			{
				CheckCTAPopupsOnUpdate = true;
			}
		}
	}
}
