using System;
using System.Collections;
using System.Collections.Generic;
using AcierNetwork.Communication;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.CameraManager;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Mission.Loading
{
	public class SceneLoadingProvider : IProvider
	{
		public class SceneLoadingData : Params
		{
			public string Scene { get; set; }
		}

		private const float ProgressUpdateDelta = 0.1f;

		private Query<SceneLoadingModel> m_currentRequest;

		public override void Initialize()
		{
		}

		public override bool Accepts(Type _type)
		{
			return _type == typeof(SceneLoadingModel);
		}

		public override bool BeginRead<T>(Request _request)
		{
			if (m_currentRequest != null || !Accepts(typeof(T)))
			{
				return false;
			}
			m_currentRequest = (Query<SceneLoadingModel>)_request;
			return true;
		}

		protected override IEnumerator UpdateAsync()
		{
			SceneLoadingModel sceneLoadingModel = new SceneLoadingModel();
			SceneLoadingData scene = (SceneLoadingData)m_currentRequest.Param;
			bool localScene = Assets.Scripts.Offline.LocalBundles.HasScene(scene.Scene);
			if (!localScene && !Application.CanStreamedLevelBeLoaded(scene.Scene))
			{
				base.Result = new QueryResult(RequestResult.Status.Failed);
			}
			else
			{
				Singleton<UICameraManager>.Instance.PurgeManager();
				Singleton<UIScreenManager>.Instance.PurgeManager();
				// Editor: level scenes come from the imported bundles, not from the build settings.
				AsyncOperation asyncResult2 = localScene ? Assets.Scripts.Offline.LocalBundles.LoadSceneAsync(scene.Scene) : Application.LoadLevelAsync(scene.Scene);
				while (!asyncResult2.isDone)
				{
					m_currentRequest.Progress(asyncResult2.progress * 0.5f);
					yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
				}
				List<string> loadUIScenes = new List<string>();
				if (Main.Instance.SceneLoader.InAnimus)
				{
					loadUIScenes.Add("UI");
					loadUIScenes.Add("UIAssassinsSelectionScreen");
					loadUIScenes.Add("UIAssassinsNamingScreen");
					loadUIScenes.Add("UIAssassinsSkillTreeScreen");
					loadUIScenes.Add("UIAssassinSkillWheelScreen");
					loadUIScenes.Add("UICodexScreen");
					loadUIScenes.Add("UIShopScreen");
					loadUIScenes.Add("UISocialScreen");
					loadUIScenes.Add("UIFriendsListScreen");
					loadUIScenes.Add("UIInviteFriendsScreen");
					loadUIScenes.Add("UIMissionsHirelingScreen");
					loadUIScenes.Add("UIMissionsScreen");
					loadUIScenes.Add("UIOptionsScreen");
					loadUIScenes.Add("UISoundOptionsScreen");
					loadUIScenes.Add("UIAccountCreation");
					loadUIScenes.Add("UIGeneralSettingsMenu");
					loadUIScenes.Add("UIMailScreen");
					loadUIScenes.Add("UISupportOptionsScreen");
					loadUIScenes.Add("UIAchievementsScreen");
					loadUIScenes.Add("UILeaderboardScreen");
					loadUIScenes.Add("UICreditsScreen");
					loadUIScenes.Add("UIAssassinsInventoryScreenNew");
					loadUIScenes.Add("UIMysteryBoxScreen");
					loadUIScenes.Add("UITabUnlockScreen");
					loadUIScenes.Add("UIGamepadOptionsScreen");
					loadUIScenes.Add("UIEventsScreen");
					if (UbiCore.Utils.Singleton<UiMain>.Instance.ShouldShowAnimusStartupScreens)
					{
						loadUIScenes.Add("UINewsFeedScreen");
					}
				}
				else
				{
					loadUIScenes.Add("UIInGame");
					loadUIScenes.Add("UIEndResultScreen");
					loadUIScenes.Add("UIOptionsScreen");
					loadUIScenes.Add("UISoundOptionsScreen");
					loadUIScenes.Add("UIControlsOptionsScreen");
					loadUIScenes.Add("UIGamepadOptionsScreen");
				}
				float startProgress = 0.5f;
				float progressPerScene = 0.5f / (float)loadUIScenes.Count;
				for (int i = 0; i < loadUIScenes.Count; i++)
				{
					asyncResult2 = Application.LoadLevelAdditiveAsync(loadUIScenes[i]);
					if (asyncResult2 != null)
					{
						while (!asyncResult2.isDone)
						{
							m_currentRequest.Progress(startProgress + asyncResult2.progress * progressPerScene);
							yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.1f));
						}
					}
					startProgress += progressPerScene;
				}
				if (retinaProAtlasController.sharedInstance != null)
				{
					retinaProAtlasController.sharedInstance.refreshAll();
				}
				CreateAllProxyButtons();
				RemoveUnusedUICameras(Main.Instance.SceneLoader.InAnimus);
				Singleton<UIScreenManager>.Instance.Initialize();
				Singleton<UICameraManager>.Instance.Initialize();
				base.Result = new QueryResult(RequestResult.Status.Suceeded);
			}
			m_currentRequest.EndRead(base.Result, sceneLoadingModel);
			if (UiMain.UIAlignmentRightHanded)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.OnUIAlignmentChanged();
			}
			m_currentRequest = null;
			base.Result = null;
		}

		private static void AddStaticFlagToAllChildren(GameObject _go)
		{
			_go.isStatic = true;
			int childCount = _go.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = _go.transform.GetChild(i);
				AddStaticFlagToAllChildren(child.gameObject);
			}
		}

		private static void CreateAllProxyButtons()
		{
			for (int i = 0; i < UIRoot.list.Count; i++)
			{
				UIRoot uIRoot = UIRoot.list[i];
				if (uIRoot != null)
				{
					UIPrefabProxy[] componentsInChildren = uIRoot.GetComponentsInChildren<UIPrefabProxy>(true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].Init();
					}
				}
			}
		}

		private static void RemoveUnusedUICameras(bool _isAnimusMission)
		{
			int num = 0;
			while (num < UICamera.list.size)
			{
				UICamera uICamera = UICamera.list[num];
				UICameraOwner component = uICamera.GetComponent<UICameraOwner>();
				if (component == null || (_isAnimusMission && !component.DontDestroyCameraInAnimus) || (!_isAnimusMission && !component.DontDestroyCameraInGame))
				{
					NGUITools.Destroy(uICamera.gameObject);
				}
				else
				{
					num++;
				}
			}
		}
	}
}
