using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class MailAttachmentController : MonoBehaviour
	{
		public delegate void HandleLayoutFinished();

		public delegate void ProcessMailRequestHandler(bool _accept, MailHeader _mail, MailBody _mailBody);

		private static readonly StringId s_claimId = new StringId(1808);

		private static readonly StringId s_acceptId = new StringId(5677);

		public ProcessMailRequestHandler OnProcessMailRequest;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_titleLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_rewardValueLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_rewardLabel;

		[CheckAssigned]
		[SerializeField]
		private UIButtonProxy m_rejectButton;

		[SerializeField]
		[CheckAssigned]
		private UIButtonProxy m_acceptButton;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_rewardSprite;

		[CheckAssigned]
		[SerializeField]
		private UITexture m_rankAvatar;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_titleBackground;

		[CheckAssigned]
		[SerializeField]
		private UIRewardItemController m_rewardItem;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_vBox;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_rewardObject;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_backgroundSmall;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_backgroundLarge;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_countLabelName;

		private static readonly Vector2 s_itemAnchor = new Vector2(-54f, 0f);

		private static readonly Vector2 s_playerAnchor = new Vector2(-84f, -30f);

		private MailHeader m_header;

		private MailBody m_body;

		public event HandleLayoutFinished OnLayoutFinished;

		public void Hide()
		{
			m_rewardItem.Close();
			base.gameObject.SetActive(false);
		}

		public void UpdateData(MailHeader _header, MailBody _body)
		{
			m_header = _header;
			m_body = _body;
			m_rewardItem.Close();
			MailAttachment attachment = _body.attachment;
			if (_header.attachmentType == EMailAttachmentType.MailAttachmentTypeFriendRequest)
			{
				HandleFriendRequest(attachment.friendRequest, _body.attachment);
			}
			else if (_header.attachmentType == EMailAttachmentType.MailAttachmentTypeGift || _header.attachmentType == EMailAttachmentType.MailAttachmentTypeHirelingUsageStatistics)
			{
				HandleGift(attachment.gift);
			}
			m_rejectButton.isEnabled = _body.CanBeRejected();
			m_vBox.RepositionNow = true;
			string text = ((!_body.IsItemGift()) ? LocalizationManager.Instance.Get(8407) : LocalizationManager.Instance.Get(4565));
			if (m_countLabelName != null)
			{
				m_countLabelName.text = text;
			}
		}

		public void ClaimReward(bool _isSentToInventory, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null)
		{
			if (m_body.IsResourceGift() && m_rewardObject.activeSelf)
			{
				Resource resource = m_body.attachment.gift.resources.resources[0];
				if (resource.amount > 0)
				{
					UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
					bool sendToInventory = _isSentToInventory;
					recivedItemMgr.StartReciveCoinAnimation(resource, m_rewardSprite.gameObject, null, false, _animationFinished, _resourceMovedFinished, sendToInventory);
				}
			}
			else if (m_rewardItem.IsActive)
			{
				m_rewardItem.Close();
				if (m_body.IsResourceGift())
				{
					m_rewardItem.ClaimReward(true, null, _animationFinished, _resourceMovedFinished);
				}
				if (m_body.IsItemGift() || m_body.IsEntitlementGift())
				{
					m_rewardItem.ClaimReward(true, null, _animationFinished, _resourceMovedFinished);
				}
			}
		}

		private void Awake()
		{
			m_acceptButton.AllowDisabledClick = true;
			m_backgroundLarge.gameObject.SetActive(false);
			m_backgroundSmall.gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_rejectButton.onClick, OnReject);
			EventDelegate.Add(m_acceptButton.onClick, OnAccept);
			EventDelegate.Add(m_vBox.OnReposition, OnPositionFinished);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_rejectButton.onClick, OnReject);
			EventDelegate.Remove(m_acceptButton.onClick, OnAccept);
			EventDelegate.Remove(m_vBox.OnReposition, OnPositionFinished);
		}

		private void OnReject()
		{
			if (OnProcessMailRequest != null)
			{
				OnProcessMailRequest(false, m_header, m_body);
			}
		}

		private void OnAccept()
		{
			if (m_body.attachment.gift != null && !m_body.CanClaim())
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4625));
			}
			else if (m_acceptButton.isEnabled && OnProcessMailRequest != null)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
				OnProcessMailRequest(true, m_header, m_body);
			}
		}

		private void OnPositionFinished()
		{
			if (this.OnLayoutFinished != null)
			{
				this.OnLayoutFinished();
			}
		}

		private void HandleFriendRequest(MailFriendRequest _request, MailAttachment _attachment)
		{
			m_rewardItem.Hide();
			m_rewardSprite.gameObject.SetActive(true);
			m_rankAvatar.gameObject.SetActive(true);
			m_titleLabel.text = _request.userName;
			m_rewardLabel.text = LocalizationManager.Instance.Get(1521);
			Resource resource = ((_attachment.gift == null) ? null : _attachment.gift.resources.resources[0]);
			if (resource == null || resource.amount == 0L)
			{
				m_rewardObject.SetActive(false);
			}
			else
			{
				m_rewardValueLabel.text = LocalizationManager.Instance.GetInteger(resource.amount);
				m_rewardObject.SetActive(true);
			}
			m_rewardSprite.spriteName = ((resource == null) ? string.Empty : UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions[(int)resource.type].SmallIconName);
			UIItemUtil.UpdateRankAvatarByLvl(_request.userlevel, m_rankAvatar);
			UIRect.AnchorPoint topAnchor = m_titleBackground.topAnchor;
			Vector2 vector = s_playerAnchor;
			topAnchor.absolute = (int)vector.y;
			UIRect.AnchorPoint bottomAnchor = m_titleBackground.bottomAnchor;
			Vector2 vector2 = s_playerAnchor;
			bottomAnchor.absolute = (int)vector2.x;
			m_acceptButton.TranslationKey = (int)s_acceptId.Value;
			m_backgroundLarge.gameObject.SetActive(true);
			m_backgroundSmall.gameObject.SetActive(false);
		}

		private void HandleGift(MailGift _gift)
		{
			if (_gift.elements != null)
			{
				HandleItemGift(_gift.elements);
			}
			else if (_gift.resources != null)
			{
				HandleResourceGift(_gift.resources);
			}
			else if (_gift.entitlements != null && _gift.entitlements.Length > 0)
			{
				HandleEntitlementGift(_gift.entitlements);
			}
			UIRect.AnchorPoint topAnchor = m_titleBackground.topAnchor;
			Vector2 vector = s_itemAnchor;
			topAnchor.absolute = (int)vector.y;
			UIRect.AnchorPoint bottomAnchor = m_titleBackground.bottomAnchor;
			Vector2 vector2 = s_itemAnchor;
			bottomAnchor.absolute = (int)vector2.x;
			m_acceptButton.TranslationKey = (int)s_claimId.Value;
			m_backgroundLarge.gameObject.SetActive(false);
			m_backgroundSmall.gameObject.SetActive(true);
		}

		private void HandleItemGift(InventoryElements _elements)
		{
			m_rewardItem.Show();
			m_rankAvatar.gameObject.SetActive(false);
			string empty = string.Empty;
			int num = _elements.elements.Length;
			InventoryElement element = _elements.elements[0];
			if (num == 1)
			{
				empty = element.GetTitle();
				m_rewardItem.Initialize(element);
			}
			else
			{
				m_rewardItem.Initialize(num);
				empty = LocalizationManager.Instance.Get(4637);
			}
			m_titleLabel.text = empty;
			m_rewardObject.SetActive(false);
			m_acceptButton.isEnabled = m_body.CanClaim();
		}

		private void HandleResourceGift(AcierProtocol.Resources _resources)
		{
			m_rewardItem.Show();
			m_rankAvatar.gameObject.SetActive(false);
			int num = _resources.resources.Length;
			string text = string.Empty;
			if (num == 1)
			{
				Resource resource = _resources.resources[0];
				ResourceDescription value;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)resource.type, out value))
				{
					text = LocalizationManager.Instance.Get(value.InfoHeadingId);
					m_rewardItem.Initialize(resource);
					m_rewardValueLabel.text = LocalizationManager.Instance.GetInteger(resource.amount);
					m_rewardLabel.text = LocalizationManager.Instance.Get(4565);
					CurrencyDescription value2;
					if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)resource.type, out value2))
					{
						m_rewardSprite.spriteName = value2.SmallIconName;
						m_rewardObject.SetActive(true);
					}
					else
					{
						m_rewardObject.SetActive(false);
					}
				}
			}
			else
			{
				m_rewardItem.Initialize(num);
				text = LocalizationManager.Instance.Get(4637);
			}
			m_titleLabel.text = text;
		}

		private void HandleEntitlementGift(string[] _entitlements)
		{
			m_rewardItem.Show();
			m_rankAvatar.gameObject.SetActive(false);
			int num = 4637;
			int num2 = _entitlements.Length;
			if (num2 == 1)
			{
				string guid = _entitlements[0];
				EntitlementDefinition entitlementByGUID = EntitlementExtensions.GetEntitlementByGUID(guid);
				if (entitlementByGUID != null && StringId.ValidStringId(entitlementByGUID.NameLocaKey.Key))
				{
					num = entitlementByGUID.NameLocaKey.Key;
				}
			}
			m_rewardItem.Initialize(num2);
			m_titleLabel.text = LocalizationManager.Instance.Get(num);
			m_rewardObject.SetActive(false);
			m_acceptButton.isEnabled = m_body.CanClaim();
		}
	}
}
