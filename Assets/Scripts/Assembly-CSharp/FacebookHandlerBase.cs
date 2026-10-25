using System;
using System.Collections.Generic;
using Assets.Scripts.UI.State;
using UnityEngine;

public class FacebookHandlerBase
{
	public enum EFBRequestType
	{
		Init = 1,
		Login = 2,
		Logout = 3,
		GetPicture = 4,
		GetName = 5,
		GetFriends = 6,
		GetPermissions = 7,
		InviteFriends = 8,
		EnsurePermissions = 9,
		GetAppRequests = 10,
		DeleteAppRequest = 11,
		PostAchievement = 12
	}

	public enum EFBReturnCode
	{
		ErrorReadingJson = -2,
		Error = -1,
		PlatformNotSupported = 0,
		Ok = 1,
		NotInitialized = 2,
		NotLoggedIn = 3,
		AlreadyInitialized = 4,
		AlreadyLoggedIn = 5,
		AlreadyLoggedOut = 6,
		UserDeniedPermission = 7,
		UserCancelled = 8,
		NameHasUnsupportedGlyphs = 9
	}

	public class FBFriendInfo
	{
		public string Id;

		public string Name;
	}

	public class FBAppRequestInfo
	{
		public string Id;

		public string Time;

		public string FromId;

		public string FromName;
	}

	public class FBShareItemInfo
	{
		public Uri link;

		public Uri picture;

		public string toId = string.Empty;

		public string linkName = string.Empty;

		public string linkCaption = string.Empty;

		public string linkDescription = string.Empty;

		public string mediaUrl = string.Empty;

		public string reference = "ShareItem";
	}

	public delegate void FBPermissionCallback(IDictionary<string, bool> _permissions);

	public delegate void FBFriendCallback(IList<FBFriendInfo> _friends);

	public delegate void FBNameCallback(string _name);

	public delegate void FBPictureCallback(Texture2D _picture);

	public delegate void FBAppRequestCallback(IList<FBAppRequestInfo> _friends);

	public delegate void FBRequestCallback(bool _success, EFBRequestType _requestType, EFBReturnCode _returnCode);

	protected IList<FBFriendInfo> m_lastSelectedFriends;

	protected IList<FBFriendInfo> m_lastUserFriends;

	protected IDictionary<string, bool> m_lastUserPermissions;

	protected IDictionary<string, object> m_purchaseToLog = new Dictionary<string, object>();

	protected Texture2D m_lastPicture;

	protected string m_lastStatus = string.Empty;

	public static string FBPageURL = "https://www.facebook.com/AssassinsCreedIdentity";

	public virtual bool IsAvailable
	{
		get
		{
			return false;
		}
	}

	public virtual bool IsInitialized
	{
		get
		{
			return false;
		}
	}

	public virtual bool IsLoggedIn
	{
		get
		{
			return false;
		}
	}

	public virtual string AccessToken
	{
		get
		{
			return string.Empty;
		}
	}

	public virtual string UserId
	{
		get
		{
			return string.Empty;
		}
	}

	public event FBRequestCallback OnFBRequestCompleted;

	public virtual void FBInit(FBRequestCallback _callback = null)
	{
	}

	public virtual void FBLogin(FBRequestCallback _callback = null)
	{
	}

	public virtual void FBLogout(FBRequestCallback _callback = null)
	{
	}

	public virtual void FBGetPicture(string userId = "me", float size = 256f, FBRequestCallback _callback = null, FBPictureCallback _pictureCallback = null)
	{
	}

	public virtual void FBGetName(string userId = "me", FBRequestCallback _callback = null, FBNameCallback _nameCallback = null)
	{
	}

	public virtual void FBGetAppRequests(FBRequestCallback _callback = null, FBAppRequestCallback _appRequestCallback = null)
	{
	}

	public virtual void FBDeleteAppRequest(FBAppRequestInfo _request, FBRequestCallback _callback = null)
	{
	}

	public virtual void FBGetPermissions(FBRequestCallback _callback = null, FBPermissionCallback _permissionCallback = null)
	{
	}

	public virtual void FBEnsurePermissions(IList<string> _permissions, bool _askIfNeeded = true, FBRequestCallback _callback = null)
	{
	}

	public virtual void FBInviteFriends(FBRequestCallback _callback = null, FBFriendCallback _friendCallback = null)
	{
	}

	public virtual void FBGetFriends(FBRequestCallback _callback = null, FBFriendCallback _friendCallback = null)
	{
	}

	public virtual void FBShareItem(FBShareItemInfo _itemInfo, FBRequestCallback _callback = null)
	{
	}

	protected void CompleteRequest(bool _success, EFBRequestType _requestType, EFBReturnCode _returnCode, FBRequestCallback _callback = null)
	{
		if (this.OnFBRequestCompleted != null)
		{
			this.OnFBRequestCompleted(_success, _requestType, _returnCode);
		}
		if (_callback != null)
		{
			_callback(_success, _requestType, _returnCode);
		}
	}

