using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendScreen : UIScreenChild
	{
		[SerializeField]
		private GameObject m_contentContainer;

		[SerializeField]
		private UIExtendedWrapContent m_contentWrapper;

		[SerializeField]
		private GameObject m_loadingAnimation;

		[SerializeField]
		private GameObject m_noFriendsWidget;

		[SerializeField]
		private UIButton m_addFriendButton;

		[SerializeField]
		private UIButton m_writeMsgButton;

		[SerializeField]
		private UIButton m_removeFriendButton;

		[SerializeField]
		private FriendDetails m_details;

		[SerializeField]
		private SendMailController m_friendMail;

		[SerializeField]
		private UITweenedScreenComponent[] m_tweenedScreenComponents;

		private readonly List<UIRequestToken> m_requests = new List<UIRequestToken>(32);

		private readonly List<FriendProfile> m_friendList = new List<FriendProfile>(64);

		private FriendTab m_selectedTab;

		private FriendProfile m_selectedFriend;

		private int m_selectedIndex = -1;

		private int m_requestInProgress;

		private string m_addedFriendHash;

		private string m_removedFriendId;

		private string m_removedFriendName;

		private FriendProfile m_player;

		private FriendErrorHandler m_errorHandlerAddFriend;

		private int m_maxFriends = -1;

		private float m_nextTimeUpdate = float.MaxValue;

		private bool m_startScreenTweenAfterRequest;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Add(m_removeFriendButton.onClick, OnRemoveFriendClick);
			EventDelegate.Add(m_writeMsgButton.onClick, OnWriteMsgClick);
			m_errorHandlerAddFriend = new FriendErrorHandler(ConvertAlreadyFriendHashToUsername);
			m_contentWrapper.OnItemCreated += OnItemCreated;
			m_contentWrapper.OnUpdateItem += OnItemUpdated;
			m_contentWrapper.Init();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
			if ((bool)m_removeFriendButton)
			{
				EventDelegate.Remove(m_removeFriendButton.onClick, OnRemoveFriendClick);
			}
			if ((bool)m_writeMsgButton)
			{
				EventDelegate.Remove(m_writeMsgButton.onClick, OnWriteMsgClick);
			}
			m_errorHandlerAddFriend = null;
			if (m_contentWrapper != null)
			{
				m_contentWrapper.OnItemCreated -= OnItemCreated;
				m_contentWrapper.OnUpdateItem -= OnItemUpdated;
			}
		}

		private void OnDestroy()
		{
			m_selectedTab = null;
			m_friendList.ClearIfNotEmpty();
		}

		private void Update()
		{
			if (Time.time >= m_nextTimeUpdate)
			{
				UpdateActivity();
			}
		}

		private void OnScreenActivate()
		{
			for (int i = 0; i < m_tweenedScreenComponents.Length; i++)
			{
				UITweenedScreenComponent uITweenedScreenComponent = m_tweenedScreenComponents[i];
				if (uITweenedScreenComponent != null)
				{
					uITweenedScreenComponent.SetToBegining();
				}
			}
			m_startScreenTweenAfterRequest = true;
			RequestFriends();
		}

		private void OnScreenDeActivate()
		{
			for (int i = 0; i < m_requests.Count; i++)
			{
				m_requests[i].Abandoned = true;
			}
		}

		private void OnRemoveFriendClick()
		{
			FriendUtil.PopUpDeleteFriend(m_selectedFriend.userName, OnRemoveFriendPopupClick);
		}

		private void OnRemoveFriendPopupClick(bool _success)
		{
			if (_success)
			{
				RequestRemoveSelectedFriend();
			}
		}

		private void OnWriteMsgClick()
		{
			if (m_selectedFriend.friendType == EFriendType.FriendTypeBidirectional)
			{
				SendMailConfiguration sendMailConfiguration = new SendMailConfiguration();
				sendMailConfiguration.RecieverId = m_selectedFriend.id;
				sendMailConfiguration.RecieverName = m_selectedFriend.userName;
				sendMailConfiguration.OnMailSent = OnMailSent;
				SendMailConfiguration config = sendMailConfiguration;
				m_friendMail.Show(config);
			}
			else
			{
				FriendUtil.NotificationGeneric(4485, new StringMapper { { "player_name", m_selectedFriend.userName } });
			}
		}

		private void OnMailSent(string _recieverid)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_social_email_send);
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4570));
		}

		public override void OnBackKey()
		{
			if (m_friendMail.gameObject.activeSelf)
			{
				m_friendMail.HideMailScreen();
			}
			else
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.SocialSelectionScreen, new UITweenHandle(null, null));
			}
		}

		private void RequestFriends()
		{
			QueueRequest();
			UIRequestToken item = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMyFriends(_result);
			}, null, OnResponseFriends);
			m_requests.Add(item);
		}

		private void OnResponseFriends(IAsyncResult _response)
		{
			UserProfile playerProfile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			m_maxFriends = playerProfile.maxFriends;
			if (!FriendUtil.IsError(_response))
			{
				m_friendList.ClearIfNotEmpty();
				FriendProfiles friends = playerProfile.friends;
				if (friends != null)
				{
					m_friendList.AddRange(friends.friendProfiles);
				}
				if (m_friendList.Count > 1)
				{
					m_friendList.Sort(CompareFriends);
				}
				m_player = m_friendList.Find((FriendProfile _profile) => _profile.id == playerProfile.id);
			}
			UpdateFriendsView();
			ConsumeRequest();
		}

		public void RequestAddFriend(string _friendHash)
		{
			_friendHash = FriendUtil.FromHumanReadableUserHash(_friendHash);
			if (_friendHash == m_player.userIdHash)
			{
				FriendUtil.PopUpGeneric(4599, 4600);
				return;
			}
			if (string.IsNullOrEmpty(_friendHash))
			{
				FriendUtil.PopUpGeneric(4458, 4484);
				return;
			}
			m_addedFriendHash = _friendHash;
			QueueRequest();
			UIRequestToken item = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.SendFriendRequest(_friendHash, EModuleOrigin.Origin_FriendScreen, _result);
			}, m_errorHandlerAddFriend, OnResponseAddFriend);
			m_requests.Add(item);
		}

		private void OnResponseAddFriend(IAsyncResult _response)
		{
			if (FriendUtil.IsError(_response))
			{
				m_errorHandlerAddFriend.AddedFriendHash = m_addedFriendHash;
				m_addedFriendHash = null;
			}
			else
			{
				RequestFriends();
			}
			Singleton<UIScreenManager>.Instance.ChangeScreen(m_screen.ScreenId, new UITweenHandle());
			ConsumeRequest();
		}

		private void RequestRemoveSelectedFriend()
		{
			m_removedFriendId = m_selectedFriend.id;
			m_removedFriendName = m_selectedFriend.userName;
			QueueRequest();
			UIRequestToken item = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.RemoveFriend(m_selectedFriend.id, _result);
			}, null, OnResponseRemoveSelectedFriend);
			m_requests.Add(item);
		}

		private void OnResponseRemoveSelectedFriend(IAsyncResult _response)
		{
			if (FriendUtil.IsError(_response))
			{
				m_removedFriendId = null;
				m_removedFriendName = null;
			}
			else if (m_selectedFriend == null || m_selectedIndex < 0)
			{
				m_selectedIndex = -1;
				m_selectedFriend = null;
				RequestFriends();
			}
			else
			{
				m_friendList.RemoveAt(m_selectedIndex);
				if (m_friendList.Count > 0)
				{
					m_selectedIndex = Mathf.Clamp(m_selectedIndex, 0, m_friendList.Count - 1);
					m_selectedFriend = m_friendList[m_selectedIndex];
				}
				else
				{
					m_selectedIndex = -1;
					m_selectedFriend = null;
				}
				UpdateFriendsView();
			}
			ConsumeRequest();
		}

		private void UpdateFriendsView()
		{
			bool flag = false;
			int num = -1;
			if (m_removedFriendId != null)
			{
				if (m_friendList.Count > 0)
				{
					m_selectedFriend = m_friendList[Mathf.Clamp(m_selectedIndex, 0, m_friendList.Count - 1)];
				}
				else
				{
					m_selectedFriend = null;
				}
				flag = true;
			}
			if (m_addedFriendHash != null)
			{
				num = m_friendList.FindIndex((FriendProfile _fp) => _fp.userIdHash == m_addedFriendHash);
				if (num >= 0)
				{
					m_selectedFriend = m_friendList[num];
					flag = true;
				}
			}
			if (m_selectedFriend != null && !flag)
			{
				m_selectedFriend = m_friendList.Find((FriendProfile _fp) => _fp.id == m_selectedFriend.id);
			}
			if (m_selectedFriend == null && m_friendList.Count >= 2)
			{
				m_selectedFriend = m_friendList[0];
			}
			if (m_selectedFriend == null)
			{
				m_selectedFriend = m_player;
			}
			m_selectedTab = null;
			m_selectedIndex = -1;
			if (m_friendList.Count == 1 && m_friendList[0] == m_player)
			{
				m_contentWrapper.ItemCount = 0;
			}
			else
			{
				m_contentWrapper.ItemCount = m_friendList.Count;
			}
			m_contentWrapper.WrapScrollView.UpdatePosition();
			if (m_selectedIndex >= 0)
			{
				m_contentWrapper.SetFocusOnChildByIndex(m_selectedIndex);
			}
			m_details.UpdateData(m_selectedFriend, m_selectedFriend == m_player);
			m_nextTimeUpdate = Time.time + 60f;
			if (num >= 0)
			{
				FriendProfile friendProfile = m_friendList[num];
				FriendUtil.NotificationGeneric(4483, new StringMapper { { "player_name", friendProfile.userName } });
				m_addedFriendHash = null;
			}
			if (m_removedFriendId != null)
			{
				FriendUtil.NotificationGeneric(4481, new StringMapper { { "player_name", m_removedFriendName } });
				m_removedFriendName = null;
				m_removedFriendId = null;
			}
			if (m_startScreenTweenAfterRequest)
			{
				StartTweenInScreen();
			}
		}

		private void StartTweenInScreen()
		{
			m_startScreenTweenAfterRequest = false;
			StartCoroutine(OnStartTweenInScreen());
		}

		private IEnumerator OnStartTweenInScreen()
		{
			yield return new WaitForEndOfFrame();
			for (int i = 0; i < m_tweenedScreenComponents.Length; i++)
			{
				UITweenedScreenComponent comp = m_tweenedScreenComponents[i];
				if (comp != null)
				{
					comp.ForcePlayTween();
				}
			}
		}

		private void UpdateActivity()
		{
			m_details.UpdateActivity();
			m_nextTimeUpdate = Time.time + 60f;
		}

		private void OnItemUpdated(Transform _transform, int _index)
		{
			FriendProfile friendProfile = m_friendList[_index];
			FriendTab component = _transform.GetComponent<FriendTab>();
			bool flag = component.Friend != friendProfile;
			bool flag2 = m_selectedFriend == friendProfile;
			bool isPlayer = m_player == friendProfile;
			component.UpdateData(friendProfile, _index, isPlayer);
			if (flag)
			{
				component.SetSelectedInstant(flag2);
			}
			else
			{
				component.SetSelected(flag2);
			}
			if (flag2)
			{
				m_selectedTab = component;
				m_selectedIndex = _index;
			}
		}

		private void OnItemCreated(Transform _transform)
		{
			FriendTab component = _transform.GetComponent<FriendTab>();
			EventDelegate.Add(component.onClick, OnItemSelected);
			UIWidget component2 = _transform.GetComponent<UIWidget>();
			component2.leftAnchor.target = (component2.rightAnchor.target = m_contentContainer.transform);
		}

		private void OnItemSelected()
		{
			if (!(m_selectedTab != null) || FriendTab.Current.Friend != m_selectedFriend)
			{
				if (m_selectedTab != null)
				{
					m_selectedTab.SetSelected(false);
				}
				m_selectedTab = FriendTab.Current;
				m_selectedFriend = m_selectedTab.Friend;
				m_selectedIndex = m_selectedTab.Index;
				m_details.UpdateData(m_selectedFriend, m_selectedFriend == m_player);
				SetButtonsEnabled(true);
				m_selectedTab.SetSelected(true);
			}
		}

		private string ConvertAlreadyFriendHashToUsername(string _userIdHash)
		{
			int friendIndex = m_friendList.FindIndex((FriendProfile _fp) => _fp.userIdHash == _userIdHash);
			if (friendIndex < 0)
			{
				return null;
			}
			if (m_selectedTab != null && m_selectedTab.Friend == m_selectedFriend)
			{
				m_selectedTab.SetSelected(false);
			}
			m_contentWrapper.SetFocusOnChildByIndex(friendIndex);
			Transform transform = m_contentWrapper.Children.Find((Transform _t) => _t.GetComponent<FriendTab>().Index == friendIndex);
			m_selectedTab = ((!(transform != null)) ? null : transform.GetComponent<FriendTab>());
			m_selectedFriend = m_friendList[friendIndex];
			m_selectedIndex = friendIndex;
			m_details.UpdateData(m_selectedFriend, m_selectedFriend == m_player);
			SetButtonsEnabled(true);
			if (m_selectedTab != null)
			{
				m_selectedTab.SetSelected(true);
			}
			return m_selectedFriend.userName;
		}

		public static long GetHighestAssassinItemPower(FriendProfile _friendProfile)
		{
			long num = 0L;
			for (int i = 0; i < _friendProfile.friendAssassin.Length; i++)
			{
				long itemPower = _friendProfile.friendAssassin[i].itemPower;
				if (itemPower > num)
				{
					num = itemPower;
				}
			}
			return num;
		}

		public static int CompareFriends(FriendProfile _a, FriendProfile _b)
		{
			int num = string.Compare(_a.userName, _b.userName, StringComparison.CurrentCulture);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(_a.userIdHash, _b.userIdHash);
		}

		private void SetButtonsEnabled(bool _enable)
		{
			bool flag = m_selectedFriend != null && m_selectedFriend == m_player;
			m_addFriendButton.isEnabled = _enable && m_friendList.Count < m_maxFriends + 1;
			m_removeFriendButton.isEnabled = _enable && m_friendList.Count > 1 && !flag;
			m_writeMsgButton.isEnabled = _enable && m_friendList.Count > 1 && !flag;
		}

		private void QueueRequest()
		{
			m_requestInProgress++;
			if (m_requestInProgress == 1)
			{
				SetButtonsEnabled(false);
				m_contentContainer.SetActive(false);
				m_noFriendsWidget.SetActive(false);
				m_loadingAnimation.SetActive(true);
			}
		}

		private void ConsumeRequest()
		{
			m_requestInProgress--;
			if (!Assets.Scripts.Engine.Helpers.Verify(m_requestInProgress >= 0, "FriendScreen::ConsumeRequest"))
			{
				m_requestInProgress = 0;
			}
			if (m_requestInProgress == 0)
			{
				SetButtonsEnabled(true);
				m_loadingAnimation.SetActive(false);
				m_noFriendsWidget.SetActive(m_friendList.Count <= 1);
				m_contentContainer.SetActive(m_friendList.Count >= 2);
			}
			m_contentWrapper.SortBasedOnScrollMovement();
		}
	}
}
