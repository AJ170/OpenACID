using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.State;
using MiniJSON;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
	public class ClientAchievementsAndroid : IClientAchievements
	{
		public enum State
		{
			STATE_UNLOCKED = 0,
			STATE_REVEALED = 1,
			STATE_HIDDEN = 2,
			STATE_UNKNOWN = 3
		}

		private class GGPAchievement
		{
			public string id = string.Empty;

			public string name = string.Empty;

			public string description = string.Empty;

			public string lastUpdate = string.Empty;

			public State state;

			public bool isIncremental;

			public double currentSteps;

			public int totalSteps;
		}

		private const int STATE_SIGN_IN = 1;

		private const int STATE_SIGN_OUT = 2;

		private const int STATE_LOAD_ACHIEVEMENTS = 3;

		private const int STATE_SHOW_ACHIEVEMENTS = 4;

		private const int STATE_UPDATE_NON_INCREMENTAL_ACHIEVEMENT = 5;

		private const int STATE_UPDATE_INCREMENTAL_ACHIEVEMENT = 6;

		private const int RESULT_ERROR = 0;

		private const int RESULT_OK = 1;

		private const int RESULT_SIGN_IN_SUCCESSFUL = 100;

		private const int RESULT_SIGN_IN_CANCELLED = 101;

		private const int RESULT_SIGN_IN_FAILED = 102;

		private const int RESULT_SIGN_OUT_SUCCESSFUL = 103;

		private const int RESULT_SIGN_OUT_FAILED = 104;

		private const int RESULT_NOT_LOGGED_IN = 105;

		private const int RESULT_UPDATE_ACHIEVEMENT_SUCCESSFUL = 106;

		private const int RESULT_NON_IMMEDIATE_UPDATE_ACHIEVEMENT_SUCCESSFUL = 107;

		private const int RESULT_SHOW_ACHIEVEMENTS_SUCCESSFUL = 108;

		private static AndroidJavaObject ggpServices = new AndroidJavaObject("com.ubi.ggpservices.GGPServices");

		private static BBList<GGPAchievement> m_achievementsCurrentList = new BBList<GGPAchievement>();

		private static bool ms_showAchievements = false;

		public void Init(string _gameObjectName)
		{
			ggpServices.Call("SetGameObject", _gameObjectName);
			if (ggpServices.Call<bool>("IsAutoSignInEnabled", new object[0]))
			{
				ggpServices.Call("SignIn");
			}
		}

		public void OnNativeGGPServicesCallback(string _jsonMessage)
		{
			try
			{
				Dictionary<string, object> dictionary = Json.Deserialize(_jsonMessage) as Dictionary<string, object>;
				int state = int.Parse((string)dictionary["current-state"]);
				int num = int.Parse((string)dictionary["status"]);
				string msgcode = (string)dictionary["message"];
				MessageCodeResult(num == 1, state, msgcode);
			}
			catch (Exception)
			{
			}
		}

		public bool IsSignedIn()
		{
			return ggpServices.Call<bool>("IsSignedIn", new object[0]);
		}

		public void SignIn()
		{
			if (!IsSignedIn())
			{
				ggpServices.Call("SignIn");
			}
			else
			{
				ClientAchievementsManager.SignInCallback();
			}
		}

		public void SignOut()
		{
			if (IsSignedIn())
			{
				ggpServices.Call("SignOut");
			}
			else
			{
				ClientAchievementsManager.SignOutCallback();
			}
		}

		public void ShowAchievements()
		{
			ms_showAchievements = true;
			if (!IsSignedIn())
			{
				ggpServices.Call("SignIn");
			}
			else
			{
				ggpServices.Call("ShowAchievements");
			}
		}

		public void ShowLeaderBoards()
		{
		}

		public void ResetAllAchievements()
		{
			ggpServices.Call("ResetAchievements");
		}

		public void UpdateAchievementProgressPoints(string _thirdPartyAchievementId, double _currentProgress)
		{
			if (!IsSignedIn() || string.IsNullOrEmpty(_thirdPartyAchievementId) || m_achievementsCurrentList == null)
			{
				return;
			}
			GGPAchievement achievementWithID = getAchievementWithID(_thirdPartyAchievementId);
			if (achievementWithID == null || (achievementWithID.state == State.STATE_HIDDEN && _currentProgress < 100.0))
			{
				return;
			}
			if (!achievementWithID.isIncremental)
			{
				if (achievementWithID.state != 0)
				{
					getAchievementWithID(_thirdPartyAchievementId).state = State.STATE_UNLOCKED;
					ggpServices.Call("ReportNonIncrementalAchievementProgress", _thirdPartyAchievementId, true);
				}
			}
			else if ((achievementWithID.currentSteps != 100.0 || achievementWithID.state != 0) && (achievementWithID.currentSteps != _currentProgress || achievementWithID.state == State.STATE_HIDDEN) && !(_currentProgress < achievementWithID.currentSteps))
			{
				double num = 0.0;
				if (achievementWithID.state == State.STATE_HIDDEN && _currentProgress == 100.0)
				{
					num = _currentProgress;
					getAchievementWithID(_thirdPartyAchievementId).state = State.STATE_UNLOCKED;
				}
				else
				{
					num = _currentProgress - achievementWithID.currentSteps;
				}
				getAchievementWithID(_thirdPartyAchievementId).currentSteps = _currentProgress;
				if (num > 0.0)
				{
					ggpServices.Call("ReportIncrementalAchievementProgress", _thirdPartyAchievementId, (int)num, true);
				}
			}
		}

		public void UpdateLeaderboardProgressScore(string _leaderboardId, long _score)
		{
		}

		public void MessageCodeResult(bool _success, int _state, string _msgcode)
		{
			if (_success)
			{
				switch (_state)
				{
				case 1:
					if (int.Parse(_msgcode) == 101)
					{
						if (ms_showAchievements)
						{
							ms_showAchievements = false;
						}
						break;
					}
					ClientAchievementsManager.SignInCallback();
					DownloadAchievementsFromGooglePlay();
					if (ms_showAchievements)
					{
						ShowAchievements();
					}
					break;
				case 2:
					ClientAchievementsManager.SignOutCallback();
					break;
				case 3:
					ProcessAchievements(_msgcode);
					break;
				case 4:
					ms_showAchievements = false;
					break;
				case 5:
					break;
				case 6:
					break;
				}
			}
			else
			{
				switch (_state)
				{
				case 1:
					ClientAchievementsManager.SignInCallback();
					break;
				case 2:
					ClientAchievementsManager.SignOutCallback();
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
				case 6:
					break;
				}
			}
		}

		private void ProcessAchievements(string achievementsJsonlist)
		{
			m_achievementsCurrentList.Clear();
			try
			{
				List<object> list = Json.Deserialize(achievementsJsonlist) as List<object>;
				int num = 0;
				foreach (Dictionary<string, object> item in list)
				{
					GGPAchievement gGPAchievement = new GGPAchievement();
					gGPAchievement.id = (string)item["id"];
					gGPAchievement.name = (string)item["name"];
					switch (int.Parse((string)item["state"]))
					{
					case 1:
						gGPAchievement.state = State.STATE_REVEALED;
						break;
					case 2:
						gGPAchievement.state = State.STATE_HIDDEN;
						break;
					default:
						gGPAchievement.state = State.STATE_UNLOCKED;
						break;
					}
					gGPAchievement.isIncremental = (((string)item["isIncremental"] == "1") ? true : false);
					gGPAchievement.currentSteps = int.Parse((string)item["currentSteps"]);
					num++;
					m_achievementsCurrentList.Add(gGPAchievement);
				}
				Main.Instance.StartCoroutine(OnAchievementsDownloadedRoutine());
			}
			catch (Exception)
			{
			}
		}

		private void DownloadAchievementsFromGooglePlay()
		{
			ggpServices.Call("LoadAchievements");
		}

		public static IEnumerator OnAchievementsDownloadedRoutine()
		{
			while (Singleton<StateManager>.Instance.AchievementsManager.CounterToGcAchievement.Count == 0)
			{
				yield return null;
			}
			Singleton<StateManager>.Instance.AchievementsManager.OnThirdPartyCountersAvailable();
		}

		private GGPAchievement getAchievementWithID(string _thirdPartyAchievementId)
		{
			if (m_achievementsCurrentList != null)
			{
				foreach (GGPAchievement achievementsCurrent in m_achievementsCurrentList)
				{
					if (achievementsCurrent.id == _thirdPartyAchievementId)
					{
						return achievementsCurrent;
					}
				}
			}
			return null;
		}
	}
}
