using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Controllers.Rankings;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Leaderboard
{
	public class UILeaderboardRanksEventsOverviewController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIHeaderTitleController m_headerTitleController;

		[SerializeField]
		[CheckAssigned]
		private UIHeaderTitleController m_descriptionController;

		[CheckAssigned]
		[SerializeField]
		private UITimerController m_timerController;

		[SerializeField]
		[CheckAssigned]
		private UIRankShortDetailsController m_rankShortDetailsController;

		private static readonly StringId s_eventIsRunningStringId = 9696;

		private static readonly StringId s_eventIsNotRunninttStringId = 9697;

		public void UpdateLeaderboard(GetLeaderBoardsResponse _getLeaderBoardsResponse)
		{
			LeaderBoardUser currentPlayerLeaderboardUserByType = _getLeaderBoardsResponse.GetCurrentPlayerLeaderboardUserByType(ELeaderboardType.EventsHeader);
			string title = LocalizationManager.Instance.Get(8553);
			m_headerTitleController.UpdateTitle(title);
			UpdateScore(currentPlayerLeaderboardUserByType);
			UpdateCurrentEventDisplay();
		}

		public override void Show()
		{
			base.Show();
			base.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			base.gameObject.SetActive(false);
		}

		private void UpdateScore(LeaderBoardUser _leaderboardUser)
		{
			m_rankShortDetailsController.UpdateRankShortDetails(_leaderboardUser);
		}

		private void UpdateCurrentEventDisplay()
		{
			UIEventModel eventModel = EventProgressionChallengesManager.Instance.GetEventModel();
			StringId key = ((!eventModel.HasEventEnded()) ? s_eventIsRunningStringId : s_eventIsNotRunninttStringId);
			m_descriptionController.UpdateTitle(LocalizationManager.Instance.Get(key));
			m_timerController.UpdateController(eventModel.EndTimeUTC, UIDataMapper.UITextColors.TimerColor);
		}
	}
}
