using System.Collections.Generic;
using Assets.Scripts.UI.Ingame.Behaviours;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;

namespace Assets.Scripts.UI
{
	public class UIScreenWidgetManager : UbiCore.Utils.Singleton<UIScreenWidgetManager>
	{
		public enum EUiScreenWidget
		{
			Invalid = 0,
			LoadingScreen = 1,
			EnemyUnitFrame = 2,
			PlayerUnitFrame = 3,
			ObjectiveTracker = 4,
			SkillWheel = 5,
			MiniMap = 6,
			Movement = 7,
			Pause = 8,
			OnScreenMessage = 9,
			OnScreenTimer = 10,
			DownloadScreen = 11,
			MissionIntroduction = 12,
			MissionOutro = 13,
			Action = 14,
			KillTokenBar = 15,
			Notification = 16,
			IngamePopup = 17,
			MissionResult = 18,
			TutorialResult = 19,
			OnScreenPreview = 20
		}

		private BBDictionary<int, UIBaseBehaviour> m_inGameScreenRegister = new BBDictionary<int, UIBaseBehaviour>();

		private BBDictionary<int, UIBaseBehaviour> m_sharedScreenRegister = new BBDictionary<int, UIBaseBehaviour>();

		private BBDictionary<int, UIBaseBehaviour> m_downloadScreenRegister = new BBDictionary<int, UIBaseBehaviour>();

		public UIBaseBehaviour GetWidgetByType(EUiScreenWidget _type)
		{
			UIBaseBehaviour value = null;
			if (m_inGameScreenRegister.TryGetValue((int)_type, out value))
			{
				return value;
			}
			if (m_sharedScreenRegister.TryGetValue((int)_type, out value))
			{
				return value;
			}
			if (m_downloadScreenRegister.TryGetValue((int)_type, out value))
			{
				return value;
			}
			return null;
		}

		public void UnRegisterAllInGameBehaviours()
		{
			m_inGameScreenRegister.Clear();
		}

		public void UnRegisterAllDownloadBehaviours()
		{
			m_downloadScreenRegister.Clear();
		}

		public void UnRegisterWidget(EUiScreenWidget _type)
		{
			UnRegisterWidget(_type, m_inGameScreenRegister);
			UnRegisterWidget(_type, m_sharedScreenRegister);
			UnRegisterWidget(_type, m_downloadScreenRegister);
		}

		private void UnRegisterWidget(EUiScreenWidget _type, Dictionary<int, UIBaseBehaviour> _register)
		{
			if (_register.ContainsKey((int)_type))
			{
				_register[(int)_type] = null;
				_register.Remove((int)_type);
			}
		}

		public void RegisterAllInGameBehaviours()
		{
			m_inGameScreenRegister.Clear();
			for (int i = 0; i < UIRoot.list.Count; i++)
			{
				UIRoot uIRoot = UIRoot.list[i];
				if (uIRoot.gameObject.layer != Layers.UISharedLayer)
				{
					RegisterAllBehavioursFromUIRoot(uIRoot, m_inGameScreenRegister);
				}
			}
		}

		public void RegisterAllSharedBehaviours(UIRoot _sharedUIRoot)
		{
			m_sharedScreenRegister.Clear();
			RegisterAllBehavioursFromUIRoot(_sharedUIRoot, m_sharedScreenRegister);
		}

		public void RegisterAllDownloadScreenBehaviours(UIRoot _downloadScreenRoot)
		{
			m_downloadScreenRegister.Clear();
			RegisterAllBehavioursFromUIRoot(_downloadScreenRoot, m_downloadScreenRegister);
		}

		protected override void OnDestroy()
		{
			m_inGameScreenRegister.Clear();
			m_inGameScreenRegister = null;
			m_sharedScreenRegister.Clear();
			m_sharedScreenRegister = null;
			m_downloadScreenRegister.Clear();
			m_downloadScreenRegister = null;
		}

		private void RegisterAllBehavioursFromUIRoot(UIRoot _uiRoot, Dictionary<int, UIBaseBehaviour> _register)
		{
			if (!(_uiRoot != null))
			{
				return;
			}
			UIBaseBehaviour[] componentsInChildren = _uiRoot.GetComponentsInChildren<UIBaseBehaviour>(true);
			foreach (UIBaseBehaviour uIBaseBehaviour in componentsInChildren)
			{
				if (uIBaseBehaviour.ShouldRegisterWidgetWithMgr)
				{
					_register.Add((int)uIBaseBehaviour.ScreenWidgetType, uIBaseBehaviour);
				}
			}
		}
	}
}
