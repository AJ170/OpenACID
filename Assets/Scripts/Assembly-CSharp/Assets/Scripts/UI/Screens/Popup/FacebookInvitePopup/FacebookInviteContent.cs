using System.Collections.Generic;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.FacebookInvitePopup
{
	public class FacebookInviteContent : PopUpContentView<FacebookInviteContent.FacebookInviteData>
	{
		public class FacebookInviteData : PopUpContentData
		{
			public string FacebookName;

			public FacebookManager.InviteStatus Status;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.FacebookInvite;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIExtendedLabel m_invitedLabel;

		[SerializeField]
		private UIAlignmentBox m_dontHaveGameBox;

		[SerializeField]
		private UIExtendedLabel m_dontHaveGameData;

		[SerializeField]
		private UIAlignmentBox m_failedBox;

		[SerializeField]
		private UIExtendedLabel m_failedData;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		public override Vector2 Size
		{
			get
			{
				return m_vBox.Size;
			}
		}

		private void Awake()
		{
		}

		protected override void Render(FacebookInviteData _data)
		{
			m_nameLabel.text = _data.FacebookName;
			if (_data.Status.Invited.Count > 0)
			{
				m_invitedLabel.text = LocalizationManager.Instance.Get(6137, new StringMapper { 
				{
					"friendCount",
					_data.Status.Invited.Count
				} });
			}
			else
			{
				m_invitedLabel.gameObject.SetActive(false);
			}
			string text = LocalizationManager.Instance.Get(3470);
			string text2 = LocalizationManager.Instance.Get(3471);
			string text3 = string.Empty;
			IList<FacebookHandlerBase.FBFriendInfo> dontHaveGame = _data.Status.DontHaveGame;
			m_dontHaveGameBox.gameObject.SetActive(dontHaveGame.Count > 0);
			for (int i = 0; i < dontHaveGame.Count; i++)
			{
				FacebookHandlerBase.FBFriendInfo fBFriendInfo = dontHaveGame[i];
				text3 = text3 + fBFriendInfo.Name + ((i >= dontHaveGame.Count - 1) ? text2 : text);
			}
			m_dontHaveGameData.text = text3;
			string text4 = string.Empty;
			BBList<FacebookHandlerBase.FBFriendInfo> bBList = new BBList<FacebookHandlerBase.FBFriendInfo>();
			bBList.AddRange(_data.Status.Failed);
			bBList.AddRange(_data.Status.AlreadyFriends);
			bBList.AddRange(_data.Status.FriendListFull);
			m_failedBox.gameObject.SetActive(bBList.Count > 0);
			for (int j = 0; j < bBList.Count; j++)
			{
				FacebookHandlerBase.FBFriendInfo fBFriendInfo2 = bBList[j];
				text4 = text4 + fBFriendInfo2.Name + ((j >= bBList.Count - 1) ? text2 : text);
			}
			m_failedData.text = text4;
		}

		public override void UpdateLayout()
		{
			UpdateDontHaveGameBox();
		}

		private void UpdateDontHaveGameBox()
		{
			if (!m_dontHaveGameBox.gameObject.activeSelf)
			{
				UpdateFailedBox();
				return;
			}
			EventDelegate.Add(m_dontHaveGameBox.OnReposition, UpdateFailedBox, true);
			m_dontHaveGameBox.RepositionNow = true;
		}

		private void UpdateFailedBox()
		{
			if (!m_failedBox.gameObject.activeSelf)
			{
				UpdateVBox();
				return;
			}
			EventDelegate.Add(m_failedBox.OnReposition, UpdateVBox, true);
			m_failedBox.RepositionNow = true;
		}

		private void UpdateVBox()
		{
			EventDelegate.Add(m_vBox.OnReposition, delegate
			{
				EventDelegate.Execute(onLayoutFinished);
			}, true);
			m_vBox.RepositionNow = true;
		}
	}
}
