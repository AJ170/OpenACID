using AcierProtocol;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UbiPlayground
{
	public class UbiPlaygroundManager
	{
		public static bool OverrideIsUplayAvailable { get; set; }

		public static bool IsEnabledForTesting { get; set; }

		public static bool IsLinkedToProfileForTesting { get; set; }

		public static bool IsUplayAvailable
		{
			get
			{
				if (OverrideIsUplayAvailable)
				{
					return IsEnabledForTesting;
				}
				return false;
			}
		}

		public static bool IsProfileLinkedToUplay
		{
			get
			{
				if (OverrideIsUplayAvailable)
				{
					return IsLinkedToProfileForTesting;
				}
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				string linkedGameUserId = LinkedGameUserId;
				string text = ((profile == null) ? null : profile.userIdHash);
				return linkedGameUserId != null && linkedGameUserId == text;
			}
		}

		public static string LinkedGameUserId
		{
			get
			{
				return PlayerPrefs.GetString("LinkedUplayGameUserId", string.Empty);
			}
			set
			{
				PlayerPrefs.SetString("LinkedUplayGameUserId", value);
			}
		}
	}
}
