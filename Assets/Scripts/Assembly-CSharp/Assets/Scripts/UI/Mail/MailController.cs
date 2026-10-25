using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class MailController : MonoBehaviour, IInitialized
	{
		private static readonly StringId s_NoMailLoaded = new StringId(4405);

		[CheckAssigned]
		[SerializeField]
		private MailBodyController[] m_bodyController;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_background;

		[SerializeField]
		[CheckAssigned]
		private UITexture m_noMailTexture;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_noMailHeader;

		[CheckAssigned]
		[SerializeField]
		private BBTargetedTween m_tween;

		[SerializeField]
		[CheckAssigned]
		private SendMailController m_sendMail;

		private MailBody m_mailBody;

		private MailHeader m_mailHeader;

		private int m_currentBuffer;

		private bool m_visible;

		private MailBodyController CurrentController
		{
			get
			{
				return m_bodyController[m_currentBuffer];
			}
		}

		private MailBodyController BufferController
		{
			get
			{
				return m_bodyController[(m_currentBuffer == 0) ? 1 : 0];
			}
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				if (value)
				{
					m_background.SetActive(true);
					m_tween.SetTweenToBeginning();
					m_tween.StartTweenPlayForward();
				}
				else
				{
					m_tween.SetTweenToEnd();
					m_tween.StartTweenPlayReverse();
				}
				m_visible = value;
			}
		}

		public void UpdateMail(MailHeader _header)
		{
			m_mailHeader = _header;
			if (_header != null)
			{
				Singleton<StateManager>.Instance.MailManager.GetMailBody(_header, OnBodyLoaded);
			}
			else
			{
				ActivateHelpContent();
			}
		}

		public void Initialize()
		{
			m_tween.SetTweenToBeginning();
		}

		public void Clear()
		{
			m_tween.SetTweenToBeginning();
		}

		private void Awake()
		{
			m_noMailTexture.gameObject.SetActive(true);
			m_noMailHeader.gameObject.SetActive(true);
		}

		private void OnEnable()
		{
			m_background.SetActive(true);
			m_noMailHeader.SetLocalizationKey(s_NoMailLoaded);
			MailBodyController currentController = CurrentController;
			currentController.OnReply = (Action<MailHeader, MailBody>)Delegate.Combine(currentController.OnReply, new Action<MailHeader, MailBody>(OnReply));
			MailBodyController bufferController = BufferController;
			bufferController.OnReply = (Action<MailHeader, MailBody>)Delegate.Combine(bufferController.OnReply, new Action<MailHeader, MailBody>(OnReply));
		}

		private void OnDisable()
		{
			MailBodyController currentController = CurrentController;
			currentController.OnReply = (Action<MailHeader, MailBody>)Delegate.Remove(currentController.OnReply, new Action<MailHeader, MailBody>(OnReply));
			MailBodyController bufferController = BufferController;
			bufferController.OnReply = (Action<MailHeader, MailBody>)Delegate.Remove(bufferController.OnReply, new Action<MailHeader, MailBody>(OnReply));
		}

		private void ActivateHelpContent()
		{
			if (CurrentController.Visible)
			{
				CurrentController.Visible = false;
			}
			m_noMailTexture.gameObject.SetActive(true);
			m_noMailHeader.gameObject.SetActive(true);
			m_noMailTexture.color = Color.white;
			m_noMailHeader.SetLocalizationKey(6102);
		}

		private void OnReply(MailHeader _mailHeader, MailBody _mailBody)
		{
			m_sendMail.Show(new SendMailConfiguration
			{
				RecieverId = _mailHeader.senderId,
				RecieverName = _mailHeader.senderName,
				Subject = LocalizationManager.Instance.Get(4503, new StringMapper { { "Original_Mail_Subject", _mailHeader.subject } }),
				Body = LocalizationManager.Instance.Get(4504, new StringMapper { { "Original_Mail_Text", _mailBody.body } }),
				OnMailSent = OnMailSent
			});
		}

		private void OnMailSent(string _recieverid)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_social_email_send);
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4570));
		}

		private void OnBodyLoaded(MailBody _body)
		{
			if (_body != null)
			{
				MailCache.MarkAsRead(m_mailHeader, true);
				m_mailBody = _body;
				StopCoroutine("co_fadeIn");
				StartCoroutine("co_fadeIn");
			}
			else
			{
				ActivateHelpContent();
			}
		}

		private IEnumerator co_fadeIn()
		{
			BufferController.PrepareMail(m_mailHeader, m_mailBody);
			while (!CurrentController.CanDeActivate && !BufferController.CanActivate)
			{
				yield return new WaitForEndOfFrame();
			}
			m_noMailTexture.gameObject.SetActive(false);
			m_noMailHeader.gameObject.SetActive(false);
			BufferController.Visible = true;
			CurrentController.Visible = false;
			m_currentBuffer = ((m_currentBuffer == 0) ? 1 : 0);
		}
	}
}
