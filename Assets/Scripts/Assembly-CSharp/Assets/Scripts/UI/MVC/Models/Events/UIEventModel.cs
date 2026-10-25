using System;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.MVC.Models.Events
{
	public class UIEventModel : UITabButtonModel
	{
		public EventProgressionChallenges EventProgressionChallenges { get; set; }

		public override long EndTimeUTC
		{
			get
			{
				return (!IsEnabled) ? 0 : EventProgressionChallenges.Event.endTimeUTC;
			}
		}

		public override StringId TitleLoca
		{
			get
			{
				return (!IsEnabled) ? 8292 : EventProgressionChallenges.EventProgressionData.MainHeaderTitleKey;
			}
		}

		public override StringId SubTitleLoca
		{
			get
			{
				return (!IsEnabled) ? ((StringId)8295) : StringId.Invalid;
			}
		}

		public override bool IsEnabled
		{
			get
			{
				return EventProgressionChallenges != null && EventProgressionChallenges.EventProgressionData != null && EventProgressionChallenges.EventProgressionData.HasProgressDataForEvent();
			}
		}

		public bool HasEventEnded()
		{
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			DateTime dateTime = ServerTimeManager.ToUtcTime(EndTimeUTC);
			double totalSeconds = (dateTime - utcNow).TotalSeconds;
			return totalSeconds < 0.0;
		}
	}
}
