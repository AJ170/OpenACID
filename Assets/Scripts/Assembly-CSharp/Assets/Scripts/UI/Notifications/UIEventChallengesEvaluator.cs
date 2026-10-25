using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Notifications
{
	public class UIEventChallengesEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Event;
			}
		}

		public override void Init()
		{
			base.Init();
			Singleton<StateManager>.Instance.ProfileManager.OnEventProgressChanged += new SimpleDelegate.CallbackFunction(OnEventProgressChanged);
		}

		public override void Destroy()
		{
			base.Destroy();
			Singleton<StateManager>.Instance.ProfileManager.OnEventProgressChanged -= new SimpleDelegate.CallbackFunction(OnEventProgressChanged);
		}

		public override void Update()
		{
			UpdateEvntProgressNewState();
		}

		private void OnEventProgressChanged(object[] _params)
		{
			UpdateEvntProgressNewState();
		}

		private void UpdateEvntProgressNewState()
		{
			int contractHeroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.ContractHeroicMissionRank;
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			ProgressionQuests.RankQuests questByRank = profile.GetQuestByRank(contractHeroicMissionRank);
			if (questByRank != null)
			{
				ProgressionQuests.RankQuests.Quest latestActiveQuest = questByRank.GetLatestActiveQuest();
				if (latestActiveQuest != null && latestActiveQuest.status == EQuestStatus.Finished)
				{
					base.Value = true;
					return;
				}
			}
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			base.Value = eventProgressionChallengesManager.HasValidProgressionEvent() && Singleton<StateManager>.Instance.ProfileManager.Profile.HasClaimableEvent();
		}
	}
}
