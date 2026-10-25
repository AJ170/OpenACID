using System;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.DailyQuest;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Events
{
	public class UIEventScreenChild : UIScreenChild
	{
		public enum ETabButtonType
		{
			None = 0,
			WeaklyEvent = 1,
			DailyQuest = 2
		}

		public static ETabButtonType SelectTabOnEnter;

		private static int s_isWaitingForItemClaimCount;

		[SerializeField]
		[CheckAssigned]
		private UITabButtonController m_weaklyHeaderTabButtons;

		[CheckAssigned]
		[SerializeField]
		private UITabButtonController m_dailyHeaderTabButtons;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_headerAlignmentBox;

		[CheckAssigned]
		[SerializeField]
		private UIEventItemController[] m_itemController;

		[SerializeField]
		[CheckAssigned]
		private UIFocusChildInScrollView m_focusChildInScroll;

		[SerializeField]
		[CheckAssigned]
		private UIScrollView m_scrollView;

		[CheckAssigned]
		[SerializeField]
		private UITable m_itemTable;

		private UITabButtonController m_currentTabButton;

		private Transform m_focusOnObject;

		private int m_lastUnlockedIndex;

		public static bool IsWaitingForItemClaim
		{
			get
			{
				return s_isWaitingForItemClaimCount > 0;
			}
			set
			{
				s_isWaitingForItemClaimCount += (value ? 1 : (-1));
			}
		}

		public UIEventItemController[] ItemController
		{
			get
			{
				return m_itemController;
			}
		}

		public void SelectDailyQuestButton()
		{
			OnTabButtonClicked(m_dailyHeaderTabButtons);
		}

		public override void OnBackKey()
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(Singleton<UIScreenManager>.Instance.CurrentScreen.m_previousScreen, new UITweenHandle(null, null));
		}

		private UIDailyQuestModel GetDailyQuestModel()
		{
			int contractHeroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.ContractHeroicMissionRank;
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			ProgressionQuests.RankQuests questByRank = profile.GetQuestByRank(contractHeroicMissionRank);
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(contractHeroicMissionRank, out value);
			UIDailyQuestModel uIDailyQuestModel = new UIDailyQuestModel();
			uIDailyQuestModel.RankQuest = questByRank;
			uIDailyQuestModel.RankDescription = value;
			return uIDailyQuestModel;
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			m_focusChildInScroll.FocusIsCentered = true;
		}

		protected override void OnScreenDestroy()
		{
			EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivate);
			m_currentTabButton = null;
			m_focusOnObject = null;
		}

		private void OnScreenActivate()
		{
			s_isWaitingForItemClaimCount = 0;
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			eventProgressionChallengesManager.OnChanged += new SimpleDelegate.CallbackFunction(OnProgressionEventChanged);
			m_focusChildInScroll.MoveInstant = true;
			Singleton<StateManager>.Instance.ProfileManager.OnEventProgressChanged += new SimpleDelegate.CallbackFunction(OnEventProgressChanged);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnRankChanged += new SimpleDelegate.CallbackFunction(OnRankChanged);
			UIDailyQuestModel dailyQuestModel = GetDailyQuestModel();
			m_dailyHeaderTabButtons.Initialize(dailyQuestModel);
			UITabButtonController dailyHeaderTabButtons = m_dailyHeaderTabButtons;
			dailyHeaderTabButtons.OnClicked = (Action<UITabButtonController>)Delegate.Combine(dailyHeaderTabButtons.OnClicked, new Action<UITabButtonController>(OnTabButtonClicked));
			UITabButtonHeaderController headerController = m_dailyHeaderTabButtons.HeaderController;
			headerController.OnItemClicked = (Action)Delegate.Combine(headerController.OnItemClicked, new Action(OnHeaderRewardItemClicked));
			UIEventModel eventModel = EventProgressionChallengesManager.Instance.GetEventModel();
			m_weaklyHeaderTabButtons.Initialize(eventModel);
			UITabButtonController weaklyHeaderTabButtons = m_weaklyHeaderTabButtons;
			weaklyHeaderTabButtons.OnClicked = (Action<UITabButtonController>)Delegate.Combine(weaklyHeaderTabButtons.OnClicked, new Action<UITabButtonController>(OnTabButtonClicked));
			UITabButtonHeaderController headerController2 = m_weaklyHeaderTabButtons.HeaderController;
			headerController2.OnItemClicked = (Action)Delegate.Combine(headerController2.OnItemClicked, new Action(OnHeaderRewardItemClicked));
			bool flag = EventProgressionChallengesManager.Instance.HasValidProgressionEvent();
			switch (SelectTabOnEnter)
			{
			case ETabButtonType.None:
				if (m_currentTabButton == null)
				{
					m_currentTabButton = ((!flag) ? m_dailyHeaderTabButtons : m_weaklyHeaderTabButtons);
				}
				break;
			case ETabButtonType.WeaklyEvent:
				m_currentTabButton = m_weaklyHeaderTabButtons;
				break;
			case ETabButtonType.DailyQuest:
				m_currentTabButton = m_dailyHeaderTabButtons;
				break;
			}
			if (!flag)
			{
				m_currentTabButton = m_dailyHeaderTabButtons;
			}
			m_currentTabButton.IsSelected = true;
			for (int i = 0; i < m_itemController.Length; i++)
			{
				UIEventItemController uIEventItemController = m_itemController[i];
				uIEventItemController.OnItemClicked = (Action<UIEventItemController>)Delegate.Combine(uIEventItemController.OnItemClicked, new Action<UIEventItemController>(OnRewardItemClicked));
				uIEventItemController.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Combine(uIEventItemController.OnItemClaimed, new Action<UIEventItemController, bool>(OnRewardItemClaimed));
			}
			UpdateItemList(true);
			UITutorialManager tutorialManager = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager;
			TutorialCategory category = tutorialManager.GetCategory(ETutorialCategory.TutCatMs26QuestIntro);
			if (category != null && category.Finished)
			{
				TutorialCategory category2 = tutorialManager.GetCategory(ETutorialCategory.TutCatMs26QuestRewards);
				if (category2 != null && category2.IsAtFlowId(-1) && !category2.GetIsStepActive())
				{
					tutorialManager.SetFirstAvailableStep();
				}
			}
		}

		private void OnScreenDeactivate()
		{
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			if (eventProgressionChallengesManager != null)
			{
				eventProgressionChallengesManager.OnChanged -= new SimpleDelegate.CallbackFunction(OnProgressionEventChanged);
			}
			Singleton<StateManager>.Instance.ProfileManager.OnEventProgressChanged -= new SimpleDelegate.CallbackFunction(OnEventProgressChanged);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnRankChanged -= new SimpleDelegate.CallbackFunction(OnRankChanged);
			if (m_itemTable != null)
			{
				UITable itemTable = m_itemTable;
				itemTable.onReposition = (UITable.OnReposition)Delegate.Remove(itemTable.onReposition, new UITable.OnReposition(OnLayoutTableReposition));
			}
			if (m_weaklyHeaderTabButtons != null)
			{
				UITabButtonController weaklyHeaderTabButtons = m_weaklyHeaderTabButtons;
				weaklyHeaderTabButtons.OnClicked = (Action<UITabButtonController>)Delegate.Remove(weaklyHeaderTabButtons.OnClicked, new Action<UITabButtonController>(OnTabButtonClicked));
				if (m_weaklyHeaderTabButtons.HeaderController != null)
				{
					UITabButtonHeaderController headerController = m_weaklyHeaderTabButtons.HeaderController;
					headerController.OnItemClicked = (Action)Delegate.Remove(headerController.OnItemClicked, new Action(OnHeaderRewardItemClicked));
				}
			}
			if (m_dailyHeaderTabButtons != null)
			{
				UITabButtonController dailyHeaderTabButtons = m_dailyHeaderTabButtons;
				dailyHeaderTabButtons.OnClicked = (Action<UITabButtonController>)Delegate.Remove(dailyHeaderTabButtons.OnClicked, new Action<UITabButtonController>(OnTabButtonClicked));
				if (m_dailyHeaderTabButtons.HeaderController != null)
				{
					UITabButtonHeaderController headerController2 = m_dailyHeaderTabButtons.HeaderController;
					headerController2.OnItemClicked = (Action)Delegate.Remove(headerController2.OnItemClicked, new Action(OnHeaderRewardItemClicked));
				}
			}
			for (int i = 0; i < m_itemController.Length; i++)
			{
				UIEventItemController uIEventItemController = m_itemController[i];
				if (uIEventItemController != null)
				{
					uIEventItemController.OnItemClicked = (Action<UIEventItemController>)Delegate.Remove(uIEventItemController.OnItemClicked, new Action<UIEventItemController>(OnRewardItemClicked));
					uIEventItemController.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Remove(uIEventItemController.OnItemClaimed, new Action<UIEventItemController, bool>(OnRewardItemClaimed));
					uIEventItemController.DeselectRewardItem();
				}
			}
		}

		private void UpdateItemList(bool _initialize = false)
		{
			if (m_currentTabButton == null)
			{
				return;
			}
			m_focusOnObject = null;
			UIDailyQuestModel uIDailyQuestModel = m_currentTabButton.Model as UIDailyQuestModel;
			if (uIDailyQuestModel != null)
			{
				UIDailyQuestModel uIDailyQuestModel2 = uIDailyQuestModel;
				ProgressionQuests.RankQuests.Quest[] array = ((uIDailyQuestModel2.RankQuest == null) ? null : uIDailyQuestModel2.RankQuest.questChain);
				for (int i = 0; i < m_itemController.Length; i++)
				{
					UIEventItemController uIEventItemController = m_itemController[i];
					if (array != null && array.Length > i && array[i] != null)
					{
						UIDailyQuestItemModel uIDailyQuestItemModel = new UIDailyQuestItemModel();
						uIDailyQuestItemModel.Quest = array[i];
						UIDailyQuestItemModel model = uIDailyQuestItemModel;
						if (_initialize)
						{
							uIEventItemController.Initialize(model);
							if (array[i].status != 0)
							{
								m_focusOnObject = uIEventItemController.transform;
								m_lastUnlockedIndex = i;
							}
						}
						else if (uIEventItemController.IsActive)
						{
							uIEventItemController.UpdateController(model);
							if (array[i].status != 0 && i > m_lastUnlockedIndex)
							{
								m_focusOnObject = uIEventItemController.transform;
								m_lastUnlockedIndex = 1;
							}
						}
					}
					else
					{
						uIEventItemController.Hide();
					}
				}
			}
			else
			{
				UIEventModel uIEventModel = m_currentTabButton.Model as UIEventModel;
				if (uIEventModel != null)
				{
					UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
					EventsProgress.ChallengeEvent[] array2 = ((profile.eventsProgress == null) ? null : profile.eventsProgress.challengeEvents);
					for (int j = 0; j < m_itemController.Length; j++)
					{
						UIEventItemController uIEventItemController2 = m_itemController[j];
						if (array2 != null && array2.Length > j && array2[j] != null)
						{
							UIEventItemModel uIEventItemModel = new UIEventItemModel();
							uIEventItemModel.EventChallengeId = array2[j].challengeId;
							UIEventItemModel model2 = uIEventItemModel;
							if (_initialize)
							{
								uIEventItemController2.Initialize(model2);
							}
							else if (uIEventItemController2.IsActive)
							{
								uIEventItemController2.UpdateController(model2);
							}
						}
						else
						{
							uIEventItemController2.Hide();
						}
					}
				}
			}
			if (_initialize)
			{
				EventDelegate.Add(m_headerAlignmentBox.OnReposition, OnLayoutAlignmentBoxReposition, true);
				m_headerAlignmentBox.RepositionNow = true;
			}
			else if (m_focusOnObject != null)
			{
				m_focusChildInScroll.FocusedObject = m_focusOnObject;
			}
		}

		private void OnHeaderRewardItemClicked()
		{
			for (int i = 0; i < m_itemController.Length; i++)
			{
				UIEventItemController uIEventItemController = m_itemController[i];
				if (uIEventItemController != null)
				{
					uIEventItemController.DeselectRewardItem();
				}
			}
		}

		private void OnRewardItemClicked(UIEventItemController _uiEventItemController)
		{
			m_weaklyHeaderTabButtons.DeselectHeaderItem();
			m_dailyHeaderTabButtons.DeselectHeaderItem();
			for (int i = 0; i < m_itemController.Length; i++)
			{
				UIEventItemController uIEventItemController = m_itemController[i];
				if (uIEventItemController != null && uIEventItemController != _uiEventItemController)
				{
					uIEventItemController.ClearRewardItemHighlight();
				}
			}
		}

		private void OnRewardItemClaimed(UIEventItemController _obj, bool _isSentToInventory)
		{
			if (!m_weaklyHeaderTabButtons.IsSelected)
			{
				return;
			}
			UIEventItemModel uIEventItemModel = _obj.Model as UIEventItemModel;
			if (uIEventItemModel != null)
			{
				UIEventHeaderWeeklyController uIEventHeaderWeeklyController = m_weaklyHeaderTabButtons.HeaderController as UIEventHeaderWeeklyController;
				if (uIEventHeaderWeeklyController != null)
				{
					int eventScoreItemIndex = uIEventItemModel.GetEventScoreItemIndex();
					UIRewardItemController rewardControllerByIndex = _obj.GetRewardControllerByIndex(eventScoreItemIndex);
					uIEventHeaderWeeklyController.OnEventScoreItemClaimed(rewardControllerByIndex);
				}
			}
		}

		private void OnTabButtonClicked(UITabButtonController _uiTabButtonController)
		{
			if (!(m_currentTabButton == _uiTabButtonController))
			{
				if (m_currentTabButton != null)
				{
					m_currentTabButton.IsSelected = false;
				}
				m_currentTabButton = _uiTabButtonController;
				m_currentTabButton.IsSelected = true;
				if (m_weaklyHeaderTabButtons.IsSelected || m_dailyHeaderTabButtons.IsSelected)
				{
					m_weaklyHeaderTabButtons.DeselectHeaderItem();
					m_dailyHeaderTabButtons.DeselectHeaderItem();
				}
				m_focusChildInScroll.MoveInstant = true;
				UpdateItemList(true);
			}
		}

		private void OnProgressionEventChanged(object[] _params)
		{
			UIEventModel eventModel = EventProgressionChallengesManager.Instance.GetEventModel();
			m_weaklyHeaderTabButtons.Initialize(eventModel);
			m_weaklyHeaderTabButtons.IsEnabled = EventProgressionChallengesManager.Instance.HasValidProgressionEvent();
			if (!m_weaklyHeaderTabButtons.IsEnabled)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(8136));
				if (!m_weaklyHeaderTabButtons.IsSelected)
				{
					OnTabButtonClicked(m_dailyHeaderTabButtons);
				}
			}
		}

		private void OnEventProgressChanged(object[] _params)
		{
			UIEventModel eventModel = EventProgressionChallengesManager.Instance.GetEventModel();
			m_weaklyHeaderTabButtons.UpdateController(eventModel);
			if (m_weaklyHeaderTabButtons.IsSelected)
			{
				EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
				if (eventProgressionChallengesManager.HasValidProgressionEvent())
				{
					UpdateItemList();
				}
			}
		}

		private void OnProfileUpdate(object[] _params)
		{
			UIDailyQuestModel dailyQuestModel = GetDailyQuestModel();
			m_dailyHeaderTabButtons.UpdateController(dailyQuestModel);
			if (m_dailyHeaderTabButtons.IsSelected)
			{
				UpdateItemList();
			}
		}

		private void OnRankChanged(object[] _params)
		{
			UIDailyQuestModel dailyQuestModel = GetDailyQuestModel();
			m_dailyHeaderTabButtons.UpdateController(dailyQuestModel);
			if (m_dailyHeaderTabButtons.IsSelected)
			{
				UpdateItemList();
			}
		}

		private void OnLayoutAlignmentBoxReposition()
		{
			UITable itemTable = m_itemTable;
			itemTable.onReposition = (UITable.OnReposition)Delegate.Combine(itemTable.onReposition, new UITable.OnReposition(OnLayoutTableReposition));
			m_itemTable.repositionNow = true;
		}

		private void OnLayoutTableReposition()
		{
			UITable itemTable = m_itemTable;
			itemTable.onReposition = (UITable.OnReposition)Delegate.Remove(itemTable.onReposition, new UITable.OnReposition(OnLayoutTableReposition));
			m_scrollView.ResetPosition();
			if (m_focusOnObject != null)
			{
				m_focusChildInScroll.FocusedObject = m_focusOnObject;
			}
		}
	}
}
