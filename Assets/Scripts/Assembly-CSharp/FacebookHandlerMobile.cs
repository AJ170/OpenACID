using System.Collections;
using System.Collections.Generic;

public class FacebookHandlerMobile : FacebookHandlerBase
{
	private bool m_shouldBeLoggedIn;

	public override bool IsAvailable
	{
		get
		{
			return false;
		}
	}

	public override bool IsInitialized
	{
		get
		{
			return false;
		}
	}

	public override bool IsLoggedIn
	{
		get
		{
			return false;
		}
	}

	public override string AccessToken
	{
		get
		{
			return string.Empty;
		}
	}

	public override string UserId
	{
		get
		{
			return string.Empty;
		}
	}

	public override void FBInit(FBRequestCallback _callback = null)
	{
	}

	public override void FBLogin(FBRequestCallback _callback = null)
	{
	}

	public override void FBLogout(FBRequestCallback _callback = null)
	{
	}

	private IEnumerator OnProcessingLogout(FBRequestCallback _callback = null)
	{
		yield break;
	}

	public override void FBGetPicture(string userId = "me", float size = 256f, FBRequestCallback _callback = null, FBPictureCallback _pictureCallback = null)
	{
	}

	public override void FBGetName(string userId = "me", FBRequestCallback _callback = null, FBNameCallback _nameCallback = null)
	{
	}

	public override void FBGetAppRequests(FBRequestCallback _callback = null, FBAppRequestCallback _appRequestCallback = null)
	{
	}

	public override void FBDeleteAppRequest(FBAppRequestInfo _request, FBRequestCallback _callback = null)
	{
	}

	public override void FBEnsurePermissions(IList<string> _permissions, bool _askIfNeeded = true, FBRequestCallback _callback = null)
	{
	}

	public override void FBGetPermissions(FBRequestCallback _callback = null, FBPermissionCallback _permissionCallback = null)
	{
	}

	public override void FBInviteFriends(FBRequestCallback _callback = null, FBFriendCallback _friendCallback = null)
	{
	}

	public override void FBGetFriends(FBRequestCallback _callback = null, FBFriendCallback _friendCallback = null)
	{
	}

	public override void FBShareItem(FBShareItemInfo _itemInfo, FBRequestCallback _callback = null)
	{
	}
}
