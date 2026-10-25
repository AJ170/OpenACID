using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendInput : MonoBehaviour
	{
		[SerializeField]
		private InviteScreen m_inviteScreen;

		[SerializeField]
		private SubScreen m_subScreen;

		[SerializeField]
		private GameObject m_background;

		[SerializeField]
		private GameObject m_fakeKeyboard;

		[SerializeField]
		private UIInput m_fakeKeyboardLabel;

		[SerializeField]
		private UIExtendedButton m_fakeKeyboardButton;

		private TouchScreenKeyboard m_keyboard;

		private bool m_visible;

		private void Awake()
		{
			m_fakeKeyboard.SetActive(false);
		}

		private void Update()
		{
			if (m_visible && (m_keyboard.done || m_keyboard.wasCanceled || !m_keyboard.active))
			{
				Hide();
			}
			if (!m_subScreen.gameObject.activeSelf)
			{
				m_keyboard.active = false;
				base.gameObject.SetActive(false);
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			m_background.SetActive(true);
			m_subScreen.Show();
			m_visible = true;
			m_keyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.NamePhonePad, false, false, false, false, "XXX XXXX XXX");
		}

		public void Hide()
		{
			m_background.SetActive(false);
			m_subScreen.Hide();
			string text = null;
			m_visible = false;
			if (m_keyboard.done && !m_keyboard.wasCanceled)
			{
				text = m_keyboard.text;
			}
			if (text != null)
			{
				m_inviteScreen.RequestAddFriend(text);
			}
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.FriendsListScreen, new UITweenHandle());
		}

		public void HideOnAndroidBackKey()
		{
			m_background.SetActive(false);
			m_subScreen.Hide();
		}
	}
}
