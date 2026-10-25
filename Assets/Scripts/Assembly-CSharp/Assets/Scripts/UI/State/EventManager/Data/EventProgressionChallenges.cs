using System;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventProgressionChallenges : EventBase
	{
		public class EventProgression
		{
			public class EventProgressionReward
			{
				public LootTemplate RewardItem;

				public int RewardMultiplier;

				public int EventScoreMarker;
			}

			public class EventProgressionChallenge
			{
				public string ChallengeId;

				public ECounterType ChallengeCounterType;

				public int SortIndex;
			}

			public class EventProgressionTheme
			{
				public string NotificationImageGuiId;

				public string ScreenImageGuiId;

				public string BannerImageGuiId;

				public string BannerBackgroundLargeGuId;

				public string BannerBackgroundSmallGuId;

				public string BannerFontColorHeaderGuId;

				public string BannerFontColorDescriptionGuId;

				public string BannerFontColorTimerHeaderGuId;

				public string BannerFontColorTimerGuId;

				public string BannerTintingColorGuId;
			}

			public string MainEventId;

			public int MainHeaderTitleKey;

			public int MainHeaderDescriptionKey;

			public EventProgressionTheme LayoutTheme = new EventProgressionTheme();

			public EventProgressionReward[] MainRewards = new EventProgressionReward[3];
		}

		private const string c_themeNotificationImageGuiId = "notificationImageGuId";

		private const string c_themeScreenImageGuiId = "screenImageGuiId";

		private const string c_themeBannerBannerImageGuiId = "bannerImageGuiId";

		private const string c_themeBannerBackgroundLargeGuId = "bannerBackgroundLargeGuId";

		private const string c_themeBannerBackgroundSmallGuId = "bannerBackgroundSmallGuId";

		private const string c_themeBannerFontColorHeaderGuId = "bannerFontColor1GuId";

		private const string c_themeBannerFontColorDescriptionGuId = "bannerFontColor2GuId";

		private const string c_themeBannerFontColorTimerHeaderGuId = "bannerFontColor3GuId";

		private const string c_themeBannerFontColorTimerGuId = "bannerFontColor4GuId";

		private const string c_themeBannerTintingColorGuId = "bannerTintingColorGuId";

		private const string c_progressionEventId = "id";

		private const string c_progressionEventHeader = "h";

		private const string c_progressionEventSubHeader = "sh";

		private const string c_progressionEventReward1 = "reward.1";

		private const string c_progressionEventReward2 = "reward.2";

		private const string c_progressionEventReward3 = "reward.3";

		private const string c_eventScoreMarkerReward1 = "reward.m.1";

		private const string c_eventScoreMarkerReward2 = "reward.m.2";

		private const string c_eventScoreMarkerReward3 = "reward.m.3";

		private const string c_eventCountReward1 = "reward.mu.1";

		private const string c_eventCountReward2 = "reward.mu.2";

		private const string c_eventCountReward3 = "reward.mu.3";

		public EventProgression EventProgressionData { get; set; }

		public EventProgressionChallenges(Event _event)
			: base(_event)
		{
		}

		protected override void OnProcessEvent(Event _event)
		{
			EventProgressionData = new EventProgression();
			if (base.Event.lootProperties != null)
			{
				for (int i = 0; i < base.Event.lootProperties.Length; i++)
				{
					Event.LootProperty lootProperty = base.Event.lootProperties[i];
					if (lootProperty != null)
					{
						if (string.Compare(lootProperty.name, "reward.1", StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							GetMainRewardByIndex(0).RewardItem = lootProperty.value;
						}
						else if (string.Compare(lootProperty.name, "reward.2", StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							GetMainRewardByIndex(1).RewardItem = lootProperty.value;
						}
						else if (string.Compare(lootProperty.name, "reward.3", StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							GetMainRewardByIndex(2).RewardItem = lootProperty.value;
						}
					}
				}
			}
			for (int j = 0; j < _event.properties.Length; j++)
			{
				Event.Property property = _event.properties[j];
				if (property != null)
				{
					if (string.Compare(property.name, "id", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.MainEventId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "h", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.MainHeaderTitleKey = property.GetIntValue();
					}
					else if (string.Compare(property.name, "sh", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.MainHeaderDescriptionKey = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.mu.1", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(0).RewardMultiplier = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.mu.2", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(1).RewardMultiplier = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.mu.3", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(2).RewardMultiplier = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.m.1", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(0).EventScoreMarker = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.m.2", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(1).EventScoreMarker = property.GetIntValue();
					}
					else if (string.Compare(property.name, "reward.m.3", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						GetMainRewardByIndex(2).EventScoreMarker = property.GetIntValue();
					}
					else if (string.Compare(property.name, "notificationImageGuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.NotificationImageGuiId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "screenImageGuiId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.ScreenImageGuiId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerImageGuiId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerImageGuiId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerBackgroundLargeGuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerBackgroundLargeGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerBackgroundSmallGuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerBackgroundSmallGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerFontColor1GuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerFontColorHeaderGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerFontColor2GuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerFontColorDescriptionGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerFontColor3GuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerFontColorTimerHeaderGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerFontColor4GuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerFontColorTimerGuId = property.GetStringValue();
					}
					else if (string.Compare(property.name, "bannerTintingColorGuId", StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						EventProgressionData.LayoutTheme.BannerTintingColorGuId = property.GetStringValue();
					}
				}
			}
		}

		private EventProgression.EventProgressionReward GetMainRewardByIndex(int _index)
		{
			EventProgression.EventProgressionReward eventProgressionReward = EventProgressionData.MainRewards[_index];
			if (eventProgressionReward == null)
			{
				eventProgressionReward = new EventProgression.EventProgressionReward();
			}
			EventProgressionData.MainRewards[_index] = eventProgressionReward;
			return eventProgressionReward;
		}
	}
}
