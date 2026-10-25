using Assets.Scripts.Achievements;
using UnityEngine;

public class ClientAchievementsManager
{
	public delegate void ClientAchievementLoginDelegate();

	public delegate void ClientAchievementLogoutDelegate();

	private static IClientAchievements m_achievementsMgr = null;

	private static string m_gameObjectName = string.Empty;

	private static bool m_isAvailable;

	public static event ClientAchievementLoginDelegate ClientAchievementSignInCallback;

	public static event ClientAchievementLogoutDelegate ClientAchievementSignOutCallback;

	public ClientAchievementsManager(string _objectName)
	{
		m_gameObjectName = _objectName;
		if (m_achievementsMgr == null && Application.platform == RuntimePlatform.Android)
		{
			m_achievementsMgr = new ClientAchievementsAndroid();
			m_isAvailable = true;
		}
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.Init(m_gameObjectName);
		}
	}

	public static void SignInCallback()
	{
		if (ClientAchievementsManager.ClientAchievementSignInCallback != null)
		{
			ClientAchievementsManager.ClientAchievementSignInCallback();
		}
	}

	public static void SignOutCallback()
	{
		if (ClientAchievementsManager.ClientAchievementSignOutCallback != null)
		{
			ClientAchievementsManager.ClientAchievementSignOutCallback();
		}
	}

	public static bool IsAvailable()
	{
		return m_isAvailable;
	}

	public static bool IsGooglePlayAvailable()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return true;
		}
		return false;
	}

	public static bool IsSignedIn()
	{
		return m_achievementsMgr != null && m_achievementsMgr.IsSignedIn();
	}

	public static void SignIn()
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.SignIn();
		}
	}

	public static void SignOut()
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.SignOut();
		}
	}

	public static void ShowAchievements()
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.ShowAchievements();
		}
	}

	public static void ShowLeaderBoards()
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.ShowLeaderBoards();
		}
	}

	public static void ResetAllAchievements()
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.ResetAllAchievements();
		}
	}

	public static void UpdateAchievementProgressPoints(string _thirdPartyAchievementId, double _currentProgress)
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.UpdateAchievementProgressPoints(_thirdPartyAchievementId, _currentProgress);
		}
	}

	public static void UpdateLeaderboardProgressScore(string _leaderboardId, long _score)
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.UpdateLeaderboardProgressScore(_leaderboardId, _score);
		}
	}

	public void OnNativeGGPServicesCallback(string _message)
	{
		if (m_achievementsMgr != null)
		{
			m_achievementsMgr.OnNativeGGPServicesCallback(_message);
		}
	}
}
