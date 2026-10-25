using System;
using System.Collections;
using AcierNetwork;
using AcierNetwork.AssetBundles;
using AcierNetwork.Communication;
using AcierNetwork.Provider;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Crowd;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Mission.Loading;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission
{
	public class SceneLoader
	{
		public enum ELoadingStep
		{
			LoadingStart = 0,
			LoadingFailed = 1,
			LoadingSucceeded = 2,
			LoadingMissionDone = 3,
			LoadingSceneDone = 4,
			PreInitialize = 5,
			CameraCreated = 6,
			Unload = 7,
			Unloaded = 8,
			AllDone = 9
		}

		private enum EMissionLoadSteps
		{
			Started = 0,
			BundlesReceived = 10,
			SceneLoaded = 100
		}

		public delegate void LoadingProgress(int _progress);

		public delegate void LoadingStep(ELoadingStep _step);

		private bool m_bIsRestart;

		public GameObject Root { get; set; }

		public MissionDescExt CurrentMissionDescription { get; private set; }

		public MissionLoaderProvider.ResultData LoadedMissionData { get; private set; }

		public MissionLoaderProvider.ResultData LastLoadedMissionData { get; private set; }

		public bool InAnimus
		{
			get
			{
				return CurrentMissionDescription != null && CurrentMissionDescription.MissionType == EMissionType.Animus_EMissionType;
			}
		}

		public event LoadingStep OnStep = delegate
		{
		};

		public event LoadingProgress OnProgress = delegate
		{
		};

		public void PrepareScene(string _missionUrl, bool _isRestart)
		{
			this.OnStep(ELoadingStep.LoadingStart);
			m_bIsRestart = _isRestart;
			if (!m_bIsRestart)
			{
				LoadedMissionData = null;
				CurrentMissionDescription = null;
				MissionLoaderProvider.Input input = new MissionLoaderProvider.Input();
				input.MissionUrl = _missionUrl;
				MissionLoaderProvider.Input param = input;
				NetworkProvider.Instance.Request<MissionLoaderProvider.ResultData>(OnMissionDataLoaded, param);
			}
			else
			{
				this.OnStep(ELoadingStep.LoadingMissionDone);
			}
		}

		private AssetBundleInfo GetBundleInfo(GameID _id)
		{
			if (_id == null)
			{
				return null;
			}
			return Main.Instance.GameDB.GetAssetBundleInfo(_id);
		}

		private void FillBundles(AvatarData.IAvatarDefinition _avatarDef, ref BBHashSet<AssetBundleInfo> _bundles)
		{
			AssetBundleInfo bundleInfo = GetBundleInfo(_avatarDef.HumanoidGameID);
			if (bundleInfo != null && !_bundles.Contains(bundleInfo))
			{
				_bundles.Add(bundleInfo);
			}
			bundleInfo = GetBundleInfo(_avatarDef.WeaponGameID);
			if (bundleInfo != null && !_bundles.Contains(bundleInfo))
			{
				_bundles.Add(bundleInfo);
			}
			bundleInfo = GetBundleInfo(_avatarDef.HeadGameID);
			if (bundleInfo != null && !_bundles.Contains(bundleInfo))
			{
				_bundles.Add(bundleInfo);
			}
		}

		public void LoadScene()
		{
			MissionProviderInput missionProviderInput = new MissionProviderInput();
			missionProviderInput.AssetBundles = new BBHashSet<AssetBundleInfo>();
			MissionProviderInput missionProviderInput2 = missionProviderInput;
			if (Application.loadedLevelName != CurrentMissionDescription.Scene)
			{
				missionProviderInput2.AssetBundles = new BBHashSet<AssetBundleInfo> { CurrentMissionDescription.AssetBundle };
			}
			foreach (AssetBundleInfo runtimeAssetBundle in LoadedMissionData.RuntimeAssetBundles)
			{
				if (!AssetBundleManager.Instance.HasBundleLoaded(runtimeAssetBundle))
				{
					missionProviderInput2.AssetBundles.Add(runtimeAssetBundle);
				}
			}
			AvatarData.IAvatarDefinition playerDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
			BBHashSet<AssetBundleInfo> _bundles = missionProviderInput2.AssetBundles;
			FillBundles(playerDefinition, ref _bundles);
			AvatarData.IAvatarDefinition hirelingDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetHirelingDefinition();
			if (hirelingDefinition != null)
			{
				FillBundles(hirelingDefinition, ref _bundles);
			}
			for (int i = 0; i < CrowdSettings.GuidList.Length; i++)
			{
				string guid = CrowdSettings.GuidList[i];
				missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(Main.Instance.GameDB.FindGameIdFromGuid(guid)));
			}
			missionProviderInput2.AssetBundles.UnionWith(Main.Instance.GameDB.GetEnforcedLoadingAssetBundleInfos());
			missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_alternativeCharacterMesh));
			if (CurrentMissionDescription.MissionType == EMissionType.Animus_EMissionType)
			{
				missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_silhouette));
				AvatarClassDefinitions avatarDefinition = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition;
				for (int j = 0; j < avatarDefinition.classes.Length; j++)
				{
					AvatarClassDefinitions.AvatarClass avatarClass = avatarDefinition.classes[j];
					GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(avatarClass.humanoidGuid);
					missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(gameID));
					GameID gameID2 = Main.Instance.GameDB.FindGameIdFromGuid(avatarClass.weaponGuid);
					missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(gameID2));
					GameID gameID3 = Main.Instance.GameDB.FindGameIdFromGuid(avatarClass.heads.headGuid);
					missionProviderInput2.AssetBundles.Add(Main.Instance.GameDB.GetAssetBundleInfo(gameID3));
				}
			}
			NetworkProvider.Instance.Request<AcierNetwork.Provider.MissionData>(OnBundlesLoaded, missionProviderInput2);
		}

		public void PrepareScene(MissionFile _proto, MissionGoals _missionGoals, bool _eliteMission, bool _isRestart)
		{
			this.OnStep(ELoadingStep.LoadingStart);
			m_bIsRestart = _isRestart;
			if (!m_bIsRestart)
			{
				LastLoadedMissionData = LoadedMissionData;
				LoadedMissionData = null;
				CurrentMissionDescription = null;
				MissionLoaderProvider.Input input = new MissionLoaderProvider.Input();
				input.FullMissionProto = _proto;
				input.MissionGoals = _missionGoals;
				input.IsEliteMission = _eliteMission;
				MissionLoaderProvider.Input param = input;
				NetworkProvider.Instance.Request<MissionLoaderProvider.ResultData>(OnMissionDataLoaded, param);
			}
			else
			{
				SetLoadedMissionData(LoadedMissionData);
			}
		}

		public void OnLevelWasLoaded(string _loadedLevelName)
		{
			RemoveUIMasksFromMainCamera();
			if (!(_loadedLevelName == "Empty"))
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Main");
				gameObject.BroadcastMessage("OnPreInitialize", SendMessageOptions.DontRequireReceiver);
				if (UnityEngine.Camera.main == null)
				{
					GameObject gameObject2 = Main.Instance.CreateSharedCamera();
					gameObject2.name = "Main Camera";
					gameObject2.transform.SetParent(new GameObject("Main Camera Seat").transform, false);
					RemoveUIMasksFromMainCamera();
				}
				AssetBundleManager.Instance.UnloadBundles();
				HumanoidData.OnSceneLoad(CurrentMissionDescription.Age.Fix());
				TriggerManager.Instance.enabled = true;
				this.OnStep(ELoadingStep.CameraCreated);
				UpdateLighting();
				gameObject.BroadcastMessage("OnPostInitialize", SendMessageOptions.DontRequireReceiver);
				this.OnStep(ELoadingStep.LoadingSceneDone);
			}
		}

		public IEnumerator UnloadScene(bool _isRestarting = false)
		{
			if (!Profiler.enabled)
			{
				Profiling.Start("SceneLoader::UnloadScene", float.MaxValue);
			}
			this.OnStep(ELoadingStep.Unload);
			Profiling.DumpMemoryStats();
			TriggerManager.Instance.enabled = false;
			if (!_isRestarting)
			{
				AnimationController.Clear();
				Singleton<HumanoidFactory>.Instance.Reset();
				HumanoidData.OnSceneUnload();
				Main.Instance.GameDB.ClearCachedResources();
				AsyncOperation result3 = UnityEngine.Resources.UnloadUnusedAssets();
				while (!result3.isDone)
				{
					yield return 0;
				}
				GC.Collect();
				AsyncOperation load = Application.LoadLevelAsync("Empty");
				while (!load.isDone)
				{
					yield return 0;
				}
				UIDrawCall.ReleaseInactive();
				GC.Collect();
				result3 = UnityEngine.Resources.UnloadUnusedAssets();
				while (!result3.isDone)
				{
					yield return 0;
				}
				this.OnStep(ELoadingStep.Unloaded);
			}
			else
			{
				AsyncOperation result = UnityEngine.Resources.UnloadUnusedAssets();
				while (!result.isDone)
				{
					yield return 0;
				}
				GC.Collect();
			}
			if (!Profiler.enabled)
			{
				Profiling.Stop();
			}
		}

		private void OnLevelLoaded(QueryResult _result, SceneLoadingModel _sceneLoading)
		{
			switch (_result.RequestStatus)
			{
			case RequestResult.Status.InProgress:
				this.OnProgress((int)Mathf.Lerp(10f, 100f, _result.Progress));
				break;
			case RequestResult.Status.Failed:
				this.OnStep(ELoadingStep.LoadingFailed);
				break;
			case RequestResult.Status.Suceeded:
			{
				UnityEngine.Camera[] array = UnityEngine.Object.FindObjectsOfType<UnityEngine.Camera>();
				UnityEngine.Camera[] array2 = array;
				foreach (UnityEngine.Camera camera in array2)
				{
					GameObject gameObject = camera.gameObject;
					if (gameObject.GetComponent<CombinedPostEffect>() == null && (camera.cullingMask & Layers.DefaultLayer) != 0)
					{
						camera.renderingPath = RenderingPath.VertexLit;
						camera.useOcclusionCulling = false;
						camera.hdr = false;
					}
				}
				this.OnProgress(100);
				this.OnStep(ELoadingStep.AllDone);
				break;
			}
			}
		}

		private void OnMissionDataLoaded(QueryResult _result, MissionLoaderProvider.ResultData _missionData)
		{
			if (_result.RequestStatus == RequestResult.Status.Suceeded)
			{
				SetLoadedMissionData(_missionData);
			}
			else
			{
				this.OnStep(ELoadingStep.LoadingFailed);
			}
		}

		private void SetLoadedMissionData(MissionLoaderProvider.ResultData _missionData)
		{
			LoadedMissionData = _missionData;
			CurrentMissionDescription = Main.Instance.Startup.FillBundles(_missionData.MissionRootData);
			if (InAnimus)
			{
				Application.targetFrameRate = 60;
			}
			else
			{
				Application.targetFrameRate = 25;
			}
			this.OnStep(ELoadingStep.LoadingMissionDone);
		}

		private void UpdateLighting()
		{
			RegionDefinition regionDefinition = CurrentMissionDescription.GetRegionDefinition();
			if (regionDefinition != null)
			{
				GameObject @object = regionDefinition.CharacterLighting.GetObject<GameObject>();
				GameObject gameObject = GameObject.Find("EnvScenario");
				if (@object != null && gameObject != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(@object);
					RenderingCharacterLightingSettings renderingCharacterLightingSettings = gameObject.GetComponentInChildren<RenderingCharacterLightingSettings>();
					if (renderingCharacterLightingSettings == null)
					{
						renderingCharacterLightingSettings = gameObject.AddComponentIfNotExist<RenderingCharacterLightingSettings>();
					}
					RenderingCharacterLightingSettings componentInChildren = gameObject2.GetComponentInChildren<RenderingCharacterLightingSettings>();
					if (renderingCharacterLightingSettings != null && componentInChildren != null)
					{
						renderingCharacterLightingSettings.CopyFrom(componentInChildren);
					}
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			else
			{
				GameObject gameObject3 = GameObject.Find("EnvScenario");
				if (gameObject3 != null)
				{
					RenderingCharacterLightingSettings componentInChildren2 = gameObject3.GetComponentInChildren<RenderingCharacterLightingSettings>();
					componentInChildren2.LightDimmer = 1f;
					componentInChildren2.SphericalHarmonicsTerm = 1f;
				}
			}
		}

		private void OnBundlesLoaded(QueryResult _result, AcierNetwork.Provider.MissionData _missionData)
		{
			switch (_result.RequestStatus)
			{
			case RequestResult.Status.InProgress:
				this.OnProgress((int)Mathf.Lerp(0f, 10f, _result.Progress));
				break;
			case RequestResult.Status.Failed:
				this.OnStep(ELoadingStep.LoadingFailed);
				break;
			case RequestResult.Status.Suceeded:
				if (m_bIsRestart || Application.loadedLevelName == CurrentMissionDescription.Scene)
				{
					this.OnStep(ELoadingStep.LoadingSucceeded);
					OnLevelWasLoaded(Application.loadedLevelName);
					this.OnProgress(100);
					this.OnStep(ELoadingStep.AllDone);
				}
				else
				{
					this.OnProgress(10);
					SceneLoadingProvider.SceneLoadingData sceneLoadingData = new SceneLoadingProvider.SceneLoadingData();
					sceneLoadingData.Scene = CurrentMissionDescription.Scene;
					SceneLoadingProvider.SceneLoadingData param = sceneLoadingData;
					NetworkProvider.Instance.Request<SceneLoadingModel>(OnLevelLoaded, param);
				}
				break;
			}
		}

		private void RemoveUIMasksFromMainCamera()
		{
			if (UnityEngine.Camera.main != null)
			{
				UnityEngine.Camera.main.cullingMask &= ~((1 << Layers.UILayer) | (1 << Layers.UISharedLayer) | (1 << Layers.InteractableUiLayer) | (1 << Layers.NonInteractableUiLayer));
			}
		}
	}
}
