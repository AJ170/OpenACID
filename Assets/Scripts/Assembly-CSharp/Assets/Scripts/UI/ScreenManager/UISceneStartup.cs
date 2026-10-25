using Assets.Scripts.Audio;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UISceneStartup : MonoBehaviour
	{
		[SerializeField]
		private UISceneData m_sceneData;

		private void Awake()
		{
			EnvironmentBridgeMain.ResetManager();
			LocalizationManager.Initialize();
			Singleton<UIScreenManager>.Instance.SceneData = m_sceneData;
		}

		private void Start()
		{
			if (Singleton<UIScreenManager>.Instance.IsInitialized)
			{
				OnScreenManagerInitialized();
			}
			else
			{
				EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenManagerInitialized, OnScreenManagerInitialized, true);
			}
		}

		private void OnScreenManagerInitialized()
		{
			AudioManager.Instance.EventManager.OnAnimusFullyLoadedAndInitialized();
			Singleton<UIScreenManager>.Instance.ActivateStartupScreen();
		}

		private void OnDestroy()
		{
			if (LocalizationManager.Instance != null)
			{
				LocalizationManager.Instance.PurgeManager();
			}
			if (Singleton<UIScreenManager>.Instance != null)
			{
				Singleton<UIScreenManager>.Instance.PurgeManager();
			}
		}
	}
}
