using AcierProtocol;
using Assets.Scripts.UI.MVC.Controllers.Accordion;
using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Leaderboard
{
	public class UILeaderboardAccordionController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIAccordionRootController m_accordionRootController;

		private UIAccordionCategoryBaseModel m_rootModel;

		public void Initialize()
		{
			PreCreateModelTree();
		}

		public void UpdateLeaderboard(GetLeaderBoardsResponse _leaderBoardsResponse)
		{
			m_rootModel = CreateModelTree(_leaderBoardsResponse);
			m_rootModel.Sort();
			m_accordionRootController.Initialize(m_rootModel);
		}

		private static UIAccordionCategoryBaseModel CreateModelTree(GetLeaderBoardsResponse _leaderBoardsResponse)
		{
			UIAccordionCategoryBaseModel _rootModel = new UIAccordionCategoryBaseModel();
			AddEventsModelTree(_leaderBoardsResponse.latestEventLeaderBoardResult, ref _rootModel);
			AddGlobalModelTree(_leaderBoardsResponse.globalLeaderBoardResult, ref _rootModel);
			return _rootModel;
		}

		private static void AddEventsModelTree(EventLeaderBoardResult _eventLeaderBoardResult, ref UIAccordionCategoryBaseModel _rootModel)
		{
			bool flag = _eventLeaderBoardResult == null;
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
			uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.EventsHeader;
			uIAccordionCategoryLeaderboardModel.SortId = 1;
			uIAccordionCategoryLeaderboardModel.NameIconName = "aUI_icon_event";
			uIAccordionCategoryLeaderboardModel.IsLocked = flag;
			uIAccordionCategoryLeaderboardModel.NameId = 8553;
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel2 = uIAccordionCategoryLeaderboardModel;
			_rootModel.Add(uIAccordionCategoryLeaderboardModel2);
			if (!flag)
			{
				uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
				uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.EventsFriends;
				uIAccordionCategoryLeaderboardModel.SortId = -1;
				uIAccordionCategoryLeaderboardModel.NameIconName = "aUI_icon_friend";
				uIAccordionCategoryLeaderboardModel.NameId = 685;
				uIAccordionCategoryLeaderboardModel.Users = _eventLeaderBoardResult.friendsleaderBoards;
				UIAccordionCategoryLeaderboardModel achievement = uIAccordionCategoryLeaderboardModel;
				uIAccordionCategoryLeaderboardModel2.Add(achievement);
				uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
				uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.EventsAll;
				uIAccordionCategoryLeaderboardModel.SortId = 0;
				uIAccordionCategoryLeaderboardModel.NameId = 9460;
				uIAccordionCategoryLeaderboardModel.Users = _eventLeaderBoardResult.eventsleaderBoards;
				UIAccordionCategoryLeaderboardModel achievement2 = uIAccordionCategoryLeaderboardModel;
				uIAccordionCategoryLeaderboardModel2.Add(achievement2);
			}
		}

		private static void AddGlobalModelTree(GlobalLeaderBoardResult _globalLeaderBoardResult, ref UIAccordionCategoryBaseModel _rootModel)
		{
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
			uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.GlobalsHeader;
			uIAccordionCategoryLeaderboardModel.SortId = 0;
			uIAccordionCategoryLeaderboardModel.NameIconName = "aUI_icon_world";
			uIAccordionCategoryLeaderboardModel.NameId = 8552;
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel2 = uIAccordionCategoryLeaderboardModel;
			_rootModel.Add(uIAccordionCategoryLeaderboardModel2);
			uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
			uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.GlobalFriends;
			uIAccordionCategoryLeaderboardModel.SortId = -1;
			uIAccordionCategoryLeaderboardModel.NameIconName = "aUI_icon_friend";
			uIAccordionCategoryLeaderboardModel.NameId = 685;
			uIAccordionCategoryLeaderboardModel.Users = _globalLeaderBoardResult.friendsleaderBoards;
			UIAccordionCategoryLeaderboardModel achievement = uIAccordionCategoryLeaderboardModel;
			uIAccordionCategoryLeaderboardModel2.Add(achievement);
			for (int i = 0; i < _globalLeaderBoardResult.rankLeaderBoards.Length; i++)
			{
				GlobalLeaderBoardResult.RankLeaderBoard rankLeaderBoard = _globalLeaderBoardResult.rankLeaderBoards[i];
				HeroicRankDescription value;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(rankLeaderBoard.rank, out value))
				{
					bool flag = rankLeaderBoard.leaderBoardUsers.ContainsCurrentPlayer();
					uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
					uIAccordionCategoryLeaderboardModel.Type = ELeaderboardType.GlobalHeroicRank;
					uIAccordionCategoryLeaderboardModel.NameId = value.NameId;
					uIAccordionCategoryLeaderboardModel.NameIconName = ((!flag) ? string.Empty : "aUI_icon_assassin");
					uIAccordionCategoryLeaderboardModel.SortId = rankLeaderBoard.rank;
					uIAccordionCategoryLeaderboardModel.Users = rankLeaderBoard.leaderBoardUsers;
					uIAccordionCategoryLeaderboardModel.ShowPlayerColor = flag;
					UIAccordionCategoryLeaderboardModel achievement2 = uIAccordionCategoryLeaderboardModel;
					uIAccordionCategoryLeaderboardModel2.Add(achievement2);
				}
			}
		}

		private void PreCreateModelTree()
		{
			UIAccordionCategoryBaseModel uIAccordionCategoryBaseModel = new UIAccordionCategoryBaseModel();
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel = new UIAccordionCategoryLeaderboardModel();
			UIAccordionCategoryLeaderboardModel uIAccordionCategoryLeaderboardModel2 = new UIAccordionCategoryLeaderboardModel();
			for (int i = 0; i < 5; i++)
			{
				uIAccordionCategoryLeaderboardModel.Add(new UIAccordionCategoryLeaderboardModel());
				uIAccordionCategoryLeaderboardModel2.Add(new UIAccordionCategoryLeaderboardModel());
			}
			uIAccordionCategoryBaseModel.Add(uIAccordionCategoryLeaderboardModel);
			uIAccordionCategoryBaseModel.Add(uIAccordionCategoryLeaderboardModel2);
			m_accordionRootController.PreInitialize(uIAccordionCategoryBaseModel);
		}
	}
}
