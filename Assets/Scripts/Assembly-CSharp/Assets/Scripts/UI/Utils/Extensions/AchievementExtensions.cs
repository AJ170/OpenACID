using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AchievementExtensions
	{
		public static AchievementRank GetNextAvailableAchievementRank(this Achievement _definition)
		{
			AchievementRank achievementRank = null;
			AchievementRank achievementRank2 = null;
			for (int i = 0; i < _definition.ranks.Length; i++)
			{
				AchievementRank achievementRank3 = _definition.ranks[i];
				if (_definition.ranks[i].state != EAchievementState.AchievementClaimed && (achievementRank2 == null || achievementRank3.idx < achievementRank2.idx))
				{
					achievementRank2 = achievementRank3;
				}
				if (achievementRank == null || achievementRank3.idx > achievementRank.idx)
				{
					achievementRank = achievementRank3;
				}
			}
			if (achievementRank2 == null)
			{
				achievementRank2 = achievementRank;
			}
			return achievementRank2;
		}

		public static AchievementRank GetLastClaimedAchievementRank(this Achievement _definition)
		{
			AchievementRank achievementRank = null;
			for (int i = 0; i < _definition.ranks.Length; i++)
			{
				AchievementRank achievementRank2 = _definition.ranks[i];
				if (achievementRank2.state == EAchievementState.AchievementClaimed && (achievementRank == null || achievementRank2.idx > achievementRank.idx))
				{
					achievementRank = achievementRank2;
				}
			}
			return achievementRank;
		}

		public static int GetHighestRankIdx(this AchievementMapping.Definition _definition)
		{
			int num = 1;
			for (int i = 0; i < _definition.ranks.Length; i++)
			{
				AchievementMapping.RankDefinition rankDefinition = _definition.ranks[i];
				if (rankDefinition.idx > num)
				{
					num = rankDefinition.idx;
				}
			}
			return num;
		}

		public static AchievementMapping.RankDefinition GetRankDefinitionByIdx(this AchievementMapping.Definition _definition, int _id)
		{
			for (int i = 0; i < _definition.ranks.Length; i++)
			{
				AchievementMapping.RankDefinition rankDefinition = _definition.ranks[i];
				if (rankDefinition.idx == _id)
				{
					return rankDefinition;
				}
			}
			return null;
		}
	}
}
