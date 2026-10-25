using System.Text;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts
{
	public static class EveryplayManager
	{
		public delegate void RecordingStartedDelegate();

		public delegate void RecordingStoppedDelegate();

		public delegate void OverlayClosedDelegate();

		private static GameObject m_everyplayRoot;

		private static bool m_isInitialized;

		private static bool IsSupportedByQualitySettings
		{
			get
			{
				return Main.Instance.Startup.QualitySettings.everyPlayEnabled;
			}
		}

		public static bool IsEnabledByBackend { get; private set; }

		public static bool IsSupported
		{
			get
			{
				return false;
			}
		}

		public static bool IsReadyForRecording { get; private set; }

		public static bool IsRecording
		{
			get
			{
				if (!IsSupported)
				{
					return false;
				}
				return Everyplay.IsRecording();
			}
		}

		public static event RecordingStartedDelegate RecordingStarted;

		public static event RecordingStoppedDelegate RecordingStopped;

		public static event OverlayClosedDelegate OverlayClosed;

		static EveryplayManager()
		{
			IsReadyForRecording = false;
			IsEnabledByBackend = true;
		}

		public static void EnableEveryplaySettings(bool enable)
		{
			EveryplaySettings everyplaySettings = (EveryplaySettings)UnityEngine.Resources.Load("EveryplaySettings");
			everyplaySettings.iosSupportEnabled = (everyplaySettings.androidSupportEnabled = enable);
		}

		public static void Initialize()
		{
			if (!m_isInitialized && IsEnabledByBackend && IsSupportedByQualitySettings)
			{
				EnableEveryplaySettings(true);
				Everyplay.ReadyForRecording += OnReadyForRecording;
				Everyplay.RecordingStopped += OnRecordingStopped;
				Everyplay.RecordingStarted += OnRecordingStarted;
				Everyplay.WasClosed += OnOverlayWasClosed;
				Everyplay.SetDisableSingleCoreDevices(false);
				Everyplay.SetLowMemoryDevice(false);
				m_everyplayRoot = Object.Instantiate(UnityEngine.Resources.Load<GameObject>("UI/Animus/Screens/UIEveryplayRoot"));
				m_isInitialized = true;
				IsReadyForRecording = true;
			}
		}

		public static void Uninitialize()
		{
			if (m_isInitialized)
			{
				if (m_everyplayRoot != null)
				{
					Object.Destroy(m_everyplayRoot);
				}
				EveryplaySettings everyplaySettings = (EveryplaySettings)UnityEngine.Resources.Load("EveryplaySettings");
				everyplaySettings.iosSupportEnabled = (everyplaySettings.androidSupportEnabled = false);
				IsReadyForRecording = false;
				Everyplay.ReadyForRecording -= OnReadyForRecording;
				Everyplay.RecordingStopped -= OnRecordingStopped;
				Everyplay.RecordingStarted -= OnRecordingStarted;
				Everyplay.WasClosed -= OnOverlayWasClosed;
				m_isInitialized = false;
			}
		}

		public static void HandleDiscoveryServiceResponse(DiscoveryServiceResponse _response)
		{
			IsEnabledByBackend = _response.everplayEnabled;
		}

		public static void ToggleRecording()
		{
			if (IsSupported && IsReadyForRecording)
			{
				if (Everyplay.IsRecording())
				{
					Everyplay.StopRecording();
					Everyplay.ShowSharingModal();
				}
				else
				{
					Everyplay.StartRecording();
					Everyplay.SetMetadata("level_name", Main.Instance.SceneLoader.CurrentMissionDescription.LocaName());
				}
			}
		}

		public static void StopRecording(bool _showSharingDialog)
		{
			if (IsSupported && Everyplay.IsRecording())
			{
				Everyplay.StopRecording();
				if (_showSharingDialog)
				{
					Everyplay.ShowSharingModal();
				}
			}
		}

		public static void DumpInfos()
		{
			if (IsSupported)
			{
				StringBuilder stringBuilder = new StringBuilder(string.Format("[Everyplay] IsSupported: {0}", Everyplay.IsSupported()));
				stringBuilder.Append(string.Format("[Everyplay] IsRecordingSupported: {0}", Everyplay.IsRecordingSupported()));
				stringBuilder.Append(string.Format("[Everyplay] IsSupportedByQualitySettings: {0}", IsSupportedByQualitySettings));
				stringBuilder.Append(string.Format("[Everyplay] IsEnabledByBackend: {0}", IsEnabledByBackend));
				stringBuilder.Append(string.Format("[Everyplay] IsRecording: {0}", Everyplay.IsRecording()));
				stringBuilder.Append(string.Format("[Everyplay] IsPaused: {0}", Everyplay.IsPaused()));
			}
		}

		public static void OnReadyForRecording(bool _isReady)
		{
			IsReadyForRecording = _isReady;
		}

		private static void OnRecordingStarted()
		{
			if (IsSupported && EveryplayManager.RecordingStarted != null)
			{
				EveryplayManager.RecordingStarted();
			}
		}

		private static void OnRecordingStopped()
		{
			if (IsSupported && EveryplayManager.RecordingStopped != null)
			{
				EveryplayManager.RecordingStopped();
			}
		}

		private static void OnOverlayWasClosed()
		{
			if (IsSupported && EveryplayManager.OverlayClosed != null)
			{
				EveryplayManager.OverlayClosed();
			}
		}

		public static void OnRecordButtonClicked()
		{
			if (IsSupported && Everyplay.IsRecording())
			{
				Everyplay.StopRecording();
				Everyplay.ShowSharingModal();
			}
		}
	}
}
