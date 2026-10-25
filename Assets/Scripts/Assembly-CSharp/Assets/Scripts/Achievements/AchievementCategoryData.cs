using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;

namespace Assets.Scripts.Achievements
{
	public class AchievementCategoryData
	{
		public string CategoryId = string.Empty;

		public readonly List<AchievementMapping.Definition> AchievementsDef = new List<AchievementMapping.Definition>();

		public readonly List<AchievementCategoryData> SubCategories = new List<AchievementCategoryData>();

		public float Progress;

		public bool HasUnread;

		public int OwnCount;

		public int OwnClaimedCount;

		public int OwnIncludingAllRanksCount;

		public int OwnIncludingAllRanksClaimedCount;

		public int TotalCount;

		public int TotalClaimedCount;

		public int TotalShownIncludingAllRanksCount;

		public int TotalIncludingAllRanksClaimedCount;

		public long LastClaimedDate;

		public long LastClaimableDate;

		public Achievement LastClaimedAchievement;

		public Achievement LastClaimableAchievement;

		public readonly BBDictionary<string, Achievement> IdToAchievements = new BBDictionary<string, Achievement>();

		public void Sort()
		{
			SubCategories.Sort(SortSubCategories);
			AchievementsDef.Sort(SortAchievementsDef);
			for (int i = 0; i < SubCategories.Count; i++)
			{
				SubCategories[i].Sort();
			}
		}

		public void AddAchievementDef(AchievementMapping.Definition _achievement)
		{
			AchievementsDef.Add(_achievement);
		}

		public void AddAchievements(Achievement _achievement)
		{
			long num = 0L;
			AchievementRank lastClaimedAchievementRank = _achievement.GetLastClaimedAchievementRank();
			if (lastClaimedAchievementRank != null)
			{
				AchievementMapping.Definition achievementMappingDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementMappingDefinition(_achievement.definitionId);
				if (lastClaimedAchievementRank.idx == achievementMappingDefinition.GetHighestRankIdx())
				{
					OwnClaimedCount++;
				}
				num = lastClaimedAchievementRank.unlockDate;
			}
			AchievementRank nextAvailableAchievementRank = _achievement.GetNextAvailableAchievementRank();
			if (nextAvailableAchievementRank != null && nextAvailableAchievementRank.state == EAchievementState.AchievementClaimable)
			{
				UpdateLastClaimable(_achievement, nextAvailableAchievementRank.unlockDate);
			}
			for (int i = 0; i < _achievement.ranks.Length; i++)
			{
				AchievementRank achievementRank = _achievement.ranks[i];
				HasUnread |= achievementRank.state == EAchievementState.AchievementClaimable;
				if (achievementRank.state != EAchievementState.AchievementClaimed)
				{
					break;
				}
				num = achievementRank.unlockDate;
				OwnIncludingAllRanksClaimedCount++;
			}
			if (num > 0)
			{
				UpdateLastClaimed(_achievement, num);
			}
			IdToAchievements.Add(_achievement.definitionId, _achievement);
		}

		public void UpdateLastClaimed(Achievement _achievement, long _lastClaimedDate)
		{
			if (LastClaimedAchievement == null || LastClaimedDate < _lastClaimedDate)
			{
				LastClaimedDate = _lastClaimedDate;
				LastClaimedAchievement = _achievement;
			}
		}

		public void UpdateLastClaimable(Achievement _achievement, long _date)
		{
			if (LastClaimableAchievement == null || LastClaimableDate < _date)
			{
				LastClaimableDate = _date;
				LastClaimableAchievement = _achievement;
			}
		}

		public void ClearDynamicData()
		{
			for (int i = 0; i < SubCategories.Count; i++)
			{
				SubCategories[i].ClearDynamicData();
			}
			Progress = 0f;
			HasUnread = false;
			LastClaimedDate = 0L;
			LastClaimableDate = 0L;
			LastClaimedAchievement = null;
			LastClaimableAchievement = null;
			OwnCount = 0;
			OwnClaimedCount = 0;
			TotalCount = 0;
			TotalClaimedCount = 0;
			OwnIncludingAllRanksCount = 0;
			OwnIncludingAllRanksClaimedCount = 0;
			TotalShownIncludingAllRanksCount = 0;
			TotalIncludingAllRanksClaimedCount = 0;
			IdToAchievements.Clear();
		}

