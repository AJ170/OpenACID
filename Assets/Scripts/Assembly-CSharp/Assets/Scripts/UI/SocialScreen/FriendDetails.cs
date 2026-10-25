using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.AppRatingPopup;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendDetails : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_idLabel;

		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIExtendedLabel m_levelLabel;

		[SerializeField]
		private UIHeroicPortraitTexture m_heroicPortrait;

		[SerializeField]
		private UIExtendedLabel m_activityLabel;

		[SerializeField]
		private UIExtendedLabel m_relationLabel;

		[SerializeField]
		private UISprite m_relationSprite;

		[SerializeField]
		private UIButton m_copyIdButton;

		[SerializeField]
		private UIExtendedLabel m_copyIdButtonlabel;

		[SerializeField]
		private FriendAssassinDetails m_assassins;

		private PopUpConfiguration m_popupConfiguration;

		private string m_userName;

		private string m_userHash;

		private string m_linkToAppStore;

		private string m_linkToGoogleStore;

		private StringMapper m_stringMapper;

		public FriendProfile Friend { private get; set; }

		public bool IsPlayer { private get; set; }

		private void Awake()
		{
			EventDelegate.Add(m_copyIdButton.onClick, OnCopyIdClick);
		}

		private void OnDestroy()
		{
			EventDelegate.Remove(m_copyIdButton.onClick, OnCopyIdClick);
		}

		public void Start()
		{
			m_idLabel.text = " ";
			m_nameLabel.text = " ";
			UpdateStringMapper();
		}

		public void UpdateData(FriendProfile _friend, bool _isPlayer)
		{
			if (_friend != null)
			{
				Friend = _friend;
				IsPlayer = _isPlayer;
				m_copyIdButtonlabel.text = ((!IsPlayer) ? LocalizationManager.Instance.Get(6600) : LocalizationManager.Instance.Get(4596));
				UIDataMapper instance = UbiCore.Utils.Singleton<UIDataMapper>.Instance;
				m_userHash = FriendUtil.ToHumanReadableUserHash(Friend.userIdHash);
				m_userName = Friend.userName;
				UpdateStringMapper();
				m_idLabel.SetLocalizationKey(4440, new StringMapper { { "player_id", m_userHash } });
				m_nameLabel.text = m_userName;
				m_levelLabel.text = Friend.level.ToString();
				FriendAssassin[] friendAssassin = Friend.friendAssassin;
				Array.Sort(friendAssassin, (FriendAssassin _ass1, FriendAssassin _ass2) => _ass2.itemPower.CompareTo(_ass1.itemPower));
				FriendAssassin friendAssassin2 = ((friendAssassin.Length <= 0) ? null : friendAssassin[0]);
				UIPortraitUtil.UpdateHeroicPortraitByFriendAssassin(friendAssassin2, Friend.maxRankIdx, m_heroicPortrait);
				m_relationLabel.gameObject.SetActive(!IsPlayer);
				m_relationSprite.gameObject.SetActive(!IsPlayer);
				if (!IsPlayer)
				{
					EFriendType friendType = Friend.friendType;
					int num = ((friendType != 0) ? 4551 : 4550);
					m_relationLabel.SetLocalizationKey(num);
					m_relationSprite.spriteName = instance.FriendRelationSprite[(int)friendType];
				}
				UpdateActivity();
				m_assassins.UpdateData(Friend);
			}
		}

		public void UpdateActivity()
		{
			m_activityLabel.gameObject.SetActive(!IsPlayer);
			if (!IsPlayer)
			{
				FriendUtil.SetActivityLabel(m_activityLabel, Friend.lastActivity);
			}
		}

		private void UpdateStringMapper()
		{
			if (m_stringMapper == null)
			{
				m_linkToAppStore = LocalizationManager.Instance.Get(6610);
				m_linkToGoogleStore = LocalizationManager.Instance.Get(6611);
				m_stringMapper = new StringMapper
				{
					{ "playerName", m_userName },
					{ "playerID", m_userHash },
					{ "link_store_IOS", m_linkToAppStore },
					{ "link_store_GOOGLE", m_linkToGoogleStore }
				};
			}
			else
			{
				m_stringMapper["playerName"] = m_userName;
				m_stringMapper["playerID"] = m_userHash;
			}
		}

		private void OnCopyIdClick()
		{
			if (Friend != null)
			{
				if (IsPlayer)
				{
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(CreatePopupConfiguration());
				}
				else
				{
					CopyId();
				}
			}
		}

		private PopUpConfiguration CreatePopupConfiguration()
		{
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			list.Add(new PopUpButtonConfiguration
			{
				Data = true,
				Label = LocalizationManager.Instance.Get(6568),
				ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
				OnButtonPressed = delegate
				{
					CopyId();
				},
				SoundIntent = EButtonIntent.Buy
			});
			list.Add(new PopUpButtonConfiguration
			{
				Data = true,
				Label = LocalizationManager.Instance.Get(6569),
				ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
				OnButtonPressed = delegate
				{
					UseEmail();
				},
				SoundIntent = EButtonIntent.Buy
			});
			if (!NativeTools.IsTVDevice)
			{
				list.Add(new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(6570),
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					OnButtonPressed = delegate
					{
						UseShortMessage();
					},
					SoundIntent = EButtonIntent.Buy
				});
			}
			m_popupConfiguration = new PopUpConfiguration
			{
				Data = new AppRatingContent.AppRatingData
				{
					Description = LocalizationManager.Instance.Get(6567)
				},
				ShowCloseButton = true,
				OnCloseButtonClicked = ClosePopup,
				Heading = LocalizationManager.Instance.Get(6566),
				Buttons = list.ToArray()
			};
			return m_popupConfiguration;
		}

		private void ClosePopup()
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		}

		private void CopyId()
		{
			string userIdHash = Friend.userIdHash;
			userIdHash = FriendUtil.ToHumanReadableUserHash(userIdHash);
			SendMessageManager.SendMessage.SendMessageThroughClipboard(userIdHash, SendMessageManager.MessageType.FriendInvite);
			NativeTools.CopyToClipboard(userIdHash);
			ClosePopup();
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(6619));
		}

		private void UseEmail()
		{
			int num = 6599;
			string subject = LocalizationManager.Instance.Get(6575);
			string body = LocalizationManager.Instance.Get(num, m_stringMapper);
			SendMessageManager.SendMessage.SendEmail(string.Empty, subject, body, SendMessageManager.MessageType.FriendInvite);
			ClosePopup();
		}

		private void UseShortMessage()
		{
			int num = 6576;
			string message = LocalizationManager.Instance.Get(num, m_stringMapper);
			SendMessageManager.SendMessage.SendSMS(string.Empty, message, SendMessageManager.MessageType.FriendInvite);
			ClosePopup();
		}
	}
}
