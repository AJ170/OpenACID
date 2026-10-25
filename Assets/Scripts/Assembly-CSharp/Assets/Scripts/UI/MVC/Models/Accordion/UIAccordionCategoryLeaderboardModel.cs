using AcierProtocol;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Models.Accordion
{
	public class UIAccordionCategoryLeaderboardModel : UIAccordionCategoryBaseModel
	{
		public ELeaderboardType Type;

		public LeaderBoardUser[] Users;

		public bool ShowPlayerColor;

		public override Color DeselectedTextColor
		{
			get
			{
				return (!ShowPlayerColor) ? base.DeselectedTextColor : UIDataMapper.UITextColors.LeaderboardOwnPlayerColor;
			}
		}
	}
}
