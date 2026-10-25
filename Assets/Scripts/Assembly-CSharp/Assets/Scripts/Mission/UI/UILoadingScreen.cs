using System;
using System.Collections;
using AcierNetwork;
using AcierNetwork.AssetBundles;
using AcierNetwork.Communication;
using AcierNetwork.Provider;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Network.Missions;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.LoadingScreen;
using Assets.Scripts.UI.State;
using Database;
using Humanoids;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public class UILoadingScreen : UIBase
	{
		public enum EState
		{
			Hide = 0,
			Open = 1,
			WaitForTap = 2,
			FadeOut = 3
		}

		private bool m_openAfterSharedUILoaded;

		private GameObject m_preloadedAssassin;

		private bool m_wasLastMissionElite;

		private MissionDescExt m_lastPlayedMissionDesc;

		private UILoadingScreenDetails m_screenDetails;

		public static UILoadingScreen Instance { get; private set; }

		public EState State { get; private set; }

		public bool IsHidden
		{
			get
			{
				return m_screenDetails == null || !m_screenDetails.gameObject.activeInHierarchy;
			}
		}

		public float Progress
		{
			get
			{
				return (!(m_screenDetails != null)) ? 0f : m_screenDetails.Progress;
			}
			set
			{
				if (m_screenDetails != null)
				{
					m_screenDetails.Progress = Mathf.Clamp01(value);
				}
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.LoadingScreen;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_screenDetails;
			}
		}

		public override UIRoot UIMainRoot
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.UISharedRoot;
			}
		}

		public UILoadingScreen()
		{
			Instance = this;
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (!base.IsInitialized)
			{
				Init();
			}
			if (base.IsInitialized)
			{
				CheckInput();
				if (m_openAfterSharedUILoaded)
				{
					OpenNow();
					m_openAfterSharedUILoaded = false;
				}
			}
		}

		public override void SetVisible(bool _value)
		{
		}

		public override void OnMissionUnloaded()
		{
		}

		public override bool Tap(TapEvent _gesture)
		{
			return State != EState.Hide;
		}

		public override bool IsOnUi(Vector2 _position)
		{
			return State != EState.Hide;
		}

		public void Open()
		{
			if (UIDownloadScreen.Instance.State != UIDownloadScreen.EState.Open && State == EState.Hide)
			{
				if (UIMainRoot != null)
				{
					OpenNow();
				}
				else
				{
					m_openAfterSharedUILoaded = true;
				}
				base.IsActive = true;
			}
		}

		private void OpenNow()
		{
			if (!base.IsInitialized)
			{
				Init();
			}
			if (base.IsInitialized)
			{
				State = EState.Open;
				AssetBundleManager.Instance.AutoUnloadEnabled = false;
				if (m_screenDetails != null)
				{
					m_screenDetails.Open();
					UpdateDetailScreen();
				}
			}
		}

		private void UpdateDetailScreen()
		{
			MissionDescExt missionDescExt = Main.Instance.SceneLoader.CurrentMissionDescription;
			bool flag = Main.Instance.SceneLoader.LoadedMissionData != null && Main.Instance.SceneLoader.LoadedMissionData.IsEliteMission;
			if (missionDescExt != null)
			{
				if (Main.Instance.SceneLoader.InAnimus)
				{
					missionDescExt = m_lastPlayedMissionDesc;
					flag = m_wasLastMissionElite;
				}
				else
				{
					m_lastPlayedMissionDesc = missionDescExt;
					m_wasLastMissionElite = flag;
				}
			}
			if (m_screenDetails != null)
			{
				m_screenDetails.UpdateScreen(missionDescExt, flag, Main.Instance.SceneLoader.InAnimus);
			}
		}

		public void Close(bool _delayed)
		{
			AssetBundleManager.Instance.AutoUnloadEnabled = true;
			State = EState.Hide;
			if (m_screenDetails != null)
			{
				m_screenDetails.Close(_delayed);
			}
		}

		public IEnumerator LoadMission(MissionFile _contentProto, MissionGoals _missionGoals, bool _eliteMission, bool _isQuickRestart, bool _isSceneSwitch)
		{
			yield return new WaitForEndOfFrame();
			Main.Instance.SceneLoader.PrepareScene(_contentProto, _missionGoals, _eliteMission, _isQuickRestart);
			while (Main.Instance.SceneLoader.CurrentMissionDescription == null)
			{
				yield return 0;
			}
			if (!UIDownloadScreen.Instance.IsActive || UIDownloadScreen.Instance.State == UIDownloadScreen.EState.Hide)
			{
				if (!Instance.IsInitialized)
				{
					Instance.Open();
				}
				while (!Instance.Details.FullyVisible)
				{
					yield return 0;
				}
			}
			yield return Main.Instance.StartCoroutine(UnloadAndLoadNextScene(_isQuickRestart, _isSceneSwitch));
		}

		public IEnumerator LoadMission(string _missionUrl, bool _isQuickRestart)
		{
			yield return new WaitForEndOfFrame();
			Main.Instance.SceneLoader.PrepareScene(_missionUrl, _isQuickRestart);
			while (Main.Instance.SceneLoader.CurrentMissionDescription == null)
			{
				yield return 0;
			}
			if (!UIDownloadScreen.Instance.IsActive)
			{
				if (!Instance.IsInitialized)
				{
					Instance.Open();
				}
				while (!Instance.Details.FullyVisible)
				{
					yield return 0;
				}
			}
			yield return Main.Instance.StartCoroutine(UnloadAndLoadNextScene(_isQuickRestart, true));
		}

		public IEnumerator RetryMission()
		{
			yield return Main.Instance.StartCoroutine(LoadMission(null, null, false, true, false));
		}

		private IEnumerator UnloadAndLoadNextScene(bool _isQuickRestart, bool _isSceneSwitch)
		{
			UnityEngine.Object.Destroy(m_preloadedAssassin);
			m_preloadedAssassin = null;
			UbiCore.Utils.Singleton<Timer>.Instance.Reset();
			UbiCore.Utils.Singleton<MissionManager>.Shutdown();
			yield return Main.Instance.StartCoroutine(Main.Instance.SceneLoader.UnloadScene(_isQuickRestart || !_isSceneSwitch));
			UbiCore.Utils.Singleton<MissionManager>.Initialize();
			if (!Main.Instance.SceneLoader.InAnimus)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.PrepareMissionLoad(_isQuickRestart);
			}
			Main.Instance.SceneLoader.LoadScene();
		}

		public IEnumerator PreloadForAnimus()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			for (int i = 0; i < profile.assassins.assassins.Length; i++)
			{
				Assassin assassin = profile.assassins.assassins[i];
				if (assassin.id == Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId)
				{
					GameDB db = Main.Instance.GameDB;
					HeadDef headDef = db.GetItem<HeadDef>(db.FindGameIdFromGuid(assassin.headGuid));
					HumanoidDef def = db.GetItem<HumanoidDef>(db.FindGameIdFromGuid(assassin.humanoidGuid));
					CharacterAppearance appearance = new CharacterAppearance(def, headDef);
					AssetBundleProviderInput bundleProviderInput = new AssetBundleProviderInput
					{
						AssetBundles = new BBHashSet<AssetBundleInfo>()
					};
					bundleProviderInput.AssetBundles.Add(def.AssetBundleInfo);
					QueryResult result = null;
					NetworkProvider.Instance.Request(delegate(QueryResult _result, AssetBundleProviderResult _type)
					{
						result = _result;
					}, bundleProviderInput);
					while (result == null || !result.IsCompleted)
					{
						yield return 0;
					}
					yield return Main.Instance.StartCoroutine(def.BodyPortrait.PrepareAsync());
					yield return Main.Instance.StartCoroutine(def.BodyPortraitMask.PrepareAsync());
					yield return Main.Instance.StartCoroutine(Singleton<HumanoidFactory>.Instance.PreloadHumanoidFromDefinitionAsync(def, appearance));
					AnimatorDef animDef = Main.SafeGameDb.GetItem<AnimatorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
					HumanoidFactoryHelpers.AddGoToCache(_object: animDef.AnimatorPrefab.GetObject<GameObject>(), _id: animDef.GameID);
					m_preloadedAssassin = Singleton<HumanoidFactory>.Instance.CreateHumanoidFromDefinition(def, true, appearance, null, UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorPlayerAnimus);
					yield return new WaitForEndOfFrame();
					m_preloadedAssassin.SetActive(false);
					HeadDef.HeadMaterials headMat = appearance.HeadMaterials;
					if (headMat != null)
					{
						yield return Main.Instance.StartCoroutine(headMat.Material.PrepareAsync());
						yield return Main.Instance.StartCoroutine(headMat.Portrait.PrepareAsync());
					}
					break;
				}
			}
		}

		private IEnumerator PreloadAnimusAndClose()
		{
			yield return Main.Instance.StartCoroutine(PreloadForAnimus());
			Close(true);
		}

		private void CheckInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (gamepad.AcceptDown || gamepad.CloseDown)
			{
				OnTapScreen();
			}
		}

		private void OnTapScreen()
		{
			if (State == EState.WaitForTap)
			{
				Close(false);
				if (!Main.Instance.SceneLoader.InAnimus)
				{
					AudioManager.Instance.EventManager.OnTapStartMission();
					UbiCore.Utils.Singleton<MissionManager>.Instance.Go();
				}
			}
		}

		private void OnScreenClosed()
		{
			base.IsActive = false;
			if (Main.Instance.SceneLoader.InAnimus)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.OnEnterAnimus(false);
			}
			else
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.OnEnterMission();
			}
		}

		private void OnSceneLoaderProgress(int _progress)
		{
			Progress = (float)_progress / 100f;
		}

		private void OnSceneLoaderStep(SceneLoader.ELoadingStep _step)
		{
			switch (_step)
			{
			case SceneLoader.ELoadingStep.LoadingStart:
			case SceneLoader.ELoadingStep.Unload:
				break;
			case SceneLoader.ELoadingStep.LoadingMissionDone:
				Open();
				break;
			case SceneLoader.ELoadingStep.LoadingFailed:
				Close(false);
				break;
			case SceneLoader.ELoadingStep.AllDone:
				if (Main.Instance.SceneLoader.InAnimus)
				{
					Main.Instance.StartCoroutine(PreloadAnimusAndClose());
				}
				else
				{
					UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionStateChanged;
				}
				break;
			case SceneLoader.ELoadingStep.LoadingSucceeded:
			case SceneLoader.ELoadingStep.LoadingSceneDone:
			case SceneLoader.ELoadingStep.PreInitialize:
			case SceneLoader.ELoadingStep.CameraCreated:
			case SceneLoader.ELoadingStep.Unloaded:
				break;
			}
		}

		private void OnMissionStateChanged(EMissionState _state)
		{
			if (_state == EMissionState.WaitingForGo)
			{
				ActivateWaitForTap();
				UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionStateChanged;
			}
		}

		private void ActivateWaitForTap()
		{
			State = EState.WaitForTap;
			AudioManager.Instance.EventManager.OnTapToStartMission();
			if (m_screenDetails != null)
			{
				m_screenDetails.CurrentState = UILoadingScreenDetails.EState.WaitForTap;
			}
		}

		private void Init()
		{
			bool flag = false;
			if (m_screenDetails == null)
			{
				flag = false;
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_screenDetails = widgetByType as UILoadingScreenDetails;
					if (m_screenDetails != null)
					{
						UILoadingScreenDetails screenDetails = m_screenDetails;
						screenDetails.OnTapScreen = (UILoadingScreenDetails.TapScreen)Delegate.Combine(screenDetails.OnTapScreen, new UILoadingScreenDetails.TapScreen(OnTapScreen));
						UILoadingScreenDetails screenDetails2 = m_screenDetails;
						screenDetails2.OnScreenClosed = (UILoadingScreenDetails.ScreenClosed)Delegate.Combine(screenDetails2.OnScreenClosed, new UILoadingScreenDetails.ScreenClosed(OnScreenClosed));
						flag = true;
					}
				}
			}
			if (flag)
			{
				Main.Instance.SceneLoader.OnStep += OnSceneLoaderStep;
				Main.Instance.SceneLoader.OnProgress += OnSceneLoaderProgress;
				base.IsInitialized = true;
			}
		}
	}
}
