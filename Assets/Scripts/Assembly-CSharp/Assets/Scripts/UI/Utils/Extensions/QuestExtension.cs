using System;
using AcierProtocol;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class QuestExtension
	{
		public static ProgressionQuests.RankQuests.Quest GetLatestActiveQuest(this ProgressionQuests.RankQuests _rankQuest)
		{
			ProgressionQuests.RankQuests.Quest[] questChain = _rankQuest.questChain;
			foreach (ProgressionQuests.RankQuests.Quest quest in questChain)
			{
				if (quest.status != EQuestStatus.Claimed)
				{
					return quest;
				}
			}
			return null;
		}

		public static bool HasClaimableQuest(this ProgressionQuests.RankQuests _rankQuest)
		{
			ProgressionQuests.RankQuests.Quest[] questChain = _rankQuest.questChain;
			foreach (ProgressionQuests.RankQuests.Quest quest in questChain)
			{
				if (quest.status == EQuestStatus.Finished)
				{
					if (quest.reward.elements.elements.Length == 0)
					{
						return true;
					}
					if (quest.reward.elements.elements[0].HasInventorySpaceForItem())
					{
						return true;
					}
				}
			}
			return false;
		}

		public static long ComputeRemainingMilliSeconds(this ProgressionQuests.RankQuests _rankQuest)
		{
			DateTime dateTime = ServerTimeManager.ToUtcTime((long)_rankQuest.expiresAtTimeStamp);
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			long val = (long)(dateTime - utcNow).TotalMilliseconds;
			return Math.Max(val, 0L);
		}
	}
}
