using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AcierNetwork;
using AcierNetwork.AssetBundles;
using AcierNetwork.Communication;
using AcierNetwork.Provider;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Audio;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Database;
using Mission;
using SharedBaseLib.Core;
using SharedBaseLib.System.Cache;
using UbiCore.Helpers;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public class UIDownloadScreen : UIBase
	{
		public enum EState
		{
			Hide = 0,
			Open = 1,
			WaitForTap = 2,
			FadeOut = 3
		}

		private GameObject m_downloadScreenObject;

		private bool m_openAfterLoaded = true;

		private UIDownloadScreenDetails m_screenDetails;

		private AsyncOperation m_loadDownloadScreenOperation;

		private bool m_isAnimus;

		public List<UIDownloadScreenDetails.ImagePackage> imagePackageList = new List<UIDownloadScreenDetails.ImagePackage>();

		public bool ImageLoadingDone { get; set; }

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public static UIDownloadScreen Instance { get; private set; }

		public static bool IsLoadedAndFullyVisible
		{
			get
			{
				return Instance.IsSceneLoaded && Instance.Details.FullyVisible;
			}
		}

		public EState State { get; private set; }

		public string ProgressValue { get; set; }

		public bool IsHidden
		{
			get
			{
				return m_screenDetails == null || !m_screenDetails.gameObject.activeInHierarchy;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.DownloadScreen;
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
				return (!(m_downloadScreenObject != null)) ? null : m_downloadScreenObject.GetComponent<UIRoot>();
			}
		}

		public ErrorState ErrorState { get; set; }

		public bool IsSceneLoaded
		{
			get
			{
				return m_downloadScreenObject != null;
			}
		}

		public bool IsSoundReady { get; private set; }

		public UIDownloadScreen()
		{
			Instance = this;
		}

		public void WaitForTap()
		{
			m_screenDetails.CurrentState = UIDownloadScreenDetails.EState.WaitForTap;
			State = EState.WaitForTap;
			AudioManager.Instance.EventManager.OnLeaveDownloadScreen();
			AudioManager.Instance.EventManager.OnTapToStartMission();
			if (m_screenDetails != null)
			{
				m_screenDetails.CurrentState = UIDownloadScreenDetails.EState.WaitForTap;
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (!base.IsInitialized)
			{
				Init();
			}
			if (base.IsInitialized && m_openAfterLoaded)
			{
				OpenNow();
				m_openAfterLoaded = false;
			}
		}

		public override void SetVisible(bool _value)
		{
		}

		public override void OnMissionLoaded()
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
			if (State != EState.Open)
			{
				m_openAfterLoaded = true;
				if (UIMainRoot != null)
				{
					OpenNow();
				}
				base.IsActive = true;
				AssetBundleManager.Instance.AutoUnloadEnabled = false;
				Main.Instance.SceneLoader.OnStep += OnSceneLoaderStep;
				Main.Instance.SceneLoader.OnProgress += OnSceneLoaderProgress;
			}
		}

		private void OnSceneLoaderProgress(int _progress)
		{
			ProgressValue = string.Format("{0}", _progress.ToString());
		}

		private void OnSceneLoaderStep(SceneLoader.ELoadingStep _step)
		{
			switch (_step)
			{
			case SceneLoader.ELoadingStep.LoadingMissionDone:
				ProgressValue = string.Empty;
				m_screenDetails.CurrentState = UIDownloadScreenDetails.EState.LoadingMission;
				m_isAnimus = Main.Instance.SceneLoader.CurrentMissionDescription != null && Main.Instance.SceneLoader.CurrentMissionDescription.MissionType == EMissionType.Animus_EMissionType;
				break;
			case SceneLoader.ELoadingStep.AllDone:
				ProgressValue = string.Empty;
				if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain == null || Main.Instance.BuildDefinition.EditorMode)
				{
					Main.Instance.StartCoroutine(PreloadAnimusAndClose());
				}
				else if (!m_isAnimus)
				{
					UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionStateChanged;
				}
				break;
			}
		}

		private IEnumerator PreloadAnimusAndClose()
		{
			yield return Main.Instance.StartCoroutine(UILoadingScreen.Instance.PreloadForAnimus());
			Close(true);
		}

		private void OnMissionStateChanged(EMissionState _state)
		{
			if (_state == EMissionState.WaitingForGo)
			{
				WaitForTap();
				UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionStateChanged;
			}
		}

		public BBList<AssetBundleInfo> GetPreloadBundles(GameDB _db)
		{
			if (Assets.Scripts.Offline.OfflineMode.Enabled)
			{
				// Nothing to download up front: the content is on local storage and is opened when it is used.
				return null;
			}
			HashSet<AssetBundleInfo> assetBundleInfos = _db.GetAssetBundleInfos();
			BBList<AssetBundleInfo> bBList = null;
			foreach (AssetBundleInfo item in assetBundleInfos)
			{
				string acierDataURL = GameFolders.GetAcierDataURL(Main.Instance.BaseURL, item.URL);
				if (UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(acierDataURL, (uint)item.BuildNumber, true) == null)
				{
					for (SharedBaseLib.System.Cache.Item entry = UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(acierDataURL, 0u, true); entry != null; entry = UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(acierDataURL, 0u, true))
					{
						UbiCore.Utils.Singleton<Manager>.Instance.Remove(entry);
					}
					if (bBList == null)
					{
						bBList = new BBList<AssetBundleInfo>();
					}
					bBList.Add(item);
				}
			}
			BBDictionary<string, SceneAssetBundle> sceneDictionary = Main.Instance.Startup.SceneDictionary;
			foreach (KeyValuePair<string, SceneAssetBundle> item2 in sceneDictionary)
			{
				AssetBundleInfo assetBundleInfos2 = item2.Value.GetAssetBundleInfos(Main.Instance.Startup.Quality);
				string acierDataURL2 = GameFolders.GetAcierDataURL(Main.Instance.BaseURL, assetBundleInfos2.URL);
				if (Main.Instance.Startup.PreloadBundles.Contains(assetBundleInfos2) && UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(acierDataURL2, (uint)assetBundleInfos2.BuildNumber) == null)
				{
					if (bBList == null)
					{
						bBList = new BBList<AssetBundleInfo>();
					}
					bBList.Add(assetBundleInfos2);
				}
			}
			if (bBList != null)
			{
				bBList.Sort((AssetBundleInfo a, AssetBundleInfo b) => b.Type.CompareTo(a.Type));
			}
			return bBList;
		}

		public BBList<AssetBundleInfo> GetSceneBundles(LocationSet _set)
		{
			LocationUnlockMappings.Entry mapping = EarlyAccessHelpers.GetMapping(_set.era, _set.region, _set.mood);
			string sceneName = mapping.sceneName;
			BBList<AssetBundleInfo> bBList = null;
			AssetBundleInfo sceneBundle = Main.Instance.Startup.GetSceneBundle(sceneName);
			string acierDataURL = GameFolders.GetAcierDataURL(Main.Instance.BaseURL, sceneBundle.URL);
			if (UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(acierDataURL, (uint)sceneBundle.BuildNumber) == null)
			{
				if (bBList == null)
				{
					bBList = new BBList<AssetBundleInfo>(1);
				}
				bBList.Add(sceneBundle);
			}
			return bBList;
		}

		public IEnumerator PrepareBundles()
		{
			AssetBundleProviderInput input = new AssetBundleProviderInput
			{
				AssetBundles = new BBHashSet<AssetBundleInfo>()
			};
			foreach (AssetBundleInfo bi in Main.Instance.GameDB.GetAssetBundleInfos())
			{
				if (bi.EnforceLoading)
				{
					input.AssetBundles.Add(bi);
				}
			}
			QueryResult result = null;
			NetworkProvider.Instance.Request(delegate(QueryResult _result, AssetBundleProviderResult _type)
			{
				result = _result;
			}, input);
			while (result == null || !result.IsCompleted)
			{
				yield return 0;
			}
		}

		public void GetTotalSizes(BBList<AssetBundleInfo> _infos, ref ulong _totalSize, ref ulong _remainingSize)
		{
			_totalSize = 0uL;
			_remainingSize = 0uL;
			for (int i = 0; i < _infos.Count; i++)
			{
				AssetBundleInfo assetBundleInfo = _infos[i];
				string acierDataURL = GameFolders.GetAcierDataURL(Main.Instance.BaseURL, assetBundleInfo.URL);
				SharedBaseLib.System.Cache.Item entryUnsafe = UbiCore.Utils.Singleton<Manager>.Instance.GetEntryUnsafe(acierDataURL, (uint)assetBundleInfo.BuildNumber);
				if (entryUnsafe != null)
				{
					_remainingSize += entryUnsafe.Size - entryUnsafe.CurrentSize;
					_totalSize += entryUnsafe.Size;
				}
				else
				{
					_totalSize += (ulong)assetBundleInfo.AssetBundleFileSize;
					_remainingSize += (ulong)assetBundleInfo.AssetBundleFileSize;
				}
			}
		}

		public IEnumerator DownloadData(BBList<AssetBundleInfo> _infos, Action _onDoneCallback)
		{
			ShowWidget(UIDownloadScreenDetails.EState.LoadingData);
			ErrorState = null;
			ulong totalSize = 0uL;
			ulong remainingSize = 0uL;
			GetTotalSizes(_infos, ref totalSize, ref remainingSize);
			if (totalSize > SharedMain.GetFreeDiskSpace())
			{
				ErrorState = new ErrorState
				{
					ErrorCode = ErrorCode.NotEnoughDiskspace,
					ErrorGroup = ErrorCategory.AcierStartupErrors
				};
			}
			else if (_infos.Count > 0)
			{
				AssetBundleInfo bundle2 = null;
				float currentSize = 0f;
				SimpleDelegate.CallbackFunction handler = delegate(object[] _objects)
				{
					ShowWidget(UIDownloadScreenDetails.EState.LoadingData);
					ProgressValue = string.Format("{0}", (int)(((float)totalSize - currentSize - (float)(uint)_objects[1]) / 1000000f));
				};
				UbiCore.Utils.Singleton<Manager>.Instance.ProgressDelegate.Add(handler);
				IsSoundReady = true;
				foreach (AssetBundleInfo assetBundleInfo in _infos)
				{
					if (assetBundleInfo.Type == AssetBundleInfo.EType.SoundContainer)
					{
						IsSoundReady = false;
						break;
					}
				}
				for (int i = 0; i < _infos.Count; i++)
				{
					bundle2 = _infos[i];
					string url = GameFolders.GetAcierDataURL(Main.Instance.BaseURL, bundle2.URL);
					DateTime start = DateTime.UtcNow;
					yield return Main.Instance.StartCoroutine(UbiCore.Utils.Singleton<Manager>.Instance.PrepareFile(url, (uint)bundle2.BuildNumber, bundle2.BuildNumber == 0, Manager.GetCacheItemType(bundle2), (ulong)bundle2.AssetBundleFileSize));
					DateTime end = DateTime.UtcNow;
					if (string.IsNullOrEmpty(UbiCore.Utils.Singleton<Manager>.Instance.LastError))
					{
						if (!IsSoundReady && bundle2.Type != AssetBundleInfo.EType.SoundContainer)
						{
							IsSoundReady = true;
						}
						TrackingManager.AddCDNTrackPoint(start, end, DateTime.UtcNow, url, bundle2.AssetBundleFileSize);
						uint crc;
						if (uint.TryParse(bundle2.AssetBundleHash, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out crc))
						{
							SharedBaseLib.System.Cache.Item entry = UbiCore.Utils.Singleton<Manager>.Instance.GetEntry(url, (uint)bundle2.BuildNumber, true);
							if (entry.CRC != (int)crc)
							{
								ErrorState = new ErrorState
								{
									ErrorCode = ErrorCode.BundleDownloadCRCFailed,
									ErrorGroup = ErrorCategory.AcierStartupErrors
								};
								UbiCore.Utils.Singleton<Manager>.Instance.Remove(entry);
								break;
							}
						}
						currentSize += (float)bundle2.AssetBundleFileSize;
						continue;
					}
					TrackingManager.AddCDNTrackPoint(start, end, url, bundle2.AssetBundleFileSize);
					ErrorState errorState;
					switch (UbiCore.Utils.Singleton<Manager>.Instance.ErrorReason)
					{
					case Manager.EErrorReason.OutOfDiskspace:
						errorState = new ErrorState
						{
							ErrorCode = ErrorCode.NotEnoughDiskspace,
							ErrorGroup = ErrorCategory.AcierStartupErrors
						};
						break;
					case Manager.EErrorReason.CacheFull:
						errorState = new ErrorState
						{
							ErrorCode = ErrorCode.NotEnoughCacheSpace,
							ErrorGroup = ErrorCategory.AcierStartupErrors
						};
						break;
					default:
						errorState = new ErrorState
						{
							ErrorCode = ErrorCode.BundleDownloadFailed,
							ErrorGroup = ErrorCategory.AcierStartupErrors
						};
						break;
					}
					if (UbiCore.Utils.Singleton<Manager>.Instance.PotentialConnectionIssue)
					{
						yield return Main.Instance.StartCoroutine(errorState.DetermineReachabilityError());
					}
					ErrorState = errorState;
					break;
				}
				ShowWidget(UIDownloadScreenDetails.EState.None);
				ProgressValue = "0";
				UbiCore.Utils.Singleton<Manager>.Instance.ProgressDelegate.Remove(handler);
			}
			if (_onDoneCallback != null)
			{
				_onDoneCallback();
			}
		}

		public void ShowWidget(UIDownloadScreenDetails.EState state)
		{
			if (m_screenDetails.CurrentState != state)
			{
				m_screenDetails.CurrentState = state;
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
				if (m_screenDetails != null)
				{
					m_screenDetails.Open();
				}
			}
		}

		public void Close(bool _delayed)
		{
			if (State != 0)
			{
				AssetBundleManager.Instance.AutoUnloadEnabled = true;
				Main.Instance.SceneLoader.OnStep -= OnSceneLoaderStep;
				Main.Instance.SceneLoader.OnProgress -= OnSceneLoaderProgress;
				State = EState.Hide;
				if (m_screenDetails != null)
				{
					m_screenDetails.Close(_delayed);
				}
				else
				{
					ClearScene();
				}
			}
		}

		private void OnTapScreen()
		{
			if (State == EState.WaitForTap)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.Go();
				AudioManager.Instance.EventManager.OnTapStartMission();
				Close(false);
				TutorialHelper.TrackIntroMissionStep();
			}
		}

		private void OnScreenClosed()
		{
			ClearScene();
			if (Main.Instance.SceneLoader.InAnimus)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.OnEnterAnimus(true);
			}
		}

		private void Init()
		{
			if (base.IsInitialized || !LoadScene())
			{
				return;
			}
			bool flag = false;
			if (m_screenDetails == null)
			{
				flag = false;
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_screenDetails = widgetByType as UIDownloadScreenDetails;
					if (m_screenDetails != null)
					{
						UIDownloadScreenDetails screenDetails = m_screenDetails;
						screenDetails.OnTapScreen = (UIDownloadScreenDetails.TapScreen)Delegate.Combine(screenDetails.OnTapScreen, new UIDownloadScreenDetails.TapScreen(OnTapScreen));
						UIDownloadScreenDetails screenDetails2 = m_screenDetails;
						screenDetails2.OnScreenClosed = (UIDownloadScreenDetails.ScreenClosed)Delegate.Combine(screenDetails2.OnScreenClosed, new UIDownloadScreenDetails.ScreenClosed(OnScreenClosed));
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.IsInitialized = true;
			}
		}

		private bool LoadScene()
		{
			if (m_loadDownloadScreenOperation == null)
			{
				m_loadDownloadScreenOperation = Application.LoadLevelAdditiveAsync("UIDownloadScreen");
			}
			if (m_loadDownloadScreenOperation != null && m_loadDownloadScreenOperation.isDone)
			{
				m_downloadScreenObject = GameObject.FindGameObjectWithTag("UIDownloadRoot");
				UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.RegisterAllDownloadScreenBehaviours(UIMainRoot);
				return true;
			}
			return false;
		}

		private void ClearScene()
		{
			NGUITools.DestroyImmediate(m_downloadScreenObject);
			Main.Instance.RequestResourceUnload();
			m_openAfterLoaded = false;
			m_screenDetails = null;
			m_loadDownloadScreenOperation = null;
			UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.UnRegisterAllDownloadBehaviours();
			base.IsInitialized = false;
			base.IsActive = false;
		}
	}
}