		public void UpdateProgress()
		{
			UpdateOwnCount();
			TotalCount = OwnCount;
			TotalClaimedCount = OwnClaimedCount;
			TotalShownIncludingAllRanksCount = OwnIncludingAllRanksCount;
			TotalIncludingAllRanksClaimedCount = OwnIncludingAllRanksClaimedCount;
			for (int i = 0; i < SubCategories.Count; i++)
			{
				AchievementCategoryData achievementCategoryData = SubCategories[i];
				achievementCategoryData.UpdateProgress();
				HasUnread |= achievementCategoryData.HasUnread;
				TotalCount += achievementCategoryData.TotalCount;
				TotalClaimedCount += achievementCategoryData.TotalClaimedCount;
				TotalShownIncludingAllRanksCount += achievementCategoryData.TotalShownIncludingAllRanksCount;
				TotalIncludingAllRanksClaimedCount += achievementCategoryData.TotalIncludingAllRanksClaimedCount;
				UpdateLastClaimed(achievementCategoryData.LastClaimedAchievement, achievementCategoryData.LastClaimedDate);
				UpdateLastClaimable(achievementCategoryData.LastClaimableAchievement, achievementCategoryData.LastClaimableDate);
			}
			Progress = (float)TotalIncludingAllRanksClaimedCount / (float)TotalShownIncludingAllRanksCount;
		}

		public void GetAllAchievements(ref List<AchievementMapping.Definition> _achievementsDef)
		{
			for (int i = 0; i < AchievementsDef.Count; i++)
			{
				AchievementMapping.Definition definition = AchievementsDef[i];
				if (!definition.hidden || IdToAchievements.ContainsKey(definition.id))
				{
					_achievementsDef.Add(definition);
				}
			}
			for (int j = 0; j < SubCategories.Count; j++)
			{
				SubCategories[j].GetAllAchievements(ref _achievementsDef);
			}
		}

		private void UpdateOwnCount()
		{
			for (int i = 0; i < AchievementsDef.Count; i++)
			{
				AchievementMapping.Definition definition = AchievementsDef[i];
				OwnIncludingAllRanksCount += definition.ranks.Length;
				OwnCount++;
			}
		}

		private static int SortSubCategories(AchievementCategoryData _categoryA, AchievementCategoryData _categoryB)
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			AchievementCategoryDefinition achievementCategoryDefinition = achievementsManager.GetAchievementCategoryDefinition(_categoryA.CategoryId);
			AchievementCategoryDefinition achievementCategoryDefinition2 = achievementsManager.GetAchievementCategoryDefinition(_categoryB.CategoryId);
			if (Helpers.Verify(achievementCategoryDefinition != null && achievementCategoryDefinition2 != null, "AchievementCategoryData and GameDB AchievementCategoryDefinition data is OutOfSync!"))
			{
				return achievementCategoryDefinition.UiPriority.CompareTo(achievementCategoryDefinition2.UiPriority);
			}
			return 0;
		}

		private static int SortAchievementsDef(AchievementMapping.Definition _definitionA, AchievementMapping.Definition _definitionB)
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			AchievementDefinition achievementDefinition = achievementsManager.GetAchievementDefinition(_definitionA.id);
			AchievementDefinition achievementDefinition2 = achievementsManager.GetAchievementDefinition(_definitionB.id);
			if (Helpers.Verify(achievementDefinition != null && achievementDefinition2 != null, "AchievementMapping.Definition and GameDB AchievementDefinition data is OutOfSync"))
			{
				return achievementDefinition.UiPriority.CompareTo(achievementDefinition2.UiPriority);
			}
			return 0;
		}
	}
}
