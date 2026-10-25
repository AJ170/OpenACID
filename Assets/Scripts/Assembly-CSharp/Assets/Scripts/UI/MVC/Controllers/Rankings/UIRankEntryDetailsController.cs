using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankEntryDetailsController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIRankEntryDetailsView m_playerRankSummery;

		public virtual void UpdateRankEntryDetails(UIAssassinLeaderboardUserDetailsModel _leaderboardUser)
		{
			m_playerRankSummery.SetLeaderboardRank(_leaderboardUser.Position, _leaderboardUser.SelectedTextColor);
			m_playerRankSummery.SetPlayerName(_leaderboardUser.UserName, _leaderboardUser.SelectedTextColor);
			m_playerRankSummery.SetAssassinName(_leaderboardUser.AssassinName);
			m_playerRankSummery.SetAssassinClasses(_leaderboardUser.ClassName);
			base.IsInitialized = true;
			Show();
		}
	}
}
