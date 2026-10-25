using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
	public class UISocialUpdateHandler : MonoBehaviour
	{
		public enum ETarget
		{
			All = 0,
			Mail = 1,
			Friends = 2,
			Achievements = 3
		}

		[SerializeField]
		private UISprite m_icon;

		[SerializeField]
		private ETarget m_target;

		private void Awake()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdate);
			Singleton<StateManager>.Instance.MailManager.OnMailsChanged += OnMailsChanged;
		}

		private void OnEnable()
		{
			OnProfileUpdate();
		}

		private void OnDestroy()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdate);
			Singleton<StateManager>.Instance.MailManager.OnMailsChanged -= OnMailsChanged;
		}

		private void OnMailsChanged(List<MailHeader> _headers)
		{
			OnProfileUpdate();
		}

		private void OnProfileUpdate(params object[] _params)
		{
			switch (m_target)
			{
			case ETarget.All:
				m_icon.gameObject.SetActive(UpdateAll());
				break;
			case ETarget.Mail:
				m_icon.gameObject.SetActive(UpdateMail());
				break;
			case ETarget.Friends:
				m_icon.gameObject.SetActive(UpdateFriends());
				break;
			case ETarget.Achievements:
				m_icon.gameObject.SetActive(UpdateAchievements());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private bool UpdateAll()
		{
			bool flag = UpdateMail();
			flag |= UpdateFriends();
			return flag | UpdateAchievements();
		}

		private bool UpdateMail()
		{
			if (!Singleton<StateManager>.Instance.IsProfileManagerInitialized)
			{
				return false;
			}
			if (Singleton<StateManager>.Instance.ProfileManager.Profile == null)
			{
				return false;
			}
			bool flag = Singleton<StateManager>.Instance.ProfileManager.Profile.dirtyFlags.IsBitSet(Convert.ToInt32(EDirtyFlag.DirtyMails));
			return flag | (Singleton<StateManager>.Instance.MailManager.UnreadMails > 0);
		}

		private bool UpdateFriends()
		{
			return false;
		}

		private bool UpdateAchievements()
		{
			return false;
		}
	}
}
