using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class EventProgressionExtensions
	{
		public static bool HasProgressDataForEvent(this EventProgressionChallenges.EventProgression _progression)
		{
			EventsProgress eventsProgress = Singleton<StateManager>.Instance.ProfileManager.Profile.eventsProgress;
			return eventsProgress != null && eventsProgress.mainEventId == _progression.MainEventId;
		}

		public static long GetFormatedTimesToCompleteVaule(this EventsProgress.ChallengeEvent _challenge)
		{
			UIAchievementGoal uIAchievementGoal = new UIAchievementGoal(_challenge.timesToComplete, _challenge.challengeCounter);
			return uIAchievementGoal.DisplayValue;
		}

		public static string GetChallengeHeaderText(this EventsProgress.ChallengeEvent _challenge)
		{
			CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry counterTypeLocaKey = Singleton<StateManager>.Instance.TemplateManager.CounterTypeLocaKeyMapping.GetCounterTypeLocaKey(_challenge.challengeCounter);
			if (counterTypeLocaKey == null)
			{
				return "Error Missing loca mapping";
			}
			return LocalizationManager.Instance.Get(counterTypeLocaKey.titleLocaKey);
		}

		public static string GetChallengeDescriptionText(this EventsProgress.ChallengeEvent _challenge)
		{
			CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry counterTypeLocaKey = Singleton<StateManager>.Instance.TemplateManager.CounterTypeLocaKeyMapping.GetCounterTypeLocaKey(_challenge.challengeCounter);
			if (counterTypeLocaKey == null)
			{
				return "Error Missing loca mapping";
			}
			return LocalizationManager.Instance.Get(counterTypeLocaKey.descriptionLocaKey, _challenge.CreateProgressionMappings());
		}

		public static StringMapper CreateProgressionMappings(this EventsProgress.ChallengeEvent _challenge)
		{
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("event_amount", _challenge.GetFormatedTimesToCompleteVaule());
			StringMapper stringMapper2 = stringMapper;
			CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry counterTypeLocaKey = Singleton<StateManager>.Instance.TemplateManager.CounterTypeLocaKeyMapping.GetCounterTypeLocaKey(_challenge.challengeCounter);
			for (int i = 0; i < counterTypeLocaKey.placeholderLocaKeys.Length; i++)
			{
				stringMapper2.Add("event_type_" + i, LocalizationManager.Instance.Get(counterTypeLocaKey.placeholderLocaKeys[i]));
			}
			return stringMapper2;
		}

		public static long GetChallengeCount(this EventsProgress.ChallengeEvent _challenge)
		{
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
			if (firstValidEventProgressionEvent != null)
			{
				long counterAmountByType = Singleton<StateManager>.Instance.ProfileManager.Profile.GetCounterAmountByType(_challenge.challengeCounter);
				long value = Math.Min(_challenge.timesToComplete, counterAmountByType - _challenge.startCounterValue);
				UIAchievementGoal uIAchievementGoal = new UIAchievementGoal(value, _challenge.challengeCounter);
				return uIAchievementGoal.DisplayValue;
			}
			return -1L;
		}

		public static long GetPreviousChallengeCount(this EventsProgress.ChallengeEvent _challenge, LastProfileData _lastData)
		{
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
			if (firstValidEventProgressionEvent != null)
			{
				long counterAmountByType = _lastData.GetCounterAmountByType(_challenge.challengeCounter);
				long value = Math.Min(_challenge.timesToComplete, counterAmountByType - _challenge.startCounterValue);
				UIAchievementGoal uIAchievementGoal = new UIAchievementGoal(value, _challenge.challengeCounter);
				return uIAchievementGoal.DisplayValue;
			}
			return -1L;
		}
	}
}
