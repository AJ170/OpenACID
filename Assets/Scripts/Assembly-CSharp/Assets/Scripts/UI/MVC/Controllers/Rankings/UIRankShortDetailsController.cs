using AcierProtocol;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankShortDetailsController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIRankShortDetailsView m_shortDeatailsView;

		public virtual void UpdateRankShortDetails(LeaderBoardUser _leaderboardUser)
		{
			int playerRank = 0;
			long num = 0L;
			if (_leaderboardUser != null)
			{
				playerRank = _leaderboardUser.position;
				num = _leaderboardUser.score;
			}
			if (num == 0L)
			{
				m_shortDeatailsView.OnPlayerNoRank();
			}
			else
			{
				m_shortDeatailsView.PlayerRank = playerRank;
			}
			m_shortDeatailsView.ScoreValue = num;
			m_shortDeatailsView.AlignDetails();
			base.IsInitialized = true;
			Show();
		}
	}
}
