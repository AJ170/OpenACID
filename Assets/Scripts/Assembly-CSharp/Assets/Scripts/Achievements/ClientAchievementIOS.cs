using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.UI.State;
using SharedBaseLib.Core;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Assets.Scripts.Achievements
{
	public class ClientAchievementIOS : IClientAchievements
	{
		private static BBList<IAchievement> m_achievementsCurrentList = new BBList<IAchievement>();

		public void Init(string _gameObjectName = "")
		{
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			SignIn();
		}

		public bool IsSignedIn()
		{
			bool authenticated = Social.localUser.authenticated;
			if (!authenticated)
			{
			}
			return authenticated;
		}

		public void SignIn()
		{
			if (Social.localUser.authenticated)
			{
				DownloadAchievementsFromGameCenter();
				return;
			}
			Social.localUser.Authenticate(delegate(bool authenticated)
			{
				if (authenticated)
				{
					DownloadAchievementsFromGameCenter();
				}
			});
		}

		public void SignOut()
		{
		}

		private void DownloadAchievementsFromGameCenter()
		{
			Social.LoadAchievements(delegate(IAchievement[] achievements)
			{
				if (achievements != null)
				{
					if (m_achievementsCurrentList != null)
					{
						m_achievementsCurrentList.Clear();
					}
					foreach (IAchievement item in achievements)
					{
						m_achievementsCurrentList.Add(item);
					}
					Main.Instance.StartCoroutine(OnAchievementsDownloadedRoutine());
				}
			});
		}

		public static IEnumerator OnAchievementsDownloadedRoutine()
		{
			while (Singleton<StateManager>.Instance.AchievementsManager.CounterToGcAchievement.Count == 0)
			{
				yield return null;
			}
			Singleton<StateManager>.Instance.AchievementsManager.OnThirdPartyCountersAvailable();
		}

		public void ShowAchievements()
		{
			if (!Social.localUser.authenticated)
			{
				Social.localUser.Authenticate(delegate(bool success)
				{
					if (success)
					{
						Social.ShowAchievementsUI();
					}
				});
			}
			else
			{
				Social.ShowAchievementsUI();
			}
		}

		public void ResetAllAchievements()
		{
			GameCenterPlatform.ResetAllAchievements(delegate
			{
			});
		}

		public void UpdateAchievementProgressPoints(string _thirdPartyAchievementId, double _currentProgress)
		{
			if (!Social.localUser.authenticated || string.IsNullOrEmpty(_thirdPartyAchievementId))
			{
				return;
			}
			bool flag = false;
			bool flag2 = true;
			if (m_achievementsCurrentList != null)
			{
				foreach (IAchievement achievementsCurrent in m_achievementsCurrentList)
				{
					if (!(_thirdPartyAchievementId == achievementsCurrent.id))
					{
						continue;
					}
					flag2 = false;
					if (achievementsCurrent.completed)
					{
						return;
					}
					AchievementMapping.Definition achievementMappingDefinitionByGcId = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementMappingDefinitionByGcId(_thirdPartyAchievementId);
					if (achievementMappingDefinitionByGcId == null || (achievementMappingDefinitionByGcId.hidden && _currentProgress < 100.0))
					{
						return;
					}
					if (achievementsCurrent.percentCompleted < _currentProgress)
					{
						flag = true;
					}
					achievementsCurrent.percentCompleted = _currentProgress;
					break;
				}
			}
			if (flag2)
			{
				flag = true;
				IAchievement achievement = Social.CreateAchievement();
				achievement.id = _thirdPartyAchievementId;
				achievement.percentCompleted = _currentProgress;
				m_achievementsCurrentList.Add(achievement);
			}
			if (flag)
			{
				ReportAchievementProgress(_thirdPartyAchievementId, _currentProgress, delegate
				{
				});
			}
		}

		private void ReportAchievementProgress(string _achievementId, double _percentCompleted, Action<bool> _callback = null)
		{
			bool obj = true;
			if (!Social.localUser.authenticated)
			{
				obj = false;
			}
			else
			{
				IAchievement achievement = Social.CreateAchievement();
				achievement.id = _achievementId;
				achievement.percentCompleted = _percentCompleted;
				achievement.ReportProgress(_callback);
			}
			if (_callback != null)
			{
				_callback(obj);
			}
		}

		private void LoadAchievements(Action<IAchievement[]> _callback = null)
		{
			if (Social.localUser.authenticated)
			{
				Social.LoadAchievements(_callback);
			}
		}

		public void ShowLeaderBoards()
		{
			if (!Social.localUser.authenticated)
			{
				Social.localUser.Authenticate(delegate(bool success)
				{
					if (success)
					{
						Social.ShowLeaderboardUI();
					}
				});
			}
			else
			{
				Social.ShowLeaderboardUI();
			}
		}

		public void UpdateLeaderboardProgressScore(string _leaderboardId, long _score)
		{
			ReportLeaderboardScore(_leaderboardId, _score, delegate
			{
			});
		}

		private void ReportLeaderboardScore(string _leaderboardId, long _score, Action<bool> _callback = null)
		{
			if (Social.localUser.authenticated)
			{
				Social.ReportScore(_score, _leaderboardId, _callback);
			}
			else if (_callback != null)
			{
				_callback(false);
			}
		}

		public void OnNativeGGPServicesCallback(string _jsonMessage)
		{
		}
	}
}
