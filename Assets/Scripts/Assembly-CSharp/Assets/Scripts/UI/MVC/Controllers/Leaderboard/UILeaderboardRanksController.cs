using AcierProtocol;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Controllers.Rankings;
using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.MVC.Models.Rankings;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Leaderboard
{
	public class UILeaderboardRanksController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIRankHeaderController m_headerController;

		[CheckAssigned]
		[SerializeField]
		private UIRankBottomController m_bottomTimerController;

		[SerializeField]
		[CheckAssigned]
		private UIRankEntriesWrapperController m_entriesWrapperController;

		[SerializeField]
		[CheckAssigned]
		private UIRankEntryController m_ownRankEntry;

		[CheckAssigned]
		[SerializeField]
		private UIAnchorChangeTargetController m_anchorChangerController;

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

		public void Initialize()
		{
			m_entriesWrapperController.Initialize();
		}

		public void DestroyController()
		{
			m_entriesWrapperController.DestroyController();
		}

		public void UpdateLeaderboard(UIRankHeaderBaseModel _data, UIAccordionCategoryLeaderboardModel _modelData)
		{
			m_headerController.UpdateRankHeader(_data);
			m_bottomTimerController.UpdateRefreshed(_data.NextUpdateTimeStamp);
			bool flag = IsPlayerRankShownSeparately(_modelData);
			UIRankEntryBaseModel uIRankEntryBaseModel = null;
			int num = ((!flag) ? _modelData.Users.Length : (_modelData.Users.Length - 1));
			UIRankEntryBaseModel[] array = new UIRankEntryBaseModel[num];
			int num2 = 0;
			for (int i = 0; i < _modelData.Users.Length; i++)
			{
				LeaderBoardUser leaderBoardUser = _modelData.Users[i];
				UIRankEntryBaseModel uIRankEntryBaseModel2 = new UIRankEntryBaseModel();
				uIRankEntryBaseModel2.User = leaderBoardUser;
				uIRankEntryBaseModel2.HeroicRank = leaderBoardUser.rank;
				uIRankEntryBaseModel2.Name = leaderBoardUser.userName;
				uIRankEntryBaseModel2.Position = leaderBoardUser.position;
				uIRankEntryBaseModel2.RelationIconName = (leaderBoardUser.currentUser ? "aUI_icon_assassin" : ((!leaderBoardUser.friend) ? string.Empty : "aUI_icon_friend"));
				uIRankEntryBaseModel2.Score = leaderBoardUser.score;
				uIRankEntryBaseModel2.Color = ((!leaderBoardUser.currentUser) ? Color.white : UIDataMapper.UITextColors.LeaderboardOwnPlayerColor);
				UIRankEntryBaseModel uIRankEntryBaseModel3 = uIRankEntryBaseModel2;
				if (flag && leaderBoardUser.currentUser)
				{
					uIRankEntryBaseModel = uIRankEntryBaseModel3;
					continue;
				}
				array[num2] = uIRankEntryBaseModel3;
				num2++;
			}
			m_entriesWrapperController.UpdateRankEntries(array);
			if (uIRankEntryBaseModel != null)
			{
				m_ownRankEntry.Show();
				m_ownRankEntry.UpdateRankController(uIRankEntryBaseModel, 0);
				m_anchorChangerController.SetShortAnchorTarget();
			}
			else
			{
				m_ownRankEntry.Hide();
				m_anchorChangerController.SetLongAnchorTarget();
			}
		}

		private bool IsPlayerRankShownSeparately(UIAccordionCategoryLeaderboardModel _modelData)
		{
			bool result = false;
			if (_modelData.Type != ELeaderboardType.GlobalFriends && _modelData.Type != ELeaderboardType.EventsFriends && _modelData.Users.Length >= 2)
			{
				LeaderBoardUser leaderBoardUser = _modelData.Users[_modelData.Users.Length - 1];
				if (leaderBoardUser.currentUser)
				{
					LeaderBoardUser leaderBoardUser2 = _modelData.Users[_modelData.Users.Length - 2];
					bool flag = leaderBoardUser2.position + 1 == leaderBoardUser.position;
					bool flag2 = leaderBoardUser2.position == leaderBoardUser.position;
					if (!flag && !flag2)
					{
						result = true;
					}
				}
			}
			return result;
		}
	}
}
