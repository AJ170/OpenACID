using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
	public class MailScreen : UIScreenChild
	{
		[CheckAssigned]
		[SerializeField]
		private UIButton m_sendButton;

		[CheckAssigned]
		[SerializeField]
		private SendMailController m_sendMailController;

		[SerializeField]
		[CheckAssigned]
		private RecieverListController m_recievers;

		[CheckAssigned]
		[SerializeField]
		private MailController m_mailController;

		[CheckAssigned]
		[SerializeField]
		private MailHeadersController m_headersController;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			m_mailController.Initialize();
			m_recievers.Initialize();
			m_headersController.Initialize();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
			if (m_recievers != null)
			{
				m_recievers.Clear();
			}
			if (m_mailController != null)
			{
				m_mailController.Clear();
			}
			if ((bool)m_headersController)
			{
				m_headersController.Clear();
			}
		}

		private void OnScreenActivate()
		{
			m_sendButton.isEnabled = true;
			m_recievers.ScreenActivate();
			ActivateListeners();
			m_headersController.UpdateData();
			m_mailController.Visible = true;
			m_recievers.Visible = false;
		}

		private void OnScreenDeActivate()
		{
			m_headersController.ScreenDeactivate();
			DeactivateListeners();
		}

		private void ActivateListeners()
		{
			EventDelegate.Add(m_sendButton.onClick, OnSendMailClicked);
			m_recievers.OnRecieverSelected += OnRecieverSelected;
			m_headersController.OnMailSelected += OnMailSelected;
		}

		private void DeactivateListeners()
		{
			EventDelegate.Remove(m_sendButton.onClick, OnSendMailClicked);
			m_recievers.OnRecieverSelected -= OnRecieverSelected;
			m_headersController.OnMailSelected -= OnMailSelected;
		}

		private void OnSendMailClicked()
		{
			if (m_headersController != null)
			{
				m_headersController.DeselectCurrentHeader();
			}
			if (m_mailController.Visible)
			{
				m_mailController.Visible = false;
			}
			m_sendButton.isEnabled = false;
			m_recievers.ResetWrapScrollView();
			m_recievers.Visible = true;
		}

		private void OnMailSelected(MailHeader _header)
		{
			m_sendButton.isEnabled = true;
			m_recievers.Visible = false;
			m_mailController.UpdateMail(_header);
			if (!m_mailController.Visible)
			{
				m_mailController.Visible = true;
			}
		}

		private void OnRecieverSelected(FriendProfile _reciever)
		{
			m_sendMailController.Show(new SendMailConfiguration
			{
				RecieverId = _reciever.id,
				RecieverName = _reciever.userName,
				OnMailSent = OnMailSent
			});
		}

		private void OnMailSent(string _recieverId)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_social_email_send);
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4570));
		}

		public override void OnBackKey()
		{
			if (m_sendMailController.gameObject.activeSelf)
			{
				m_sendMailController.HideMailScreen();
			}
			else
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.SocialSelectionScreen, new UITweenHandle(null, null));
			}
		}
	}
}
