using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class MailBodyController : MonoBehaviour
	{
		private static readonly StringId s_mailDeletedId = new StringId(7794);

		public Action<MailHeader, MailBody> OnReply;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_content;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_buttonBar;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_titleLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_senderLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_subjectLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_dateLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_timeLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_bodyLabel;

		[SerializeField]
		[CheckAssigned]
		private UIButton m_replyButton;

		[SerializeField]
		[CheckAssigned]
		private UIButton m_deleteButton;

		[CheckAssigned]
		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_vBox;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_senderSubjectAlignment;

		[SerializeField]
		[CheckAssigned]
		private BBTargetedTween m_tween;

		[CheckAssigned]
		[SerializeField]
		private MailAttachmentController m_attachmentController;

		private MailBody m_mailBody;

		private MailHeader m_mailHeader;

		private bool m_isVisible;

		private int m_animationWaitCounter;

		private bool m_pendingProcess;

		private bool m_animating;

		private bool m_layoutFinished;

		private bool m_listenersAdded;

		private UIRecivedItemManager m_itemAnimator;

		public bool Visible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				if (value)
				{
					m_tween.SetTweenToBeginning();
					m_tween.StartTweenPlayForward();
					EnableListeners();
				}
				else
				{
					m_tween.SetTweenToEnd();
					m_tween.StartTweenPlayReverse();
					DisableListeners();
				}
				m_isVisible = value;
			}
		}

		public bool CanDeActivate
		{
			get
			{
				return !m_pendingProcess && !m_animating;
			}
		}

		public bool CanActivate
		{
			get
			{
				return m_layoutFinished;
			}
		}

		public void PrepareMail(MailHeader _header, MailBody _body)
		{
			m_mailHeader = _header;
			m_mailBody = _body;
			m_layoutFinished = false;
			UpdateContent();
		}

		private void Awake()
		{
			m_content.SetActive(false);
		}

		private void OnEnable()
		{
			m_itemAnimator = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			m_attachmentController.OnLayoutFinished += UpdateLayout;
			EnableListeners();
		}

		private void OnDisable()
		{
			m_mailBody = null;
			m_mailHeader = null;
			m_animationWaitCounter = 0;
			m_attachmentController.OnLayoutFinished -= UpdateLayout;
			DisableListeners();
		}

		private void EnableListeners()
		{
			if (!m_listenersAdded)
			{
				m_listenersAdded = true;
				EventDelegate.Add(m_replyButton.onClick, OnReplyInternal);
				EventDelegate.Add(m_deleteButton.onClick, OnDelete);
				m_attachmentController.OnProcessMailRequest = OnProcessMailRequest;
				Singleton<StateManager>.Instance.MailManager.OnGiftProcessedSuccessful += OnGiftProcessedSuccessful;
			}
		}

		private void DisableListeners()
		{
			m_listenersAdded = false;
			EventDelegate.Remove(m_replyButton.onClick, OnReplyInternal);
			EventDelegate.Remove(m_deleteButton.onClick, OnDelete);
			m_attachmentController.OnProcessMailRequest = null;
			Singleton<StateManager>.Instance.MailManager.OnGiftProcessedSuccessful -= OnGiftProcessedSuccessful;
		}

		private void OnGiftProcessedSuccessful(MailHeader _header, MailBody _body)
		{
			m_animationWaitCounter = 0;
			if (_body.IsResourceGift())
			{
				m_animationWaitCounter++;
				m_animating = true;
				MailAttachmentController attachmentController = m_attachmentController;
				EventDelegate.Callback resourceMovedFinished = OnClaimFinished;
				attachmentController.ClaimReward(true, null, resourceMovedFinished);
			}
			if (_body.IsItemGift() || _body.IsEntitlementGift())
			{
				m_animationWaitCounter++;
				m_animating = true;
				m_attachmentController.ClaimReward(true, OnClaimFinished);
			}
		}

		private void OnProcessMailRequest(bool _accept, MailHeader _mail, MailBody _mailBody)
		{
			m_pendingProcess = true;
			Singleton<StateManager>.Instance.MailManager.ProcessMail(_mail, _mailBody, _accept, delegate
			{
				m_pendingProcess = false;
			});
		}

		private void OnReplyInternal()
		{
			if (OnReply != null)
			{
				OnReply(m_mailHeader, m_mailBody);
			}
		}

		private void OnDelete()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1835),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						Singleton<StateManager>.Instance.MailManager.DeleteMail(m_mailHeader, OnMailDeleted);
						AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_delete);
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(4562)
			};
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.ShowCloseButton = true;
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void OnMailDeleted(List<MailHeader> _headers)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(s_mailDeletedId));
		}

		private void UpdateContent()
		{
			m_content.SetActive(true);
			UpdateHeader();
			UpdateAttachment();
			UpdateBody();
		}

		private void UpdateHeader()
		{
			m_subjectLabel.supportEncoding = true;
			m_senderLabel.supportEncoding = true;
			m_titleLabel.text = m_mailHeader.GetFolderName();
			m_subjectLabel.text = m_mailHeader.GetSubject();
			m_senderLabel.text = ColorExtensions.ToNGUIColorString(UIDataMapper.UITextColors.SkillColor) + m_mailHeader.GetSenderName();
			DateTime time = ServerTimeManager.ToLocalTime(m_mailHeader.creationDate);
			m_dateLabel.text = new UIDate(time, UIDate.EDisplayType.Date).ToString();
			m_timeLabel.text = new UIDate(time, UIDate.EDisplayType.Time).ToString();
			m_replyButton.isEnabled = m_mailHeader.CanBeReplied();
			m_deleteButton.isEnabled = m_mailHeader.CanBeDeleted();
			m_buttonBar.SetActive(m_mailHeader.CanBeDeleted() || m_mailHeader.CanBeReplied());
		}

		private void UpdateAttachment()
		{
			if (m_mailHeader.attachmentType != EMailAttachmentType.MailAttachmentTypeNone)
			{
				m_attachmentController.gameObject.SetActive(true);
				m_attachmentController.UpdateData(m_mailHeader, m_mailBody);
			}
			else
			{
				m_attachmentController.Hide();
				UpdateLayout();
			}
		}

		private void UpdateBody()
		{
			m_bodyLabel.text = m_mailBody.GetBody(m_mailHeader);
		}

		private void UpdateLayout()
		{
			EventDelegate.Add(m_vBox.OnReposition, OnLayoutFinishedInternal, true);
			EventDelegate.Add(m_senderSubjectAlignment.OnReposition, delegate
			{
				m_vBox.RepositionNow = true;
			}, true);
			m_senderSubjectAlignment.RepositionNow = true;
		}

		private void OnLayoutFinishedInternal()
		{
			m_scrollView.ResetPosition();
			m_layoutFinished = true;
		}

		private void OnClaimFinished()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
			m_animationWaitCounter--;
			if (m_animationWaitCounter < 0)
			{
				m_animating = false;
			}
		}
	}
}
