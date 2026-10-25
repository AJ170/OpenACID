using AcierProtocol;
using Assets.Scripts.UI.MVC.Controllers.Rankings;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Leaderboard
{
	public class UILeaderboardRanksOverViewController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIRankTopPlayersController m_rankTopPlayersController;

		[SerializeField]
		[CheckAssigned]
		private UIRankAssassinDetailsController m_assassinRankDetailsController;

		public void UpdateLeaderboard(GetLeaderBoardsResponse _leaderboardResult, ELeaderboardType _type)
		{
			m_rankTopPlayersController.UpdateTopRankPlayers(_leaderboardResult, _type);
		}

		public override void OnChanged(UIControllerBase _changedController)
		{
			UIAssassinLeaderboardUserDetailsModel leaderboardUserModel = (UIAssassinLeaderboardUserDetailsModel)m_rankTopPlayersController.CurrentSelectedModel;
			m_assassinRankDetailsController.UpdateRankAssassinDetails(leaderboardUserModel);
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
	}
}
