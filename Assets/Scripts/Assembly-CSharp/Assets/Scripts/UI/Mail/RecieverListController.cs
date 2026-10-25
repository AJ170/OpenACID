using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class RecieverListController : MonoBehaviour, IInitialized
	{
		public delegate void HandleRecieverSelected(FriendProfile _reciever);

		[SerializeField]
		private UIExtendedWrapContent m_wrapper;

		[SerializeField]
		private UIButton m_selectButton;

		[SerializeField]
		private Transform m_anchorTarget;

		[SerializeField]
		private GameObject m_content;

		[SerializeField]
		private GameObject m_loadingIndicator;

		[SerializeField]
		private GameObject m_background;

		[SerializeField]
		private UITexture m_noMailTexture;

		[SerializeField]
		private UIExtendedLabel m_noMailHeader;

		[SerializeField]
		private GameObject m_helpLabel;

		private List<FriendProfile> m_recievers;

		private FriendTab m_selected;

		private FriendProfile m_profile;

		private readonly Color k_textureDarkColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				UpdateHelpElements(true);
				if (value)
				{
					if (Singleton<StateManager>.Instance.ProfileManager.Profile.friends == null)
					{
						m_loadingIndicator.SetActive(true);
						m_background.SetActive(true);
						Singleton<StateManager>.Instance.ProfileManager.GetMyFriends(delegate
						{
							if (Visible)
							{
								m_loadingIndicator.SetActive(false);
								UpdateList((Singleton<StateManager>.Instance.ProfileManager.Profile.friends == null) ? new FriendProfile[0] : Singleton<StateManager>.Instance.ProfileManager.Profile.friends.friendProfiles);
								UpdateController();
							}
						});
					}
					else
					{
						UpdateList(Singleton<StateManager>.Instance.ProfileManager.Profile.friends.friendProfiles);
						UpdateController();
					}
					base.gameObject.SetActive(true);
					m_wrapper.SortBasedOnScrollMovement();
				}
				else
				{
					m_loadingIndicator.SetActive(false);
					base.gameObject.SetActive(false);
				}
			}
		}

		public event HandleRecieverSelected OnRecieverSelected;

		public void ScreenActivate()
		{
			m_content.SetActive(false);
			m_loadingIndicator.SetActive(false);
		}

		public void Initialize()
		{
			m_wrapper.OnItemCreated += OnItemCreated;
			m_wrapper.OnUpdateItem += OnItemUpdated;
			m_wrapper.Init();
			m_content.SetActive(false);
			m_loadingIndicator.SetActive(false);
		}

		public void Clear()
		{
			m_wrapper.OnItemCreated -= OnItemCreated;
			m_wrapper.OnUpdateItem -= OnItemUpdated;
		}

		private void UpdateList(FriendProfile[] _recievers)
		{
			m_recievers = _recievers.Where((FriendProfile _friend) => _friend.friendType == EFriendType.FriendTypeBidirectional).ToList();
			m_recievers.Sort(FriendScreen.CompareFriends);
			m_wrapper.ItemCount = m_recievers.Count;
			m_wrapper.WrapScrollView.ResetPosition();
		}

		private void OnEnable()
		{
			EnableListeners();
		}

		private void OnDisable()
		{
			DisableListeners();
		}

		private void EnableListeners()
		{
			EventDelegate.Add(m_selectButton.onClick, OnSelectButtonClicked);
		}

		private void DisableListeners()
		{
			EventDelegate.Remove(m_selectButton.onClick, OnSelectButtonClicked);
		}

		private void Awake()
		{
		}

		private void OnItemUpdated(Transform _transform, int _index)
		{
			FriendProfile friendProfile = m_recievers[_index];
			FriendTab component = _transform.GetComponent<FriendTab>();
			bool flag = component.Friend != friendProfile;
			bool flag2 = m_profile == friendProfile;
			component.UpdateData(friendProfile, _index, false);
			if (flag)
			{
				component.SetSelectedInstant(flag2);
			}
			else
			{
				component.SetSelected(flag2);
			}
			UIWidget component2 = _transform.GetComponent<UIWidget>();
			component2.ForceUpdate(true);
		}

		private void OnItemCreated(Transform _transform)
		{
			FriendTab component = _transform.GetComponent<FriendTab>();
			EventDelegate.Add(component.onClick, OnItemSelected);
			UIWidget component2 = _transform.GetComponent<UIWidget>();
			component2.leftAnchor.target = m_anchorTarget;
			component2.rightAnchor.target = m_anchorTarget;
		}

		private void OnItemSelected()
		{
			if (m_selected != null && m_selected.Index == FriendTab.Current.Index)
			{
				OnSelectButtonClicked();
				return;
			}
			if (m_selected != null)
			{
				m_selected.SetSelected(false);
			}
			m_selected = FriendTab.Current;
			m_profile = m_selected.Friend;
			m_selected.SetSelected(true);
			m_selectButton.isEnabled = m_selected.Friend.friendType == EFriendType.FriendTypeBidirectional;
		}

		private void OnSelectButtonClicked()
		{
			if (this.OnRecieverSelected != null && m_profile != null && m_profile.friendType == EFriendType.FriendTypeBidirectional)
			{
				this.OnRecieverSelected(m_profile);
			}
		}

		private void UpdateHelpElements(bool _hasFriends)
		{
			if (_hasFriends)
			{
				m_noMailTexture.gameObject.SetActive(false);
				m_noMailTexture.color = Color.white;
				m_helpLabel.SetActive(false);
				m_noMailHeader.gameObject.SetActive(false);
				m_selectButton.gameObject.SetActive(true);
			}
			else
			{
				m_noMailTexture.gameObject.SetActive(true);
				m_noMailTexture.color = k_textureDarkColor;
				m_helpLabel.SetActive(true);
				m_noMailHeader.SetLocalizationKey(6498);
				m_noMailHeader.gameObject.SetActive(true);
				m_selectButton.gameObject.SetActive(false);
			}
		}

		private void UpdateController()
		{
			bool flag = m_recievers != null && m_recievers.Count > 0;
			UpdateHelpElements(flag);
			m_selectButton.gameObject.SetActive(flag);
			m_content.SetActive(flag);
			m_background.SetActive(!flag);
		}

		public void ResetWrapScrollView()
		{
			if (m_recievers != null && m_recievers.Count > 0)
			{
				m_wrapper.WrapScrollView.MoveRelative(Vector3.left);
			}
		}
	}
}
