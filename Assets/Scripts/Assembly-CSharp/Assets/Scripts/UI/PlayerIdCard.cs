using AcierProtocol;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class PlayerIdCard : MonoBehaviour
	{
		[SerializeField]
		private Renderer m_renderer;

		[SerializeField]
		private UIExtendedLabel m_lblFrontName;

		[SerializeField]
		private UIExtendedLabel m_lblBackName;

		[SerializeField]
		private UIExtendedLabel m_lblBackUserId;

		public void SetUserName(string _userName)
		{
			if (!string.IsNullOrEmpty(_userName))
			{
				m_lblFrontName.text = _userName;
				m_lblBackName.text = _userName;
			}
		}

		private void OnEnable()
		{
			m_renderer.useLightProbes = true;
			string text = "User Name";
			string userId = "User Id";
			if (Singleton<StateManager>.Instance.ProfileManager != null)
			{
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				if (profile != null)
				{
					text = profile.userName;
					userId = FriendUtil.ToHumanReadableUserHash(profile.userIdHash);
				}
			}
			Set(text, userId);
		}

		private void Set(string _name, string _userId)
		{
			m_lblFrontName.text = _name;
			m_lblBackName.text = _name;
			m_lblBackUserId.text = _userId;
		}
	}
}
