using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	internal class UISocialButtonUnlock : MonoBehaviour
	{
		public enum ESocialButtons
		{
			Achievements = 0,
			Facebook = 1,
			Friends = 2,
			Mail = 3,
			Twitter = 4,
			Leaderboard = 5
		}

		[SerializeField]
		private ESocialButtons m_type;

		[SerializeField]
		private UIButtonProxy m_button;

		[SerializeField]
		private BoxCollider m_secondaryCollider;

		private void Awake()
		{
		}

		private void OnEnable()
		{
			bool flag = IsSocialTabUnlocked();
			if (m_button != null)
			{
				m_button.AllowDisabledClick = flag;
				m_button.isEnabled = flag;
			}
			if (m_secondaryCollider != null)
			{
				m_secondaryCollider.enabled = flag;
			}
		}

		private bool IsSocialTabUnlocked()
		{
			switch (m_type)
			{
			case ESocialButtons.Achievements:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabAchievementsGUID);
			case ESocialButtons.Facebook:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabFacebookGUID);
			case ESocialButtons.Friends:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabFriendsGUID);
			case ESocialButtons.Mail:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabMailGUID);
			case ESocialButtons.Twitter:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabTwitterGUID);
			case ESocialButtons.Leaderboard:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabLeaderboardGUID);
			default:
				return true;
			}
		}
	}
}
