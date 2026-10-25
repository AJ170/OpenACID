using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Models.Assassin
{
	public class UIAssassinLeaderboardUserDetailsModel : UIAssassinBaseModel
	{
		public long Score { get; set; }

		public int HeroicRank { get; set; }

		public LeaderBoardUser LeaderBoardUser { get; set; }

		public string UserName
		{
			get
			{
				return (LeaderBoardUser == null) ? string.Empty : LeaderBoardUser.userName;
			}
		}

		public int Position
		{
			get
			{
				return (LeaderBoardUser != null) ? LeaderBoardUser.position : 0;
			}
		}

		public override AcierProtocol.Assassin Assassin
		{
			get
			{
				return (LeaderBoardUser == null) ? null : LeaderBoardUser.assassin;
			}
		}

		public override AssassinSummary Summery
		{
			get
			{
				AssassinSummary summery = base.Summery;
				if (summery != null)
				{
					summery.Power = (int)LeaderBoardUser.score;
				}
				return summery;
			}
		}

		public override string Title
		{
			get
			{
				HeroicRankDescription value;
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(HeroicRank, out value);
				return LocalizationManager.Instance.Get(value.NameId);
			}
		}

		public override Color SelectedTextColor
		{
			get
			{
				Color result = Color.white;
				if (LeaderBoardUser != null && LeaderBoardUser.currentUser)
				{
					result = UIDataMapper.UITextColors.LeaderboardOwnPlayerColor;
				}
				return result;
			}
		}

		public override Color DeselectedTextColor
		{
			get
			{
				Color result = UIDataMapper.UITextColors.LeaderboardDeselectedColor;
				if (LeaderBoardUser != null && LeaderBoardUser.currentUser)
				{
					result = UIDataMapper.UITextColors.LeaderboardOwnPlayerDeselectedColor;
				}
				return result;
			}
		}
	}
}
