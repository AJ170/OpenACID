namespace Assets.Scripts.Achievements
{
	public interface IClientAchievements
	{
		void Init(string _gameObjectName = "");

		bool IsSignedIn();

		void SignIn();

		void SignOut();

		void ShowAchievements();

		void ShowLeaderBoards();

		void ResetAllAchievements();

		void UpdateAchievementProgressPoints(string _thirdPartyAchievementId, double _currentProgress);

		void UpdateLeaderboardProgressScore(string _leaderboardId, long _score);

		void OnNativeGGPServicesCallback(string _jsonMessage);
	}
}
