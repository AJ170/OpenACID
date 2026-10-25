using System;
using AcierProtocol;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.Screens.Leaderboard;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class LeaderBoardExtensions
	{
		public static bool ContainsCurrentPlayer(this LeaderBoardUser[] _leaderBoardUsers)
		{
			for (int i = 0; i < _leaderBoardUsers.Length; i++)
			{
				if (_leaderBoardUsers[i].currentUser)
				{
					return true;
				}
			}
			return false;
		}

		public static LeaderBoardUser GetCurrentPlayerLeaderboardUserByType(this GetLeaderBoardsResponse _leaderboardResult, ELeaderboardType _type)
		{
			LeaderBoardUser[] array = null;
			switch (_type)
			{
			case ELeaderboardType.GlobalsHeader:
				array = _leaderboardResult.globalLeaderBoardResult.GetLeaderboardForCurrentUser();
				break;
			case ELeaderboardType.EventsHeader:
				if (_leaderboardResult.latestEventLeaderBoardResult != null)
				{
					array = _leaderboardResult.latestEventLeaderBoardResult.eventsleaderBoards;
				}
				break;
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].currentUser)
					{
						return array[i];
					}
				}
			}
			return null;
		}

		public static UIAssassinBaseModel[] CreateTopPlayerModels(this GetLeaderBoardsResponse _leaderboardResult, ELeaderboardType _type)
		{
			switch (_type)
			{
			case ELeaderboardType.GlobalsHeader:
				return _leaderboardResult.globalLeaderBoardResult.CreateGlobalLeaderboardTopPlayerModels();
			case ELeaderboardType.EventsHeader:
				if (_leaderboardResult.latestEventLeaderBoardResult != null)
				{
					return _leaderboardResult.latestEventLeaderBoardResult.CreateEventLeaderboardTopPlayerModels();
				}
				break;
			}
			return null;
		}

		private static UIAssassinBaseModel[] CreateEventLeaderboardTopPlayerModels(this EventLeaderBoardResult _eventLeaderBoardResult)
		{
			int num = 5;
			UIAssassinBaseModel[] array = new UIAssassinBaseModel[num];
			LeaderBoardUser[] eventsleaderBoards = _eventLeaderBoardResult.eventsleaderBoards;
			if (eventsleaderBoards != null)
			{
				for (int i = 0; i < num; i++)
				{
					if (eventsleaderBoards.Length > i)
					{
						LeaderBoardUser leaderBoardUser = eventsleaderBoards[i];
						UIAssassinLeaderboardUserDetailsModel uIAssassinLeaderboardUserDetailsModel = null;
						uIAssassinLeaderboardUserDetailsModel = ((leaderBoardUser.generatedLeaderBoardAssassin == null) ? new UIAssassinLeaderboardUserDetailsModel() : new UIAssassinLeaderboardGeneratedUserDetailsModel());
						uIAssassinLeaderboardUserDetailsModel.SortId = i;
						uIAssassinLeaderboardUserDetailsModel.HeroicRank = leaderBoardUser.rank;
						uIAssassinLeaderboardUserDetailsModel.LeaderBoardUser = leaderBoardUser;
						uIAssassinLeaderboardUserDetailsModel.Score = leaderBoardUser.score;
						array[i] = uIAssassinLeaderboardUserDetailsModel;
					}
				}
			}
			return array;
		}

		private static UIAssassinBaseModel[] CreateGlobalLeaderboardTopPlayerModels(this GlobalLeaderBoardResult _globalLeaderBoardResult)
		{
			HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
			UIAssassinBaseModel[] array = new UIAssassinBaseModel[heroicRankMapping.Length];
			for (int i = 0; i < heroicRankMapping.Length; i++)
			{
				int rankIdx = heroicRankMapping[i].rankIdx;
				LeaderBoardUser topPlayerByHeroicRank = _globalLeaderBoardResult.GetTopPlayerByHeroicRank(rankIdx);
				UIAssassinLeaderboardUserDetailsModel uIAssassinLeaderboardUserDetailsModel = null;
				uIAssassinLeaderboardUserDetailsModel = ((topPlayerByHeroicRank == null || topPlayerByHeroicRank.generatedLeaderBoardAssassin == null) ? new UIAssassinLeaderboardUserDetailsModel() : new UIAssassinLeaderboardGeneratedUserDetailsModel());
				uIAssassinLeaderboardUserDetailsModel.SortId = rankIdx;
				uIAssassinLeaderboardUserDetailsModel.HeroicRank = rankIdx;
				uIAssassinLeaderboardUserDetailsModel.LeaderBoardUser = topPlayerByHeroicRank;
				array[i] = uIAssassinLeaderboardUserDetailsModel;
			}
			Array.Sort(array, (UIAssassinBaseModel _model1, UIAssassinBaseModel _model2) => _model2.SortId.CompareTo(_model1.SortId));
			return array;
		}

		private static LeaderBoardUser GetTopPlayerByHeroicRank(this GlobalLeaderBoardResult _globalLeaderBoardResult, int _heroicRank)
		{
			LeaderBoardUser result = null;
			int num = _globalLeaderBoardResult.rankLeaderBoards.Length;
			for (int i = 0; i < num; i++)
			{
				GlobalLeaderBoardResult.RankLeaderBoard rankLeaderBoard = _globalLeaderBoardResult.rankLeaderBoards[i];
				if (_heroicRank == rankLeaderBoard.rank && rankLeaderBoard.leaderBoardUsers.Length > 0)
				{
					result = rankLeaderBoard.leaderBoardUsers[0];
				}
			}
			return result;
		}

		private static LeaderBoardUser[] GetLeaderboardForCurrentUser(this GlobalLeaderBoardResult _globalLeaderBoardResult)
		{
			int maxRankIdx = Singleton<StateManager>.Instance.ProfileManager.Profile.maxRankIdx;
			int num = _globalLeaderBoardResult.rankLeaderBoards.Length;
			for (int i = 0; i < num; i++)
			{
				GlobalLeaderBoardResult.RankLeaderBoard rankLeaderBoard = _globalLeaderBoardResult.rankLeaderBoards[i];
				if (maxRankIdx == rankLeaderBoard.rank)
				{
					return rankLeaderBoard.leaderBoardUsers;
				}
			}
			return null;
		}
	}
}
