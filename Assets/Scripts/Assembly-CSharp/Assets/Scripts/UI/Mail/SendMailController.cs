using System;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class SendMailController : MonoBehaviour
	{
		private const int MIN_SUBJECT_LENGTH = 1;

		private const int MIN_BODY_LENGTH = 1;

		private const float RESERVED_BODY_SPACE = 0.3f;

		private string changedBodyText = string.Empty;

		private bool isHtcOneMini;

		[SerializeField]
		private UIButtonProxy m_sendButton;

		[SerializeField]
		private UIButtonProxy m_cancelButton;

		[SerializeField]
		private UIInput m_subject;

		[SerializeField]
		private UIInput m_body;

		[SerializeField]
		private UIExtendedLabel m_title;

		private SendMailConfiguration m_config;

		private bool Visible
		{
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public void Show(SendMailConfiguration _config)
		{
			m_config = _config;
			InitScreen();
			UpdateScreen();
			Visible = true;
		}

		private void Awake()
		{
			string deviceIdentifier = QualitySettingsHelper.GetDeviceIdentifier();
			if (deviceIdentifier.Contains("HTC") && deviceIdentifier.Contains("One") && deviceIdentifier.Contains("mini"))
			{
				isHtcOneMini = true;
			}
			m_subject.characterLimit = Singleton<StateManager>.Instance.TemplateManager.Globals.mailSubjectCharLimit;
			m_body.characterLimit = Singleton<StateManager>.Instance.TemplateManager.Globals.mailBodyCharLimit;
		}

		private void Clear()
		{
			DisableListeners();
			m_body.value = string.Empty;
			m_subject.value = string.Empty;
			m_title.text = string.Empty;
			m_config = null;
		}

		private void OnDisable()
		{
			m_subject.onValidate = null;
			m_body.onValidate = null;
			DisableListeners();
		}

		private void OnEnable()
		{
			m_subject.onValidate = ValidateInput;
			m_body.onValidate = ValidateInput;
			EnableListeners();
		}

		private void EnableListeners()
		{
			EventDelegate.Add(m_cancelButton.onClick, OnCancel);
			EventDelegate.Add(m_sendButton.onClick, OnSend);
			EventDelegate.Add(m_subject.onChange, OnSubjectChange);
			EventDelegate.Add(m_body.onChange, OnBodyChange);
		}

		private void DisableListeners()
		{
			EventDelegate.Remove(m_cancelButton.onClick, OnCancel);
			EventDelegate.Remove(m_sendButton.onClick, OnSend);
			EventDelegate.Remove(m_subject.onChange, OnSubjectChange);
			EventDelegate.Remove(m_body.onChange, OnBodyChange);
		}

		private void OnSubjectChange()
		{
			UpdateScreen();
		}

		private void OnBodyChange()
		{
			if (isHtcOneMini)
			{
				CheckForKeyBoardClose();
			}
			UpdateScreen();
		}

		private void CheckForKeyBoardClose()
		{
			if (TouchScreenKeyboard.visible)
			{
				changedBodyText = m_body.value;
			}
			else
			{
				m_body.value = changedBodyText;
			}
		}

		public void HideMailScreen()
		{
			OnCancel();
		}

		private void OnCancel()
		{
			Clear();
			Visible = false;
		}

		private void OnSend()
		{
			string subject = ((!string.IsNullOrEmpty(m_subject.value)) ? m_subject.value.Substring(0, Mathf.Min(Singleton<StateManager>.Instance.TemplateManager.Globals.mailSubjectCharLimit, m_subject.value.Length)) : string.Empty);
			string body = ((!string.IsNullOrEmpty(m_body.value)) ? m_body.value.Substring(0, Mathf.Min(Singleton<StateManager>.Instance.TemplateManager.Globals.mailBodyCharLimit, m_body.value.Length)) : string.Empty);
			AsyncCallback callback = OnMailSent;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.SendMail(m_config.RecieverId, subject, body, _asyncResult);
			}, null, callback);
		}

		private void OnMailSent(IAsyncResult _asyncResult)
		{
			AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
			if (asyncErrorResult == null || asyncErrorResult.Exception == null)
			{
				Visible = false;
				if (m_config.OnMailSent != null)
				{
					m_config.OnMailSent(m_config.RecieverId);
				}
			}
		}

		private void UpdateScreen()
		{
			m_sendButton.isEnabled = m_subject.value.Length >= 1 && m_body.value.Length >= 1;
		}

		private void InitScreen()
		{
			int mailBodyCharLimit = Singleton<StateManager>.Instance.TemplateManager.Globals.mailBodyCharLimit;
			int num = (int)Math.Floor(0.3f * (float)mailBodyCharLimit);
			string value = ((!string.IsNullOrEmpty(m_config.Body)) ? m_config.Body.Substring(0, Mathf.Min(m_config.Body.Length, mailBodyCharLimit - num)) : string.Empty);
			m_body.value = value;
			m_body.cursorPosition = 0;
			m_subject.value = m_config.Subject;
			m_title.text = LocalizationManager.Instance.Get(4393, new StringMapper { { "Mail_Username", m_config.RecieverName } });
		}

		private char ValidateInput(string _text, int _index, char _addedChar)
		{
			return LocalizationManager.Instance.CanDisplayGlyph(_addedChar) ? _addedChar : '\0';
		}
	}
}
