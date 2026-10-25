using System;
using AcierProtocol;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendErrorHandler : IUIErrorHandler
	{
		public delegate string ConvertAlreadyFriendHashToUsername(string _userIdHash);

		private readonly ConvertAlreadyFriendHashToUsername m_convertAlreadyFriendHash;

		private readonly EventDelegate.Callback m_callbackReopen;

		private readonly EventDelegate.Callback m_callbackProceed;

		public string AddedFriendHash { get; set; }

		public FriendErrorHandler(ConvertAlreadyFriendHashToUsername _convertAlreadyFriendHash, EventDelegate.Callback _callbackReopen = null, EventDelegate.Callback _callbackProceed = null)
		{
			m_convertAlreadyFriendHash = _convertAlreadyFriendHash;
			m_callbackReopen = _callbackReopen;
			m_callbackProceed = _callbackProceed;
		}

		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			if (_error.Error.ErrorCategory != ErrorCategory.SERVER_FRIENDLIST && _error.Error.ErrorCategory != ErrorCategory.SERVER_MAIL)
			{
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(UIErrorHelper.GetDefaultPopUpConfiguration(_error, m_callbackReopen));
				return;
			}
			switch (_error.Error.ErrorCode)
			{
			case ErrorCode.FRIEND_NOT_FOUND:
				FriendUtil.PopUpGeneric(4458, 4484, m_callbackProceed);
				return;
			case ErrorCode.MAIL_ALREADY_FRIENDS:
			{
				string text = m_convertAlreadyFriendHash(AddedFriendHash);
				if (text == null)
				{
					break;
				}
				FriendUtil.NotificationGeneric(4556, new StringMapper { { "player_name", text } });
				if (m_callbackProceed != null)
				{
					m_callbackProceed();
				}
				return;
			}
			case ErrorCode.REACHED_MAX_FRIEND_COUNT:
				FriendUtil.PopUpGeneric(4603, 4604, m_callbackProceed);
				return;
			}
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(UIErrorHelper.GetDefaultPopUpConfiguration(_error, m_callbackReopen));
		}
	}
}
