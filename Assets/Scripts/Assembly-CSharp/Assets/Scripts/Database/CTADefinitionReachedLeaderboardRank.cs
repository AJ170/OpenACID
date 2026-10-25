using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.ProfileManager;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionReachedLeaderboardRank : CTADefinitionBase
	{
		public int WaitHours { get; set; }

		public int MinRank { get; set; }

		public CTADefinitionReachedLeaderboardRank(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			base.CheckOnProfileUpdate = true;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			WaitHours = _s.SerializeInt("WaitHours", WaitHours);
			MinRank = _s.SerializeInt("MinRank", MinRank);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (!instance.CTACounterCheckTimeDiff(LocalStorage.CTACounterLastReachedLeaderboardRank, WaitHours * 3600))
			{
				return EResult.Proceed;
			}
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			if (!eventProgressionChallengesManager.HasValidProgressionEvent())
			{
				return EResult.Proceed;
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (profileManager.PreviousProfileData.EventsProgress == null)
			{
				return EResult.Proceed;
			}
			int leaderboardPosition = profileManager.Profile.eventsProgress.leaderboardPosition;
			int leaderboardPosition2 = profileManager.PreviousProfileData.EventsProgress.leaderboardPosition;
			long score = profileManager.PreviousProfileData.EventsProgress.score;
			if (leaderboardPosition <= 0 || leaderboardPosition > MinRank || leaderboardPosition == leaderboardPosition2 || score == 0L)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastReachedLeaderboardRank);
			return EResult.Fullfilled;
		}
	}
}
