using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Behaviours.AccordionTab;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Accordion;
using Assets.Scripts.UI.MVC.Controllers.Leaderboard;
using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.MVC.Models.Rankings;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Leaderboard
{
	public class UILeaderboardScreenChild : UIScreenChild
	{
		private const int c_updateItervalTimerInMilliseconds = 900000;

		private const int c_updateWaitTimeInSeconds = 60;

		[SerializeField]
		[CheckAssigned]
		private UILeaderboardRanksEventsOverviewController m_leaderboardRanksEventsOverviewController;

		[SerializeField]
		[CheckAssigned]
		private UILeaderboardRanksOverViewController m_leaderboardRanksOverViewController;

		[SerializeField]
		[CheckAssigned]
		private UILeaderboardRanksController m_leaderboarRankController;

		[SerializeField]
		[CheckAssigned]
		private UILeaderboardAccordionController m_leaderboardAccordionController;

		[SerializeField]
		[CheckAssigned]
		private UIAccordionRootController m_accordionRootController;

		[CheckAssigned]
		[SerializeField]
		private UITweenedScreenComponent[] m_tweenedScreenComponents;

		private UIRequestToken m_updateRequestToken;

		private GetLeaderBoardsResponse m_leaderboardResult;

		private float m_minUpdateWaitTime;

		private bool m_waitingForUpdate;

		private bool m_startScreenTweenAfterLeaderboardUpdate;

		public override void OnBackKey()
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(m_screen.m_previousScreen, new UITweenHandle(null, null));
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			m_leaderboardAccordionController.Initialize();
			m_leaderboarRankController.Initialize();
			UIAccordionRootController accordionRootController = m_accordionRootController;
			accordionRootController.OnSelectionChanged = (Action<UIAccordionBaseTab>)Delegate.Combine(accordionRootController.OnSelectionChanged, new Action<UIAccordionBaseTab>(OnAccordionItemSelectionChanged));
		}

		protected override void OnScreenDestroy()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeactivate);
			}
			if (m_accordionRootController != null)
			{
				UIAccordionRootController accordionRootController = m_accordionRootController;
				accordionRootController.OnSelectionChanged = (Action<UIAccordionBaseTab>)Delegate.Combine(accordionRootController.OnSelectionChanged, new Action<UIAccordionBaseTab>(OnAccordionItemSelectionChanged));
			}
			if (m_leaderboarRankController != null)
			{
				m_leaderboarRankController.DestroyController();
			}
		}

		private void OnScreenActivate()
		{
			for (int i = 0; i < m_tweenedScreenComponents.Length; i++)
			{
				UITweenedScreenComponent uITweenedScreenComponent = m_tweenedScreenComponents[i];
				if (uITweenedScreenComponent != null)
				{
					uITweenedScreenComponent.SetToBegining();
				}
			}
			CTADefinitionUnlockLeaderboard.UpdateLeaderboardLastShown();
			CTADefinitionUnlockNewHeroicRank.UpdateHeroicRankShown();
			m_leaderboarRankController.Hide();
			m_leaderboardRanksOverViewController.Hide();
			m_leaderboardRanksEventsOverviewController.Hide();
			m_startScreenTweenAfterLeaderboardUpdate = true;
			if (m_leaderboardResult == null)
			{
				TriggerLeaderboardDataUpdate(false);
			}
			else
			{
				UpdateLeaderboard();
			}
		}

		private void OnScreenDeactivate()
		{
			m_minUpdateWaitTime = 60f;
			if (m_updateRequestToken != null)
			{
				m_updateRequestToken.Abandoned = true;
			}
			m_updateRequestToken = null;
		}

		private void OnAccordionItemSelectionChanged(UIAccordionBaseTab _accordionBaseTab)
		{
			UpdateLeaderboardRanks(_accordionBaseTab);
		}

		private void StartTweenInScreen()
		{
			m_startScreenTweenAfterLeaderboardUpdate = false;
			StartCoroutine(OnStartTweenInScreen());
		}

		private IEnumerator OnStartTweenInScreen()
		{
			yield return new WaitForEndOfFrame();
			for (int i = 0; i < m_tweenedScreenComponents.Length; i++)
			{
				UITweenedScreenComponent comp = m_tweenedScreenComponents[i];
				if (comp != null)
				{
					comp.ForcePlayTween();
				}
			}
		}

		private void Update()
		{
			if (!m_waitingForUpdate && m_leaderboardResult != null)
			{
				m_minUpdateWaitTime -= Time.deltaTime;
				long javaTimestamp = m_leaderboardResult.lastUpdatedSince + 900000;
				DateTime dateTime = ServerTimeManager.ToUtcTime(javaTimestamp);
				DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
				double totalSeconds = (dateTime - utcNow).TotalSeconds;
				if (totalSeconds <= 0.0 && m_minUpdateWaitTime < 0f)
				{
					TriggerLeaderboardDataUpdate();
				}
			}
		}

		private void UpdateLeaderboardRanks(UIAccordionBaseTab _accordionBaseTab)
		{
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel = (UIAccordionCategoryLeaderboardModel)_accordionBaseTab.ModelData;
			if (uIAccordionCategoryLeaderboardModel != null)
			{
				if (uIAccordionCategoryLeaderboardModel.Type == ELeaderboardType.GlobalsHeader)
				{
					m_leaderboarRankController.Hide();
					m_leaderboardRanksEventsOverviewController.Hide();
					m_leaderboardRanksOverViewController.Show();
					m_leaderboardRanksOverViewController.UpdateLeaderboard(m_leaderboardResult, uIAccordionCategoryLeaderboardModel.Type);
					return;
				}
				if (uIAccordionCategoryLeaderboardModel.Type == ELeaderboardType.EventsHeader)
				{
					m_leaderboarRankController.Hide();
					m_leaderboardRanksOverViewController.Hide();
					m_leaderboardRanksEventsOverviewController.Show();
					m_leaderboardRanksEventsOverviewController.UpdateLeaderboard(m_leaderboardResult);
					return;
				}
				m_leaderboarRankController.Show();
				m_leaderboardRanksOverViewController.Hide();
				m_leaderboardRanksEventsOverviewController.Hide();
				LeaderboardTypeDescription value = null;
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.LeaderboardTypeDescription.TryGetValue((int)uIAccordionCategoryLeaderboardModel.Type, out value);
				UIRankHeaderBaseModel uIRankHeaderBaseModel = new UIRankHeaderBaseModel();
				uIRankHeaderBaseModel.Name = LocalizationManager.Instance.Get(8554, new StringMapper { { "type", _accordionBaseTab.TabName } });
				uIRankHeaderBaseModel.ScoreName = LocalizationManager.Instance.Get(value.ScoreType);
				uIRankHeaderBaseModel.ScoreIcon = value.ScoreTypeIcon;
				uIRankHeaderBaseModel.NextUpdateTimeStamp = m_leaderboardResult.lastUpdatedSince + 900000;
				UIRankHeaderBaseModel data = uIRankHeaderBaseModel;
				m_leaderboarRankController.UpdateLeaderboard(data, uIAccordionCategoryLeaderboardModel);
			}
		}

		private void UpdateLeaderboard()
		{
			m_leaderboardAccordionController.UpdateLeaderboard(m_leaderboardResult);
			if (m_accordionRootController.CurrentSelectedTab != null)
			{
				UpdateLeaderboardRanks(m_accordionRootController.CurrentSelectedTab);
			}
			else
			{
				m_accordionRootController.SelectFirstAvailableCategoryTab();
			}
			if (m_startScreenTweenAfterLeaderboardUpdate)
			{
				StartTweenInScreen();
			}
		}

		private void TriggerLeaderboardDataUpdate(bool _silentUpdate = true)
		{
			m_waitingForUpdate = true;
			if (_silentUpdate)
			{
				if (m_updateRequestToken != null)
				{
					m_updateRequestToken.Abandoned = true;
				}
				m_updateRequestToken = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.GetLeaderBoards(_asyncCallback);
				}, null, OnGetLeaderBoards);
			}
			else
			{
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.GetLeaderBoards(_asyncCallback);
				}, null, OnGetLeaderBoards);
			}
		}

		private void OnGetLeaderBoards(IAsyncResult _result)
		{
			m_waitingForUpdate = false;
			m_minUpdateWaitTime = 60f;
			if (_result.IsError())
			{
				return;
			}
			GetLeaderBoardsResponse leaderboardResult = ((AsyncDataResult<GetLeaderBoardsResponse>)_result).Result();
			m_leaderboardResult = leaderboardResult;
			GlobalLeaderBoardResult globalLeaderBoardResult = m_leaderboardResult.globalLeaderBoardResult;
			for (int i = 0; i < globalLeaderBoardResult.rankLeaderBoards.Length; i++)
			{
				GlobalLeaderBoardResult.RankLeaderBoard rankLeaderBoard = globalLeaderBoardResult.rankLeaderBoards[i];
				for (int j = 0; j < rankLeaderBoard.leaderBoardUsers.Length; j++)
				{
					rankLeaderBoard.leaderBoardUsers[j].rank = rankLeaderBoard.rank;
				}
			}
			UpdateLeaderboard();
		}
	}
}