	public void DrawDebugMenu()
	{
		if (this.OnFBRequestCompleted == null)
		{
			this.OnFBRequestCompleted = (FBRequestCallback)Delegate.Combine(this.OnFBRequestCompleted, new FBRequestCallback(DisplayStatus));
		}
		GUILayout.BeginVertical("box");
		GUILayout.Label("Facebook flow:");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("AutoLogin"))
		{
			FacebookManager.AutoLogin();
		}
		if (GUILayout.Button("Login"))
		{
			FacebookManager.Login();
		}
		if (GUILayout.Button("Logout"))
		{
			FacebookManager.Logout();
		}
		if (GUILayout.Button("Invite"))
		{
			FacebookManager.Invite();
		}
		if (GUILayout.Button("Reset"))
		{
			FacebookManager.ResetProfile();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Get permissions"))
		{
			FBGetPermissions(null, delegate(IDictionary<string, bool> permissions)
			{
				m_lastUserPermissions = permissions;
			});
		}
		if (GUILayout.Button("Get picture"))
		{
			FBGetPicture("me", 256f, null, delegate(Texture2D picture)
			{
				m_lastPicture = picture;
			});
		}
		if (GUILayout.Button("Post achievement"))
		{
			FBShareItemInfo fBShareItemInfo = new FBShareItemInfo();
			fBShareItemInfo.linkName = "Wall Post Title";
			fBShareItemInfo.linkDescription = "Wall post description";
			fBShareItemInfo.linkCaption = "Wall post caption";
			fBShareItemInfo.picture = new Uri("https://static21.cdn.ubi.com/v1/AUTH_af4f760a333b49878eed6e1b3ab95dd4/acier/achievement_922255906_silver.png");
			fBShareItemInfo.reference = "TestPostAchievement";
			FBShareItem(fBShareItemInfo);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Invite friends"))
		{
			FBInviteFriends(null, delegate(IList<FBFriendInfo> friends)
			{
				m_lastSelectedFriends = friends;
				if (friends != null)
				{
					for (int j = 0; j < friends.Count; j++)
					{
						FBFriendInfo friend = friends[j];
						FBGetName(friend.Id, null, delegate(string name)
						{
							friend.Name = name;
						});
					}
				}
			});
		}
		if (GUILayout.Button("Get friends"))
		{
			FBGetFriends(null, delegate(IList<FBFriendInfo> friends)
			{
				m_lastUserFriends = friends;
			});
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical("box");
		GUILayout.Label(m_lastStatus);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		if (!FacebookManager.IsProfileLinkedToFacebook)
		{
			GUILayout.Label("Profile is not linked to Facebook");
		}
		else
		{
			GUILayout.Label(string.Format("Linked to Facebook as {0} ({1} - {2})", FacebookManager.LinkedFacebookUserName, FacebookManager.LinkedFacebookUserId, FacebookManager.LinkedFacebookGameId));
		}
		GUILayout.EndVertical();
		GUILayout.Label("Initialized: " + IsInitialized);
		GUILayout.Label("UserId: " + UserId);
		GUILayout.Label("GameId: " + ((Singleton<StateManager>.Instance.ProfileManager.Profile == null) ? "N/A" : Singleton<StateManager>.Instance.ProfileManager.Profile.userIdHash));
		GUILayout.Label("AccessToken: " + AccessToken);
		GUILayout.Label("UserFullName: " + FacebookManager.Name);
		if (m_lastPicture != null)
		{
			GUILayout.Label(m_lastPicture, GUILayout.MaxWidth(128f), GUILayout.MaxHeight(128f));
		}
		if (m_lastUserPermissions != null)
		{
			string text = string.Empty;
			foreach (KeyValuePair<string, bool> lastUserPermission in m_lastUserPermissions)
			{
				text = ((!lastUserPermission.Value) ? (text + " -" + lastUserPermission.Key) : (text + " +" + lastUserPermission.Key));
			}
			GUILayout.Label("Permissions:" + text);
		}
		if (m_lastUserFriends != null)
		{
			GUILayout.Label("Number of friends in app: " + m_lastUserFriends.Count);
		}
		if (m_lastSelectedFriends != null)
		{
			GUILayout.Label("Number of friends invited: " + m_lastSelectedFriends.Count);
			for (int i = 0; i < m_lastSelectedFriends.Count; i++)
			{
				GUILayout.Label(m_lastSelectedFriends[i].Id + " \t" + m_lastSelectedFriends[i].Name);
			}
		}
		GUILayout.EndVertical();
	}

	private void DisplayStatus(bool success, EFBRequestType _requestType, EFBReturnCode _returnCode)
	{
		m_lastStatus = string.Concat((!success) ? "[FAILURE] " : "[SUCCESS]: ", _requestType, ": ", _returnCode);
	}
}
