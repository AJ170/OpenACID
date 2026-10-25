using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class InviteScreen : UIScreenChild
	{
		[SerializeField]
		private FriendInput m_friendInput;

		[SerializeField]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		private UIWidget m_facebookInviteContainer;

		[SerializeField]
		private UIWidget m_gameIdInviteContainer;

		[SerializeField]
		private BBTweenPosition m_facebookInviteTweenPosition;

		[SerializeField]
		private BBTweenPosition m_gameIdInviteTweenPosition;

		protected override void OnScreenInit()
		{
		}

		protected override void OnScreenDestroy()
		{
		}

		public void OnRefreshAddFriendOptions()
		{
			m_facebookInviteContainer.gameObject.SetActive(FacebookManager.IsAvailable);
			m_gameIdInviteContainer.gameObject.SetActive(true);
			EventDelegate.Add(m_alignmentBox.OnReposition, OnReposition, true);
			m_alignmentBox.RepositionNow = true;
		}

		public void OnReposition()
		{
			int num = 0;
			if (FacebookManager.IsAvailable)
			{
				UpdateTween(m_facebookInviteContainer, m_facebookInviteTweenPosition, num++);
				m_facebookInviteTweenPosition.StartTweenPlayForward();
			}
			UpdateTween(m_gameIdInviteContainer, m_gameIdInviteTweenPosition, num++);
			m_gameIdInviteTweenPosition.StartTweenPlayForward();
		}

		public void UpdateTween(UIWidget _widget, BBTweenPosition _tweenPosition, int _startOrder)
		{
			_widget.ForceUpdate(true);
			float offscreenPositionX = UIUtil.GetOffscreenPositionX(_widget, UIUtil.ESide.Left);
			_tweenPosition.SetEndToCurrentValue();
			_tweenPosition.from = new Vector3(offscreenPositionX, 0f);
			_tweenPosition.StartOrder = _startOrder;
		}

		public void OnAddFriendByNameClick()
		{
			m_friendInput.Show();
		}

		public void OnInviteFacebookFriendsClick()
		{
			FacebookManager.Invite(delegate
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.FriendsListScreen, new UITweenHandle());
			});
		}

		public void RequestAddFriend(string userId)
		{
			FriendScreen[] componentsInChildren = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.FriendsListScreen).GetComponentsInChildren<FriendScreen>(true);
			if (componentsInChildren.Length >= 1)
			{
				componentsInChildren[0].RequestAddFriend(userId);
			}
		}

		public override void OnBackKey()
		{
			if (m_friendInput.gameObject.activeSelf)
			{
				m_friendInput.HideOnAndroidBackKey();
			}
			else
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.FriendsListScreen, new UITweenHandle(null, null));
			}
		}
	}
}
